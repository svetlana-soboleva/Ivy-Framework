namespace Ivy.Services;

using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

public class CompositeServiceProvider : IServiceProvider
{
    private readonly List<IServiceProvider> _serviceProviders;

    public CompositeServiceProvider(params IServiceCollection[] serviceCollections)
    {
        if (serviceCollections == null || serviceCollections.Length == 0)
            throw new ArgumentNullException(nameof(serviceCollections));

        _serviceProviders = new List<IServiceProvider>();
        foreach (var collection in serviceCollections)
        {
            _serviceProviders.Add(collection.BuildServiceProvider());
        }
    }

    public object GetService(Type serviceType)
    {
        foreach (var provider in _serviceProviders)
        {
            var service = provider.GetService(serviceType);
            if (service != null)
                return service;
        }

        return null!; // Service not found in any collection
    }
}
