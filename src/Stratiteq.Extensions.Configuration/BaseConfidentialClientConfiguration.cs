// Copyright (c) Stratiteq Sweden AB. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Stratiteq.Extensions.Configuration
{
    public abstract class BaseConfidentialClientConfiguration : IValidatableObject
    {
        protected const string MissingAppSettingTemplate = "Configuration not valid. App setting \"{0}\" that is required for the application to start is missing.";

        /// <summary>
        /// Gets the Azure Active Directory (AAD) application identifier of the web API that the calling application needs authenticated access to.
        /// These identifiers can be found in the AAD application settings, and is separate from the client id / application id. It has the form of a URI.
        /// </summary>
        public string? AppIdentifier { get; internal set; }

        /// <summary>
        /// Gets the scopes the application requests.
        /// </summary>
        public string?[] Scopes { get; internal set; } = default!;

        /// <summary>
        /// Gets the tenant id of the Azure Active Directory (AAD) that hosts the application that is requesting access to another application.
        /// The tenant id is the id of the AAD-instance. This is always a GUID.
        /// </summary>
        public string? TenantId { get; internal set; }

        /// <summary>
        /// Gets the client id (aka application id) of the Azure Active Directory-application that is requesting access to another application. This is always a GUID.
        /// </summary>
        public string? ClientId { get; internal set; }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errors = new List<ValidationResult>();

            if (string.IsNullOrEmpty(this.ClientId))
            {
                errors.Add(new ValidationResult(string.Format(MissingAppSettingTemplate, nameof(this.ClientId))));
            }

            if (string.IsNullOrEmpty(this.TenantId))
            {
                errors.Add(new ValidationResult(string.Format(MissingAppSettingTemplate, nameof(this.TenantId))));
            }

            if (string.IsNullOrEmpty(this.AppIdentifier))
            {
                errors.Add(new ValidationResult(string.Format(MissingAppSettingTemplate, nameof(this.AppIdentifier))));
            }

            return errors;
        }
    }
}
