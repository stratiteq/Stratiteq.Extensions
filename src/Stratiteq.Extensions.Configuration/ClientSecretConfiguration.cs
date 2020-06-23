// Copyright (c) Stratiteq Sweden AB. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel.DataAnnotations;

namespace Stratiteq.Extensions.Configuration
{
    public class ClientSecretConfiguration : AzureADConfiguration
    {
        /// <summary>
        /// Gets the secret string that the application uses to prove its identity when requesting a token. Also can be referred to as application password.
        /// </summary>
        [Required]
        public string? ClientSecret { get; internal set; }
    }
}
