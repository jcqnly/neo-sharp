﻿namespace NeoSharp.VM.TestHelper
{
    public enum VMUTStackItemType
    {
        /// <summary>
        /// Bool (true,false)
        /// </summary>
        Bool,
        
        /// <summary>
        /// ByteArray
        /// </summary>
        ByteArray,
        
        /// <summary>
        /// ByteArray as UTF8 string
        /// </summary>
        String,
        
        /// <summary>
        /// String
        /// </summary>
        Interop,
        
        /// <summary>
        /// BigInteger
        /// </summary>
        Integer,
        
        /// <summary>
        /// Array
        /// </summary>
        Array,

        /// <summary>
        /// Struct
        /// </summary>
        Struct,

        /// <summary>
        /// Map
        /// </summary>
        Map
    }
}