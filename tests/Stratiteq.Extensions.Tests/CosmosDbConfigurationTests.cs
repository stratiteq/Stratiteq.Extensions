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
    public class CosmosDbConfigurationTests
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
        [TestOf(nameof(CosmosDBConfiguration))]
        public void CosmosDbConfiguration_Should_Validate_Successfully()
        {
            // Arrange
            var cosmosDbConfiguration = new CosmosDBConfiguration
            {
                AccountEndpoint = "accountEndpoint",
                PrimaryKey = "primaryKey",
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var success = Validator.TryValidateObject(cosmosDbConfiguration, new ValidationContext(cosmosDbConfiguration), validationResults, true);

            // Assert
            Assert.IsTrue(success);
            Assert.IsTrue(validationResults.Count == 0);
        }

        [Test]
        [TestOf(nameof(CosmosDBConfiguration))]
        [TestCase("", "")]
        [TestCase("endpoint", "")]
        [TestCase("", "primaryKey")]
        public void Invalid_CosmosDbConfiguration_Should_Fail_Validation(string accountEndpoint, string primaryKey)
        {
            // Arrange
            var cosmosDbConfiguration = new CosmosDBConfiguration
            {
                AccountEndpoint = accountEndpoint,
                PrimaryKey = primaryKey,
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var success = Validator.TryValidateObject(cosmosDbConfiguration, new ValidationContext(cosmosDbConfiguration), validationResults, true);

            // Assert
            Assert.IsFalse(success);
            Assert.IsTrue(validationResults.Count != 0);
        }

        [Test]
        [TestOf(nameof(CosmosDBConfiguration))]
        public void CosmosDbConfiguration_From_Configuration_Should_Validate_Successfully()
        {
            // Arrange
            var validConfiguration =
                this.configuration.GetSection("CosmosDbConfiguration1").GetValid<CosmosDBConfiguration>();

            Assert.IsNotNull(validConfiguration);
        }

        [Test]
        [TestOf(nameof(CosmosDBConfiguration))]
        public void Invalid_CosmosDbConfiguration_From_Configuration_Should_Fail_Validation()
        {
            Assert.Throws<ValidationException>(() =>
                this.configuration.GetSection("InvalidCosmosDbConfiguration1").GetValid<CosmosDBConfiguration>());
        }
    }
}
