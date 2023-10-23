using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Serilog.Events;

namespace Act.Lib
{
    public static class LoggerExtensions
    {


        private static string GetCorrectPropertyName<T>(Expression<Func<T>> expression)
        {
            if (expression.Body is MemberExpression)
                return ((MemberExpression)expression.Body).Member.Name;
            else
                return ((ConstantExpression)expression.Body).Value.ToString();
        }

        public static ILogger AddProperty<T>(this ILogger logger, Expression<Func<T>> expression)
        {
           
            var variableName = GetCorrectPropertyName(expression);
            var valueOriginal = expression.Compile()();

            Type ty = valueOriginal.GetType();
            bool b = valueOriginal.GetType().IsPrimitiveType();


            if (!b)
                if (valueOriginal.GetType().Name == "Dictionary`2" || valueOriginal.GetType().Name == "List`1")
                    return logger.ForContext(variableName, valueOriginal, true);
                else
                    return logger.ForContext(expression.Body.Type.Name + "." + variableName, valueOriginal, true);
           
            else
                return logger.ForContext(variableName, valueOriginal, true);
        }

         //else if (valueOriginal.GetType() == typeof(DateTime) || valueOriginal.GetType() == typeof(TimeSpan))
         //       return logger.ForContext(variableName, valueOriginal);


        //public static void Trace<T>(this ILogger logger, string messageTemplate, Expression<Func<T>> expression, params object[] propertyValuesTrace)
        //{
        //    LogEvent evt = null;

        //    //default message
        //    if (Log.BindMessageTemplate(messageTemplate, Array.Empty<object>(), out var parsedTemplate, out var boundProperties))
        //        evt = new LogEvent(DateTimeOffset.Now, LogEventLevel.Debug, (Exception)null, parsedTemplate, boundProperties);

        //    if (evt != null) throw new ArgumentNullException(nameof(evt));

        //    logger.AddProperty(expression).Write(evt);

        //    //trace message
        //    DateTimeOffset t = evt.Timestamp;

        //    if (Log.BindMessageTemplate("Trace " + messageTemplate, NoPropertyValues, out var parsedTemplate1, out var boundProperties1))
        //    {
        //        evt = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, (Exception)null, parsedTemplate1, boundProperties1);

        //        logger.Write(BindProperty;

        //    }





        //}

        public static void Verbose<T>(this ILogger logger, string messageTemplate, Expression<Func<T>> expression, params object[] propertyValues)
        {

            logger.AddProperty(expression).Verbose(messageTemplate, propertyValues);

        }


        public static void Debug<T>(this ILogger logger, string messageTemplate, Expression<Func<T>> expression, params object[] propertyValues)
        {

            logger.AddProperty(expression).Debug(messageTemplate, propertyValues);

        }

        #region Info
        public static void Information<T>(this ILogger logger, string messageTemplate, Expression<Func<T>> expression)
        {

            logger.AddProperty(expression).Information(messageTemplate);

        }

        public static void Information<T0, T1>(this ILogger logger, string messageTemplate,
            Expression<Func<T0>> expression,
            Expression<Func<T1>> expression1 = null)
        {
            try
            {


                logger.AddProperty(expression).AddProperty(expression1).Information(messageTemplate);

            }
            catch (Exception ex)
            {
                logger.Error(ex, "Alcuni parametri sono nulli");
            }

        }

        public static void Information<T0, T1, T2>(this ILogger logger, string messageTemplate,
            Expression<Func<T0>> expression,
            Expression<Func<T1>> expression1,
            Expression<Func<T2>> expression2)
        {



            logger.AddProperty(expression).AddProperty(expression1).AddProperty(expression2).Information(messageTemplate);





        }
        public static void Information<T0, T1, T2, T3>(this ILogger logger, string messageTemplate,
            Expression<Func<T0>> expression,
            Expression<Func<T1>> expression1,
            Expression<Func<T2>> expression2,
            Expression<Func<T3>> expression3
            )
        {

            logger.AddProperty(expression).AddProperty(expression1).AddProperty(expression2).AddProperty(expression3).Information(messageTemplate);




        }

        #endregion


        #region da capire se tornano utili
        private static readonly string expressionCannotBeNullMessage = "The expression cannot be null.";
        private static readonly string invalidExpressionMessage = "Invalid expression.";
        public static string GetMemberName<T>(this T instance, Expression<Func<T, object>> expression)
        {
            return GetMemberName(expression.Body);
        }
        public static List<string> GetMemberNames<T>(this T instance, params Expression<Func<T, object>>[] expressions)
        {
            List<string> memberNames = new List<string>();
            foreach (var cExpression in expressions)
            {
                memberNames.Add(GetMemberName(cExpression.Body));
            }
            return memberNames;
        }
        public static string GetMemberName<T>(this T instance, Expression<Action<T>> expression)
        {
            return GetMemberName(expression.Body);
        }
        private static string GetMemberName(Expression expression)
        {
            if (expression == null)
            {
                throw new ArgumentException(expressionCannotBeNullMessage);
            }
            if (expression is MemberExpression)
            {
                // Reference type property or field
                var memberExpression = (MemberExpression)expression;
                return memberExpression.Member.Name;
            }
            if (expression is MethodCallExpression)
            {
                // Reference type method
                var methodCallExpression = (MethodCallExpression)expression;
                return methodCallExpression.Method.Name;
            }
            if (expression is UnaryExpression)
            {
                // Property, field of method returning value type
                var unaryExpression = (UnaryExpression)expression;
                return GetMemberName(unaryExpression);
            }
            throw new ArgumentException(invalidExpressionMessage);
        }
        private static string GetMemberName(UnaryExpression unaryExpression)
        {
            if (unaryExpression.Operand is MethodCallExpression)
            {
                var methodExpression = (MethodCallExpression)unaryExpression.Operand;
                return methodExpression.Method.Name;
            }
            return ((MemberExpression)unaryExpression.Operand).Member.Name;
        }
        #endregion
    }

    
}
