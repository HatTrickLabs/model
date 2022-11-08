select 
sys.objects.name as view_name, 
sys.objects.object_id,
sys.objects.schema_id,
sys.schemas.name as schema_name
from sys.objects
INNER JOIN sys.schemas on sys.objects.schema_id = sys.schemas.schema_id
where type = 'V' order by view_name asc;