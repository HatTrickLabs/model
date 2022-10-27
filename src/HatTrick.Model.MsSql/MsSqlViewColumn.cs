using HatTrick.Model.Sql;

namespace HatTrick.Model.MsSql
{
    public class MsSqlViewColumn : MsSqlColumn, ISqlViewColumn
	{
        #region set parent
        public void SetParent(MsSqlView view)
        {
            base.SetParent(view);
        }
        #endregion
    }
}
