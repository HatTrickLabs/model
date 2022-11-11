using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace HatTrick.Model.Sql
{
    public class DatabaseObjectNotFoundException<T> : KeyNotFoundException
        where T : IDatabaseObject
    {
        public string Name { get; private set; }

        public DatabaseObjectNotFoundException(string name) : base($"{typeof(T).Name} with name '{name}' was not found.") { Name = name; }
        public DatabaseObjectNotFoundException(string name, string message) : base(message) { Name = name; }
        public DatabaseObjectNotFoundException(string name, string message, Exception innerException) : base(message, innerException) { Name = name; }
        public DatabaseObjectNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { Name = info.GetString(nameof(Name))!; }
    }
}
