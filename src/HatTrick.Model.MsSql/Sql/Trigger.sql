select
sys.objects.name as trigger_name,
sys.objects.object_id,
sys.schemas.name as schema_name,
sys.tables.object_id as table_object_id,
sys.tables.name as table_name,
sys.triggers.is_disabled,
sys.triggers.is_instead_of_trigger,
sys.trigger_events.type_desc
from sys.objects
INNER JOIN sys.schemas on sys.objects.schema_id = sys.schemas.schema_id
INNER JOIN sys.triggers on sys.objects.object_id = sys.triggers.object_id
INNER JOIN sys.trigger_events on sys.objects.object_id = sys.trigger_events.object_id
INNER JOIN sys.tables on sys.objects.parent_object_id = sys.tables.object_id
where sys.objects.type = 'TR' and sys.triggers.is_ms_shipped = 0;