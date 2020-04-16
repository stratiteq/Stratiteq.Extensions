// Copyright (c) Stratiteq Sweden AB. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;

namespace Stratiteq.Extensions.Identity.Abstractions
{
    /// <summary>
    /// Summary text.
    /// </summary>
    public interface IIdentityClient
    {
        /// <summary>
        /// Request token from the identity client.
        /// </summary>
        /// <returns>A valid access token for the endpoint.</returns>
        Task<string?> RequestTokenAsync();
    }
}
