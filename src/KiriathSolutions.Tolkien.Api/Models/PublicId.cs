namespace KiriathSolutions.Tolkien.Api.Models;

public struct PublicId
{
    public Guid Value { get; init; }

    public PublicId(Guid value)
    {
        Value = value;
    }

    public static PublicId NewPublicId() => new(Guid.NewGuid());
    public static implicit operator Guid(PublicId id) => id.Value;
    public static implicit operator PublicId(Guid id) => new(id);
}
