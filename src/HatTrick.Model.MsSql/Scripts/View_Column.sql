SELECT
sys.objects.name as view_name, 
sys.objects.object_id as view_id,
sys.columns.name as column_name, 
sys.columns.column_id,
sys.columns.is_nullable,
sys.types.name as data_type_name,
sys.columns.max_length,
sys.columns.scale,
sys.columns.precision,
sys.columns.is_computed,
sys.columns.is_identity
FROM sys.columns
INNER JOIN sys.objects ON sys.objects.object_id = sys.columns.object_id
INNER JOIN sys.types ON sys.columns.user_type_id = sys.types.user_type_id
WHERE sys.objects.type = 'V'
order by view_name asc, column_id asc;