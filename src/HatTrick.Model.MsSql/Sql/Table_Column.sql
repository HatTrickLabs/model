select
sys.objects.name as table_name, 
sys.objects.object_id as table_id,
sys.columns.name as column_name, 
sys.columns.column_id,
sys.columns.is_identity, 
sys.columns.default_object_id,
sys.default_constraints.definition as default_definition, 
sys.columns.is_nullable,
sys.types.name as data_type_name,
sys.columns.max_length,
sys.columns.scale,
sys.columns.precision,
sys.columns.is_computed,
sys.default_constraints.definition
from sys.columns
inner join sys.objects on sys.objects.object_id = sys.columns.object_id
inner join sys.types on sys.columns.user_type_id = sys.types.user_type_id
left outer join sys.default_constraints on sys.columns.default_object_id = sys.default_constraints.object_id
where sys.objects.type = 'U'
order by table_name asc, sys.columns.column_id asc;