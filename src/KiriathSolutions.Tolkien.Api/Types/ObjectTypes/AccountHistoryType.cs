using KiriathSolutions.Tolkien.Api.Entities;
using KiriathSolutions.Tolkien.Api.Types.Scalars;

namespace KiriathSolutions.Tolkien.Api.Types.ObjectTypes;

public class AccountHistoryItemType : ObjectType<AccountHistory>
{
    protected override void Configure(IObjectTypeDescriptor<AccountHistory> descriptor)
    {
        descriptor
            .Field((history) => history.Balance)
            .Name("balance");

        descriptor
            .Field((history) => history.Date)
            .Name("date");

        descriptor
            .Field((history) => history.PublicId)
            .Type<AccountHistoryIdType>()
            .Name("id");

        descriptor
            .Field((history) => history.AccountId)
            .Ignore();

        descriptor
            .Field((history) => history.Account)
            .Ignore();

        descriptor
            .Field((history) => history.Id)
            .Ignore();
    }
}