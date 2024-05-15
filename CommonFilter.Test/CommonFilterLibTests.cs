using System.Linq.Expressions;

namespace CommonFilter.Test
{
    [TestFixture]
    public class CommonFilterLibTests
    {
        private ParameterExpression _objectParameter;

        [SetUp]
        public void Setup()
        {
            _objectParameter = Expression.Parameter(typeof(SampleClass), "sample");
        }

        [Test]
        public void CreateExpression_Equal_ReturnsCorrectExpression()
        {
            // Arrange
            var value = 5;
            string propertyName = "ValueProperty";
            string operatorType = "=";

            // Act
            var result = CommonFilterLib.CreateExpression(value, null, propertyName, _objectParameter, operatorType);

            // Assert
            Assert.IsInstanceOf<BinaryExpression>(result);
            Assert.AreEqual(ExpressionType.Equal, ((BinaryExpression)result).NodeType);
        }

        [Test]
        public void CreateExpression_GreaterThan_ReturnsCorrectExpression()
        {
            // Arrange
            var value = 5;
            string propertyName = "ValueProperty";
            string operatorType = ">";

            // Act
            var result = CommonFilterLib.CreateExpression(value, null, propertyName, _objectParameter, operatorType);

            // Assert
            Assert.IsInstanceOf<BinaryExpression>(result);
            Assert.AreEqual(ExpressionType.GreaterThan, ((BinaryExpression)result).NodeType);
        }

        // Additional tests for "<", ">=", "<="
        [Test]
        public void CreateExpression_LessThan_ReturnsCorrectExpression()
        {
            // Arrange
            var value = 5;
            string propertyName = "ValueProperty";
            string operatorType = "<";

            // Act
            var result = CommonFilterLib.CreateExpression(value, null, propertyName, _objectParameter, operatorType);

            // Assert
            Assert.IsInstanceOf<BinaryExpression>(result);
            Assert.AreEqual(ExpressionType.LessThan, ((BinaryExpression)result).NodeType);
        }

        [Test]
        public void CreateExpression_GreaterOrEqualThan_ReturnsCorrectExpression()
        {
            // Arrange
            var value = 5;
            string propertyName = "ValueProperty";
            string operatorType = ">=";

            // Act
            var result = CommonFilterLib.CreateExpression(value, null, propertyName, _objectParameter, operatorType);

            // Assert
            Assert.IsInstanceOf<BinaryExpression>(result);
            Assert.AreEqual(ExpressionType.GreaterThanOrEqual, ((BinaryExpression)result).NodeType);
        }

        [Test]
        public void CreateExpression_LessOrEqualThan_ReturnsCorrectExpression()
        {
            // Arrange
            var value = 5;
            string propertyName = "ValueProperty";
            string operatorType = "<=";

            // Act
            var result = CommonFilterLib.CreateExpression(value, null, propertyName, _objectParameter, operatorType);

            // Assert
            Assert.IsInstanceOf<BinaryExpression>(result);
            Assert.AreEqual(ExpressionType.LessThanOrEqual, ((BinaryExpression)result).NodeType);
        }

        [Test]
        public void CreateExpression_CombinesExpressionsWithAnd()
        {
            // Arrange
            var firstExpression = CommonFilterLib.CreateExpression(5, null, "ValueProperty", _objectParameter, "=");
            var secondValue = 10;

            // Act
            var result = CommonFilterLib.CreateExpression(secondValue, firstExpression, "ValueProperty", _objectParameter, ">");

            // Assert
            Assert.IsInstanceOf<BinaryExpression>(result);
            Assert.AreEqual(ExpressionType.And, ((BinaryExpression)result).NodeType);
        }


        [Test]
        public void CreateExpression_WithNullCurrentExpression_HandlesNull()
        {
            var result = CommonFilterLib.CreateExpression(5, null, "ValueProperty", _objectParameter, "=");
            Assert.NotNull(result);
        }

        [Test]
        public void CreateExpression_StringValue_CreatesCorrectExpression()
        {
            var value = "test";
            string propertyName = "StringProperty";
            var result = CommonFilterLib.CreateExpression(value, null, propertyName, _objectParameter, "=");
            Assert.IsInstanceOf<BinaryExpression>(result);
            Assert.AreEqual(ExpressionType.Equal, ((BinaryExpression)result).NodeType);
        }

        [Test]
        public void CreateExpression_DateTimeValue_CreatesCorrectExpression()
        {
            var value = DateTime.Now;
            string propertyName = "DateProperty";
            var result = CommonFilterLib.CreateExpression(value, null, propertyName, _objectParameter, ">");
            Assert.IsInstanceOf<BinaryExpression>(result);
            Assert.AreEqual(ExpressionType.GreaterThan, ((BinaryExpression)result).NodeType);
        }

        [Test]
        public void CreateExpression_InvalidPropertyName_ThrowsException()
        {
            Assert.Throws<System.ArgumentException>(() =>
                CommonFilterLib.CreateExpression(5, null, "NonexistentProperty", _objectParameter, "=")
            );
        }

        [Test]
        public void CreateExpression_Evaluation_ReturnsExpectedResult()
        {
            var sampleObject = new SampleClass { ValueProperty = 10 };
            var expr = CommonFilterLib.CreateExpression(5, null, "ValueProperty", _objectParameter, ">");
            var lambda = Expression.Lambda<Func<SampleClass, bool>>(expr, _objectParameter).Compile();
            var result = lambda(sampleObject);
            Assert.IsTrue(result);
        }
    }

    public class SampleClass
    {
        public int ValueProperty { get; set; }
        public string StringProperty { get; set; }
        public DateTime DateProperty { get; set; }
    }
}