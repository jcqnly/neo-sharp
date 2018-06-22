using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeoSharp.BinarySerialization;
using NeoSharp.Core.Cryptography;
using NeoSharp.Core.Extensions;
using NeoSharp.Core.Models;

namespace NeoSharp.Core.Test.Serializers
{
    [TestClass]
    public class UtBinarySerializerRaw
    {
        private ICrypto _crypto;
        private IBinarySerializer _serializer;
        private IBinaryDeserializer _deserializer;

        [TestInitialize]
        public void WarmUpSerializer()
        {
            _crypto = new BouncyCastleCrypto();
            _serializer = new BinarySerializer(typeof(BlockHeader).Assembly, typeof(UtBinarySerializer).Assembly);
            _deserializer = new BinaryDeserializer(typeof(BlockHeader).Assembly, typeof(UtBinarySerializer).Assembly);
        }

        [TestMethod]
        public void Raw_Block()
        {
            // MainNet - Block 1

            var data = "00000000bf4421c88776c53b43ce1dc45463bfd2028e322fdfb60064be150ed3e36125d418f98ec3ed2c2d1c9427385e7b85d0d1a366e29c4e399693a59718380f8bbad6d6d90358010000004490d0bb7170726c59e75d652b5d3827bf04c165bbe9ef95cca4bf5501fd4501404edf5005771de04619235d5a4c7a9a11bb78e008541f1da7725f654c33380a3c87e2959a025da706d7255cb3a3fa07ebe9c6559d0d9e6213c68049168eb1056f4038a338f879930c8adc168983f60aae6f8542365d844f004976346b70fb0dd31aa1dbd4abd81e4a4aeef9941ecd4e2dd2c1a5b05e1cc74454d0403edaee6d7a4d4099d33c0b889bf6f3e6d87ab1b11140282e9a3265b0b9b918d6020b2c62d5a040c7e0c2c7c1dae3af9b19b178c71552ebd0b596e401c175067c70ea75717c8c00404e0ebd369e81093866fe29406dbf6b402c003774541799d08bf9bb0fc6070ec0f6bad908ab95f05fa64e682b485800b3c12102a8596e6c715ec76f4564d5eff34070e0521979fcd2cbbfa1456d97cc18d9b4a6ad87a97a2a0bcdedbf71b6c9676c645886056821b6f3fec8694894c66f41b762bc4e29e46ad15aee47f05d27d822f1552102486fd15702c4490a26703112a5cc1d0923fd697a33406bd5a1c00e0013b09a7021024c7b7fb6c310fccf1ba33b082519d82964ea93868d676662d4a59ad548df0e7d2102aaec38470f6aad0042c6e877cfd8087d2676b0f516fddd362801b9bd3936399e2103b209fd4f53a7170ea4444e0cb0a6bb6a53c2bd016926989cf85f9b0fba17a70c2103b8d9d5771d8f513aa0869b9cc8d50986403b78c6da36890638c3d46a5adce04a2102ca0e27697b9c248f6f16e085fd0061e26f44da85b58ee835c110caa5ec3ba5542102df48f60e8f3e01c48ff40b9b7f1310d7a8b2a193188befe1c2e3df740e89509357ae0100004490d0bb00000000".HexToBytes();
            var block = _deserializer.Deserialize<Block>(data);

            Assert.AreEqual(block.Version, 0U);
            Assert.AreEqual(block.PreviousBlockHash.ToString(true), "0xd42561e3d30e15be6400b6df2f328e02d2bf6354c41dce433bc57687c82144bf");
            Assert.AreEqual(block.MerkleRoot.ToString(true), "0xd6ba8b0f381897a59396394e9ce266a3d1d0857b5e3827941c2d2cedc38ef918");
            Assert.AreEqual(block.Timestamp, 1476647382U);
            Assert.AreEqual(block.Index, 1U);
            Assert.AreEqual(block.ConsensusData, 7814431937225855044UL);
            Assert.AreEqual(block.NextConsensus.ToString(true), "0x55bfa4cc95efe9bb65c104bf27385d2b655de759");

            block.UpdateHash(_serializer, _crypto);

            Assert.AreEqual(block.Script.InvocationScript.ToHexString(true), "0x404edf5005771de04619235d5a4c7a9a11bb78e008541f1da7725f654c33380a3c87e2959a025da706d7255cb3a3fa07ebe9c6559d0d9e6213c68049168eb1056f4038a338f879930c8adc168983f60aae6f8542365d844f004976346b70fb0dd31aa1dbd4abd81e4a4aeef9941ecd4e2dd2c1a5b05e1cc74454d0403edaee6d7a4d4099d33c0b889bf6f3e6d87ab1b11140282e9a3265b0b9b918d6020b2c62d5a040c7e0c2c7c1dae3af9b19b178c71552ebd0b596e401c175067c70ea75717c8c00404e0ebd369e81093866fe29406dbf6b402c003774541799d08bf9bb0fc6070ec0f6bad908ab95f05fa64e682b485800b3c12102a8596e6c715ec76f4564d5eff34070e0521979fcd2cbbfa1456d97cc18d9b4a6ad87a97a2a0bcdedbf71b6c9676c645886056821b6f3fec8694894c66f41b762bc4e29e46ad15aee47f05d27d822");
            Assert.AreEqual(block.Script.VerificationScript.ToHexString(true), "0x552102486fd15702c4490a26703112a5cc1d0923fd697a33406bd5a1c00e0013b09a7021024c7b7fb6c310fccf1ba33b082519d82964ea93868d676662d4a59ad548df0e7d2102aaec38470f6aad0042c6e877cfd8087d2676b0f516fddd362801b9bd3936399e2103b209fd4f53a7170ea4444e0cb0a6bb6a53c2bd016926989cf85f9b0fba17a70c2103b8d9d5771d8f513aa0869b9cc8d50986403b78c6da36890638c3d46a5adce04a2102ca0e27697b9c248f6f16e085fd0061e26f44da85b58ee835c110caa5ec3ba5542102df48f60e8f3e01c48ff40b9b7f1310d7a8b2a193188befe1c2e3df740e89509357ae");
            Assert.AreEqual(block.Script.Hash.ToString(true), "0x55bfa4cc95efe9bb65c104bf27385d2b655de759");

            Assert.AreEqual(block.Hash.ToString(true), "0xd782db8a38b0eea0d7394e0f007c61c71798867578c77c387c08113903946cc9");

            Assert.AreEqual(block.Transactions.Length, 1);

            MinerTransaction tx = block.Transactions[0] as MinerTransaction;

            tx.UpdateHash(_serializer, _crypto);

            Assert.AreEqual(tx.Hash.ToString(true), "0xd6ba8b0f381897a59396394e9ce266a3d1d0857b5e3827941c2d2cedc38ef918");
            Assert.AreEqual(tx.Nonce, 3151007812);
            Assert.AreEqual(tx.Attributes.Length, 0);
            Assert.AreEqual(tx.Inputs.Length, 0);
            Assert.AreEqual(tx.Outputs.Length, 0);

            CollectionAssert.AreEqual(data, _serializer.Serialize(block));
        }

        [TestMethod]
        public void SerializeDeserialize_ClaimTransaction()
        {
            // Mainnet Block=4275 / Tx=1

            var data = "020001fda149910702cc19ed967c32f883a322f2e1713790c1398f538a42e489d485ee0000000001e72d286979ee6cb1b7e65dfddfb2e384100b8d148e7758de42e4168b71792c60c074110000000000f41cdd4b7ec41847443fa36bf8dde0009d7ecebc01414019fcb645e67b870a657fe028bcb057f866347d211dc26a25fe0570250f41d0c881113e1820ac55a029e6fc5acab80587f9bebf8b84dbd4503ba816c417b8bf522321039f07df7861c216de3b78c647b77f8b01404b400a437302b651cdf206ec1af626ac".HexToBytes();
            var tx = (ClaimTransaction)_deserializer.Deserialize<Transaction>(data);

            tx.UpdateHash(_serializer, _crypto);

            Assert.AreEqual(tx.Hash.ToString(true), "0x462c0e6fcd68853dd44f4055e2aa759548038d3b1362b6182398a6d44c0d1bf0");
            Assert.AreEqual(tx.Attributes.Length, 0);

            Assert.AreEqual(tx.Claims.Length, 1);
            Assert.AreEqual(tx.Claims[0].PrevHash.ToString(true), "0xee85d489e4428a538f39c1903771e1f222a383f8327c96ed19cc02079149a1fd");
            Assert.AreEqual(tx.Claims[0].PrevIndex, 0);

            Assert.AreEqual(tx.Scripts.Length, 1);
            Assert.AreEqual(tx.Scripts[0].Hash.ToString(true), "0xbcce7e9d00e0ddf86ba33f444718c47e4bdd1cf4");
            Assert.AreEqual(tx.Scripts[0].InvocationScript.ToHexString(true), "0x4019fcb645e67b870a657fe028bcb057f866347d211dc26a25fe0570250f41d0c881113e1820ac55a029e6fc5acab80587f9bebf8b84dbd4503ba816c417b8bf52");
            Assert.AreEqual(tx.Scripts[0].VerificationScript.ToHexString(true), "0x21039f07df7861c216de3b78c647b77f8b01404b400a437302b651cdf206ec1af626ac");

            Assert.AreEqual(tx.Inputs.Length, 0);

            Assert.AreEqual(tx.Outputs.Length, 1);
            Assert.AreEqual(tx.Outputs[0].AssetId.ToString(true), "0x602c79718b16e442de58778e148d0b1084e3b2dffd5de6b7b16cee7969282de7");
            Assert.AreEqual(tx.Outputs[0].ScriptHash.ToString(true), "0xbcce7e9d00e0ddf86ba33f444718c47e4bdd1cf4");
            Assert.AreEqual(tx.Outputs[0].Value.ToString(), "0.01144");

            Assert.AreEqual(tx.Version, 0);

            CollectionAssert.AreEqual(data, _serializer.Serialize(tx));
        }

        [TestMethod]
        public void SerializeDeserialize_ContractTransaction()
        {
            // Mainnet Block=47320 / Tx=1

            var data = "80000730cf62fd54fc761f291d07d68088dd81b8b35a7c444f3af8acd78a3ad4ff75d16330aac6d49da8f63cf6442c5f707317bc3e7490029af1a75b83adc0ec3b1b3e1f0f30febc956626564e8318c1f6c11cb4e36d4ded9af1be07e25b40af39d73e4b3dc630ce2e790a02d3794e60109450943358d280389e9cdba1d09f6c105d136f38e731303329124a4a2ea122fa14dbfee41b0fae43a35b29eed33ac81c699202018dfe1530509da7d029445f07d8218fcb73a0cff2acaf76659d1f5eda826b9e896eba991030c214154a649ce8ac5ee97f3c170b6574c122731f757f2a425e5eaeab62d66586012346ed8739bb9d76afb4df8254dc237eff14013041ed694c7dab2e76753d319f0000019b7cffdaa674beae0f930ebe6085af9093e5fe56b34a5c220ccdcf6efc336fc50080e03779c311005fa99d93303775fe50ca119c327759313eccfa1c01fd0401403d2ccc242d953c3b312f37b1b3aaa21a372cbb7adc1efcfc8e07f3704caa0e82aecbff5f28f17935b6432a571754060881d221a6069270c2e532f58f68248aea408cecfdd1639cae103fcf853bdf44600a6617592928fba26fa9301a222a9b4a384751453c793c2c99460a0e6e324f340abb54daf229b807cf4c8a634e5a4a1f574078891ade2cf73114de7e47b454cb88c71cca614162a7728df5f2511fd20e809ed12827139f6efae0d152cfa411d3e072f63f27f2cef4ee698327f600cc4281ff4056d91a17c56287aba509877eedc2e0541370880fb9bd4cb24a9fc754442048c29975018fbe5d16f27eeb47ca7d17d53d70fbefb8fd5c8144a82c3b72e6ca190cf1542102486fd15702c4490a26703112a5cc1d0923fd697a33406bd5a1c00e0013b09a7021024c7b7fb6c310fccf1ba33b082519d82964ea93868d676662d4a59ad548df0e7d2102aaec38470f6aad0042c6e877cfd8087d2676b0f516fddd362801b9bd3936399e2103b209fd4f53a7170ea4444e0cb0a6bb6a53c2bd016926989cf85f9b0fba17a70c2103b8d9d5771d8f513aa0869b9cc8d50986403b78c6da36890638c3d46a5adce04a2102ca0e27697b9c248f6f16e085fd0061e26f44da85b58ee835c110caa5ec3ba5542102df48f60e8f3e01c48ff40b9b7f1310d7a8b2a193188befe1c2e3df740e89509357ae".HexToBytes();
            var tx = (ContractTransaction)_deserializer.Deserialize<Transaction>(data);

            tx.UpdateHash(_serializer, _crypto);

            Assert.AreEqual(tx.Hash.ToString(true), "0x01a6e985c1c1da04996b3472f04dfcacc4384d8b5b2f21f17367ea92f6a9cb26");
            Assert.AreEqual(tx.Attributes.Length, 7);

            Assert.AreEqual(tx.Attributes[0].Data.ToHexString(true), "0xcf62fd54fc761f291d07d68088dd81b8b35a7c444f3af8acd78a3ad4ff75d163");
            Assert.AreEqual(tx.Attributes[0].Usage, TransactionAttributeUsage.Vote);
            Assert.AreEqual(tx.Attributes[1].Data.ToHexString(true), "0xaac6d49da8f63cf6442c5f707317bc3e7490029af1a75b83adc0ec3b1b3e1f0f");
            Assert.AreEqual(tx.Attributes[1].Usage, TransactionAttributeUsage.Vote);
            Assert.AreEqual(tx.Attributes[2].Data.ToHexString(true), "0xfebc956626564e8318c1f6c11cb4e36d4ded9af1be07e25b40af39d73e4b3dc6");
            Assert.AreEqual(tx.Attributes[2].Usage, TransactionAttributeUsage.Vote);
            Assert.AreEqual(tx.Attributes[3].Data.ToHexString(true), "0xce2e790a02d3794e60109450943358d280389e9cdba1d09f6c105d136f38e731");
            Assert.AreEqual(tx.Attributes[3].Usage, TransactionAttributeUsage.Vote);
            Assert.AreEqual(tx.Attributes[4].Data.ToHexString(true), "0x3329124a4a2ea122fa14dbfee41b0fae43a35b29eed33ac81c699202018dfe15");
            Assert.AreEqual(tx.Attributes[4].Usage, TransactionAttributeUsage.Vote);
            Assert.AreEqual(tx.Attributes[5].Data.ToHexString(true), "0x509da7d029445f07d8218fcb73a0cff2acaf76659d1f5eda826b9e896eba9910");
            Assert.AreEqual(tx.Attributes[5].Usage, TransactionAttributeUsage.Vote);
            Assert.AreEqual(tx.Attributes[6].Data.ToHexString(true), "0xc214154a649ce8ac5ee97f3c170b6574c122731f757f2a425e5eaeab62d66586");
            Assert.AreEqual(tx.Attributes[6].Usage, TransactionAttributeUsage.Vote);

            Assert.AreEqual(tx.Scripts.Length, 1);
            Assert.AreEqual(tx.Scripts[0].Hash.ToString(true), "0x1cfacc3e315977329c11ca50fe753730939da95f");
            Assert.AreEqual(tx.Scripts[0].InvocationScript.ToHexString(true), "0x403d2ccc242d953c3b312f37b1b3aaa21a372cbb7adc1efcfc8e07f3704caa0e82aecbff5f28f17935b6432a571754060881d221a6069270c2e532f58f68248aea408cecfdd1639cae103fcf853bdf44600a6617592928fba26fa9301a222a9b4a384751453c793c2c99460a0e6e324f340abb54daf229b807cf4c8a634e5a4a1f574078891ade2cf73114de7e47b454cb88c71cca614162a7728df5f2511fd20e809ed12827139f6efae0d152cfa411d3e072f63f27f2cef4ee698327f600cc4281ff4056d91a17c56287aba509877eedc2e0541370880fb9bd4cb24a9fc754442048c29975018fbe5d16f27eeb47ca7d17d53d70fbefb8fd5c8144a82c3b72e6ca190c");
            Assert.AreEqual(tx.Scripts[0].VerificationScript.ToHexString(true), "0x542102486fd15702c4490a26703112a5cc1d0923fd697a33406bd5a1c00e0013b09a7021024c7b7fb6c310fccf1ba33b082519d82964ea93868d676662d4a59ad548df0e7d2102aaec38470f6aad0042c6e877cfd8087d2676b0f516fddd362801b9bd3936399e2103b209fd4f53a7170ea4444e0cb0a6bb6a53c2bd016926989cf85f9b0fba17a70c2103b8d9d5771d8f513aa0869b9cc8d50986403b78c6da36890638c3d46a5adce04a2102ca0e27697b9c248f6f16e085fd0061e26f44da85b58ee835c110caa5ec3ba5542102df48f60e8f3e01c48ff40b9b7f1310d7a8b2a193188befe1c2e3df740e89509357ae");

            Assert.AreEqual(tx.Inputs.Length, 1);
            Assert.AreEqual(tx.Inputs[0].PrevHash.ToString(true), "0x9f313d75762eab7d4c69ed41300114ff7e23dc5482dfb4af769dbb3987ed4623");
            Assert.AreEqual(tx.Inputs[0].PrevIndex, 0);

            Assert.AreEqual(tx.Outputs.Length, 1);
            Assert.AreEqual(tx.Outputs[0].AssetId.ToString(true), "0xc56f33fc6ecfcd0c225c4ab356fee59390af8560be0e930faebe74a6daff7c9b");
            Assert.AreEqual(tx.Outputs[0].ScriptHash.ToString(true), "0x1cfacc3e315977329c11ca50fe753730939da95f");
            Assert.AreEqual(tx.Outputs[0].Value.ToString(), "50000000");

            Assert.AreEqual(tx.Version, 0);
            CollectionAssert.AreEqual(data, _serializer.Serialize(tx));
        }

        [TestMethod]
        public void SerializeDeserialize_IssueTransaction()
        {
            // Mainnet Block=12285 / Tx=3

            var data = "010000017ded1c83bd63e8871c8c2ad57607fe1423e8796606f2f5c2fe25be3f27f89a430000037ded1c83bd63e8871c8c2ad57607fe1423e8796606f2f5c2fe25be3f27f89a43001f8ed117000000f41cdd4b7ec41847443fa36bf8dde0009d7ecebc7ded1c83bd63e8871c8c2ad57607fe1423e8796606f2f5c2fe25be3f27f89a4300e1f5050000000055d6bc2c5a139c894df2344e03d1d2e1fbb7b609e72d286979ee6cb1b7e65dfddfb2e384100b8d148e7758de42e4168b71792c6040469af32a020000f41cdd4b7ec41847443fa36bf8dde0009d7ecebc014140420d9cdc020c525f95ae8464f7c51d0b84ee820e0073536a658f35428bd44e1941f4b1697a27cbdf3975da3366db6d3e6ec8e4aef3c50eff376a330bf728b5b42321039f07df7861c216de3b78c647b77f8b01404b400a437302b651cdf206ec1af626ac".HexToBytes();
            var tx = (IssueTransaction)_deserializer.Deserialize<Transaction>(data);

            tx.UpdateHash(_serializer, _crypto);

            Assert.AreEqual(tx.Hash.ToString(true), "0xf1ec2baf76c47bb3460369a0f962321d30423e1329d0c0734d9cd7fce8ed89c2");
            Assert.AreEqual(tx.Attributes.Length, 0);

            Assert.AreEqual(tx.Scripts.Length, 1);
            Assert.AreEqual(tx.Scripts[0].Hash.ToString(true), "0xbcce7e9d00e0ddf86ba33f444718c47e4bdd1cf4");
            Assert.AreEqual(tx.Scripts[0].InvocationScript.ToHexString(true), "0x40420d9cdc020c525f95ae8464f7c51d0b84ee820e0073536a658f35428bd44e1941f4b1697a27cbdf3975da3366db6d3e6ec8e4aef3c50eff376a330bf728b5b4");
            Assert.AreEqual(tx.Scripts[0].VerificationScript.ToHexString(true), "0x21039f07df7861c216de3b78c647b77f8b01404b400a437302b651cdf206ec1af626ac");

            Assert.AreEqual(tx.Inputs.Length, 1);
            Assert.AreEqual(tx.Inputs[0].PrevHash.ToString(true), "0x439af8273fbe25fec2f5f2066679e82314fe0776d52a8c1c87e863bd831ced7d");
            Assert.AreEqual(tx.Inputs[0].PrevIndex, 0);

            Assert.AreEqual(tx.Outputs.Length, 3);

            Assert.AreEqual(tx.Outputs[0].AssetId.ToString(true), "0x439af8273fbe25fec2f5f2066679e82314fe0776d52a8c1c87e863bd831ced7d");
            Assert.AreEqual(tx.Outputs[0].ScriptHash.ToString(true), "0xbcce7e9d00e0ddf86ba33f444718c47e4bdd1cf4");
            Assert.AreEqual(tx.Outputs[0].Value.ToString(), "1023");

            Assert.AreEqual(tx.Outputs[1].AssetId.ToString(true), "0x439af8273fbe25fec2f5f2066679e82314fe0776d52a8c1c87e863bd831ced7d");
            Assert.AreEqual(tx.Outputs[1].ScriptHash.ToString(true), "0x09b6b7fbe1d2d1034e34f24d899c135a2cbcd655");
            Assert.AreEqual(tx.Outputs[1].Value.ToString(), "1");

            Assert.AreEqual(tx.Outputs[2].AssetId.ToString(true), "0x602c79718b16e442de58778e148d0b1084e3b2dffd5de6b7b16cee7969282de7");
            Assert.AreEqual(tx.Outputs[2].ScriptHash.ToString(true), "0xbcce7e9d00e0ddf86ba33f444718c47e4bdd1cf4");
            Assert.AreEqual(tx.Outputs[2].Value.ToString(), "23834.98856");

            Assert.AreEqual(tx.Version, 0);
            CollectionAssert.AreEqual(data, _serializer.Serialize(tx));
        }

        [TestMethod]
        public void SerializeDeserialize_MinerTransaction()
        {
            // Mainnet Block=1 / tx=0

            var data = "00004490d0bb00000000".HexToBytes();
            var tx = (MinerTransaction)_deserializer.Deserialize<Transaction>(data);

            tx.UpdateHash(_serializer, _crypto);

            Assert.AreEqual(tx.Hash.ToString(true), "0xd6ba8b0f381897a59396394e9ce266a3d1d0857b5e3827941c2d2cedc38ef918");
            Assert.AreEqual(tx.Nonce, 3151007812);
            Assert.AreEqual(tx.Scripts.Length, 0);
            Assert.AreEqual(tx.Attributes.Length, 0);
            Assert.AreEqual(tx.Inputs.Length, 0);
            Assert.AreEqual(tx.Outputs.Length, 0);
            Assert.AreEqual(tx.Version, 0);

            CollectionAssert.AreEqual(data, _serializer.Serialize(tx));
        }

        [TestMethod]
        public void SerializeDeserialize_InvocationTransaction()
        {
            // Mainnet Block=2421128 / Tx=7

            var data = "d1015e0800e1f50500000000209b7cffdaa674beae0f930ebe6085af9093e5fe56b34a5c220ccdcf6efc336fc5141e542e30389997d4c076ed65d0a7438719969cd653c1076465706f73697467bd097b2fcf70e1fd30a5c3ef51e662feeafeba0100000000000000000001a50be4db475e02e665229d22e82d8820e5bf8b4022c60a5806d9f1c801672cb10100019b7cffdaa674beae0f930ebe6085af9093e5fe56b34a5c220ccdcf6efc336fc500e1f50500000000bd097b2fcf70e1fd30a5c3ef51e662feeafeba010141409d689aa663e04da2b74d1eba6608e4a3bacdd416a68b0102df7072e25263b63a7bfd1de1d2d3c951efa3c10c456ab41f6e3a6edaa021a309c6e31e12604132922321021958d772f0cb49220752c74c8ff6e873b8b3f69905d32c2d688cfae570fb98e0ac".HexToBytes();
            var tx = (InvocationTransaction)_deserializer.Deserialize<Transaction>(data);

            tx.UpdateHash(_serializer, _crypto);

            Assert.AreEqual(tx.Hash.ToString(true), "0xf76206631e9664c3251790ee362039a253bb6763a43343e852499d182883b32b");
            Assert.AreEqual(tx.Attributes.Length, 0);

            Assert.AreEqual(tx.Script.ToHexString(true), "0x0800e1f50500000000209b7cffdaa674beae0f930ebe6085af9093e5fe56b34a5c220ccdcf6efc336fc5141e542e30389997d4c076ed65d0a7438719969cd653c1076465706f73697467bd097b2fcf70e1fd30a5c3ef51e662feeafeba01");
            Assert.AreEqual(tx.Gas.ToString(), "0");

            Assert.AreEqual(tx.Scripts.Length, 1);
            Assert.AreEqual(tx.Scripts[0].Hash.ToString(true), "0xd69c96198743a7d065ed76c0d4979938302e541e");
            Assert.AreEqual(tx.Scripts[0].InvocationScript.ToHexString(true), "0x409d689aa663e04da2b74d1eba6608e4a3bacdd416a68b0102df7072e25263b63a7bfd1de1d2d3c951efa3c10c456ab41f6e3a6edaa021a309c6e31e1260413292");
            Assert.AreEqual(tx.Scripts[0].VerificationScript.ToHexString(true), "0x21021958d772f0cb49220752c74c8ff6e873b8b3f69905d32c2d688cfae570fb98e0ac");

            Assert.AreEqual(tx.Inputs.Length, 1);
            Assert.AreEqual(tx.Inputs[0].PrevHash.ToString(true), "0xb12c6701c8f1d906580ac622408bbfe520882de8229d2265e6025e47dbe40ba5");
            Assert.AreEqual(tx.Inputs[0].PrevIndex, 1);

            Assert.AreEqual(tx.Outputs.Length, 1);

            Assert.AreEqual(tx.Outputs[0].AssetId.ToString(true), "0xc56f33fc6ecfcd0c225c4ab356fee59390af8560be0e930faebe74a6daff7c9b");
            Assert.AreEqual(tx.Outputs[0].ScriptHash.ToString(true), "0x01bafeeafe62e651efc3a530fde170cf2f7b09bd");
            Assert.AreEqual(tx.Outputs[0].Value.ToString(), "1");

            Assert.AreEqual(tx.Version, 1);

            CollectionAssert.AreEqual(data, _serializer.Serialize(tx));
        }

        [TestMethod]
        public void SerializeDeserialize_RegisterTransaction()
        {
            // Mainnet Block=4329 / Tx=1

            var data = "400060335b7b226c616e67223a227a682d434e222c226e616d65223a2248656c6c6f20416e74536861726573204d61696e6e6574227d5d000084d71700000008039f07df7861c216de3b78c647b77f8b01404b400a437302b651cdf206ec1af626f41cdd4b7ec41847443fa36bf8dde0009d7ecebc0001b3ba761da52f1f5c7ce0e069707a3235613e77263b9da5dcff0737f2d09ea1f5000001e72d286979ee6cb1b7e65dfddfb2e384100b8d148e7758de42e4168b71792c6040bad59736020000f41cdd4b7ec41847443fa36bf8dde0009d7ecebc0141403af6b2ad6f7630f81eaaff485073c0fe4f337102d1ecf0a48ed9bcfbd4a4bbeb5d7ae26f7dd5e0e04527b313187dfe6a6a0cd7f85fd0ce431f609acce1d34aff2321039f07df7861c216de3b78c647b77f8b01404b400a437302b651cdf206ec1af626ac".HexToBytes();
            var tx = (RegisterTransaction)_deserializer.Deserialize<Transaction>(data);

            tx.UpdateHash(_serializer, _crypto);

            Assert.AreEqual(tx.Hash.ToString(true), "0x439af8273fbe25fec2f5f2066679e82314fe0776d52a8c1c87e863bd831ced7d");
            Assert.AreEqual(tx.Attributes.Length, 0);

            Assert.AreEqual(tx.Admin.ToString(true), "0xbcce7e9d00e0ddf86ba33f444718c47e4bdd1cf4");
            Assert.AreEqual(tx.Amount.ToString(), "1024");
            Assert.AreEqual(tx.AssetType, AssetType.Token);
            Assert.AreEqual(tx.Name, "[{\"lang\":\"zh-CN\",\"name\":\"Hello AntShares Mainnet\"}]");
            Assert.AreEqual(tx.Precision, 8);
            Assert.AreEqual(tx.Owner.ToString(true), "0x039f07df7861c216de3b78c647b77f8b01404b400a437302b651cdf206ec1af626");

            Assert.AreEqual(tx.Scripts.Length, 1);
            Assert.AreEqual(tx.Scripts[0].Hash.ToString(true), "0xbcce7e9d00e0ddf86ba33f444718c47e4bdd1cf4");
            Assert.AreEqual(tx.Scripts[0].InvocationScript.ToHexString(true), "0x403af6b2ad6f7630f81eaaff485073c0fe4f337102d1ecf0a48ed9bcfbd4a4bbeb5d7ae26f7dd5e0e04527b313187dfe6a6a0cd7f85fd0ce431f609acce1d34aff");
            Assert.AreEqual(tx.Scripts[0].VerificationScript.ToHexString(true), "0x21039f07df7861c216de3b78c647b77f8b01404b400a437302b651cdf206ec1af626ac");

            Assert.AreEqual(tx.Inputs.Length, 1);
            Assert.AreEqual(tx.Inputs[0].PrevHash.ToString(true), "0xf5a19ed0f23707ffdca59d3b26773e6135327a7069e0e07c5c1f2fa51d76bab3");
            Assert.AreEqual(tx.Inputs[0].PrevIndex, 0);

            Assert.AreEqual(tx.Outputs.Length, 1);

            Assert.AreEqual(tx.Outputs[0].AssetId.ToString(true), "0x602c79718b16e442de58778e148d0b1084e3b2dffd5de6b7b16cee7969282de7");
            Assert.AreEqual(tx.Outputs[0].ScriptHash.ToString(true), "0xbcce7e9d00e0ddf86ba33f444718c47e4bdd1cf4");
            Assert.AreEqual(tx.Outputs[0].Value.ToString(), "24334.98856");

            Assert.AreEqual(tx.Version, 0);

            CollectionAssert.AreEqual(data, _serializer.Serialize(tx));
        }

        [TestMethod]
        public void SerializeDeserialize_StateTransaction()
        {
            // Mainnet Block= / Tx=

            var data = "".HexToBytes();
            var tx = (StateTransaction)_deserializer.Deserialize<Transaction>(data);

            tx.UpdateHash(_serializer, _crypto);

        }

        [TestMethod]
        public void SerializeDeserialize_PublishTransaction()
        {
            // Mainnet Block= / Tx=

            var data = "".HexToBytes();
            var tx = (PublishTransaction)_deserializer.Deserialize<Transaction>(data);

            tx.UpdateHash(_serializer, _crypto);

        }

        [TestMethod]
        public void SerializeDeserialize_EnrollmentTransaction()
        {
            // Mainnet Block=47293 / Tx=1

            var data = "200003b209fd4f53a7170ea4444e0cb0a6bb6a53c2bd016926989cf85f9b0fba17a70c0001f2f5be8a2d2d3d62e1601646b1c8b4ab58b8ee1595caf3e4a0bbfefe029719e2000001e72d286979ee6cb1b7e65dfddfb2e384100b8d148e7758de42e4168b71792c6000e1f505000000009f2d1729a79436148dc442c25f41335ef9f78bbd014140831597a1f22cba5fb4aa85ade9629a8fd18b46a05ba576a0ab71bcccb6e3fba9593555951f219baeb3368e0c2d722694455403d191d200177afb8f5ac69b5566232103b209fd4f53a7170ea4444e0cb0a6bb6a53c2bd016926989cf85f9b0fba17a70cac".HexToBytes();
            var tx = (EnrollmentTransaction)_deserializer.Deserialize<Transaction>(data);

            tx.UpdateHash(_serializer, _crypto);

            Assert.AreEqual(tx.Hash.ToString(true), "0x63d175ffd43a8ad7acf83a4f447c5ab3b881dd8880d6071d291f76fc54fd62cf");
            Assert.AreEqual(tx.Attributes.Length, 0);

            Assert.AreEqual(tx.PublicKey.Data.ToHexString(true), "0x03b209fd4f53a7170ea4444e0cb0a6bb6a53c2bd016926989cf85f9b0fba17a70c");

            Assert.AreEqual(tx.Scripts.Length, 1);
            Assert.AreEqual(tx.Scripts[0].Hash.ToString(true), "0xbd8bf7f95e33415fc242c48d143694a729172d9f");
            Assert.AreEqual(tx.Scripts[0].InvocationScript.ToHexString(true), "0x40831597a1f22cba5fb4aa85ade9629a8fd18b46a05ba576a0ab71bcccb6e3fba9593555951f219baeb3368e0c2d722694455403d191d200177afb8f5ac69b5566");
            Assert.AreEqual(tx.Scripts[0].VerificationScript.ToHexString(true), "0x2103b209fd4f53a7170ea4444e0cb0a6bb6a53c2bd016926989cf85f9b0fba17a70cac");

            Assert.AreEqual(tx.Inputs.Length, 1);
            Assert.AreEqual(tx.Inputs[0].PrevHash.ToString(true), "0xe2199702fefebba0e4f3ca9515eeb858abb4c8b1461660e1623d2d2d8abef5f2");
            Assert.AreEqual(tx.Inputs[0].PrevIndex, 0);

            Assert.AreEqual(tx.Outputs.Length, 1);

            Assert.AreEqual(tx.Outputs[0].AssetId.ToString(true), "0x602c79718b16e442de58778e148d0b1084e3b2dffd5de6b7b16cee7969282de7");
            Assert.AreEqual(tx.Outputs[0].ScriptHash.ToString(true), "0xbd8bf7f95e33415fc242c48d143694a729172d9f");
            Assert.AreEqual(tx.Outputs[0].Value.ToString(), "1");

            Assert.AreEqual(tx.Version, 0);

            CollectionAssert.AreEqual(data, _serializer.Serialize(tx));
        }
    }
}