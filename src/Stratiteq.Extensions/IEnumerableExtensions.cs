// Copyright (c) Stratiteq Sweden AB. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;

namespace Stratiteq.Extensions
{
    public static partial class IEnumerableExtensions
    {
        /// <summary>
        /// Check if all items are equal. Non-generic version.
        /// </summary>
        /// <param name="values">Collection to iterate.</param>
        /// <param name="value">The value to check if all are equal to.</param>
        public static bool AllEqual(this IEnumerable<int> values, int value)
        {
            if (!values.Any())
            {
                return true;
            }

            return values.All(v => value.Equals(v));
        }
    }
}
