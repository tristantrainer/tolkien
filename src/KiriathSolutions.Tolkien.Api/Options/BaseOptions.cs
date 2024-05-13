using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KiriathSolutions.Tolkien.Api.Options
{
    public interface IConfigOptions
    {
        void Configure(WebApplicationBuilder builder);
    }

    public abstract class BaseOptions<TOptions> : IConfigOptions
        where TOptions : BaseOptions<TOptions>
    {
        protected abstract string Section { get; }

        void IConfigOptions.Configure(WebApplicationBuilder builder)
        {
            builder.Services.Configure<TOptions>(builder.Configuration.GetSection(Section));
        }
    }
}