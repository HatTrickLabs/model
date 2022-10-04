select
sys.objects.name as sproc_name, 
sys.objects.object_id as sproc_id,
sys.parameters.name as parameter_name, 
sys.parameters.parameter_id,
sys.types.name as data_type_name,
sys.parameters.max_length,
sys.parameters.scale,
sys.parameters.precision,
sys.parameters.is_output,
sys.parameters.has_default_value,
sys.parameters.default_value,
sys.parameters.is_readonly,
sys.parameters.is_nullable
from sys.parameters
inner join sys.objects ON sys.objects.object_id = sys.parameters.object_id
inner join sys.types ON sys.parameters.user_type_id = sys.types.user_type_id
where sys.objects.type = 'P'
order by sproc_name asc, parameter_id asc;