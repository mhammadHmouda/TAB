﻿namespace TAB.Infrastructure.Common.Options;

public class AzureBlobOptions
{
    public const string SectionName = "AzureBlob";
    public string ConnectionString { get; set; } = string.Empty;
    public string ContainerName { get; set; } = string.Empty;
}
