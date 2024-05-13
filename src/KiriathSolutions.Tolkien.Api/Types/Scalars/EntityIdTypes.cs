using KiriathSolutions.Tolkien.Api.Entities;
using KiriathSolutions.Tolkien.Api.Models;

namespace KiriathSolutions.Tolkien.Api.Types.Scalars;

public class AccountCategoryIdType : PublicIdType<AccountCategoryId, AccountCategory> { }
public class AccountIdType : PublicIdType<AccountPublicId, Account> { }
public class AccountHistoryIdType : PublicIdType<AccountHistoryId, AccountHistory> { }
public class CollectiveIdType : PublicIdType<CollectiveId, Collective> { }
public class IndividualIdType : PublicIdType<IndividualId, Individual> { }
public class TransactionIdType : PublicIdType<TransactionId, Transaction> { }
public class TransactionCategoryIdType : PublicIdType<TransactionCategoryId, TransactionCategory> { }