select 
sys.extended_properties.major_id as view_id,
sys.views.name as view_name,
sys.extended_properties.name,
sys.extended_properties.value
from sys.extended_properties
inner join sys.views on sys.views.object_id = sys.extended_properties.major_id
where sys.extended_properties.minor_id = 0;