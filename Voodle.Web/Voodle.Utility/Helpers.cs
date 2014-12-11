using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Voodle.Web.Utility
{
    public static class Helpers
    {
        public static string CreateRandomPassword(int passwordLength = 5)
        {
            string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
            char[] chars = new char[passwordLength];
            var rnd = new Random();

            for (int i = 0; i < passwordLength; i++)
                chars[i] = allowedChars[rnd.Next(0, allowedChars.Length)];

            return new string(chars);
        }
    }

    public static class ExpressionHelper
    {
        public static string GetPropertyName(LambdaExpression expression)
        {
            var body = (MemberExpression)expression.Body;
            return body.Member.Name;
        }

        public static string GetPropertyName<T>(Expression<Func<T, object>> expression)
        {
            var body = (MemberExpression)expression.Body;
            return body.Member.Name;
        }
    }
}
