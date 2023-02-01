using HatTrick.Model.Sql;
using System;
using System.Collections.Generic;

namespace HatTrick.Model.MySql
{
    public class MySqlTrigger : IDatabaseObjectModifier<MySqlTrigger>, ISqlTrigger, IChildOf<MySqlTable>
    {
        #region internals
        private MySqlTable? _parent;
        #endregion

        #region interface
        public string Name { get; set; } = string.Empty;

        public IDictionary<string, object> Meta { get; set; } = new Dictionary<string, object>();

        public string Identifier { get; set; } = string.Empty;

        public string ParentIdentifier => _parent?.Identifier ?? string.Empty;

        public TriggerEventTimingType EventTimingType { get; set; }

        public TriggerEventActionType EventActionType { get; set; }
        #endregion

        #region methods
        public void Apply(Action<MySqlTrigger> action)
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
