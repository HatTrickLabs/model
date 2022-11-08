using HatTrick.Model.Sql;
using System;
using System.Collections.Generic;
using System.Data;

namespace HatTrick.Model.MsSql
{
    public class MsSqlRelationship : IDatabaseObjectModifier<MsSqlRelationship>, ISqlRelationship, IChildOf<MsSqlTable>
    {
        #region internals
        private MsSqlTable? _parent;
        #endregion

        #region interface
        public string Name { get; set; } = string.Empty;

        public string Meta { get; set; } = string.Empty;

        public string Identifier { get; set; } = string.Empty;

        public string ParentIdentifier => _parent?.Identifier ?? string.Empty;

        public IList<string> BaseColumnIdentifiers { get; set; } = new List<string>();

        public string? ReferenceTableIdentifier { get; set; }//Foreign Key Table

        public IList<string> ReferenceColumnIdentifiers { get; set; } = new List<string>();
        #endregion

        #region methods
        public void Apply(Action<MsSqlRelationship> action)
        {
            action(this);
        }

        public void SetParent(MsSqlTable table)
        {
            _parent = table;
        }

        public MsSqlTable? GetParent()
        {
            return _parent;
        }

        public override string ToString()
            => $"{Identifier}:{Name} {ParentIdentifier}<->{ReferenceTableIdentifier}";
        #endregion
    }
}