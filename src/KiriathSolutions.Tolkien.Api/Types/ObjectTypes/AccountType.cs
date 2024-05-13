using KiriathSolutions.Tolkien.Api.Entities;
using KiriathSolutions.Tolkien.Api.Types.Resolvers;
using KiriathSolutions.Tolkien.Api.Types.Scalars;

namespace KiriathSolutions.Tolkien.Api.Types.ObjectTypes;
public class AccountType : ObjectType<Account>
{
    protected override void Configure(IObjectTypeDescriptor<Account> descriptor)
    {
        descriptor
            .Field((account) => account.Name)
            .Name("name");

        descriptor
            .Field((account) => account.PublicId)
            .Type<AccountIdType>()
            .Name("id");

        descriptor
            .Field("balance")
            .Type<DecimalType>()
            .ResolveWith<AccountResolvers>((resolver) => resolver.GetCurrentBalance(default!, default!));

        descriptor
            .Field((account) => account.Category)
            .Type<AccountCategoryType>()
            .Name("category")
            .ResolveWith<AccountResolvers>((resolver) => resolver.GetAccountCategory(default!, default!, default!));

        descriptor
            .Field("history")
            .Type<ListType<AccountHistoryItemType>>()
            .UsePaging()
            .ResolveWith<AccountResolvers>((resolver) => resolver.GetHistory(default!, default!));

        descriptor
            .Field((account) => account.Collective)
            .Ignore();

        descriptor
            .Field((account) => account.CollectiveId)
            .Ignore();

        descriptor
            .Field((account) => account.CategoryId)
            .Ignore();
    }
}