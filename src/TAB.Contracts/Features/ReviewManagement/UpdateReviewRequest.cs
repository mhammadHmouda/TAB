namespace TAB.Contracts.Features.ReviewManagement;

public record UpdateReviewRequest(int Id, string Title, string Content, int Rating);
