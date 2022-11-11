using System;

namespace HatTrick.Model.Sql
{
    public interface IDatabaseObjectModifier<T>
        where T : class, INamedMeta
    {
        abstract void Apply(Action<T> action);
    }
}
