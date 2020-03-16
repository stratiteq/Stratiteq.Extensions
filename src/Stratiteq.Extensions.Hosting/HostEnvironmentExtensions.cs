// Copyright (c) Stratiteq Sweden AB. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.Hosting;

namespace Stratiteq.Extensions.Hosting
{
    /// <summary>
    /// Extension methods for <see cref="IHostEnvironment"/>.
    /// </summary>
    public static class HostEnvironmentExtensions
    {
        /// <summary>
        /// Checks if the current hosting environment name is <see cref="EnvironmentNames.LocalDevelopment"/>.
        /// </summary>
        /// <param name="hostEnvironment">An instance of <see cref="IHostEnvironment"/>.</param>
        /// <returns>True if the environment name is <see cref="EnvironmentNames.LocalDevelopment"/>, otherwise false.</returns>
        public static bool IsLocalDevelopment(this IHostEnvironment hostEnvironment) =>
            hostEnvironment?.IsEnvironment(EnvironmentNames.LocalDevelopment) ?? throw new ArgumentNullException(nameof(hostEnvironment));

        /// <summary>
        /// Checks if the current hosting environment name is <see cref="EnvironmentNames.Test"/>.
        /// </summary>
        /// <param name="hostEnvironment">An instance of <see cref="IHostEnvironment"/>.</param>
        /// <returns>True if the environment name is <see cref="EnvironmentNames.Test"/>, otherwise false.</returns>
        public static bool IsTest(this IHostEnvironment hostEnvironment) =>
            hostEnvironment?.IsEnvironment(EnvironmentNames.Test) ?? throw new ArgumentNullException(nameof(hostEnvironment));

        /// <summary>
        /// Checks if the current hosting environment name is <see cref="EnvironmentNames.Reference"/>.
        /// </summary>
        /// <param name="hostEnvironment">An instance of <see cref="IHostEnvironment"/>.</param>
        /// <returns>True if the environment name is <see cref="EnvironmentNames.Reference"/>, otherwise false.</returns>
        public static bool IsReference(this IHostEnvironment hostEnvironment) =>
            hostEnvironment?.IsEnvironment(EnvironmentNames.Reference) ?? throw new ArgumentNullException(nameof(hostEnvironment));

        /// <summary>
        /// Checks if the current hosting environment name is <see cref="EnvironmentNames.Integration"/>.
        /// </summary>
        /// <param name="hostEnvironment">An instance of <see cref="IHostEnvironment"/>.</param>
        /// <returns>True if the environment name is <see cref="EnvironmentNames.Integration"/>, otherwise false.</returns>
        public static bool IsIntegration(this IHostEnvironment hostEnvironment) =>
            hostEnvironment?.IsEnvironment(EnvironmentNames.Integration) ?? throw new ArgumentNullException(nameof(hostEnvironment));

        /// <summary>
        /// Checks if the current hosting environment name is <see cref="EnvironmentNames.AcceptanceTest"/>.
        /// </summary>
        /// <param name="hostEnvironment">An instance of <see cref="IHostEnvironment"/>.</param>
        /// <returns>True if the environment name is <see cref="EnvironmentNames.AcceptanceTest"/>, otherwise false.</returns>
        public static bool IsAcceptanceTest(this IHostEnvironment hostEnvironment) =>
            hostEnvironment?.IsEnvironment(EnvironmentNames.AcceptanceTest) ?? throw new ArgumentNullException(nameof(hostEnvironment));
    }
}
