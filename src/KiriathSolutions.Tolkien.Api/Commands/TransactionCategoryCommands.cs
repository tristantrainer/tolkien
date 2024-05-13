using KiriathSolutions.Tolkien.Api.Models;

namespace KiriathSolutions.Tolkien.Api.Commands;

public record CreateTransactionCategoryCommand(
    CollectiveId CollectiveId,
    string Name
);

public record UpdateTransactionCategoryCommand(
    TransactionCategoryId CategoryId,
    string Name
);

public record DeleteTransactionCategoryCommand(TransactionCategoryId TransactionId);
