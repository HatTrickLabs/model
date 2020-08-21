using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace HatTrick.Model.MsSql
{
	public class MsSqlTableColumn : MsSqlColumn
	{
        #region set parent
        public void SetParent(MsSqlTable table)
        {
            base.SetParent(table);
        }
        #endregion
    }
}
