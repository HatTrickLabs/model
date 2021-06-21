select
sys.objects.name as trigger_name,
sys.objects.object_id,
sys.schemas.name as schema_name,
sys.tables.object_id as table_object_id,
sys.tables.name as table_name,
sys.triggers.is_disabled,
sys.triggers.is_instead_of_trigger,
STUFF((select ',' + te.type_desc FROM sys.trigger_events te where te.object_id = sys.objects.object_id for xml path('')), 1, 1, '') as type_desc
from sys.objects
INNER JOIN sys.schemas on sys.objects.schema_id = sys.schemas.schema_id
INNER JOIN sys.triggers on sys.objects.object_id = sys.triggers.object_id
INNER JOIN sys.tables on sys.objects.parent_object_id = sys.tables.object_id
where sys.objects.type = 'TR' and sys.triggers.is_ms_shipped = 0;