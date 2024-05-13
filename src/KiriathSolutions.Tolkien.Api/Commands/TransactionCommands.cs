using KiriathSolutions.Tolkien.Api.Models;

namespace KiriathSolutions.Tolkien.Api.Commands;

public record CreateTransactionCommand(
    CollectiveId CollectiveId,
    AccountPublicId? AccountId,
    decimal Amount,
    TransactionCategoryId CategoryId,
    DateTime Date,
    string? Description,
    string? Recurrance
);

public record UpdateTransactionCommand(
    AccountPublicId? AccountId,
    decimal Amount,
    TransactionCategoryId CategoryId,
    DateTime Date,
    string? Description,
    string? Recurrance,
    TransactionId TransactionId
);

public record DeleteTransactionCommand(TransactionId TransactionId);
