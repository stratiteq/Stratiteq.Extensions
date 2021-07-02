// Copyright (c) Stratiteq Sweden AB. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Stratiteq.Extensions.Configuration
{
    /// <summary>
    /// Contains the information needed to make authenticated requests to a resource protected with Azure Active Directory (and role based authentication).
    /// </summary>
    public class AzureADConfiguration : IValidatableObject
    {
        protected const string MissingAppSettingTemplate = "Configuration not valid. App setting \"{0}\" that is required for the application to start is missing.";

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureADConfiguration"/> class.
        /// </summary>
        public AzureADConfiguration()
        {
        }

        public AzureADConfiguration(IConfiguration configuration)
            : this(
                  configuration["AppIdentifier"],
                  configuration["TenantId"],
                  configuration["ClientId"],
                  configuration.GetSection("Scopes").Get<string[]>())
        {
        }

        public AzureADConfiguration(string? appIdentifier, string? tenantId, string? clientId, string[] scopes = null!)
        {
            this.AppIdentifier = appIdentifier ?? throw new ArgumentNullException(nameof(appIdentifier));
            this.TenantId = tenantId ?? throw new ArgumentNullException(nameof(tenantId));
            this.ClientId = clientId ?? throw new ArgumentNullException(nameof(clientId));
            this.Scopes = scopes ?? new string[] { $"{appIdentifier}/.default" };
        }

        public AzureADConfiguration(AzureADConfiguration azureADConfiguration)
            : this(
                azureADConfiguration.AppIdentifier,
                azureADConfiguration.TenantId,
                azureADConfiguration.ClientId,
                azureADConfiguration.Scopes)
        {
        }

        public AzureADConfiguration(string appIdentifier, AzureADConfiguration azureADConfiguration)
            : this(azureADConfiguration)
        {
            this.AppIdentifier = appIdentifier;
            // The .default scope is a built-in scope for every application that refers to the static list of permissions configured on the application registration.
            this.Scopes = new string[] { $"{appIdentifier}/.default" };
        }

        /// <summary>
        /// Gets the Azure AD instance (https://login.microsoftonline.com/).
        /// </summary>
        public string Instance => "https://login.microsoftonline.com/";

        /// <summary>
        /// Gets the Azure AD token issuer
        /// </summary>
        public string Issuer => this.Instance;

        /// <summary>
        /// Gets the Azure Active Directory (AAD) application identifier of the web API that the calling application needs authenticated access to.
        /// These identifiers can be found in the AAD application settings, and is separate from the client id / application id. It has the form of a URI.
        /// </summary>
        [Required]
        public string? AppIdentifier { get; internal set; }

        /// <summary>
        /// Gets the scopes the application requests.
        /// If no scope/s are provided the default scope is used. The "/.default" scope is a built-in scope for every application that refers to the static list of permissions configured on the application registration.
        /// </summary>
        public string[] Scopes { get; internal set; } = default!;

        /// <summary>
        /// Gets the tenant id of the Azure Active Directory (AAD) that hosts the application that is requesting access to another application.
        /// The tenant id is the id of the AAD-instance. This is always a GUID.
        /// </summary>
        [Required]
        public string? TenantId { get; internal set; }

        /// <summary>
        /// Gets the client id (aka application id) of the Azure Active Directory-application that is requesting access to another application. This is always a GUID.
        /// </summary>
        [Required]
        public string? ClientId { get; internal set; }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.Scopes != null)
            {
                foreach (var scope in this.Scopes)
                {
                    if (UriUtilities.GetValidUri(scope) == null)
                    {
                        yield return new ValidationResult("Configuration contains scopes but they are not in a valid form", new string[] { nameof(this.Scopes) });
                    }
                }
            }
        }
    }
}
