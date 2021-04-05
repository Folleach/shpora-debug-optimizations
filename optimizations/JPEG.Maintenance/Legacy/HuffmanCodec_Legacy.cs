using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JPEG.Utilities;

namespace JPEG.Maintenance.Legacy
{
    class HuffmanNode_Legacy
	{
		public byte? LeafLabel { get; set; }
		public int Frequency { get; set; }
		public HuffmanNode_Legacy Left { get; set; }
		public HuffmanNode_Legacy Right { get; set; }
	}

	public class BitsWithLength_Legacy
	{
		public int Bits { get; set; }
		public int BitsCount { get; set; }

		public class Comparer : IEqualityComparer<BitsWithLength_Legacy>
		{
			public bool Equals(BitsWithLength_Legacy x, BitsWithLength_Legacy y)
			{
				if(x == y) return true;
				if(x == null || y == null)
					return false;
				return x.BitsCount == y.BitsCount && x.Bits == y.Bits;
			}

			public int GetHashCode(BitsWithLength_Legacy obj)
			{
				if(obj == null)
					return 0;
				return ((397 *obj.Bits) << 5) ^ (17* obj.BitsCount);
			}
		}
		
		public bool Equals(BitsWithLength_Legacy x, BitsWithLength_Legacy y)
		{
			if(x == y) return true;
			if(x == null || y == null)
				return false;
			return x.BitsCount == y.BitsCount && x.Bits == y.Bits;
		}

		public int GetHashCode(BitsWithLength_Legacy obj)
		{
			if(obj == null)
				return 0;
			return ((397 *obj.Bits) << 5) ^ (17* obj.BitsCount);
		}
	}

	class BitsBuffer_Legacy
	{
		private List<byte> buffer = new List<byte>();
		private BitsWithLength_Legacy unfinishedBits = new BitsWithLength_Legacy();

		public void Add(BitsWithLength_Legacy bitsWithLengthLegacy)
		{
			var bitsCount = bitsWithLengthLegacy.BitsCount;
			var bits = bitsWithLengthLegacy.Bits;

			int neededBits = 8 - unfinishedBits.BitsCount;
			while(bitsCount >= neededBits)
			{
				bitsCount -= neededBits;
				buffer.Add((byte) ((unfinishedBits.Bits << neededBits) + (bits >> bitsCount)));

				bits = bits & ((1 << bitsCount) - 1);

				unfinishedBits.Bits = 0;
				unfinishedBits.BitsCount = 0;

				neededBits = 8;
			}
			unfinishedBits.BitsCount +=  bitsCount;
			unfinishedBits.Bits = (unfinishedBits.Bits << bitsCount) + bits;
		}

		public byte[] ToArray(out long bitsCount)
		{
			bitsCount = buffer.Count * 8L + unfinishedBits.BitsCount;
			var result = new byte[bitsCount / 8 + (bitsCount % 8 > 0 ? 1 : 0)];
			buffer.CopyTo(result);
			if(unfinishedBits.BitsCount > 0)
				result[buffer.Count] = (byte) (unfinishedBits.Bits << (8 - unfinishedBits.BitsCount));
			return result;
		}
	}

	class HuffmanCodec_Legacy
	{
		public static byte[] Encode(IEnumerable<byte> data, out Dictionary<BitsWithLength_Legacy, byte> decodeTable, out long bitsCount)
		{
			var frequences = CalcFrequences(data);

			var root = BuildHuffmanTree(frequences);

			var encodeTable = new BitsWithLength_Legacy[byte.MaxValue + 1];
			FillEncodeTable(root, encodeTable);

			var bitsBuffer = new BitsBuffer_Legacy();
			foreach(var b in data)
				bitsBuffer.Add(encodeTable[b]);

			decodeTable = CreateDecodeTable(encodeTable);

			return bitsBuffer.ToArray(out bitsCount);
		}

		public static byte[] Decode(byte[] encodedData, Dictionary<BitsWithLength_Legacy, byte> decodeTable, long bitsCount)
		{
			var result = new List<byte>();

			byte decodedByte;
			var sample = new BitsWithLength_Legacy { Bits = 0, BitsCount = 0 };
			for(var byteNum = 0; byteNum < encodedData.Length; byteNum++)
			{
				var b = encodedData[byteNum];
				for(var bitNum = 0; bitNum < 8 && byteNum * 8 + bitNum < bitsCount; bitNum++)
				{
					sample.Bits = (sample.Bits << 1) + ((b & (1 << (8 - bitNum - 1))) != 0 ? 1 : 0);
					sample.BitsCount++;
	
					if(decodeTable.TryGetValue(sample, out decodedByte))
					{
						result.Add(decodedByte);

						sample.BitsCount = 0;
						sample.Bits = 0;
					}
				}
			}
			return result.ToArray();
		}

		private static Dictionary<BitsWithLength_Legacy, byte> CreateDecodeTable(BitsWithLength_Legacy[] encodeTable)
		{
			var result = new Dictionary<BitsWithLength_Legacy, byte>(new BitsWithLength_Legacy.Comparer());
			for(int b = 0; b < encodeTable.Length; b++)
			{
				var bitsWithLength = encodeTable[b];
				if(bitsWithLength == null)
					continue;

				result[bitsWithLength] = (byte) b;
			}
			return result;
		}

		private static void FillEncodeTable(HuffmanNode_Legacy nodeLegacy, BitsWithLength_Legacy[] encodeSubstitutionTable, int bitvector = 0, int depth = 0)
		{
			if(nodeLegacy.LeafLabel != null)
				encodeSubstitutionTable[nodeLegacy.LeafLabel.Value] = new BitsWithLength_Legacy {Bits = bitvector, BitsCount = depth};
			else
			{
				if(nodeLegacy.Left != null)
				{
					FillEncodeTable(nodeLegacy.Left, encodeSubstitutionTable, (bitvector << 1) + 1, depth + 1);
					FillEncodeTable(nodeLegacy.Right, encodeSubstitutionTable, (bitvector << 1) + 0, depth + 1);
				}
			}
		}

		private static HuffmanNode_Legacy BuildHuffmanTree(int[] frequences)
		{
			var nodes = GetNodes(frequences);
			
			while(nodes.Count() > 1)
			{
				var firstMin = nodes.MinOrDefault(node => node.Frequency);
				nodes = nodes.Without(firstMin);
				var secondMin = nodes.MinOrDefault(node => node.Frequency);
				nodes = nodes.Without(secondMin);
				nodes = nodes.Concat(new HuffmanNode_Legacy {Frequency = firstMin.Frequency + secondMin.Frequency, Left = secondMin, Right = firstMin }.ToEnumerable());
			}
			return nodes.First();
		}

		private static IEnumerable<HuffmanNode_Legacy> GetNodes(int[] frequences)
		{
			return Enumerable.Range(0, byte.MaxValue + 1)
				.Select(num => new HuffmanNode_Legacy {Frequency = frequences[num], LeafLabel = (byte) num})
				.Where(node => node.Frequency > 0)
				.ToArray();
		}

		private static int[] CalcFrequences(IEnumerable<byte> data)
		{
			var result = new int[byte.MaxValue + 1];
			Parallel.ForEach(data, b => Interlocked.Increment(ref result[b]));
			return result;
		}
	}
}