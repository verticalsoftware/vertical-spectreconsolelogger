using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Shouldly;
using Vertical.SpectreLogger.Reflection;
using Xunit;

namespace Vertical.SpectreLogger.Tests.Internal
{
    public class TypeActivatorTests
    {
        public interface IService {}
        public class ServiceBase : IService {}
        public class Service : ServiceBase {}

        public class ActivatedTypeWithDefaultConstructor
        {
        }
        
        public class ActivatedType
        {
            public IService? Service { get; }
            
            public ActivatedType(){}

            public ActivatedType(string str, bool b, IService? service)
            {
                Service = service;
            }

            public ActivatedType(string str, bool b)
            {
            }
        }
        
        [Fact]
        public void CreateInstanceReturnsNonNullObject()
        {
            TypeActivator.CreateInstance<ActivatedType>(new object[]
            {
                new Service(),
                "a-string",
                true
            }.ToList()).ShouldNotBeNull();
        }

        [Fact]
        public void CreateInstanceSelectsMoreSpecificConstructor()
        {
            var instance = TypeActivator.CreateInstance<ActivatedType>(new object[]
            {
                new Service(),
                "a-string",
                true
            }.ToList());
            
            instance.Service.ShouldNotBeNull();
        }

        [Fact]
        public void CreateInstanceSelectsLessSpecificConstructor()
        {
            TypeActivator.CreateInstance<ActivatedType>(new object[]
            {
                "a-string",
                true
            }.ToList()).ShouldNotBeNull();
        }

        [Fact]
        public void CreateInstanceSelectsDefaultConstructor()
        {
            TypeActivator.CreateInstance<ActivatedTypeWithDefaultConstructor>(new List<object>()).ShouldNotBeNull();
        }
    }
}