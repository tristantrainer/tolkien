using HotChocolate.Execution.Configuration;

namespace KiriathSolutions.Tolkien.Api.Types
{
    public interface IResolvers
    {
        void AddResolvers(IRequestExecutorBuilder builder);
    }

    public abstract class BaseResolvers<TResolvers> : IResolvers
        where TResolvers : BaseResolvers<TResolvers>
    {
        public void AddResolvers(IRequestExecutorBuilder builder)
        {
            builder.AddResolver<TResolvers>();
        }
    }
}