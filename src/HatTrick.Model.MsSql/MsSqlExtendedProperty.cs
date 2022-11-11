using HatTrick.Model.Sql;
using System;

namespace HatTrick.Model.MsSql
{
    public class MsSqlExtendedProperty : IMsSqlExtendedProperty, IChildOf<IDatabaseObject>
    {
        #region internals
        private IDatabaseObject? _parent;
        #endregion

        #region interface
        public string Name { get; set; } = string.Empty;

        public string Meta { get; set; } = string.Empty;

        public string? ParentIdentifier => _parent?.Identifier;

        public string? Value { get; set; } = string.Empty;

        public string Identifier { get; set; } = string.Empty;//table_id or view_id

        public string? MinorIdentifier { get; set; }//column_id        
        #endregion

        #region apply
        public void Apply(Action<MsSqlExtendedProperty> action)
        {
            action(this);
        }

        public IDatabaseObject? GetParent()
        {
            return _parent;
        }

        public void SetParent(IDatabaseObject parent)
        {
            _parent = parent;
        }

        public override string ToString()
            => $"{Identifier}:{Name} {Value}";
        #endregion
    }
}
