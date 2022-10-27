using System.Collections.Generic;

namespace HatTrick.Model.Sql
{
    public interface ISqlModel<T> : ISqlModel
		where T : ISqlSchema
	{
		Dictionary<string, T> Schemas { get; set; }
	}
}
