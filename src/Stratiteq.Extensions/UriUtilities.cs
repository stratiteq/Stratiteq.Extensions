// Copyright (c) Stratiteq Sweden AB. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Stratiteq.Extensions
{
    public static class UriUtilities
    {
        public static Uri? GetValidUri(string? value)
        {
            if (!string.IsNullOrEmpty(value) && Uri.IsWellFormedUriString(value, UriKind.Absolute))
            {
                return new Uri(value);
            }

            return null;
        }
    }
}
