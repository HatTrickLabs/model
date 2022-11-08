select
sys.objects.name as table_name, 
sys.objects.object_id,
sys.objects.schema_id,
sys.schemas.name as schema_name
from sys.objects
inner join sys.schemas on sys.objects.schema_id = sys.schemas.schema_id
where type = 'U'
order by table_name asc;