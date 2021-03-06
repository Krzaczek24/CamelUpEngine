using System;
using System.Collections.Generic;
using System.Linq;

namespace CamelUpEngine.Extensions
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// Returns elements from a sequence as long as a specified condition is false
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <param name="inclusive">If set to <c>true</c> then returns also first element at which the test no longer passes</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains the elements from the input sequence that occur before the element at which the test passes</returns>
        public static IEnumerable<T> TakeUntil<T>(this IEnumerable<T> source, Func<T, bool> predicate, bool inclusive = false)
        {
            var result = source.TakeWhile(item => !predicate(item)).ToList();

            if (inclusive)
            {
                T item = source.FirstOrDefault(predicate);
                if (item != null)
                {
                    result.Add(item);
                }
            }

            return result;
        }
    }
}
