using System;
using FluentAssertions;
using Gybs.DependencyInjection.Services;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Gybs.Tests.DependencyInjection.Services
{
    public class ServiceExtensionsAddAttributeServicesTests
    {
        private const string Group1 = "grp1";
        private const string Group2 = "grp2";

        [Theory]
        [InlineData(typeof(SingletonMock)), InlineData(typeof(ScopedMock)), InlineData(typeof(TransientMock))]
        public void ForTypeShouldResolveByType(Type registeredType)
        {
            var serviceProvider = CreateServiceProvider();
            var instance = serviceProvider.GetService(registeredType);
            instance.Should().NotBeNull();
        }

        [Theory]
        [InlineData(typeof(SingletonMock)), InlineData(typeof(ScopedMock)), InlineData(typeof(TransientMock))]
        public void ForTypeShouldResolveByInterface(Type registeredType)
        {
            var serviceProvider = CreateServiceProvider();
            var instance = serviceProvider.GetService(registeredType);
            instance.Should().NotBeNull();
        }

        [Fact]
        public void ForSingletonShouldResolveSameByTypeAndInterface()
        {
            var serviceProvider = CreateServiceProvider();
            var firstInstance = serviceProvider.GetService<SingletonMock>();
            var secondInstance = serviceProvider.GetService<ISingletonMock>();
            firstInstance.Should().Be(secondInstance);
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
        public void ForTransientShouldResolveSeparateInstances()
        {
            var serviceProvider = CreateServiceProvider();
            var firstInstance = serviceProvider.GetService<ITransientMock>();
            var secondInstance = serviceProvider.GetService<ITransientMock>();

            firstInstance.Should().NotBe(secondInstance);
        }

        [Theory]
        [InlineData(typeof(SingletonMock), Group1), InlineData(typeof(ScopedMock), Group2)]
        public void ForGroupServicesShouldResolveTypesInGroup(Type type, string group)
        {
            var serviceProvider = CreateServiceProvider(group);
            var instance = serviceProvider.GetService(type);

            instance.Should().NotBeNull();
        }

        [Fact]
        public void ForGroupServicesShouldNotResolveTypesOutOfGroup()
        {
            var serviceProvider = CreateServiceProvider(Group1);
            var differentGroupInstance = serviceProvider.GetService<ScopedMock>();
            var nonGroupInstance = serviceProvider.GetService<TransientMock>();

            differentGroupInstance.Should().BeNull();
            nonGroupInstance.Should().BeNull();
        }

        private IServiceProvider CreateServiceProvider(string? group = null)
        {
            var factory = new DefaultServiceProviderFactory();
            return factory.CreateServiceProvider(
                new ServiceCollection().AddGybs(builder => builder.AddAttributeServices(group: group))
            );
        }

        private interface ISingletonMock { }
        [SingletonService, SingletonService(Group1)] private class SingletonMock : ISingletonMock { }
        private interface IScopedMock { }
        [ScopedService, ScopedService(Group2)] private class ScopedMock : IScopedMock { }
        private interface ITransientMock { }
        [TransientService] private class TransientMock : ITransientMock { }
    }
}