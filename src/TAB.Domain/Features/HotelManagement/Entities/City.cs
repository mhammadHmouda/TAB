using TAB.Domain.Core.Interfaces;
using TAB.Domain.Core.Primitives;
using TAB.Domain.Core.Shared;
using TAB.Domain.Core.Shared.Result;

namespace TAB.Domain.Features.HotelManagement.Entities;

public class City : Entity, IAuditableEntity
{
    public string Name { get; private set; }
    public string Country { get; private set; }
    public string PostOffice { get; private set; }
    public DateTime CreatedAtUtc { get; internal set; }
    public DateTime? UpdatedAtUtc { get; internal set; }

    private City() { }

    private City(string name, string country, string postOffice)
    {
        Name = name;
        Country = country;
        PostOffice = postOffice;
    }

    public static Result<City> Create(string name, string country, string postOffice)
    {
        Ensure.NotEmpty(name, " City name is required", nameof(name));
        Ensure.NotEmpty(country, "Country is required", nameof(country));
        Ensure.NotEmpty(postOffice, "Post office is required", nameof(postOffice));

        return new City(name, country, postOffice);
    }
}
