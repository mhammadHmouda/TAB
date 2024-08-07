using System.Reflection;
using FluentAssertions;
using NetArchTest.Rules;
using TAB.Domain.Core.Primitives;
using TAB.Domain.Core.Primitives.Events;

namespace ArchitectureTests;

public class DomainTests : BaseTest
{
    [Fact]
    public void DomainEvents_Should_BeSealed()
    {
        var result = Types
            .InAssembly(DomainAssembly)
            .That()
            .ImplementInterface(typeof(IDomainEvent))
            .Should()
            .BeSealed()
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void DomainEvents_Should_HaveEventPostfix()
    {
        var result = Types
            .InAssembly(DomainAssembly)
            .That()
            .ImplementInterface(typeof(IDomainEvent))
            .Should()
            .HaveNameEndingWith("Event")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void DomainEvents_Should_BeInDomainNamespace()
    {
        var domainNamespace = DomainAssembly.FullName!.Split(",")[0];

        var result = Types
            .InAssembly(DomainAssembly)
            .That()
            .ImplementInterface(typeof(IDomainEvent))
            .Should()
            .ResideInNamespace(domainNamespace)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Entities_Should_HavePrivateParameterlessConstructor()
    {
        IEnumerable<Type> entityTypes = Types
            .InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(Entity))
            .GetTypes();

        var failingTypes = (
            from entityType in entityTypes
            let constructors = entityType.GetConstructors(
                BindingFlags.NonPublic | BindingFlags.Instance
            )
            where !constructors.Any(c => c.IsPrivate && c.GetParameters().Length == 0)
            select entityType
        ).ToList();

        failingTypes.Should().OnlyContain(t => t.Name == nameof(AggregateRoot));
    }
}
