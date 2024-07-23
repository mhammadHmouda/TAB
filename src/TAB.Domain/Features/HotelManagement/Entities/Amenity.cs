using TAB.Domain.Core.Interfaces;
using TAB.Domain.Core.Primitives;
using TAB.Domain.Core.Shared;
using TAB.Domain.Features.HotelManagement.Enums;

namespace TAB.Domain.Features.HotelManagement.Entities;

public class Amenity : Entity, IAuditableEntity
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public AmenityType Type { get; private set; }
    public int TypeId { get; private set; }
    public DateTime CreatedAtUtc { get; internal set; }
    public DateTime? UpdatedAtUtc { get; internal set; }

    private Amenity(string name, string description, AmenityType type, int typeId)
    {
        Name = name;
        Description = description;
        Type = type;
        TypeId = typeId;
    }

    public static Amenity Create(string name, string description, AmenityType type, int typeId)
    {
        Ensure.NotEmpty(name, "The name is required.", nameof(name));
        Ensure.NotEmpty(description, "The description is required.", nameof(description));
        Ensure.Of(type, "The type should be one of the following: Hotel, Room", nameof(type));
        Ensure.NotDefault(typeId, "The type id is required.", nameof(typeId));

        return new Amenity(name, description, type, typeId);
    }
}
