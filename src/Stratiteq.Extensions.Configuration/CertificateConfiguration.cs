// Copyright (c) Stratiteq Sweden AB. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Stratiteq.Extensions
{
    /// <summary>
    /// Contains the information needed to make authenticated requests to a web API protected with Azure Active Directory (and role based authentication) using certificates.
    /// </summary>
    public class CertificateConfiguration : IValidatableObject
    {
        private const string MissingAppSettingTemplate = "Configuration not valid. App setting \"{0}\" that is required for the application to start is missing.";

        /// <summary>
        /// Gets the subject name of the certificate that will be loaded and passed along the request to Azure Active Directory (AAD) to get an authentication token.
        /// The certificate (without the private key, .cer format) must be uploaded to the AAD application itself so that it can verify the certificate.
        /// The certificate (with the private key, pfx-format) must be uploaded to the web application host (App service or Azure Function).
        /// </summary>
        public string? CertificateSubjectName { get; internal set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errors = new List<ValidationResult>();

            if (string.IsNullOrEmpty(CertificateSubjectName))
            {
                errors.Add(new ValidationResult(string.Format(MissingAppSettingTemplate, "CertificateSubjectName")));
            }

            return errors;
        }
    }
}
