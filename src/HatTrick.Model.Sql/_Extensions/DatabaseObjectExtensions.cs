using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HatTrick.Model.Sql
{
    public static class DatabaseObjectExtensions
    {
        public static DatabaseObjectList<ISqlRelationship> GetForeignKeyRelationships(this ISqlTable table, DatabaseObjectList<ISqlRelationship> relationships)
        {
            return relationships.Where(r => r.ReferenceTableIdentifier == table.Identifier).ToDatabaseObjectList();
        }
    }
}
