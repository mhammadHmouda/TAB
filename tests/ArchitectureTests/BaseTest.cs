using System.Reflection;
using TAB.Application.Features.UserManagement;
using TAB.Contracts.Features.UserManagement.Users;
using TAB.Domain.Features.UserManagement.Entities;
using TAB.Infrastructure.Emails;
using TAB.Persistence;
using TAB.WebApi.Controllers;

namespace ArchitectureTests;

public abstract class BaseTest
{
    protected static readonly Assembly DomainAssembly = typeof(User).Assembly;
    protected static readonly Assembly ApplicationAssembly = typeof(UserProfile).Assembly;
    protected static readonly Assembly InfrastructureAssembly = typeof(EmailService).Assembly;
    protected static readonly Assembly PersistenceAssembly = typeof(TabDbContext).Assembly;
    protected static readonly Assembly ContractAssembly = typeof(UserResponse).Assembly;
    protected static readonly Assembly WebAssembly = typeof(UserController).Assembly;
}
