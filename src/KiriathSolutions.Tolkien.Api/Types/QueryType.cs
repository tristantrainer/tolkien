using KiriathSolutions.Tolkien.Api.Types.Resolvers;

namespace KiriathSolutions.Tolkien.Api.Types;

public class Query { }

public class QueryType : ObjectType<Query>
{
    protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
    {
        AccountCategoryResolvers.ConfigureQueries(descriptor);
        AccountResolvers.ConfigureQueries(descriptor);
        CollectiveResolvers.ConfigureQueries(descriptor);
        TransactionCategoryResolvers.ConfigureQueries(descriptor);
        TransactionResolvers.ConfigureQueries(descriptor);
    }
}
