namespace KiriathSolutions.Tolkien.Api.Commands;

public record SignUpCommand(string EmailAddress, string FirstName, string LastName, string Password);
public record LogInCommand(string EmailAddress, string Password);
public record ResetPasswordCommand(string OldPassword, string NewPassword);
public record RefreshTokenCommand(string RefreshToken);
public record VerifyTokenCommand(string RefreshToken);
