using System;

namespace Util.Zlib
{
    public class SupportClass
    {
        /// <summary>
        /// This method returns the literal value received
        /// </summary>
        /// <param name="literal">The literal to return</param>
        /// <returns>The received value</returns>
        public static Int64 Identity(Int64 literal)
        {
            return literal;
        }

        /// <summary>
        /// This method returns the literal value received
        /// </summary>
        /// <param name="literal">The literal to return</param>
        /// <returns>The received value</returns>
        public static UInt64 Identity(UInt64 literal)
        {
            return literal;
        }

        /// <summary>
        /// This method returns the literal value received
        /// </summary>
        /// <param name="literal">The literal to return</param>
        /// <returns>The received value</returns>
        public static Single Identity(Single literal)
        {
            return literal;
        }

        /// <summary>
        /// This method returns the literal value received
        /// </summary>
        /// <param name="literal">The literal to return</param>
        /// <returns>The received value</returns>
        public static Double Identity(Double literal)
        {
            return literal;
        }

        /*******************************/
        /// <summary>
        /// Performs an unsigned bitwise right shift with the specified number
        /// </summary>
        /// <param name="number">Number to operate on</param>
        /// <param name="bits">Ammount of bits to shift</param>
        /// <returns>The resulting number from the shift operation</returns>
        public static Int32 URShift(Int32 number, Int32 bits)
        {
            if (number >= 0)
                return number >> bits;
            else
                return (number >> bits) + (2 << ~bits);
        }

        /// <summary>
        /// Performs an unsigned bitwise right shift with the specified number
        /// </summary>
        /// <param name="number">Number to operate on</param>
        /// <param name="bits">Ammount of bits to shift</param>
        /// <returns>The resulting number from the shift operation</returns>
        public static Int32 URShift(Int32 number, Int64 bits)
        {
            return URShift(number, (Int32)bits);
        }

        /// <summary>
        /// Performs an unsigned bitwise right shift with the specified number
        /// </summary>
        /// <param name="number">Number to operate on</param>
        /// <param name="bits">Ammount of bits to shift</param>
        /// <returns>The resulting number from the shift operation</returns>
        public static Int64 URShift(Int64 number, Int32 bits)
        {
            if (number >= 0)
                return number >> bits;
            else
                return (number >> bits) + (2L << ~bits);
        }

        /// <summary>
        /// Performs an unsigned bitwise right shift with the specified number
        /// </summary>
        /// <param name="number">Number to operate on</param>
        /// <param name="bits">Ammount of bits to shift</param>
        /// <returns>The resulting number from the shift operation</returns>
        public static Int64 URShift(Int64 number, Int64 bits)
        {
            return URShift(number, (Int32)bits);
        }

        /*******************************/
        /// <summary>Reads a number of characters from the current source Stream and writes the data to the target array at the specified index.</summary>
        /// <param name="sourceStream">The source Stream to read from.</param>
        /// <param name="target">Contains the array of characteres read from the source Stream.</param>
        /// <param name="start">The starting index of the target array.</param>
        /// <param name="count">The maximum number of characters to read from the source Stream.</param>
        /// <returns>The number of characters read. The number will be less than or equal to count depending on the data available in the source Stream. Returns -1 if the end of the stream is reached.</returns>
        public static System.Int32 ReadInput(System.IO.Stream sourceStream, Byte[] target, Int32 start, Int32 count)
        {
            // Returns 0 Bytes if not enough space in target
            if (target.Length == 0)
                return 0;

            Byte[] receiver = new Byte[target.Length];
            Int32 BytesRead = sourceStream.Read(receiver, start, count);

            // Returns -1 if EOF
            if (BytesRead == 0)
                return -1;

            for (Int32 i = start; i < start + BytesRead; i++)
                target[i] = (Byte)receiver[i];

            return BytesRead;
        }

        /// <summary>Reads a number of characters from the current source TextReader and writes the data to the target array at the specified index.</summary>
        /// <param name="sourceTextReader">The source TextReader to read from</param>
        /// <param name="target">Contains the array of characteres read from the source TextReader.</param>
        /// <param name="start">The starting index of the target array.</param>
        /// <param name="count">The maximum number of characters to read from the source TextReader.</param>
        /// <returns>The number of characters read. The number will be less than or equal to count depending on the data available in the source TextReader. Returns -1 if the end of the stream is reached.</returns>
        public static System.Int32 ReadInput(System.IO.TextReader sourceTextReader, Byte[] target, Int32 start, Int32 count)
        {
            // Returns 0 Bytes if not enough space in target
            if (target.Length == 0) return 0;

            char[] charArray = new char[target.Length];
            Int32 BytesRead = sourceTextReader.Read(charArray, start, count);

            // Returns -1 if EOF
            if (BytesRead == 0) return -1;

            for (Int32 index = start; index < start + BytesRead; index++)
                target[index] = (Byte)charArray[index];

            return BytesRead;
        }

        /// <summary>
        /// Converts a string to an array of Bytes
        /// </summary>
        /// <param name="sourceString">The string to be converted</param>
        /// <returns>The new array of Bytes</returns>
        public static Byte[] ToByteArray(System.String sourceString)
        {
            return System.Text.UTF8Encoding.UTF8.GetBytes(sourceString);
        }

        /// <summary>
        /// Converts an array of Bytes to an array of chars
        /// </summary>
        /// <param name="ByteArray">The array of Bytes to convert</param>
        /// <returns>The new array of chars</returns>
        public static char[] ToCharArray(Byte[] ByteArray)
        {
            return System.Text.UTF8Encoding.UTF8.GetChars(ByteArray);
        }
    }
}