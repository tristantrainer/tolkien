namespace KiriathSolutions.Tolkien.Api.Models;

public class DeleteResponse
{
    public Guid Id { get; set; } = default!;
    public string Status { get; set; } = "DELETED";
}
