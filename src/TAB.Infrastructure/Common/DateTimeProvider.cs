using System;
using TAB.Domain.Core.Interfaces;

namespace TAB.Infrastructure.Common;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;

    public string ToShortDateString(DateTime dateTime) => dateTime.ToShortDateString();

    public string ToLongDateString(DateTime dateTime) => dateTime.ToLongDateString();
}
