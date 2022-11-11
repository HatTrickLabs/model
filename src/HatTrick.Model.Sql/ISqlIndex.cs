namespace HatTrick.Model.Sql
{
    public interface ISqlIndex : IDatabaseObject
    {
        bool IsPrimaryKey { get; set; }

        bool IsUnique { get; set; }
    }
}
