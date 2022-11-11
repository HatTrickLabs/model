using System.Data;

namespace HatTrick.Model.Sql
{
    public interface ISqlParameter : IDatabaseObject
	{
        byte? Scale { get; set; }

        byte? Precision { get; set; }

        long? MaxLength { get; set; }

        bool IsOutput { get; set; }
    }
}
