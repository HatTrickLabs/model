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

        public ResourceAccessor(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException($"{nameof(path)} is required to access resources in assembly {typeof(T).Assembly.GetName().Name}");
            _path = path.EndsWith(".") ? path : path + ".";
            _assembly = typeof(T).Assembly;
        }

        #region get
        public string Get(string name, string extension = "sql")
        {
            using var stream = _assembly.GetManifestResourceStream($"{_path}{name}.{extension}");
            if (stream is null)
                throw new ArgumentNullException(nameof(name), $"The resource {_path}{name}.{extension} was not found.");
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
        #endregion
    }
}
