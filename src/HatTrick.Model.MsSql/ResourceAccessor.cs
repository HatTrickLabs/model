using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace HatTrick.Model.MsSql
{
    public class ResourceAccessor
    {
        #region get
        public string Get(string name)
        {
            Assembly assem = Assembly.GetExecutingAssembly();
            string fullName = $"HatTrick.Model.MsSql.Sql.{name}.sql";
            string output = null;
            using (Stream stream = assem.GetManifestResourceStream(fullName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    output = reader.ReadToEnd();
                }
            }
            return output;
        }
        #endregion
    }
}
