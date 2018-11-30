SELECT 
sys.objects.name as table_name, 
sys.objects.object_id,
sys.schemas.name as schema_name
FROM sys.objects
INNER JOIN sys.schemas on sys.objects.schema_id = sys.schemas.schema_id
WHERE type = 'U' order by table_name asc;