namespace KiriathSolutions.Tolkien.Api.Commands;

public record CreateAccountHistoryItemCommand(string Name);
public record UpdateAccountHistoryItemCommand(Guid AccountId, string Name);
public record DeleteAccountHistoryItemCommand(Guid AccountId);
