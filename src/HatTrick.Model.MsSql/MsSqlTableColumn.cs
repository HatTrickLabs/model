using HatTrick.Model.Sql;

namespace HatTrick.Model.MsSql
{
    public class MsSqlTableColumn : MsSqlColumn, ISqlTableColumn
	{
        #region set parent
        public void SetParent(MsSqlTable table)
        {
            base.SetParent(table);
        }
        #endregion
    }
}
