using FluentAssertions;
using NetArchTest.Rules;
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
}
