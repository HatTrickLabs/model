using System.Collections.Generic;

namespace HatTrick.Model.Sql
{
    public interface ISqlRelationship : IDatabaseObject
    {
        IList<string> BaseColumnIdentifiers { get; set; }

        string? ReferenceTableIdentifier { get; set; }//Foreign Key Table

        IList<string> ReferenceColumnIdentifiers { get; set; }

    }
}
