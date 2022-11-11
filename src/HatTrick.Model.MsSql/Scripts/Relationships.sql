select
s.name as schema_name,
r.name as relationship_name,
bt.object_id as base_table_id,
bt.name as base_table_name,
bc.column_id as base_column_id,
bc.name as base_column_name,
rt.object_id as referenced_table_id,
rt .name as referenced_table_name ,
rc.column_id as referenced_column_id,
rc.name as referenced_column_name
from sys.foreign_key_columns as fkc
inner join sys.objects as r  on r.object_id  = fkc.constraint_object_id
inner join sys.objects as bt on bt.object_id = fkc.referenced_object_id
inner join sys.columns as bc on bc.object_id = fkc.referenced_object_id AND bc.column_id = fkc.referenced_column_id
inner join sys.objects as rt on rt.object_id = fkc.parent_object_id
inner join sys.columns as rc on rc.object_id = fkc.parent_object_id AND rc.column_id = fkc.parent_column_id
inner join sys.schemas as s  on s.schema_id  = r.schema_id
order by relationship_name asc;
