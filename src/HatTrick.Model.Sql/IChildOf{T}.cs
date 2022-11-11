namespace HatTrick.Model.Sql
{
    public interface IChildOf<T>
        where T : IDatabaseObject
    {
        string? ParentIdentifier { get; }
        T? GetParent();
        void SetParent(T parent);
    }
}
