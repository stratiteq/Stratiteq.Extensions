// Copyright (c) Stratiteq Sweden AB. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using NUnit.Framework;
using System.ComponentModel.DataAnnotations;

namespace Stratiteq.Extensions.Configuration
{
    public class ConfigurationTests
    {
        [Test]
        public void Verify_No_Configuration_Fails_Validation()
        {
            // Arrange
            BaseConfidentialClientConfiguration obj = new AzureADConfiguration();

            // Act
            // Assert
            Assert.False(Validator.TryValidateObject(obj, new ValidationContext(obj), null, true));
        }
    }
}
