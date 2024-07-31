namespace TAB.Infrastructure.Payment.Options;

public class StripeOptions
{
    public const string SectionName = "Stripe";
    public string SecretKey { get; set; } = null!;
    public string PublishableKey { get; set; } = null!;
    public string CancelUrl { get; set; } = null!;
    public string SuccessUrl { get; set; } = null!;
    public string Mode { get; set; } = null!;
    public List<string> PaymentMethods { get; set; } = null!;
}
