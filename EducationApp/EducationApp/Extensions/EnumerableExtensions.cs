using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace EducationApp.Extensions
{
    public static class EnumerableExtensions
    {
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public static void ForEach<T>(this IEnumerable<T> ienumerable, Action<T> action)
        {
            foreach (var obj in ienumerable)
            {
                action.Invoke(obj);
            }
        }

        /// <remarks>
        ///     <see cref="http://stackoverflow.com/a/5075572/1592358" />
        /// </remarks>
        public static void SetCountProperty<T1>(this IEnumerable<T1> enumerable, Expression<Func<T1, byte>> selector)
        {
            byte count = 0;
            foreach (var obj in enumerable)
            {
                var prop = (PropertyInfo) ((MemberExpression) selector.Body).Member;
                prop.SetValue(obj, count, null);
                count++;
            }
        }
    }
}