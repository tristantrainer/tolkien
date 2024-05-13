using KiriathSolutions.Tolkien.Api.Models;

namespace KiriathSolutions.Tolkien.Api.Commands;

public record CreateAccountCategoryCommand(CollectiveId CollectiveId, string Name);
public record UpdateAccountCategoryCommand(AccountCategoryId Id, PatchField<string> Name);
public record DeleteAccountCategoryCommand(AccountCategoryId Id);
