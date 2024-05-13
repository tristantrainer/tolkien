using KiriathSolutions.Tolkien.Api.Models;

namespace KiriathSolutions.Tolkien.Api.Commands;

public record CreateCollectiveCommand(string Name);
public record UpdateCollectiveCommand(CollectiveId CollectiveId, string Name);