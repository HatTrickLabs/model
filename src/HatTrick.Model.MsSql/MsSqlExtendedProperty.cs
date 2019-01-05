using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HatTrick.Model.MsSql
{
    public class MsSqlExtendedProperty : IName
    {
        #region interface
        public int MajorId { get; set; }//table_id or view_id

        public int? MinorId { get; set; }//column_id

        public string Name { get; set; }

        public string Value { get; set; }
        #endregion

        #region apply
        public void Apply(Action<MsSqlExtendedProperty> action)
        {
            action(this);
        }
        #endregion
    }
}
