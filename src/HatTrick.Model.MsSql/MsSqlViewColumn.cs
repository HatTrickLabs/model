using System;
using System.Collections.Generic;
using System.Text;

namespace HatTrick.Model.MsSql
{
	public class MsSqlViewColumn : MsSqlColumn
	{
        #region set parent
        public void SetParent(MsSqlView view)
        {
            base.SetParent(view);
        }
        #endregion
    }
}
