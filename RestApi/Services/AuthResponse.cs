namespace RestApi.Services;


public record AuthResponse(
    string Token,
    DateTime ExpiresAt
);