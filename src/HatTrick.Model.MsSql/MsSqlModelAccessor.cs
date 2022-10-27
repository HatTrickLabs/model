using HatTrick.Model.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HatTrick.Model.MsSql
{
	public class MsSqlModelAccessor : SqlModelAccessor<MsSqlSchema>
	{
		#region constructors
		public MsSqlModelAccessor(MsSqlModel model) : base(model)
		{

		}
		#endregion

		#region resolve item
		public override INamedMeta ResolveItem(string path)
		{
			if (path == null || path.Length == 0)
				throw new ArgumentException($"{nameof(path)} must contain a value");

			if (path == ".")
				return Model;

			string[] segments = path.SplitPath();

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
					if (Model.Schemas.Contains(key))
						namedMeta = s = Model.Schemas[key];
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
		public override List<INamedMeta> ResolveItemSet(string path)
		{
			if (path == null || path.Length == 0)
				throw new ArgumentException("argument must contain a value", nameof(path));

			string[] segments = path.SplitPath();
			string nextSegment = null;

			if (path == ".")
				return new List<INamedMeta> { Model };

			//-Schemas
			//  - Tables
			//    - Columns
			//      - Ext Props
			//    - Indexes
			//    - Triggers
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
			List<MsSqlTrigger> triggers = null;
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
					schemas = Model.Schemas.GetMatchList(nextSegment, IsStringMatch);
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
					triggers = new List<MsSqlTrigger>();
					tableExtProps = new List<MsSqlExtendedProperty>();
					foreach (var table in tables)
					{
						tableColumns.AddRange(table.Columns.GetMatchList(nextSegment, IsStringMatch));
						indexes.AddRange(table.Indexes.GetMatchList(nextSegment, IsStringMatch));
						triggers.AddRange(table.Triggers.GetMatchList(nextSegment, IsStringMatch));
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
						set.AddRange(triggers);
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
		#endregion
	}
}
