using System.Security.Claims;
using KiriathSolutions.Tolkien.Api.Contexts;
using KiriathSolutions.Tolkien.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace KiriathSolutions.Tolkien.Api.Auth;

public interface ITolkienUser : IJwtBearer, ICollectiveMember
{
    int IndividualId { get; }
    int? ActiveCollectiveId { get; }

    bool CanAccess(ICollectiveEntity entity);
}

public interface ICollectiveMember
{
    bool IsCollectiveMember(int collectiveId);
    bool IsNotCollectiveMember(int collectiveId);

    int[] CollectiveIds { get; }
}

internal sealed class TolkienUser : ITolkienUser
{
    private readonly IDbContextFactory<AuthContext> _authContextFactory;

    private Individual? _individual;
    private Individual Individual => _individual ??= GetIndividual();

    private int[]? _collectiveIds;
    public int[] CollectiveIds => _collectiveIds ??= GetCollectiveIds();

    public Guid PublicId { get; private set; }
    public int IndividualId => Individual.Id;
    public int? ActiveCollectiveId => Individual.ActiveCollective?.CollectiveId;

    public TolkienUser(IHttpContextAccessor httpContextAccessor, IDbContextFactory<AuthContext> authContextFactory)
    {
        _authContextFactory = authContextFactory;

        var userIdString = httpContextAccessor.HttpContext?.User.FindFirstValue("UserId")
            ?? throw new ArgumentNullException("Missing claim UserId");

        PublicId = Guid.Parse(userIdString);
    }

    public bool IsNotCollectiveMember(int id)
        => !IsCollectiveMember(id);

    public bool IsCollectiveMember(int id)
        => CollectiveIds.Any((collectiveId) => collectiveId == id);

    public bool CanAccess(ICollectiveEntity entity)
    {
        return IsCollectiveMember(entity.CollectiveId);
    }

    private Individual GetIndividual()
    {
        using var context = _authContextFactory.CreateDbContext();

        var individual = context
            .Individuals
            .Include((record) => record.ActiveCollective)
            .AsNoTracking()
            .FirstOrDefault((record) => record.PublicId == PublicId);

        if (individual is null)
            throw new ArgumentException("No user found matching UserId claim");

        return individual;
    }

    private int[] GetCollectiveIds()
    {
        using var context = _authContextFactory.CreateDbContext();

        var collectiveIds = context
            .IndividualCollectives
            .AsNoTracking()
            .Where((record) => record.IndividualId == IndividualId)
            .Select((record) => record.CollectiveId)
            .ToArray();

        return collectiveIds;
    }
}