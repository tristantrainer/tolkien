namespace KiriathSolutions.Tolkien.Api.Auth;

public interface IJwtBearer
{
    public Guid PublicId { get; }
}

public record JwtBearer(Guid PublicId) : IJwtBearer
{
    public static JwtBearer Empty { get; } = new(Guid.Empty);
}