using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NUnit.Framework;

namespace Frontend.Tests
{
    [TestFixture]
    public class Nbt
    {
        // Test Data taken from https://wiki.vg/NBT
        private static byte[] _testBytes = new byte[]
        {
          0x0a,
          0x00, 0x0b,
          0x68 , 0x65 , 0x6c , 0x6c , 0x6f , 0x20 , 0x77 , 0x6f , 0x72 , 0x6c , 0x64,
          0x08,
          0x00, 0x04,
          0x6e, 0x61, 0x6d, 0x65,
          0x00, 0x09,
          0x42 , 0x61 , 0x6e , 0x61 , 0x6e , 0x72 , 0x61 , 0x6d , 0x61,
          0x00
        };
        
        private static NbtCompound _testCompound = new NbtCompound(new ReadOnlyDictionary<string, INbtTag>(new Dictionary<string, INbtTag>()
        {
            { "hello world", 
                new NbtCompound(new ReadOnlyDictionary<string, INbtTag>(new Dictionary<string, INbtTag>()
                {
                    { "name", new NbtString("Bananrama") }
                }))
            }
        }));

        [Test]
        public void NbtReadBasic()
        {
            var nbtReader = new NbtReader(_testBytes);
            var compound = nbtReader.ReadCompound();
            var originalCompound = (NbtCompound)_testCompound.Value["hello world"];
            var readCompound = (NbtCompound)compound.Value["hello world"];
            Assert.AreEqual(originalCompound.Value["name"], readCompound.Value["name"]);
        }

        [Test]
        public void NbtWriteBasic()
        {
            using var nbtWriter = new NbtWriter();
            nbtWriter.WriteRoot(_testCompound);
            var array = nbtWriter.Stream.ToArray();
            Assert.True(_testBytes.SequenceEqual(array));
        }
    }
}