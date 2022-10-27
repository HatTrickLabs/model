using System.IO;
using System.Reflection;
using System;

namespace HatTrick.Model.Sql
{
    public class ResourceAccessor<T>
        where T : class, ISqlModel
    {
        private string _path;
        private Assembly _assembly;
        private Assembly Assembly => _assembly ?? (_assembly = typeof(T).Assembly);

        public ResourceAccessor(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException($"{nameof(path)} is required to access resources in assembly {Assembly.GetName().Name}");
            _path = path.EndsWith(".") ? path : path + ".";
        }

        #region get
        public string Get(string name)
        {
            string fullName = $"{_path}{name}.sql";
            string output = null;
            using (Stream stream = Assembly.GetManifestResourceStream(fullName))
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
