using System;
using System.Data;
using System.Data.Common;

namespace HatTrick.Model.Sql
{
    public abstract class SqlModelBuilder<T>
        where T : class, ISqlModel, new()
    {
        #region internals
        private ResourceAccessor<T> _resourceAccessor;
        private DbConnection _connection;
        #endregion

        #region interface
        public Action<Exception> OnError { get; set; }
        #endregion

        #region constructors
        protected SqlModelBuilder(string resourcePath)
        {
            _resourceAccessor = new ResourceAccessor<T>(resourcePath);
        }
        #endregion

        #region get connection
        protected abstract DbConnection GetConnection();
        #endregion

        #region ensure connection
        private DbConnection EnsureConnection()
        {
            if (_connection is null)
            {
                _connection = GetConnection();
            }
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }

            return _connection;
        }
        #endregion

        #region close connection
        private void CloseConnection()
        {
            if (_connection != null)
            {
                if (_connection.State != ConnectionState.Closed)
                {
                    _connection.Close();
                }
            }
        }
        #endregion

        #region build
        public T Build()
        {
            T model = new T();

            try
            {
                BuildModel(ref model);
            }
            catch(Exception ex)
            {
                if (this.OnError != null)
                { this.OnError.Invoke(ex); }
                else
                { throw; }
            }
            finally
            {
                this.CloseConnection();
            }
            return model;
        }

        protected abstract void BuildModel(ref T model);
        #endregion

        #region execute sql
        protected virtual void ExecuteSql(string sql, Action<DbDataReader> action)
        {
            DbDataReader reader = null;
            try
            {
                var cmd = this.EnsureConnection().CreateCommand();
#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
                cmd.CommandText = sql;
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities
                cmd.CommandType = CommandType.Text;

                reader = cmd.ExecuteReader(CommandBehavior.SingleResult);

                action(reader);
            }
            catch
            {
                throw;
            }
            finally
            {
                if (reader != null && !reader.IsClosed) { reader.Close(); }
            }
        }
        #endregion

        #region get resource
        public string GetResource(string name)
        {
            return _resourceAccessor.Get(name);
        }
        #endregion
    }
}
