using KiriathSolutions.Tolkien.Api.Models;

namespace KiriathSolutions.Tolkien.Api.Commands;

public record CreateAccountCommand(CollectiveId CollectiveId, string Name, AccountCategoryId CategoryId, decimal Balance);
public record UpdateAccountCommand(AccountPublicId Id, PatchField<string> Name, PatchField<AccountCategoryId> CategoryId);
public record DeleteAccountCommand(AccountPublicId Id);
