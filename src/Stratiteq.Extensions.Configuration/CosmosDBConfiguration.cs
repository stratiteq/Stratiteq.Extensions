// Copyright (c) Stratiteq Sweden AB. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Stratiteq.Extensions.Tests")]

namespace Stratiteq.Extensions.Configuration
{
    public class CosmosDBConfiguration
    {
        private const string ConnectionStringTemplate = "AccountEndpoint={0};AccountKey={1}";

        public CosmosDBConfiguration()
        {
        }

        [Required]
        public string? AccountEndpoint { get; internal set; }

        [Required]
        public string? PrimaryKey { get; internal set; }

        public string? ConnectionString => string.Format(ConnectionStringTemplate, this.AccountEndpoint, this.PrimaryKey);
    }
}
