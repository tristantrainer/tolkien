using System.Diagnostics.CodeAnalysis;
using KiriathSolutions.Tolkien.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KiriathSolutions.Tolkien.Api.Entities;

public class AccountHistory : IEntity, IIndividualCreatedEntity
{
    public int Id { get; set; }
    public Guid PublicId { get; init; }

    public DateTime Created { get; init; }
    public int CreatedBy { get; init; }
    public DateTime LastUpdated { get; set; }
    public int UpdatedBy { get; set; }

    public decimal Balance { get; set; }
    public DateOnly Date { get; set; }

    public int AccountId { get; init; }
    public virtual Account? Account { get; set; }

    public static AccountHistory Create(Individual individual, AccountId accountId, DateOnly date, decimal balance, DateTime? createdAt = null)
    {
        var creationTime = createdAt ?? DateTime.UtcNow;

        return new AccountHistory()
        {
            PublicId = Guid.NewGuid(),
            Created = creationTime,
            CreatedBy = individual.Id,
            LastUpdated = creationTime,
            UpdatedBy = individual.Id,
            Balance = balance,
            Date = date,
            AccountId = accountId.Value,
        };
    }
}

public class AccountHistoryBuilder
{
    private readonly List<string> _validationErrors = new();

    private int? _accountId;
    private Individual? _creator;
    private DateTime? _createdAt;
    private DateOnly? _date;
    private decimal _balance;

    public static AccountHistoryBuilder CreateAs(Individual individual, DateTime? createdAt = null)
    {
        return new AccountHistoryBuilder
        {
            _createdAt = createdAt ?? DateTime.UtcNow,
            _creator = individual
        };
    }

    public AccountHistoryBuilder ForAccount(AccountId accountId)
    {
        _accountId = accountId.Value;
        return this;
    }

    public AccountHistoryBuilder WithBalance(decimal balance)
    {
        _balance = balance;
        return this;
    }

    public AccountHistoryBuilder OnDate(DateOnly date)
    {
        _date = date;
        return this;
    }

    public AccountHistory Build()
    {
        Validate();

        return new AccountHistory
        {
            AccountId = _accountId!.Value,
            Created = _createdAt!.Value,
            CreatedBy = _creator!.Id,
            UpdatedBy = _creator.Id,
            LastUpdated = _createdAt.Value,
            Balance = _balance,
            Date = _date!.Value,
        };
    }

    private bool Validate()
    {
        if (_accountId is null)
            _validationErrors.Add("Account id cannot be null");

        if (_validationErrors.Any())
            throw new Exception();


        return true;
    }
}

public class AccountHistoryEntityTypeConfiguration : IEntityTypeConfiguration<AccountHistory>
{
    public void Configure(EntityTypeBuilder<AccountHistory> builder)
    {
        IEntity.Configure(builder);
        IIndividualCreatedEntity.Configure(builder);

        builder
            .ToTable("account_histories")
            .HasKey((e) => e.Id);

        builder
            .Property((e) => e.AccountId)
            .HasColumnName("account_id");

        builder
            .Property((e) => e.Balance)
            .HasColumnName("balance");

        builder
            .Property((e) => e.Date)
            .HasColumnName("date");

        builder
            .HasOne((e) => e.Account)
            .WithMany((e) => e.HistoryItems)
            .HasForeignKey((e) => e.AccountId);
    }
}