using System.Linq;
using System.LongGuid.Validation;
using System.Security;
using JetBrains.Annotations;

namespace System.LongGuid
{
    /// <summary>
    /// Based on Rubbishsoft.LongGuid (https://www.nuget.org/packages/Rubbishsoft.LongGuid)
    /// </summary>
    public struct LongGuid
    {
        private const int LongGuidStringLength = 4 * (32 + 4) + 3;

        private readonly Guid _guid1;
        private readonly Guid _guid2;
        private readonly Guid _guid3;
        private readonly Guid _guid4;

        /// <summary>
        /// A read-only instance of the System.LongGuid structure whose value is all zeros.
        /// </summary>
        [PublicAPI]
        public static LongGuid Empty => new LongGuid();

        /// <summary>
        /// Initializes a new instance of the System.LongGuid structure.
        /// </summary>
        /// <returns>A new System.LongGuid object.</returns>
        [PublicAPI]
        public static LongGuid NewLongGuid()
        {
            return new LongGuid(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LongGuid"/> struct.
        /// </summary>
        /// <param name="guid1">The guid1.</param>
        /// <param name="guid2">The guid2.</param>
        /// <param name="guid3">The guid3.</param>
        /// <param name="guid4">The guid4.</param>
        [PublicAPI]
        public LongGuid(Guid guid1, Guid guid2, Guid guid3, Guid guid4)
        {
            _guid1 = guid1;
            _guid2 = guid2;
            _guid3 = guid3;
            _guid4 = guid4;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LongGuid"/> struct.
        /// </summary>
        /// <param name="byteArray">The byte array.</param>
        [PublicAPI]
        public LongGuid(byte[] byteArray)
        {
            Check.HasNoNulls(byteArray, nameof(byteArray));
            Check.Condition(byteArray, IsValidLength, nameof(byteArray));

            _guid1 = new Guid(byteArray.Skip(0).Take(16).ToArray());
            _guid2 = new Guid(byteArray.Skip(16).Take(16).ToArray());
            _guid3 = new Guid(byteArray.Skip(32).Take(16).ToArray());
            _guid4 = new Guid(byteArray.Skip(48).Take(16).ToArray());
        }

        /// <summary>
        /// Returns a 64-element byte array that contains the value of this instance.
        /// </summary>
        /// <returns>A 64-element byte array.</returns>
        [PublicAPI]
        public byte[] ToByteArray()
        {
            var numArray = new byte[64];

            byte[] byteArray1 = _guid1.ToByteArray();
            byte[] byteArray2 = _guid2.ToByteArray();
            byte[] byteArray3 = _guid3.ToByteArray();
            byte[] byteArray4 = _guid4.ToByteArray();

            Array.ConstrainedCopy(byteArray1, 0, numArray, 0, 16);
            Array.ConstrainedCopy(byteArray2, 0, numArray, 16, 16);
            Array.ConstrainedCopy(byteArray3, 0, numArray, 32, 16);
            Array.ConstrainedCopy(byteArray4, 0, numArray, 48, 16);
            return numArray;
        }

        /// <summary>
        /// Converts the string representation of a GUID to the equivalent <see cref="LongGuid"/> structure.
        /// </summary>
        /// <param name="input">The LongGuid string to convert.</param>
        /// <returns>A structure that contains the value that was parsed.</returns>
        [PublicAPI]
        public static LongGuid Parse(string input)
        {
            Check.NotNullOrEmpty(input, nameof(input));
            Check.Condition(input, IsValidLength, nameof(input));

            return new LongGuid(
                Guid.Parse(input.Substring(0, 36)),
                Guid.Parse(input.Substring(36 + 1, 36)),
                Guid.Parse(input.Substring(72 + 2, 36)),
                Guid.Parse(input.Substring(108 + 3, 36))
            );
        }

        /// <summary>
        /// Converts the string representation of a GUID to the equivalent <see cref="LongGuid"/> structure.
        /// </summary>
        /// <param name="input">The LongGuid string to convert.</param>
        /// <param name="result">The structure that will contain the parsed value. If the method returns true, result contains a valid System.LongGuid. If the method returns false, result equals <see cref="LongGuid.Empty"/>.</param>
        /// <returns>true if the parse operation was successful; otherwise, false.</returns>
        [PublicAPI]
        public static bool TryParse(string input, out LongGuid result)
        {
            result = Empty;
            if (input == null || !IsValidLength(input))
            {
                return false;
            }

            if (!(
                Guid.TryParse(input.Substring(0, 36), out Guid guid1) &
                Guid.TryParse(input.Substring(36 + 1, 36), out Guid guid2) &
                Guid.TryParse(input.Substring(72 + 2, 36), out Guid guid3) &
                Guid.TryParse(input.Substring(108 + 3, 36), out Guid guid4)))
            {
                return false;
            }

            result = new LongGuid(guid1, guid2, guid3, guid4);
            return true;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        /// <exception cref="SecurityException">Calculating a hash code for a LongGuid is insecure because the number of bits are drastically reduced from 512 to 32 - which could result in collisions.</exception>
        public override int GetHashCode()
        {
            throw new SecurityException("Calculating a hash code for a LongGuid is insecure because the number of bits are drastically reduced from 512 to 32 - which could result in collisions.");
        }

        /// <summary>
        /// Returns a string representation of the value of this instance in registry format.
        /// </summary>
        /// <returns>
        /// The value of this <see cref="LongGuid"/>, formatted by using the "D" format specifier as follows:
        /// xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx-xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx-xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx-xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
        /// where the value of the LongGuid is represented as a series of lowercase hexadecimal digits in 4 groups of 8, 4, 4, 4, and 12 digits and separated by hyphens. 
        /// </returns>
        public override string ToString()
        {
            return $"{_guid1}-{_guid2}-{_guid3}-{_guid4}";
        }

        private static bool IsValidLength(string input)
        {
            return input.Length == LongGuidStringLength;
        }

        private static bool IsValidLength(byte[] bytes)
        {
            return bytes.Length == 64;
        }
    }
}