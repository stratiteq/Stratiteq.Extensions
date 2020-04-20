// Copyright (c) Stratiteq Sweden AB. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Linq;

namespace Stratiteq.Extensions.AspNetCore.Swagger
{
    /// <summary>
    /// This class provides extension methods to make it easy to configure oauth2 authentication for SwaggerUI.
    /// </summary>
    public static class SwaggerExtensions
    {
        /// <summary>
        /// Configure SwaggerUIOptions with client id.
        /// </summary>
        /// <param name="swaggerUIOptions">the SwaggerUIOptions instance to extend.</param>
        /// <param name="clientId">The ClientId for oauth2 endpoint.</param>
        public static void ConfigureOauth2Authentication(this SwaggerUIOptions swaggerUIOptions, string clientId)
        {
            if (clientId == null)
            {
                throw new ArgumentNullException(nameof(clientId));
            }

            swaggerUIOptions.OAuthClientId(clientId);
            swaggerUIOptions.OAuthRealm(clientId);
            swaggerUIOptions.OAuthScopeSeparator(" ");
        }

        /// <summary>
        /// Configure SwaggerGenOptions for oauth2 authentication.
        /// </summary>
        /// <param name="swaggerGenOptions">The SwaggerGenOptions instance to extend.</param>
        /// <param name="instance">The oauth2 endpoint.</param>
        /// <param name="clientId">The ClientId for oauth2 endpoint.</param>
        /// <param name="tenantId">The TenantId.</param>
        /// <param name="appIdUri">The globally unique URI used to identify the web API. It is the prefix for scopes and in access tokens, it is the value of the audience claim. Also referred to as an identifier URI.</param>
        /// <param name="requestedScope">The scope that the application requests.</param>
        /// <exception cref="ArgumentNullException">Is thrown if any of the input parameters are null.</exception>
        public static void ConfigureOauth2Authentication(
            this SwaggerGenOptions swaggerGenOptions,
            string instance,
            string clientId,
            string tenantId,
            string appIdUri,
            string requestedScope)
        {
            ConfigureOauth2Authentication(swaggerGenOptions, instance, clientId, tenantId, appIdUri, new string[] { requestedScope });
        }

        /// <summary>
        /// Configure SwaggerGenOptions for oauth2 authentication.
        /// </summary>
        /// <param name="swaggerGenOptions">The SwaggerGenOptions instance to extend.</param>
        /// <param name="instance">The oauth2 endpoint.</param>
        /// <param name="clientId">The ClientId for oauth2 endpoint.</param>
        /// <param name="tenantId">The TenantId.</param>
        /// <param name="appIdUri">The globally unique URI used to identify the web API. It is the prefix for scopes and in access tokens, it is the value of the audience claim. Also referred to as an identifier URI.</param>
        /// <param name="requestedScopes">The scopes that the application requests.</param>
        /// <exception cref="ArgumentNullException">Is thrown if any of the input parameters are null.</exception>
        public static void ConfigureOauth2Authentication(
            this SwaggerGenOptions swaggerGenOptions,
            string instance,
            string clientId,
            string tenantId,
            string appIdUri,
            string[] requestedScopes)
        {
            if (string.IsNullOrEmpty(instance))
            {
                throw new ArgumentNullException(nameof(instance));
            }

            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentNullException(nameof(clientId));
            }

            if (string.IsNullOrEmpty(tenantId))
            {
                throw new ArgumentNullException(nameof(tenantId));
            }

            if (string.IsNullOrEmpty(appIdUri))
            {
                throw new ArgumentNullException(nameof(appIdUri));
            }

            if (requestedScopes == null || requestedScopes.Length == 0)
            {
                throw new ArgumentNullException(nameof(requestedScopes));
            }

            swaggerGenOptions
                .AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri($"{instance}{tenantId}/oauth2/v2.0/authorize", UriKind.Absolute),
                            Scopes = requestedScopes.ToDictionary(x => $"{appIdUri}/{x}", x => x),
                        },
                    },
                });

            swaggerGenOptions
                .AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                    new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "oauth2",
                            },
                        },
                    new[] { $"/{requestedScopes[0]}" }
                    },
                });
        }
    }
}
