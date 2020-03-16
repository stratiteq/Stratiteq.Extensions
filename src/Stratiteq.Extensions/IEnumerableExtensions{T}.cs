// Copyright (c) Stratiteq Sweden AB. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Stratiteq.Extensions
{
    public static partial class IEnumerableExtensions
    {
        /// <summary>
        /// Check if all items are equal.
        /// </summary>
        /// <typeparam name="T">Type of elements.</typeparam>
        /// <param name="values">Collection to iterate.</param>
        public static bool AllEqual<T>(this IEnumerable<T> values)
            where T : class
        {
            if (!values.Any())
            {
                return true;
            }

            var first = values.First();
            return values.Skip(1).All(v => first.Equals(v));
        }

        /// <summary>
        /// Check if all items are equal.
        /// </summary>
        /// <typeparam name="T">Type of elements.</typeparam>
        /// <param name="values">Collection to iterate.</param>
        /// <param name="value">The value to check if all are equal to.</param>
        public static bool AllEqual<T>(this IEnumerable<T> values, T value)
            where T : class
        {
            if (!values.Any())
            {
                return true;
            }

            return values.All(v => value.Equals(v));
        }

        /// <summary>
        /// Apply an action to each element.
        /// </summary>
        /// <typeparam name="T">Type of elements.</typeparam>
        /// <param name="enumerable">Collection to iterate.</param>
        /// <param name="action">Action to apply on each element.</param>
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var item in enumerable)
            {
                action(item);
            }
        }
    }
}
