using FluentAssertions;
using NetArchTest.Rules;

namespace ArchitectureTests;

public class LayerTests : BaseTest
{
    private const string DomainNamespace = "TAB.Domain";
    private const string ApplicationNamespace = "TAB.Application";
    private const string InfrastructureNamespace = "TAB.Infrastructure";
    private const string PersistenceNamespace = "TAB.Persistence";
    private const string ContractNamespace = "TAB.Contracts";
    private const string WebNamespace = "TAB.WebApi";

    [Fact]
    public void Domain_Should_NotHaveDependencyOnOtherLayers()
    {
        var assembly = DomainAssembly;

        var otherProjects = new[]
        {
            ApplicationNamespace,
            InfrastructureNamespace,
            PersistenceNamespace,
            ContractNamespace,
            WebNamespace
        };

        var result = Types
            .InAssembly(assembly)
            .That()
            .ResideInNamespace(DomainNamespace)
            .ShouldNot()
            .HaveDependencyOnAny(otherProjects)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Application_Should_NotHaveDependencyOnOtherLayers()
    {
        var assembly = ApplicationAssembly;

        var otherProjects = new[] { InfrastructureNamespace, PersistenceNamespace, WebNamespace };

        var result = Types
            .InAssembly(assembly)
            .That()
            .ResideInNamespace(ApplicationNamespace)
            .ShouldNot()
            .HaveDependencyOnAny(otherProjects)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Infrastructure_Should_NotHaveDependencyOnOtherLayers()
    {
        var assembly = InfrastructureAssembly;

        var otherProjects = new[] { PersistenceNamespace, WebNamespace };

        var result = Types
            .InAssembly(assembly)
            .That()
            .ResideInNamespace(InfrastructureNamespace)
            .ShouldNot()
            .HaveDependencyOnAny(otherProjects)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Persistence_Should_NotHaveDependencyOnOtherLayers()
    {
        var assembly = PersistenceAssembly;

        var otherProjects = new[] { ContractNamespace, InfrastructureNamespace, WebNamespace };

        var result = Types
            .InAssembly(assembly)
            .That()
            .ResideInNamespace(PersistenceNamespace)
            .ShouldNot()
            .HaveDependencyOnAny(otherProjects)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Contract_Should_NotHaveDependencyOnOtherLayers()
    {
        var assembly = ContractAssembly;

        var otherProjects = new[]
        {
            ApplicationNamespace,
            InfrastructureNamespace,
            PersistenceNamespace,
            WebNamespace
        };

        var result = Types
            .InAssembly(assembly)
            .That()
            .ResideInNamespace(ContractNamespace)
            .ShouldNot()
            .HaveDependencyOnAny(otherProjects)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
}
