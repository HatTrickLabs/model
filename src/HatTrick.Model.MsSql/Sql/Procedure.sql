select 
sys.objects.name as sproc_name, 
sys.objects.object_id,
sys.schemas.name as schema_name,
sys.procedures.is_auto_executed as is_startup_sproc
from sys.objects
INNER JOIN sys.schemas on sys.objects.schema_id = sys.schemas.schema_id
INNER JOIN sys.procedures on sys.procedures.object_id = sys.objects.object_id
where sys.objects.type = 'P' order by sproc_name asc;