using System;
using FluentAssertions;
using Gybs.DependencyInjection.Services;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Gybs.Tests.DependencyInjection.Services
{
    public class ServiceExtensionsAddInterfaceServicesTests
    {
        [Fact]
        public void ForSingletonShouldResolveByType()
        {
            var serviceProvider = CreateServiceProvider();
            var instance = serviceProvider.GetService<SingletonMock>();
            instance.Should().NotBeNull();
        }

        [Fact]
        public void ForSingletonShouldResolveByInterface()
        {
            var serviceProvider = CreateServiceProvider();
            var instance = serviceProvider.GetService<ISingletonMock>();
            instance.Should().NotBeNull();
        }

        [Fact]
        public void ForSingletonShouldResolveSameInstance()
        {
            var serviceProvider = CreateServiceProvider();
            var firstInstance = serviceProvider.GetService<ISingletonMock>();
            var secondInstance = serviceProvider.CreateScope().ServiceProvider.GetService<ISingletonMock>();

            firstInstance.Should().Be(secondInstance);
        }

        [Fact]
        public void ForScopedShouldResolveByType()
        {
            var serviceProvider = CreateServiceProvider();
            var instance = serviceProvider.GetService<ScopedMock>();
            instance.Should().NotBeNull();
        }

        [Fact]
        public void ForScopedShouldResolveByInterface()
        {
            var serviceProvider = CreateServiceProvider();
            var instance = serviceProvider.GetService<IScopedMock>();
            instance.Should().NotBeNull();
        }

        [Fact]
        public void ForScopedWithinScopeShouldResolveSameInstance()
        {
            var scopeServiceProvider = CreateServiceProvider().CreateScope().ServiceProvider;
            var firstInstance = scopeServiceProvider.GetService<ISingletonMock>();
            var secondInstance = scopeServiceProvider.GetService<ISingletonMock>();

            firstInstance.Should().Be(secondInstance);
        }

        [Fact]
        public void ForScopedWithinDifferentScopesShouldResolveSeparateInstances()
        {
            var firstInstance = CreateServiceProvider().CreateScope().ServiceProvider.GetService<ISingletonMock>();
            var secondInstance = CreateServiceProvider().CreateScope().ServiceProvider.GetService<ISingletonMock>();

            firstInstance.Should().NotBe(secondInstance);
        }

        [Fact]
        public void ForTransientShouldResolveByType()
        {
            var serviceProvider = CreateServiceProvider();
            var instance = serviceProvider.GetService<TransientMock>();
            instance.Should().NotBeNull();
        }

        [Fact]
        public void ForTransientShouldResolveByInterface()
        {
            var serviceProvider = CreateServiceProvider();
            var instance = serviceProvider.GetService<ITransientMock>();
            instance.Should().NotBeNull();
        }

        [Fact]
        public void ForTransientShouldResolveSeparateInstances()
        {
            var serviceProvider = CreateServiceProvider();
            var firstInstance = serviceProvider.GetService<ITransientMock>();
            var secondInstance = serviceProvider.GetService<ITransientMock>();

            firstInstance.Should().NotBe(secondInstance);
        }

        private IServiceProvider CreateServiceProvider()
        {
            var factory = new DefaultServiceProviderFactory();
            return factory.CreateServiceProvider(
                new ServiceCollection().AddGybs(builder => builder.AddInterfaceServices())
            );
        }

        private interface ISingletonMock { }
        private class SingletonMock : ISingletonMock, ISingletonService { }
        private interface IScopedMock { }
        private class ScopedMock : IScopedMock, IScopedService { }
        private interface ITransientMock { }
        private class TransientMock : ITransientMock, ITransientService { }
    }
}