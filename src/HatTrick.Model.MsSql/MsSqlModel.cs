using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace HatTrick.Model.MsSql
{
    public class MsSqlModel : INamedMeta
    {
        #region interface
        public string Name
        {
            get { return this.MsSqlDbName; }
            set { this.MsSqlDbName = value; }
        }

        [Obsolete("This property is marked for deletion in future versions.  Use property 'Name' instead.")]
        public string MsSqlDbName { get; set; }

        public EnumerableNamedMetaSet<MsSqlSchema> Schemas { get; set; }

        public string Meta { get; set; }
        #endregion

        #region constructors
        public MsSqlModel()
        {
        }
        #endregion

        #region resolve meta
        public INamedMeta ResolveItem(string path)
        {
            return this.ResolveItem(path?.Split('.') ?? null);
        }
        //-Schemas
        //  - Tables
        //    - Columns
        //    - Indexes
        //  - Views
        //    - Columns
        //  - Procedures
        //    - Parameters
        //  - Relationships
        //TODO: JRod, this works, but should try refactor into a recursive call working against EnumerableNamedSet<T> 
        //      OR do internal yeild  return on the EnumerableNamedSet in same resolve order being 
        public INamedMeta ResolveItem(string[] path)
        {
            if (path == null || path.Length == 0)
            { throw new ArgumentException($"{nameof(path)} must contain a value"); }

            INamedMeta namedMeta = null;

            MsSqlSchema s = null;
            MsSqlTable t = null;
            MsSqlColumn c = null;
            MsSqlIndex ix = null;
            MsSqlView v = null;
            MsSqlProcedure p = null;
            MsSqlParameter pm = null;
            MsSqlRelationship r = null;
            for (int i = 0; i < path.Length; i++)
            {
                string key = path[i];
                if (i == 0)
                {
                    if (this.Schemas.Contains(key))
                    { namedMeta = s = this.Schemas[key]; }
                }
                else if (i == 1)
                {
                    if (s.Tables.Contains(key))
                    { namedMeta = t = s.Tables[key]; }
                    else if (s.Views.Contains(key))
                    { namedMeta = v = s.Views[key]; }
                    else if (s.Procedures.Contains(key))
                    { namedMeta = p = s.Procedures[key]; }
                    else if (s.Relationships.Contains(key))
                    { namedMeta = r = s.Relationships[key]; }
                }
                else if (i == 2)
                {
                    if (t != null)
                    { 
                        if (t.Columns.Contains(key))
                        { namedMeta = c = t.Columns[key]; }
                        else if (t.Indexes.Contains(key))
                        { namedMeta = ix = t.Indexes[key]; }
                    }
                    else if (v != null)
                    {
                        if (v.Columns.Contains(key))
                        { namedMeta = c = v.Columns[key]; }
                    }
                    else if (p != null)
                    {
                        if (p.Parameters.Contains(key))
                        { namedMeta = pm = p.Parameters[key]; }
                    }
                }
            }

            return namedMeta;
        }
        #endregion
    }
}