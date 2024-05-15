using System.Linq.Expressions;

namespace CommonFilter
{
    public static class CommonFilterLib
    {
        public static Expression CreateExpression<t>(t value, Expression? currentExpression, string propertyName, ParameterExpression objectParameter, string? operatorType = "=")
        {
            var valueToTest = Expression.Constant(value);
            var propertyToCall = Expression.Property(objectParameter, propertyName);
            Expression operatorExpression;

            switch (operatorType)
            {
                case ">":
                    operatorExpression = Expression.GreaterThan(propertyToCall, valueToTest);
                    break;
                case "<":
                    operatorExpression = Expression.LessThan(propertyToCall, valueToTest);
                    break;
                case ">=":
                    operatorExpression = Expression.GreaterThanOrEqual(propertyToCall, valueToTest);
                    break;
                case "<=":
                    operatorExpression = Expression.LessThanOrEqual(propertyToCall, valueToTest);
                    break;
                default:
                    operatorExpression = Expression.Equal(propertyToCall, valueToTest);
                    break;
            }

            if (currentExpression == null)
            {
                currentExpression = operatorExpression;
            }
            else
            {
                var previousExpression = currentExpression;
                currentExpression = Expression.And(previousExpression, operatorExpression);
            }

            return currentExpression;
        }

    }
}