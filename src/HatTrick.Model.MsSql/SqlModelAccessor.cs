using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HatTrick.Model.MsSql
{
	public class SqlModelAccessor
	{
		#region internals
		private readonly char _defaultWildcard = '*';

		private char _wildcardOverride;
		private MsSqlModel _model;
		#endregion

		#region interface
		public char Wildcard => (_wildcardOverride == '\0') ? _defaultWildcard : _wildcardOverride;
		#endregion

		#region constructors
		public SqlModelAccessor(MsSqlModel model)
		{
			_model = model;
			_wildcardOverride = '\0';
		}
		#endregion

		#region apply wildcard override
		public void ApplyWildcardOverride(char wildcard)
		{
			_wildcardOverride = wildcard;
		}
		#endregion

		#region resolve item
		public INamedMeta ResolveItem(string path)
		{
			if (path == null || path.Length == 0)
				throw new ArgumentException($"{nameof(path)} must contain a value");

			if (path == ".")
				return _model;

			string[] segments = this.SplitPath(path);

			//-Schemas
			//  - Tables
			//    - Columns
			//    - Indexes
			//  - Views
			//    - Columns
			//  - Procedures
			//    - Parameters
			//  - Relationships
			MsSqlSchema s = null;
			MsSqlTable t = null;
			MsSqlColumn c = null;
			MsSqlIndex ix = null;
			MsSqlView v = null;
			MsSqlProcedure p = null;
			MsSqlParameter pm = null;
			MsSqlRelationship r = null;
			int i; //declare outside loop scope so we can ensure we found the path ALL the way through on exit...
			INamedMeta namedMeta = null;
			for (i = 0; i < segments.Length; i++)
			{
				string key = segments[i];
				if (i == 0)
				{
					if (_model.Schemas.Contains(key))
						namedMeta = s = _model.Schemas[key];
					else
						break;
				}
				else if (i == 1)
				{
					if (s.Tables.Contains(key))
						namedMeta = t = s.Tables[key];
					else if (s.Views.Contains(key))
						namedMeta = v = s.Views[key];
					else if (s.Procedures.Contains(key))
						namedMeta = p = s.Procedures[key];
					else if (s.Relationships.Contains(key))
						namedMeta = r = s.Relationships[key];
					else
						break;
				}
				else if (i == 2)
				{
					if (t != null)
					{
						if (t.Columns.Contains(key))
							namedMeta = c = t.Columns[key];
						else if (t.Indexes.Contains(key))
							namedMeta = ix = t.Indexes[key]; 
						else
							break;
					}
					else if (v != null)
					{
						if (v.Columns.Contains(key))
							namedMeta = c = v.Columns[key];
						else
							break;
					}
					else if (p != null)
					{
						if (p.Parameters.Contains(key))
							namedMeta = pm = p.Parameters[key];
						else
							break;
					}
				}
			}

			return i == segments.Length ? namedMeta : null;
		}
		#endregion

		#region resolve item set
		public List<INamedMeta> ResolveItemSet(string path)
		{
			if (path == null || path.Length == 0)
				throw new ArgumentException("argument must contain a value", nameof(path));

			string[] segments = this.SplitPath(path);
			string nextSegment = null;

			//-Schemas
			//  - Tables
			//    - Columns
			//      - Ext Props
			//    - Indexes
			//    - Ext Props
			//  - Views
			//    - Columns
			//      - Ext Props
			//	  - Ext Props
			//  - Procedures
			//    - Parameters
			//  - Relationships
			List<MsSqlSchema> schemas = null;

			List<MsSqlTable> tables = null;
			List<MsSqlTableColumn> tableColumns = null;
			List<MsSqlIndex> indexes = null;
			List<MsSqlExtendedProperty> tableColExtProps = null;
			List<MsSqlExtendedProperty> tableExtProps = null;

			List<MsSqlView> views = null;
			List<MsSqlViewColumn> viewColumns = null;
			List<MsSqlExtendedProperty> viewColExtProps = null;
			List<MsSqlExtendedProperty> viewExtProps = null;

			List<MsSqlProcedure> procs = null;
			List<MsSqlParameter> parameters = null;

			List<MsSqlRelationship> relations = null;
			int i; //declare outside loop scope so we can ensure we found the path ALL the way through on exit...
			List<INamedMeta> set = new List<INamedMeta>();
			int pathDepth = segments.Length - 1;
			for (i = 0; i < segments.Length; i++)
			{
				nextSegment = segments[i];
				if (i == 0)
				{
					schemas = _model.Schemas.GetMatchList(nextSegment, IsStringMatch);
					if (i == pathDepth)
					{
						set.AddRange(schemas);
					}
				}
				else if (i == 1)
				{
					tables = new List<MsSqlTable>();
					views = new List<MsSqlView>();
					procs = new List<MsSqlProcedure>();
					relations = new List<MsSqlRelationship>();
					foreach (var schema in schemas)
					{
						tables.AddRange(schema.Tables.GetMatchList(nextSegment, IsStringMatch));
						views.AddRange(schema.Views.GetMatchList(nextSegment, IsStringMatch));
						procs.AddRange(schema.Procedures.GetMatchList(nextSegment, IsStringMatch));
						relations.AddRange(schema.Relationships.GetMatchList(nextSegment, IsStringMatch));
					}
					if (pathDepth == i)
					{
						set.AddRange(tables);
						set.AddRange(views);
						set.AddRange(procs);
						set.AddRange(relations);
					}
				}
				else if (i == 2)
				{
					tableColumns = new List<MsSqlTableColumn>();
					indexes = new List<MsSqlIndex>();
					tableExtProps = new List<MsSqlExtendedProperty>();
					foreach (var table in tables)
					{
						tableColumns.AddRange(table.Columns.GetMatchList(nextSegment, IsStringMatch));
						indexes.AddRange(table.Indexes.GetMatchList(nextSegment, IsStringMatch));
						tableExtProps.AddRange(table.ExtendedProperties.GetMatchList(nextSegment, IsStringMatch));
					}

					viewColumns = new List<MsSqlViewColumn>();
					viewExtProps = new List<MsSqlExtendedProperty>();
					foreach (var view in views)
					{
						viewColumns.AddRange(view.Columns.GetMatchList(nextSegment, IsStringMatch));
						viewExtProps.AddRange(view.ExtendedProperties.GetMatchList(nextSegment, IsStringMatch));
					}

					parameters = new List<MsSqlParameter>();
					foreach (var proc in procs)
					{
						parameters.AddRange(proc.Parameters.GetMatchList(nextSegment, IsStringMatch));
					}
					if (i == pathDepth)
					{
						set.AddRange(tableColumns);
						set.AddRange(indexes);
						set.AddRange(tableExtProps);
						set.AddRange(viewColumns);
						set.AddRange(viewExtProps);
						set.AddRange(parameters);
					}
				}
				else if (i == 3)
				{
					tableColExtProps = new List<MsSqlExtendedProperty>();
					foreach (var col in tableColumns)
					{
						tableColExtProps.AddRange(col.ExtendedProperties.GetMatchList(nextSegment, IsStringMatch));
					}

					viewColExtProps = new List<MsSqlExtendedProperty>();
					foreach (var col in viewColumns)
					{
						viewColExtProps.AddRange(col.ExtendedProperties.GetMatchList(nextSegment, IsStringMatch));
					}

					if (i == pathDepth)
					{
						set.AddRange(tableColExtProps);
					    set.AddRange(viewColExtProps);
					}
				}
			}

			return set;
		}

		public IList<T> ResolveItemSet<T>(string path) where T : INamedMeta
		{
			var set = this.ResolveItemSet(path);
			List<T> typedSet = new List<T>();
			foreach (var item in set)
			{
				if (item is T itm)
					typedSet.Add(itm);
			}
			return typedSet;
		}

		public IList<T> ResolveItemSet<T>(string path, Predicate<T> predicate) where T : INamedMeta
		{
			var set = this.ResolveItemSet(path);
			List<T> filteredSet = new List<T>();
			foreach (var item in set)
			{
				if (item is T itm && predicate(itm))
					filteredSet.Add(itm);
			}
			return filteredSet;
		}
		#endregion

		#region split path
		private string[] SplitPath(string path)
		{
			//walk the entire string to ensure . within [] is maintained
			if (path == null)
				return null;

			if (path == string.Empty)
				return new string[0];

			char c;
			bool inBracket = false;
			List<string> segments = new List<string>();
			StringBuilder segment = new StringBuilder();
			for (int i = 0; i < path.Length; i++)
			{
				c = path[i];
				if (c == '[' || c == ']')
				{
					inBracket = !inBracket;
					continue;
				}

				if (c == '.' && !inBracket)
				{
					segments.Add(segment.ToString());
					segment.Clear();
					continue;
				}

				segment.Append(c);
			}

			if (segment.Length > 0)
			{
				segments.Add(segment.ToString());
			}
			return segments.ToArray();
		}
		#endregion

		#region is string match
		public bool IsStringMatch(string left, string right)
		{
			char wildcard = this.Wildcard;

			if (string.IsNullOrEmpty(left))
				throw new ArgumentException("argument must contain a value", nameof(left));

			if (string.IsNullOrEmpty(right))
				throw new ArgumentException("argument must contain a value", nameof(right));

			if (right.Length == 1 && right[0] == wildcard)
				return true;

			bool match = false;
			bool beginWild = (right[0] == wildcard);
			bool endWild = (right[right.Length - 1] == wildcard);

			if ((beginWild || endWild))
			{
				string sub = right;
				if (beginWild)
				{
					sub = right.Substring(1, right.Length - 1);
				}
				if (endWild)
				{
					sub = sub.Substring(0, sub.Length - 1);
				}
				if (beginWild && endWild)
				{
					match = left.IndexOf(sub, StringComparison.OrdinalIgnoreCase) > -1;
				}
				else if (beginWild)
				{
					match = left.IndexOf(sub, StringComparison.OrdinalIgnoreCase) == (left.Length - sub.Length);
				}
				else if (endWild)
				{
					match = left.IndexOf(sub, StringComparison.OrdinalIgnoreCase) == 0;
				}
			}
			else
			{
				match = string.Compare(left, right, true) == 0;
			}
			return match;
		}
		#endregion
	}
}
