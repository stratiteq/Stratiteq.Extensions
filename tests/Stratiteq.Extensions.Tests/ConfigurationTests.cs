// Copyright (c) Stratiteq Sweden AB. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace Stratiteq.Extensions.Configuration
{
    public class ConfigurationTests
    {
        private IConfiguration configuration = null;

        [SetUp]
        public void SetupFixture()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            this.configuration = builder.Build();
        }

        [Test]
        public void Empty_Configuration_Should_Fail_Validation()
        {
            // Arrange
            var obj = new AzureADConfiguration();

            // Act
            // Assert
            Assert.False(Validator.TryValidateObject(obj, new ValidationContext(obj), null, true));
        }

        [Test]
        public void Creating_CertificateConfiguration_From_AzureADConfiguration_Should_Not_Fail()
        {
            // Arrange
            var azureADConfiguration = new AzureADConfiguration(
                "TestAppIdentifier",
                "TestTenantId",
                "TestClientId");

            // Act
            var certificateConfiguration = new CertificateConfiguration("TestSubjectName", azureADConfiguration);
            var certificateConfiguration2 = new CertificateConfiguration("service2AppIdentifier", certificateConfiguration);

            // Assert
            Assert.NotNull(certificateConfiguration2);
            Assert.NotNull(certificateConfiguration2.AppIdentifier);
            Assert.NotNull(certificateConfiguration2.CertificateSubjectName);
            Assert.NotNull(certificateConfiguration2.ClientId);
            Assert.NotNull(certificateConfiguration2.TenantId);
            Assert.NotNull(certificateConfiguration2.Scopes);
        }

        [Test]
        public void CertificateConfiguration_Should_Validate_Successfully()
        {
            var certificateConfiguration = new CertificateConfiguration(
                "TestSubjectName",
                "TestAppIdentifier",
                "TestTenantId",
                "TestClientId",
                new[] { "test://testscope" });

            Assert.DoesNotThrow(() => Validator.ValidateObject(certificateConfiguration, new ValidationContext(certificateConfiguration)));
        }

        [Test]
        public void Invalid_CertificateConfiguration_Should_Fail_Validation()
        {
            var certificateConfiguration = new CertificateConfiguration(
                string.Empty,
                "TestAppIdentifier",
                "TestTenantId",
                "TestClientId",
                new[] { "TestScope" });

            Assert.Throws<ValidationException>(() => Validator.ValidateObject(certificateConfiguration, new ValidationContext(certificateConfiguration)));
        }

        [Test]
        public void Configuration_With_Missing_AppIdentifier_Should_Fail()
        {
            var azureADConfiguration = new AzureADConfiguration(
                string.Empty,
                "TestTenantId",
                "TestClientId",
                new[] { "TestScope" });

            Assert.Throws<ValidationException>(() => Validator.ValidateObject(azureADConfiguration, new ValidationContext(azureADConfiguration)));
        }

        [Test]
        public void Invalid_CertificateConfiguration_With_Invalid_Scope_Should_Fail_Validation()
        {
            var certificateConfiguration = new CertificateConfiguration(
                "TestSubjectName",
                "TestAppIdentifier",
                "TestTenantId",
                "TestClientId",
                new[] { "TestScope", string.Empty });

            Assert.Throws<ValidationException>(() => Validator.ValidateObject(certificateConfiguration, new ValidationContext(certificateConfiguration)));
        }

        [Test]
        public void Valid_AzureADConfiguration_Should_Validate_Successfully()
        {
            var azureADConfiguration = new AzureADConfiguration(configuration.GetSection("Configuration1"));

            Assert.NotNull(azureADConfiguration);
            Assert.DoesNotThrow(() => Validator.ValidateObject(azureADConfiguration, new ValidationContext(azureADConfiguration), true));
        }

        [Test]
        public void Configuration_With_No_Scopes_Should_Fail_Validation()
        {
            // Arrange
            var azureADConfiguration = new AzureADConfiguration(configuration.GetSection("InvalidConfiguration2"));

            // Act
            var validationResults = new List<ValidationResult>();
            var success = Validator.TryValidateObject(azureADConfiguration, new ValidationContext(azureADConfiguration), validationResults, true);

            // Assert
            Assert.IsFalse(success);
            Assert.NotNull(validationResults);
        }
    }
}
