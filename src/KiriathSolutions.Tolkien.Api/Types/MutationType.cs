using KiriathSolutions.Tolkien.Api.Types.Resolvers;

namespace KiriathSolutions.Tolkien.Api.Types;

public class Mutation { }

public class MutationType : ObjectType<Mutation>
{
    protected override void Configure(IObjectTypeDescriptor<Mutation> descriptor)
    {
        AccountCategoryResolvers.ConfigureMutations(descriptor);
        AccountResolvers.ConfigureMutations(descriptor);
        CollectiveResolvers.ConfigureMutations(descriptor);
        TransactionCategoryResolvers.ConfigureMutations(descriptor);
        TransactionResolvers.ConfigureMutations(descriptor);
    }
}