using KiriathSolutions.Tolkien.Api.Auth;
using KiriathSolutions.Tolkien.Api.Commands;
using KiriathSolutions.Tolkien.Api.Entities;
using KiriathSolutions.Tolkien.Api.Exceptions;
using KiriathSolutions.Tolkien.Api.Extensions;
using KiriathSolutions.Tolkien.Api.Models;
using KiriathSolutions.Tolkien.Api.Repositories;
using KiriathSolutions.Tolkien.Api.Types.InputTypes;
using KiriathSolutions.Tolkien.Api.Types.ObjectTypes;
using KiriathSolutions.Tolkien.Api.Types.Scalars;

namespace KiriathSolutions.Tolkien.Api.Types.Resolvers;

internal sealed class CollectiveResolvers : BaseResolvers<CollectiveResolvers>
{
    #region Queries 

    public static void ConfigureQueries(IObjectTypeDescriptor<Query> descriptor)
    {
        descriptor
            .Field("activeCollective")
            .ResolveWith<CollectiveResolvers>((resolver) => resolver.GetActiveCollectiveAsync(default!, default!))
            .Type<CollectiveType>()
            .Authorize();

        descriptor
            .Field("collective")
            .ResolveWith<CollectiveResolvers>((resolver) => resolver.GetCollectiveAsync(default!, default!, default!))
            .Type<CollectiveType>()
            .Argument("id", (arg) => arg.Type<CollectiveIdType>())
            .Authorize();

        descriptor
            .Field("collectives")
            .ResolveWith<CollectiveResolvers>((resolver) => resolver.GetCollectivesAsync(default!, default!))
            .Type<ListType<CollectiveType>>()
            .Authorize();
    }

    public async Task<Collective?> GetActiveCollectiveAsync([Service] ITolkienUser user, [Service] IUnitOfWork unitOfWork)
    {
        if (user.ActiveCollectiveId is null)
            return null;

        var collective = await unitOfWork
            .Collectives
            .FindByIdAsync(user.ActiveCollectiveId);

        EntityMissingException.ThrowIfNull(collective);

        return collective;
    }

    public async Task<Collective> GetCollectiveAsync(CollectiveId id, [Service] ITolkienUser user, [Service] IUnitOfWork unitOfWork)
    {
        var collective = await unitOfWork
            .Collectives
            .FindByIdAsync(id, user);

        EntityMissingException.ThrowIfNull(collective);

        return collective;
    }

    public async Task<Collective[]> GetCollectivesAsync([Service] ITolkienUser user, [Service] IUnitOfWork unitOfWork)
    {
        return await unitOfWork
            .Collectives
            .GetAllForUser(user);
    }

    #endregion

    #region Mutations

    public static void ConfigureMutations(IObjectTypeDescriptor<Mutation> descriptor)
    {
        descriptor.Field("setActiveCollective")
            .ResolveWith<CollectiveResolvers>((resolver) => resolver.SetActiveCollective(default!, default!, default!))
            .Argument("id", (arg) => arg.Type<CollectiveIdType>())
            .Type<CollectiveType>()
            .Authorize();

        descriptor.Field("createCollective")
            .ResolveWith<CollectiveResolvers>((resolver) => resolver.CreateCollectiveAsync(default!, default!, default!))
            .Argument("command", (arg) => arg
                .Type<CreateCollectiveCommandInput>()
                .Description("A command containing all the information needed to create a collective (including unchanged information)"))
            .Type<CollectiveType>()
            .Authorize();
    }

    public async Task<Collective> SetActiveCollective(CollectiveId id, [Service] ITolkienUser user, [Service] IUnitOfWork unitOfWork)
    {
        var collective = await unitOfWork
            .Collectives
            .FindByIdAsync(id, user);

        EntityMissingException.ThrowIfNull(collective);

        var activeCollective = await unitOfWork
            .IndividualActiveCollectives
            .GetActiveCollective(user.IndividualId);

        if (activeCollective is null)
        {
            var newActiveCollective = new IndividualActiveCollective()
            {
                IndividualId = user.IndividualId,
                CollectiveId = collective.Id,
            };

            await unitOfWork.AddAsync(newActiveCollective);
        }
        else
        {
            activeCollective.CollectiveId = collective.Id;
        }

        await unitOfWork.SaveChangesAsync();

        return collective;
    }

    public async Task<Collective> CreateCollectiveAsync(CreateCollectiveCommand command, [Service] ITolkienUser user, [Service] IUnitOfWork unitOfWork)
    {
        var newCollective = new Collective
        {
            PublicId = PublicId.NewPublicId(),
            Name = command.Name,
            Created = DateTime.UtcNow,
            CreatedBy = user.IndividualId,
            LastUpdated = DateTime.UtcNow,
            UpdatedBy = user.IndividualId,
        };

        await unitOfWork.AddAsync(newCollective);
        await unitOfWork.SaveChangesAsync();

        var individualCollective = new IndividualCollective
        {
            IndividualId = user.IndividualId,
            CollectiveId = newCollective.Id
        };

        await unitOfWork.AddAsync(individualCollective);
        await unitOfWork.SaveChangesAsync();

        return newCollective;
    }

    public async Task<Collective> UpdateCollectiveAsync(UpdateCollectiveCommand command, [Service] ITolkienUser user, [Service] IUnitOfWork unitOfWork)
    {
        var collective = await unitOfWork
            .Collectives
            .FindByIdAsync(command.CollectiveId, user);

        EntityMissingException.ThrowIfNull(collective);

        collective.Name = command.Name;

        await unitOfWork.SaveChangesAsync();
        return collective;
    }

    #endregion
}