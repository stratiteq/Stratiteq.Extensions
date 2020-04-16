// Copyright (c) Stratiteq Sweden AB. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Stratiteq.Extensions.Identity.Abstractions;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Stratiteq.Identity.Client
{
    /// <summary>
    /// A delegating handler that authenticates with an IIdentityClient.
    /// </summary>
    public class AuthenticatingDelegatingHandler<T> : DelegatingHandler
        where T : IIdentityClient
    {
        private readonly IIdentityClient identityClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticatingDelegatingHandler{T}"/> class.
        /// </summary>
        /// <param name="identityClient">An implementation of IIdentityClient.</param>
        public AuthenticatingDelegatingHandler(T identityClient)
        {
            this.identityClient = identityClient ?? throw new ArgumentNullException(nameof(identityClient));
        }

        /// <inheritdoc/>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string? accessToken = await this.identityClient.RequestTokenAsync();

            if (string.IsNullOrEmpty(accessToken))
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Could not get access token from endpoint."),
                };
            }

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
