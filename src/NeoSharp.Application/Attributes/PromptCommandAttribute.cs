﻿using NeoSharp.Core.Extensions;
using NeoSharp.Core.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;

namespace NeoSharp.Application.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PromptCommandAttribute : Attribute
    {
        #region Constants

        private static readonly Type _stringType = typeof(string);
        private static readonly Type _iListType = typeof(IList);
        private static readonly Type _fileInfoType = typeof(FileInfo);
        private static readonly Type _directoryInfoType = typeof(DirectoryInfo);
        private static readonly Type _objArrayType = typeof(object[]);

        private static readonly char[] _splitChars = { ';', ',', '|' };

        #endregion

        #region Variables

        private MethodInfo _method;

        #endregion

        #region Properties

        /// <summary>
        /// Commands
        /// </summary>
        public readonly string Command;
        /// <summary>
        /// Help
        /// </summary>
        public string Help { get; set; }
        /// <summary>
        /// Category
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// Command Length
        /// </summary>
        internal int CommandLength;
        /// <summary>
        /// Parameters
        /// </summary>
        internal ParameterInfo[] Parameters { get; private set; }
        /// <summary>
        /// Method
        /// </summary>
        internal MethodInfo Method
        {
            get { return _method; }
            set
            {
                if (value == null) return;

                _method = value;
                Parameters = value.GetParameters();
            }
        }

        #endregion


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="command">Command</param>
        public PromptCommandAttribute(string command)
        {
            Command = command;
        }

        /// <summary>
        /// Convert string arguments to Method arguments
        /// </summary>
        /// <param name="args">Arguments</param>
        /// <returns>Return parsed arguments</returns>
        public object[] ConvertToArguments(CommandToken[] args)
        {
            var max = Parameters.Length;
            var ret = new object[max];

            if (args.Length < max)
                throw (new ArgumentException("Missing parameters"));

            for (var x = 0; x < max; x++)
            {
                if (Parameters[x].GetCustomAttribute<PromptCommandParameterBodyAttribute>() != null)
                {
                    // From here to the end

                    ret[x] = ParseToArgument(new CommandToken(string.Join(" ", args.Skip(x)), false), Parameters[x].ParameterType);
                    return ret;
                }
                else
                {
                    // Regular parameter

                    ret[x] = ParseToArgument(args[x], Parameters[x].ParameterType);
                }
            }

            return ret;
        }

        object ParseAutoObject(CommandToken token)
        {
            if (!token.Quoted)
            {
                if (token.Value.StartsWith("0x"))
                {
                    return token.Value.HexToBytes();
                }
                else
                {
                    // Number?

                    if (BigInteger.TryParse(token.Value, out BigInteger bi))
                        return bi;

                    // Decimal?

                    if (BigDecimal.TryParse(token.Value, 20, out BigDecimal bd))
                        return bd;

                    // Hashes

                    if (UInt160.TryParse(token.Value, out UInt160 hash160))
                        return hash160;

                    if (UInt256.TryParse(token.Value, out UInt256 hash256))
                        return hash256;
                }
            }

            return token.Value;
        }

        void AddTo(object obj, List<object> ret, Stack<List<object>> arrays)
        {
            if (arrays.Count > 0)
            {
                List<object> ls = arrays.Peek();
                ls.Add(obj);
            }
            else
            {
                ret.Add(obj);
            }
        }

        object ParseAutoObject(string value)
        {
            // Separate Array tokens

            CommandToken[] tks = CleanCommand(value.SplitCommandLine());

            // Fetch parameters

            List<object> ret = new List<object>();
            Stack<List<object>> arrays = new Stack<List<object>>();

            foreach (CommandToken token in tks)
            {
                string val = token.Value;

                if (token.Quoted)
                {
                    AddTo(ParseAutoObject(token), ret, arrays);
                }
                else
                {
                    switch (val)
                    {
                        case "[":
                            {
                                List<object> ls = new List<object>();
                                arrays.Push(ls);
                                break;
                            }
                        case "]":
                            {
                                List<object> ls = arrays.Pop();
                                AddTo(ls.ToArray(), ret, arrays);
                                break;
                            }
                        default:
                            {
                                AddTo(ParseAutoObject(token), ret, arrays);
                                break;
                            }
                    }
                }
            }

            if (arrays.Count > 0) throw new ArgumentException();

            return ret.Count == 1 ? ret[0] : ret.ToArray();
        }

        private CommandToken[] CleanCommand(IEnumerable<CommandToken> tokens)
        {
            bool change = false;
            List<CommandToken> tks = new List<CommandToken>();

            foreach (CommandToken token in tokens)
            {
                if (token.Quoted || token.Value == "[" || token.Value == "]")
                {
                    tks.Add(token);
                }
                else
                {
                    string val = token.Value;
                    if (val.StartsWith("["))
                    {
                        tks.Add(new CommandToken("["));
                        val = val.Substring(1);
                        change = true;
                    }

                    CommandToken add = null;

                    if (val.EndsWith("]"))
                    {
                        add = new CommandToken("]");
                        val = val.Substring(0, val.Length - 1);
                        change = true;
                    }

                    if (!string.IsNullOrEmpty(val))
                        tks.Add(new CommandToken(val, false));

                    if (add != null)
                        tks.Add(add);
                }
            }

            // Recursive
            if (change) return CleanCommand(tks);

            return tks.ToArray();
        }

        /// <summary>
        /// Parse argument
        /// </summary>
        /// <param name="token">Token</param>
        /// <param name="type">Type</param>
        /// <returns>Return parsed argument</returns>
        private object ParseToArgument(CommandToken token, Type type)
        {
            // Auto-detect
            if (_objArrayType == type)
            {
                return ParseAutoObject(token.Value);
            }

            // FileInfo
            if (_fileInfoType == type)
            {
                return new FileInfo(token.Value);
            }

            // DirectoryInfo
            if (_directoryInfoType == type)
            {
                return new DirectoryInfo(token.Value);
            }

            // Array
            if (type.IsArray)
            {
                var l = new List<object>();
                var gt = type.GetElementType();
                foreach (var ii in token.Value.Split(_splitChars))
                {
                    var ov = ParseToArgument(new CommandToken(ii, false), gt);
                    if (ov == null) continue;

                    l.Add(ov);
                }

                var a = (Array)Activator.CreateInstance(type, l.Count);
                Array.Copy(l.ToArray(), a, l.Count);
                return a;
            }

            // List
            if (_iListType.IsAssignableFrom(type))
            {
                var l = (IList)Activator.CreateInstance(type);

                // If dosen't have T return null
                if (type.GenericTypeArguments == null || type.GenericTypeArguments.Length == 0)
                    return null;

                var gt = type.GenericTypeArguments[0];
                foreach (var ii in token.Value.Split(_splitChars))
                {
                    var ov = ParseToArgument(new CommandToken(ii, false), gt);
                    if (ov == null) continue;

                    l.Add(ov);
                }
                return l;
            }

            // Is Convertible
            var conv = TypeDescriptor.GetConverter(type);
            if (conv.CanConvertFrom(_stringType))
            {
                return conv.ConvertFrom(token.Value);
            }

            throw (new ArgumentException());
        }
    }
}