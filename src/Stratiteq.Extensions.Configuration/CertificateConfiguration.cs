// Copyright (c) Stratiteq Sweden AB. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Stratiteq.Extensions.Configuration
{
    /// <summary>
    /// Contains the information needed to make authenticated requests to a web API protected with Azure Active Directory (and role based authentication) using certificates.
    /// </summary>
    public class CertificateConfiguration : AzureADConfiguration
    {
        public CertificateConfiguration()
        {
        }

        public CertificateConfiguration(IConfiguration configuration)
            : base(configuration)
        {
            this.CertificateSubjectName = configuration["CertificateSubjectName"];
        }

        public CertificateConfiguration(string? certificateSubjectName, string? appIdentifier, string? tenantId, string? clientId, string?[] scopes)
            : base(appIdentifier, tenantId, clientId, scopes)
        {
            this.CertificateSubjectName = certificateSubjectName ?? throw new System.ArgumentNullException(nameof(certificateSubjectName));
        }

        public CertificateConfiguration(string? certificateSubjectName, string? appIdentifier, AzureADConfiguration azureADConfiguration)
            : this(certificateSubjectName, azureADConfiguration)
        {
            this.AppIdentifier = appIdentifier;
        }

        public CertificateConfiguration(string? certificateSubjectName, AzureADConfiguration? azureADConfiguration)
            : this(
                certificateSubjectName,
                azureADConfiguration?.AppIdentifier,
                azureADConfiguration?.TenantId,
                azureADConfiguration?.ClientId,
                azureADConfiguration?.Scopes!)
        {
        }

        public CertificateConfiguration(string appIdentifier, CertificateConfiguration? certificateConfiguration)
            : this(certificateConfiguration)
        {
            this.AppIdentifier = appIdentifier;
        }

        private CertificateConfiguration(CertificateConfiguration? certificateConfiguration)
            : this(
                certificateConfiguration?.CertificateSubjectName,
                certificateConfiguration?.AppIdentifier,
                certificateConfiguration?.TenantId,
                certificateConfiguration?.ClientId,
                certificateConfiguration?.Scopes!)
        {
        }

        /// <summary>
        /// Gets the subject name of the certificate that will be loaded and passed along the request to Azure Active Directory (AAD) to get an authentication token.
        /// The certificate (without the private key, .cer format) must be uploaded to the AAD application itself so that it can verify the certificate.
        /// The certificate (with the private key, pfx-format) must be uploaded to the web application host (App service or Azure Function).
        /// </summary>
        public string? CertificateSubjectName { get; internal set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errors = base.Validate(validationContext).ToList();

            if (string.IsNullOrEmpty(this.CertificateSubjectName))
            {
                errors.Add(new ValidationResult(string.Format(MissingAppSettingTemplate, "CertificateSubjectName")));
            }

            return errors;
        }
    }
}
