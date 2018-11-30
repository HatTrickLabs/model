SELECT
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
FROM sys.columns
INNER JOIN sys.objects ON sys.objects.object_id = sys.columns.object_id
INNER JOIN sys.types ON sys.columns.user_type_id = sys.types.user_type_id
LEFT OUTER JOIN sys.default_constraints ON sys.columns.default_object_id = sys.default_constraints.object_id
WHERE sys.objects.type = 'U';