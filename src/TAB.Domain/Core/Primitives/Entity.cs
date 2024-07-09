using TAB.Domain.Core.Utils;

namespace TAB.Domain.Core.Primitives;

public abstract class Entity : IEquatable<Entity>
{
    protected Entity(int id)
    {
        Ensure.NotEmpty(id, "The identifier is required.", nameof(id));

        Id = id;
    }

    protected Entity() { }

    public int Id { get; }

    public bool Equals(Entity? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return GetType() == other.GetType() && Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        return GetType() == obj.GetType() && Equals(obj as Entity);
    }

    public override int GetHashCode() => (GetType().ToString() + Id).GetHashCode();
}
