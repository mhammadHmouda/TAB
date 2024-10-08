﻿using TAB.Domain.Core.Enums;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Interfaces;
using TAB.Domain.Core.Primitives;
using TAB.Domain.Core.Shared.Result;

namespace TAB.Domain.Features.HotelManagement.Entities;

public class Image : Entity, IAuditableEntity
{
    public string Url { get; private set; }
    public ImageType Type { get; private set; }
    public int ReferenceId { get; private set; }
    public DateTime CreatedAtUtc { get; internal set; }
    public DateTime? UpdatedAtUtc { get; internal set; }

    private Image() { }

    private Image(string url, ImageType type, int referenceId)
    {
        Url = url;
        Type = type;
        ReferenceId = referenceId;
    }

    public static Result<Image> Create(string url, ImageType type, int referenceId) =>
        Result
            .Create((url, type, referenceId))
            .Ensure(x => !string.IsNullOrWhiteSpace(x.url), DomainErrors.Image.UrlNullOrEmpty)
            .Ensure(x => Enum.IsDefined(typeof(ImageType), x.type), DomainErrors.Image.TypeInvalid)
            .Ensure(x => x.referenceId > 0, DomainErrors.Image.ReferenceIdInvalid)
            .Map(x => new Image(x.url, x.type, x.referenceId));

    public static Result<IReadOnlyCollection<Image>> CreateImages(
        IEnumerable<string> urls,
        ImageType type,
        int referenceId
    )
    {
        var images = new List<Image>();
        foreach (var url in urls)
        {
            var imageResult = Create(url, type, referenceId);

            if (imageResult.IsFailure)
            {
                return imageResult.Error;
            }

            images.Add(imageResult.Value);
        }

        return images.AsReadOnly();
    }

    public Result UpdateUrl(string url)
    {
        if (string.IsNullOrEmpty(url))
        {
            return DomainErrors.Image.UrlNullOrEmpty;
        }

        Url = url;

        return Result.Success();
    }
}
