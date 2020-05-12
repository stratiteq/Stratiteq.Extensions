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
    /// If instance created with an CertificateConfiguration the RequestTokenAsync method will use the certificate as
    /// authentication agains the authority else the client secret configuration will be used. Either one has to be provided.
    /// </summary>
    public class AzureADIdentityClientBase : IIdentityClient
    {
        private readonly ClientSecretConfiguration? clientSecretConfiguration;
        private readonly ILogger<AzureADIdentityClientBase> logger;
        private readonly CertificateConfiguration? certificateConfiguration;

        private IConfidentialClientApplication? confidentialClientApplication;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureADIdentityClientBase"/> class.
        /// </summary>
        /// <param name="clientSecretConfiguration">An instance of ClientSecretConfiguration.</param>
        public AzureADIdentityClientBase(ClientSecretConfiguration clientSecretConfiguration, ILogger<AzureADIdentityClientBase> logger)
        {
            this.clientSecretConfiguration = clientSecretConfiguration ?? throw new ArgumentNullException(nameof(clientSecretConfiguration));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureADIdentityClientBase"/> class.
        /// </summary>
        /// <param name="certificateConfiguration">An instance of CertificateConfiguration.</param>
        public AzureADIdentityClientBase(CertificateConfiguration certificateConfiguration, ILogger<AzureADIdentityClientBase> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.certificateConfiguration = certificateConfiguration ?? throw new ArgumentNullException(nameof(certificateConfiguration));
        }

        /// <inheritdoc/>
        public async Task<string?> RequestTokenAsync()
        {
            this.logger?.LogInformation("Requesting token from identity provider.");

            if (this.confidentialClientApplication == null)
            {
                if (this.certificateConfiguration != null)
                {
                    this.confidentialClientApplication = ConfidentialClientApplicationBuilder.Create(this.certificateConfiguration?.ClientId)
                        .WithCertificate(CertificateFinder.FindBySubjectName(this.certificateConfiguration?.CertificateSubjectName, DateTime.UtcNow))
                        .WithAuthority(AzureCloudInstance.AzurePublic, this.certificateConfiguration?.TenantId)
                        .Build();
                }
                else
                {
                    this.confidentialClientApplication = ConfidentialClientApplicationBuilder.Create(this.clientSecretConfiguration?.ClientId)
                        .WithClientSecret(this.clientSecretConfiguration?.ClientSecret)
                        .WithAuthority(AzureCloudInstance.AzurePublic, this.clientSecretConfiguration?.TenantId)
                        .Build();
                }
            }

            AuthenticationResult? result;
            try
            {
                var scopes = this.certificateConfiguration != null ? this.certificateConfiguration.Scopes : this.clientSecretConfiguration?.Scopes;

                result = await this.confidentialClientApplication.AcquireTokenForClient(scopes)
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
