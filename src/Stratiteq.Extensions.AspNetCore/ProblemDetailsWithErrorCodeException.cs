// Copyright (c) Stratiteq Sweden AB. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Stratiteq.Extensions.AspNetCore
{
    /// <summary>
    /// Exception that contains a ProblemDetailsWithErrorCode object.
    /// </summary>
    public class ProblemDetailsWithErrorCodeException : Exception
    {
        /// <inheritDoc/>
        public ProblemDetailsWithErrorCodeException(ProblemDetailsWithErrorCode problemDetailsWithErrorCode)
        {
            this.ProblemDetailsWithErrorCode = problemDetailsWithErrorCode ?? throw new ArgumentNullException(nameof(problemDetailsWithErrorCode));
        }

        /// <inheritDoc/>
        public ProblemDetailsWithErrorCodeException()
            : base()
        {
        }

        /// <inheritDoc/>
        public ProblemDetailsWithErrorCodeException(string message)
            : base(message)
        {
        }

        /// <inheritDoc/>
        public ProblemDetailsWithErrorCodeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Gets the ProblemDetailsWithErrorCode instance.
        /// </summary>
        public ProblemDetailsWithErrorCode? ProblemDetailsWithErrorCode { get; }

        /// <summary>
        /// Clears data from the Title and Details fields in <see cref="ProblemDetailsWithErrorCode" />.
        /// </summary>
        public void ClearSensitiveInfo()
        {
            this.ProblemDetailsWithErrorCode.Title = null;
            this.ProblemDetailsWithErrorCode.Detail = null;
        }
    }
}
