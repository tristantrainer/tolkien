using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace KiriathSolutions.Tolkien.Api.Exceptions
{
    public class EntityMissingException : GraphQLException
    {
        public EntityMissingException(string message) : base(message) { }

        public static T ThrowIfNull<T>([NotNull] T? entity)
        {
            if (entity is null)
                throw new EntityMissingException("Could not find entity for user");

            return entity;
        }
    }
}