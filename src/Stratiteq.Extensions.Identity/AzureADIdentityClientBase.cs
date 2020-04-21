// Copyright (c) Stratiteq Sweden AB. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Stratiteq.Extensions.Configuration;
using Stratiteq.Extensions.Cryptography;
using Stratiteq.Extensions.Identity.Abstractions;
using System;
using System.Threading.Tasks;

namespace Stratiteq.Extensions.Identity
{
    /// <summary>
    /// An IIdentityClient implementation using Microsoft.Identity.Client to authenticate against.
    /// </summary>
    public class AzureADIdentityClientBase : IIdentityClient
    {
        private readonly AzureADConfiguration azureADConfiguration;
        private readonly ILogger<AzureADIdentityClientBase> logger;
        private IConfidentialClientApplication? confidentialClientApplication;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureADIdentityClientBase"/> class.
        /// </summary>
        public AzureADIdentityClientBase(AzureADConfiguration azureADConfiguration, ILogger<AzureADIdentityClientBase> logger)
        {
            this.azureADConfiguration = azureADConfiguration ?? throw new ArgumentNullException(nameof(azureADConfiguration));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        public async Task<string?> RequestTokenAsync()
        {
            this.logger?.LogInformation("Requesting token from identity provider.");

            if (this.confidentialClientApplication == null)
            {
                if (!string.IsNullOrEmpty(this.azureADConfiguration.CertificateSubjectName))
                {
                    this.confidentialClientApplication = ConfidentialClientApplicationBuilder.Create(this.azureADConfiguration.ClientId)
                        .WithCertificate(CertificateFinder.FindBySubjectName(this.azureADConfiguration.CertificateSubjectName, DateTime.UtcNow))
                        .WithAuthority(AzureCloudInstance.AzurePublic, this.azureADConfiguration?.TenantId)
                        .Build();
                }
                else
                {
                    this.confidentialClientApplication = ConfidentialClientApplicationBuilder.Create(this.azureADConfiguration.ClientId)
                        .WithClientSecret(this.azureADConfiguration.ClientSecret)
                        .WithAuthority(AzureCloudInstance.AzurePublic, this.azureADConfiguration.TenantId)
                        .Build();
                }
            }

            AuthenticationResult? result;
            try
            {
                result = await this.confidentialClientApplication.AcquireTokenForClient(this.azureADConfiguration?.Scopes)
                    .ExecuteAsync();

                this.logger?.LogInformation("Token requested successfully.");
                this.logger?.LogDebug($"Access token: {result.AccessToken}");
                this.logger?.LogDebug($"Expires on  : {result.ExpiresOn}");
                this.logger?.LogDebug($"Scopes      : {string.Join(";", result.Scopes)}");
            }
            catch (MsalException e)
            {
                this.logger?.LogError(e, "Requesting token failed");
                throw;
            }

            return result.AccessToken;
        }
    }
}
