// Copyright (c) Stratiteq Sweden AB. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Stratiteq.Extensions.Cryptography
{
    /// <summary>
    /// Helps to read a certificate from the CurrentUser-store or the LocalMachine-store based on the certificates subject name.
    /// Certificates that are expired are discarded.
    /// </summary>
    public static class CertificateHelper
    {
        public static X509Certificate2 ReadCertificate(string certificateSubjectName, DateTime utcNow)
        {
            if (string.IsNullOrWhiteSpace(certificateSubjectName))
            {
                throw new System.ArgumentException("subjectName is missing", nameof(certificateSubjectName));
            }

            var certificate =
                ReadCertificate(certificateSubjectName, StoreLocation.CurrentUser, utcNow) ??
                ReadCertificate(certificateSubjectName, StoreLocation.LocalMachine, utcNow);

            if (certificate == null)
            {
                throw new Exception($"Could not find the certificate with subject {certificateSubjectName} in either the CurrentUser or LocalMachine store locations. Please install this certificate on target machine before using it.");
            }

            return certificate;
        }

        public static X509Certificate2 ReadCertificate(string certificateSubjectName, StoreLocation storeLocation, DateTime utcNow)
        {
            X509Certificate2? cert = null;

            using (var store = new X509Store(StoreName.My, storeLocation))
            {
                store.Open(OpenFlags.ReadOnly);
                var certCollection = store.Certificates;

                // Find unexpired certificates.
                var currentCerts = certCollection.Find(X509FindType.FindByTimeValid, utcNow, false);

                // From the collection of unexpired certificates, find the ones with the correct name.
                var signingCert = currentCerts.Find(X509FindType.FindBySubjectName, certificateSubjectName, false);

                // Return the first certificate in the collection, has the right name and is current.
                cert = signingCert.OfType<X509Certificate2>().OrderByDescending(c => c.NotBefore).FirstOrDefault();
            }

            return cert;
        }
    }
}
