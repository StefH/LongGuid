using System.Security;
using NFluent;
using Xunit;

namespace System.LongGuid.Tests
{
    public class LongGuidTests
    {
        [Fact]
        public void LongGuid_New()
        {
            // Assign
            var longGuid = LongGuid.NewLongGuid();

            // Act
            string result = longGuid.ToString();

            // Assert
            Check.That(result).IsNotEmpty();
        }

        [Fact]
        public void LongGuid_Constructor_ToString()
        {
            // Assign
            var longGuid = new LongGuid();

            // Act
            string result = longGuid.ToString();

            // Assert
            Check.That(result).Equals("00000000-0000-0000-0000-000000000000-00000000-0000-0000-0000-000000000000-00000000-0000-0000-0000-000000000000-00000000-0000-0000-0000-000000000000");
        }

        [Fact]
        public void LongGuid_Constructor_WithGuids_ToString()
        {
            // Assign
            var guid1 = Guid.Parse("10000000-0000-0000-0000-000000000001");
            var guid2 = Guid.Parse("20000000-0000-0000-0000-000000000002");
            var guid3 = Guid.Parse("30000000-0000-0000-0000-000000000003");
            var guid4 = Guid.Parse("40000000-0000-0000-0000-000000000004");
            var longGuid = new LongGuid(guid1, guid2, guid3, guid4);

            // Act
            string result = longGuid.ToString();

            // Assert
            Check.That(result).Equals("10000000-0000-0000-0000-000000000001-20000000-0000-0000-0000-000000000002-30000000-0000-0000-0000-000000000003-40000000-0000-0000-0000-000000000004");
        }

        [Fact]
        public void LongGuid_Constructor_String()
        {
            // Assign
            var longGuid = new LongGuid("10000000-0000-0000-0000-000000000001-20000000-0000-0000-0000-000000000002-30000000-0000-0000-0000-000000000003-40000000-0000-0000-0000-000000000004");

            // Act
            string result = longGuid.ToString();

            // Assert
            Check.That(result).Equals("10000000-0000-0000-0000-000000000001-20000000-0000-0000-0000-000000000002-30000000-0000-0000-0000-000000000003-40000000-0000-0000-0000-000000000004");
        }

        [Fact]
        public void LongGuid_ToByteArray()
        {
            // Assign
            var guid1 = Guid.Parse("10000000-0000-0000-0000-000000000001");
            var guid2 = Guid.Parse("20000000-0000-0000-0000-000000000002");
            var guid3 = Guid.Parse("30000000-0000-0000-0000-000000000003");
            var guid4 = Guid.Parse("40000000-0000-0000-0000-000000000004");
            var longGuid = new LongGuid(guid1, guid2, guid3, guid4);

            // Act
            byte[] result = longGuid.ToByteArray();

            // Assert
            Check.That(result).HasSize(64);

            var longGuid2 = new LongGuid(result);
            Check.That(longGuid).Equals(longGuid2);
        }

        [Fact]
        public void LongGuid_Parse_String()
        {
            // Assign
            var guid1 = Guid.Parse("10000000-0000-0000-0000-000000000001");
            var guid2 = Guid.Parse("20000000-0000-0000-0000-000000000002");
            var guid3 = Guid.Parse("30000000-0000-0000-0000-000000000003");
            var guid4 = Guid.Parse("40000000-0000-0000-0000-000000000004");
            var longGuid = new LongGuid(guid1, guid2, guid3, guid4);

            // Act
            var longGuidParsed = LongGuid.Parse("10000000-0000-0000-0000-000000000001-20000000-0000-0000-0000-000000000002-30000000-0000-0000-0000-000000000003-40000000-0000-0000-0000-000000000004");

            // Assert
            Check.That(longGuid).Equals(longGuidParsed);
        }

        [Fact]
        public void LongGuid_TryParse_True()
        {
            // Assign
            var guid1 = Guid.Parse("10000000-0000-0000-0000-000000000001");
            var guid2 = Guid.Parse("20000000-0000-0000-0000-000000000002");
            var guid3 = Guid.Parse("30000000-0000-0000-0000-000000000003");
            var guid4 = Guid.Parse("40000000-0000-0000-0000-000000000004");
            var longGuid = new LongGuid(guid1, guid2, guid3, guid4);

            // Act
            bool result = LongGuid.TryParse("10000000-0000-0000-0000-000000000001-20000000-0000-0000-0000-000000000002-30000000-0000-0000-0000-000000000003-40000000-0000-0000-0000-000000000004", out LongGuid parsed);

            // Assert
            Check.That(result).IsTrue();
            Check.That(parsed).Equals(longGuid);
        }

        [Fact]
        public void LongGuid_TryParse_InvalidLength_False()
        {
            // Act
            bool result = LongGuid.TryParse("0", out LongGuid parsed);

            // Assert
            Check.That(result).IsFalse();
            Check.That(parsed).Equals(LongGuid.Empty);
        }

        [Fact]
        public void LongGuid_TryParse_InvalidContent_False()
        {
            // Act
            bool result = LongGuid.TryParse("test0000-0000-0000-0000-000000000001-20000000-0000-0000-0000-000000000002-30000000-0000-0000-0000-000000000003-40000000-0000-0000-0000-000000000004", out LongGuid parsed);

            // Assert
            Check.That(result).IsFalse();
            Check.That(parsed).Equals(LongGuid.Empty);
        }

        [Fact]
        public void LongGuid_GetHashCode_Throws()
        {
            // Act
            var result = LongGuid.NewLongGuid();

            // Assert
            Check.ThatCode(() => result.GetHashCode()).Throws<SecurityException>();
        }
    }
}