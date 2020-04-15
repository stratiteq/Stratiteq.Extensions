// Copyright (c) Stratiteq Sweden AB. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;

namespace Stratiteq.Extensions.Configuration
{
    public static class ConfigurationExtensions
    {
        public static T GetValid<T>(this IConfiguration configuration)
        {
            var obj = configuration.Get<T>(c => c.BindNonPublicProperties = true);
            Validator.ValidateObject(obj, new ValidationContext(obj), true);
            return obj;
        }
    }
}
