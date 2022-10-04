select 
sys.extended_properties.major_id as table_id,
sys.tables.name as table_name,
sys.extended_properties.name,
sys.extended_properties.value
from sys.extended_properties
inner join sys.tables on sys.tables.object_id = sys.extended_properties.major_id
where sys.extended_properties.minor_id = 0
order by table_name asc, sys.extended_properties.name asc;