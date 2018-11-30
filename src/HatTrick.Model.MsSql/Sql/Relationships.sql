SELECT
s.name AS schema_name,
r.name AS relationship_name,
bt.object_id AS base_table_id,
bt.name AS base_table_name,
bc.column_id AS base_column_id,
bc.name AS base_column_name,
rt.object_id AS referenced_table_id,
rt .name AS referenced_table_name ,
rc.column_id AS referenced_column_id,
rc.name AS referenced_column_name
FROM sys.foreign_key_columns AS fkc
INNER JOIN sys.objects As r  ON r.object_id  = fkc.constraint_object_id
INNER JOIN sys.objects AS bt ON bt.object_id = fkc.referenced_object_id
INNER JOIN sys.columns As bc ON bc.object_id = fkc.referenced_object_id AND bc.column_id = fkc.referenced_column_id
INNER JOIN sys.objects AS rt ON rt.object_id = fkc.parent_object_id
INNER JOIN sys.columns As rc ON rc.object_id = fkc.parent_object_id AND rc.column_id = fkc.parent_column_id
INNER JOIN sys.schemas as s  ON s.schema_id  = r.schema_id;
