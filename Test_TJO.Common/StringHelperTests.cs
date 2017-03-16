using System;
using Xunit;
using TJO.Common;

namespace Test_TJO.Common
{
    public class StringHelperTests
    {
        [Fact]
        public void StringHelper_ToCamelCase_with_null_Returns_null()
        {
            // Arrange
            string value = null;
            string expected = null;
            string actual;

            // Act
            actual = value.ToCamelCase();

            // Assert
            
        }
    }
}
