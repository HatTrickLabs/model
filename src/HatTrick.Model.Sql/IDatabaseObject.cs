namespace HatTrick.Model.Sql
{
    public interface IDatabaseObject : INamedMeta
    {
        string Identifier { get; set; }
    }
}
