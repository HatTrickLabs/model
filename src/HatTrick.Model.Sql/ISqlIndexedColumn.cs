namespace HatTrick.Model.Sql
{
    public interface ISqlIndexedColumn : IDatabaseObject
    {
        short OrdinalPosition { get; set; }

        bool IsDescending { get; set; }
    }
}
