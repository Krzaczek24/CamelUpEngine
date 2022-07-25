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

        /// <summary>
        /// Return random element from a sequence
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T GetRandom<T>(this IEnumerable<T> source) => source.ElementAt(new Random().Next(source.Count()));

        /// <summary>
        /// Returns elements from a sequence as strings
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="transformFunction">Optional string transformation function</param>
        /// <returns></returns>
        public static IEnumerable<string> ToStrings<T>(this IEnumerable<T> source, Func<string, string> transformFunction = null)
        {
            var result = source.Select(item => item.ToString());

            if (transformFunction != null)
            {
                result = result.Select(transformFunction);
            }

            return result;
        }
    }
}
