// Copyright (c) Stratiteq Sweden AB. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.ObjectModel;

namespace Stratiteq.Extensions.AspNetCore
{
    /// <summary>
    /// Collection of ProblemDetailsWithErrorCode objects. The error codes are guaranteed to be unique within this collection.
    /// </summary>
    public class ProblemDetailsWithErrorCodeCollection : KeyedCollection<string, ProblemDetailsWithErrorCode>
    {
        /// <inheritDoc/>
        protected override string GetKeyForItem(ProblemDetailsWithErrorCode item) => item.ErrorCode;
    }
}
