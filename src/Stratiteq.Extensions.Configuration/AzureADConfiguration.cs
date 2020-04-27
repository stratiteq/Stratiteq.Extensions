// Copyright (c) Stratiteq Sweden AB. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Stratiteq.Extensions.Configuration
{
    /// <summary>
    /// Contains the information needed to make authenticated requests to a resource protected with Azure Active Directory (and role based authentication).
    /// </summary>
    public class AzureADConfiguration : BaseConfidentialClientConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AzureADConfiguration"/> class.
        /// </summary>
        public AzureADConfiguration()
        {
        }

        /// <summary>
        /// Gets the Azure AD instance (https://login.microsoftonline.com/).
        /// </summary>
        public string Instance => "https://login.microsoftonline.com/";
    }
}
