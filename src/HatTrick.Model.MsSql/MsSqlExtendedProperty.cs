using HatTrick.Model.Sql;
using System;

namespace HatTrick.Model.MsSql
{
    public class MsSqlExtendedProperty : ISqlExtendedProperty
    {
        #region interface
        public int MajorId { get; set; }//table_id or view_id

        public int? MinorId { get; set; }//column_id

        public string Name { get; set; }

        public string Value { get; set; }

        public string Meta { get; set; }
        #endregion

        #region apply
        public void Apply(Action<MsSqlExtendedProperty> action)
        {
            action(this);
        }
        #endregion
    }
}
