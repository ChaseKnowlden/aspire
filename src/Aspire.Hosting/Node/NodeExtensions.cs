// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using Aspire.Hosting.ApplicationModel;
using Microsoft.Extensions.Hosting;

namespace Aspire.Hosting;

/// <summary>
/// Provides extension methods for adding Node applications to an <see cref="IDistributedApplicationBuilder"/>.
/// </summary>
public static class NodeAppHostingExtension
{
    /// <summary>
    /// Adds a node application to the application model. Node should available on the PATH.
    /// </summary>
    /// <param name="builder">The <see cref="IDistributedApplicationBuilder"/> to add the resource to.</param>
    /// <param name="name">The name of the resource.</param>
    /// <param name="workingDirectory">The working directory to use for the command. If null, the working directory of the current process is used.</param>
    /// <param name="args">The arguments to pass to the command.</param>
    /// <returns></returns>
    public static IResourceBuilder<NodeAppResource> AddNodeApp(this IDistributedApplicationBuilder builder, string name, string workingDirectory, string[] args)
    {
        var resource = new NodeAppResource(name, "node", workingDirectory, args);

        return builder.AddResource(resource)
                      .WithNodeDefaults();
    }

    /// <summary>
    /// Adds a node application to the application model. Executes the npm command with the specified script name.
    /// </summary>
    /// <param name="builder">The <see cref="IDistributedApplicationBuilder"/> to add the resource to.</param>
    /// <param name="name">The name of the resource.</param>
    /// <param name="workingDirectory">The working directory to use for the command. If null, the working directory of the current process is used.</param>
    /// <param name="scriptName">The npm script to execute. Defaults to "start".</param>
    /// <param name="args">The arguments to pass to the command.</param>
    /// <returns></returns>
    public static IResourceBuilder<NodeAppResource> AddNpmApp(this IDistributedApplicationBuilder builder, string name, string workingDirectory, string scriptName = "start", string[]? args = null)
    {
        string[] allArgs = args is { Length: > 0 }
            ? ["run", scriptName, "--", .. args]
            : ["run", scriptName];

        var resource = new NodeAppResource(name, "npm", workingDirectory, allArgs);

        return builder.AddResource(resource)
                      .WithNodeDefaults();
    }

    private static IResourceBuilder<NodeAppResource> WithNodeDefaults(this IResourceBuilder<NodeAppResource> builder) =>
        builder.WithOtlpExporter()
            .WithEnvironment("NODE_ENV", builder.ApplicationBuilder.Environment.IsDevelopment() ? "development" : "production");
}
