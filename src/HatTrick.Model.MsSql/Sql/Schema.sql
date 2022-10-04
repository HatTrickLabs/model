select
* 
from sys.schemas s
where s.principal_id = 1
order by s.name asc;