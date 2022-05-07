using System;
using System.Linq;

namespace NBitcoin.Altcoins
{
    public class HeavyHashTests
    {
        public static string Sha3InputStr = "01-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00";
		public static string Sha3ExpectedOutputStr = "03-26-21-32-93-F9-03-00-AD-E5-E1-C8-4B-95-89-ED-4F-27-DD-23-5E-67-EF-64-86-43-4E-6C-CC-8A-CD-17";
		public static string HeavyHashInputStr= "C1-EC-FD-FC";
		public static string HeavyHashExpectedOutputStr = "39-38-7F-2E-64-E7-C0-8D-3C-E0-DA-8C-49-1B-4F-CF-2C-86-27-98-DE-DB-46-90-D8-19-DE-79-26-AA-4E-CB";
		public static string HeavyFullHashSeedInputStr= "00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00";
		public static string HeavyFullHashInputStr= "01-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-f5-fe-bb-ad-19-86-4a-69-00-b6-ce-84-28-75-11-e6-e7-46-22-9c-e6-23-9f-f3-df-74-81-65-47-78-a4-c4-d3-e1-5d-60-ff-ff-00-1c-07-47-d0-42";
		public static string HeavyFullHashExpectedOutputStr = "00-00-00-00-00-11-5C-7A-7E-3F-F6-5D-77-EE-96-DE-52-79-53-CA-6E-43-E7-72-46-92-97-41-40-8F-95-C0";

        public static HeavyHash Hasher = new HeavyHash();

		public static byte[] GetBytesFromString(string bytesString, Boolean IsLittleEndian){
			byte[] bytes = bytesString.Split('-').Select(b => Convert.ToByte(b, 16)).ToArray();
			if (IsLittleEndian){
				Array.Reverse(bytes);
			}
			return bytes;
		}

		public static Boolean TestSha3(){
			Console.WriteLine("TESTING SHA3:");
			byte[] input = GetBytesFromString(Sha3InputStr, IsLittleEndian: false);

			Console.Write("\tInput:\t\t\t");
			Console.WriteLine(BitConverter.ToString(input));

			byte[] output = Hasher.GetSha3(input);

			Console.Write("\tOutput:\t\t\t");
			Console.WriteLine(BitConverter.ToString(output));

			byte[] expectedOutput = GetBytesFromString(Sha3ExpectedOutputStr, IsLittleEndian: true);

			Console.Write("\tExpected Output:\t");
			Console.WriteLine(BitConverter.ToString(expectedOutput));

			Console.Write("\tIs Equal:\t\t");
        	Console.WriteLine(output.SequenceEqual(expectedOutput));
			return true;
		}

		public static Boolean TestHeavyHash(){
			Console.WriteLine("\nTESTING HeavyHash with fixed Matrix:");
			byte[] input = GetBytesFromString(HeavyHashInputStr, IsLittleEndian: false);
			ulong[,] inputMatrix = HeavyHashMatrixTests.RefMatrix;

			Console.Write("\tInput:\t\t\t");
			Console.WriteLine(BitConverter.ToString(input));

			byte[] output = Hasher.GetHash(input, inputMatrix);

			Console.Write("\tOutput:\t\t\t");
			Console.WriteLine(BitConverter.ToString(output));

			byte[] expectedOutput = GetBytesFromString(HeavyHashExpectedOutputStr, IsLittleEndian: false);

			Console.Write("\tExpected Output:\t");
			Console.WriteLine(BitConverter.ToString(expectedOutput));

			Console.Write("\tIs Equal:\t\t");
        	Console.WriteLine(output.SequenceEqual(expectedOutput));
			return true;
		}

		public static Boolean TestHeavyHashFull(){
			Console.WriteLine("\nTESTING FULL HeavyHash:");
			byte[] input = GetBytesFromString(HeavyFullHashInputStr, IsLittleEndian: false);
			byte[] seedInput = GetBytesFromString(HeavyFullHashSeedInputStr, IsLittleEndian: false);
			byte[] seedBytes = Hasher.GetSha3(seedInput);
			uint256 seed = new uint256(seedBytes);


			HeavyHashMatrix hhObj = new HeavyHashMatrix(seed);
			ulong[,] inputMatrix = hhObj.Body;

			Console.Write("\tInput:\t\t\t");
			Console.WriteLine(BitConverter.ToString(input));

			byte[] output = Hasher.GetHash(input, inputMatrix);

			Console.Write("\tOutput:\t\t\t");
			Console.WriteLine(BitConverter.ToString(output));

			byte[] expectedOutput = GetBytesFromString(HeavyFullHashExpectedOutputStr, IsLittleEndian: true);

			Console.Write("\tExpected Output:\t");
			Console.WriteLine(BitConverter.ToString(expectedOutput));

			Console.Write("\tIs Equal:\t\t");
        	Console.WriteLine(output.SequenceEqual(expectedOutput));
			return true;
		}

    }
}