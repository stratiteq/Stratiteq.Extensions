// Copyright (c) Stratiteq Sweden AB. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Mvc;
using System;

namespace Stratiteq.Extensions.AspNetCore
{
    /// <summary>
    /// Extends the ProblemDetails class with an error code.
    /// </summary>
    public class ProblemDetailsWithErrorCode : ProblemDetails
    {
        /// <inheritDoc/>
        /// <summary>
        /// Initializes a new instance of the <see cref="ProblemDetailsWithErrorCode"/> class.
        /// </summary>
        public ProblemDetailsWithErrorCode(string errorCode, string title)
        {
            this.ErrorCode = errorCode ?? throw new ArgumentNullException(nameof(errorCode));
            this.Title = title;
        }

        /// <summary>
        /// Gets the unique code for identifying the specific error within this API.
        /// </summary>
        public string ErrorCode { get; }

        /// <summary>
        /// Creates a ProblemDetailsWithErrorCodeException.
        /// </summary>
        public ProblemDetailsWithErrorCodeException CreateException() => new ProblemDetailsWithErrorCodeException(this);
    }
}
