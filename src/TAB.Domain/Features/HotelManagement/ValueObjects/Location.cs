﻿using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Primitives;
using TAB.Domain.Core.Shared.Result;

namespace TAB.Domain.Features.HotelManagement.ValueObjects;

public class Location : ValueObject
{
    public double Latitude { get; }
    public double Longitude { get; }

    private Location(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }

    public static Result<Location> Create(double latitude, double longitude) =>
        Result
            .Create((latitude, longitude))
            .Ensure(x => x.latitude is not double.NaN, DomainErrors.Location.NullLatitude)
            .Ensure(x => x.longitude is not double.NaN, DomainErrors.Location.NullLongitude)
            .Ensure(x => x.latitude is >= -90 and <= 90, DomainErrors.Location.LatitudeOutOfRange)
            .Ensure(
                x => x.longitude is >= -180 and <= 180,
                DomainErrors.Location.LongitudeOutOfRange
            )
            .Map(x => new Location(x.latitude, x.longitude));

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Latitude;
        yield return Longitude;
    }
}
