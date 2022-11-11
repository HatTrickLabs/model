using HatTrick.Model.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HatTrick.Model.MsSql
{
	public class MsSqlModelAccessor : SqlModelAccessor<MsSqlModel>
	{
		private const char _segmentStart = '[';
        private const char _segmentEnd = ']';
        
		#region constructors
        public MsSqlModelAccessor(MsSqlModel model) : base(model)
		{

		}
		#endregion

		#region resolve item
		public override INamedMeta? ResolveItem(string path)
		{
			if (path == null || path.Length == 0)
				throw new ArgumentException($"{nameof(path)} must contain a value");

			if (path == ".")
				return Model;

			string[] segments = path.SplitPath(_segmentStart, _segmentEnd);

			//-Schemas
			//  - Tables
			//    - Columns
			//    - Indexes
			//  - Views
			//    - Columns
			//  - Procedures
			//    - Parameters
			//  - Relationships
			MsSqlSchema? schema = null;
			MsSqlTable? table = null;
			MsSqlColumn? column = null;
			MsSqlIndex? index = null;
			MsSqlView? view = null;
			MsSqlProcedure? procedure = null;
			MsSqlParameter? parameter = null;
			MsSqlRelationship? relationship = null;

			int i; //declare outside loop scope so we can ensure we found the path ALL the way through on exit...
			INamedMeta? namedMeta = null;
			for (i = 0; i < segments.Length; i++)
			{
				string key = segments[i];
				if (i == 0)
				{
					if (Model.Schemas.Contains(key))
						namedMeta = schema = Model.Schemas[key];
					else
						break;
				}
				else if (i == 1)
				{
					if (schema!.Tables.Contains(key))
						namedMeta = table = schema.Tables[key];
					else if (schema.Views.Contains(key))
						namedMeta = view = schema.Views[key];
					else if (schema.Procedures.Contains(key))
						namedMeta = procedure = schema.Procedures[key];
					else
						break;
				}
				else if (i == 2)
				{
					if (table != null)
					{
						if (table.Columns.Contains(key))
							namedMeta = column = table.Columns[key];
						else if (table.Indexes.Contains(key))
							namedMeta = index = table.Indexes[key];
                        else if (table.Relationships.Contains(key))
                            namedMeta = relationship = table.Relationships[key];
                        else
                            break;
					}
					else if (view != null)
					{
						if (view.Columns.Contains(key))
							namedMeta = column = view.Columns[key];
						else
							break;
					}
					else if (procedure != null)
					{
						if (procedure.Parameters.Contains(key))
							namedMeta = parameter = procedure.Parameters[key];
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

			string[] segments = path.SplitPath(_segmentStart, _segmentEnd);
			string? nextSegment = null;

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

			//common lists
			List<MsSqlSchema> schemas = new();
			List<MsSqlTable> tables = new();
			List<MsSqlTableColumn> tableColumns = new();
			List<MsSqlView> views = new();
			List<MsSqlViewColumn> viewColumns = new();
			List<MsSqlProcedure> procs = new();

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
                    foreach (var schema in schemas)
					{
						tables.AddRange(schema.Tables.GetMatchList(nextSegment, IsStringMatch));
						views.AddRange(schema.Views.GetMatchList(nextSegment, IsStringMatch));
						procs.AddRange(schema.Procedures.GetMatchList(nextSegment, IsStringMatch));
					}
					if (pathDepth == i)
					{
						set.AddRange(tables);
						set.AddRange(views);
						set.AddRange(procs);
					}
				}
				else if (i == 2)
				{
                    List<MsSqlRelationship> relations = new();
                    List<MsSqlIndex> indexes = new();
                    List<MsSqlTrigger> triggers = new();
                    List<MsSqlExtendedProperty> tableExtProps = new();
					foreach (var table in tables)
					{
						tableColumns.AddRange(table.Columns.GetMatchList(nextSegment, IsStringMatch));
						indexes.AddRange(table.Indexes.GetMatchList(nextSegment, IsStringMatch));
						triggers.AddRange(table.Triggers.GetMatchList(nextSegment, IsStringMatch));
                        relations.AddRange(table.Relationships.GetMatchList(nextSegment, IsStringMatch));
                        tableExtProps.AddRange(table.ExtendedProperties.GetMatchList(nextSegment, IsStringMatch));
					}

                    List<MsSqlExtendedProperty> viewExtProps = new();
					foreach (var view in views)
					{
						viewColumns.AddRange(view.Columns.GetMatchList(nextSegment, IsStringMatch));
						viewExtProps.AddRange(view.ExtendedProperties.GetMatchList(nextSegment, IsStringMatch));
					}

                    List<MsSqlParameter> parameters = new();
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
                        set.AddRange(relations);
                    }
                }
				else if (i == 3)
				{
                    List<MsSqlExtendedProperty> tableColExtProps = new();
					foreach (var col in tableColumns)
					{
						tableColExtProps.AddRange(col.ExtendedProperties.GetMatchList(nextSegment, IsStringMatch));
					}

                    List<MsSqlExtendedProperty> viewColExtProps = new();
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
