using HatTrick.Model.Sql;
using System;
using System.Collections.Generic;

namespace HatTrick.Model.MySql
{
    public class MySqlRelationship : IDatabaseObjectModifier<MySqlRelationship>, ISqlRelationship, IChildOf<MySqlTable>
    {
        #region internals
        private MySqlTable? _parent;
        #endregion

        #region interface
        public string Name { get; set; } = string.Empty;

        public IDictionary<string, object> Meta { get; set; } = new Dictionary<string, object>();

        public string Identifier { get; set; } = string.Empty;

        public string ParentIdentifier => _parent?.Identifier ?? string.Empty;

        public IList<string> BaseColumnIdentifiers { get; set; } = new List<string>();

        public string? ReferenceTableIdentifier { get; set; }//Foreign Key Table

        public IList<string> ReferenceColumnIdentifiers { get; set; } = new List<string>();
        #endregion

        #region methods
        public void Apply(Action<MySqlRelationship> action)
        {
            action(this);
        }

        public void SetParent(MySqlTable table)
        {
            _parent = table;
        }

        public MySqlTable? GetParent()
        {
            return _parent;
        }
        #endregion
    }
}