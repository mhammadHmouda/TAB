namespace TAB.Infrastructure.Payment.Options;

public class PayPalOptions
{
    public const string SectionName = "PayPal";
    public string ClientId { get; set; } = null!;
    public string ClientSecret { get; set; } = null!;
    public string SuccessUrl { get; set; } = null!;
    public string CancelUrl { get; set; } = null!;
}
