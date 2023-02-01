using HatTrick.Model.Sql;
using System;
using System.Collections.Generic;

namespace HatTrick.Model.MsSql
{
    public class MsSqlTrigger : IDatabaseObjectModifier<MsSqlTrigger>, ISqlTrigger, IChildOf<MsSqlTable>
    {
        #region internals
        private MsSqlTable? _parent;
        #endregion

        #region interface
        public string Name { get; set; } = string.Empty;

        public IDictionary<string, object> Meta { get; set; } = new Dictionary<string, object>();

        public string Identifier { get; set; } = string.Empty;

        public string ParentIdentifier => _parent?.Identifier ?? string.Empty;
        
        public bool IsDisabled { get; set; }

        public bool IsInsteadOfTrigger { get; set; }

        public TriggerEventType EventType { get; set; }
        #endregion

        #region methods
        public void Apply(Action<MsSqlTrigger> action)
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
            => $"{Identifier}:{Name}";
        #endregion
    }
}
