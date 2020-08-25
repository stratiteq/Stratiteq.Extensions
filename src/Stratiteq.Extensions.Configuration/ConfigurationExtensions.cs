// Copyright (c) Stratiteq Sweden AB. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Configuration;
using System;
using System.ComponentModel.DataAnnotations;

namespace Stratiteq.Extensions.Configuration
{
    public static class ConfigurationExtensions
    {
        public static Uri GetValidUri(this IConfiguration configuration, string key)
        {
            var value = configuration.GetValidValue<string>(key);

            if (!string.IsNullOrEmpty(value) && Uri.IsWellFormedUriString(value, UriKind.Absolute))
            {
                return new Uri(value);
            }

            throw new ValidationException("Uri is not well formed");
        }

        public static T GetValidValue<T>(this IConfiguration configuration, string key)
        {
            var value = configuration.GetValue<T>(key) ?? throw new ValidationException($"Configuration key: {key} that is required for the application to start is missing.");
            Validator.ValidateObject(value, new ValidationContext(value), true);
            return value;
        }

        public static T GetValid<T>(this IConfiguration configuration)
        {
            var obj = configuration.Get<T>(c => c.BindNonPublicProperties = true);
            Validator.ValidateObject(obj, new ValidationContext(obj), true);
            return obj;
        }

        public static T GetValid<T>(this IConfiguration configuration, string appIdentifier)
            where T : AzureADConfiguration
        {
            var obj = configuration.Get<T>(c => c.BindNonPublicProperties = true);
            obj.AppIdentifier = appIdentifier;
            // The .default scope is a built-in scope for every application that refers to the static list of permissions configured on the application registration.
            obj.Scopes = new string[] { $"{appIdentifier}/.default" };

            Validator.ValidateObject(obj, new ValidationContext(obj), true);
            return obj;
        }
    }
}
