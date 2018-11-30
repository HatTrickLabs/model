SELECT
sys.indexes.name as index_name,
sys.indexes.index_id,
sys.objects.name as table_name,
sys.index_columns.object_id as table_id,
sys.columns.name as column_name,
sys.index_columns.column_id,
sys.indexes.type as index_type_code,
sys.indexes.is_unique,
sys.index_columns.is_descending_key,
sys.index_columns.key_ordinal,
sys.indexes.is_primary_key,
sys.index_columns.is_included_column
FROM
sys.index_columns
INNER JOIN sys.objects ON sys.index_columns.object_id = sys.objects.object_id
INNER JOIN sys.columns ON sys.columns.object_id = sys.index_columns.object_id AND sys.index_columns.column_id = sys.columns.column_id
INNER JOIN sys.indexes ON sys.indexes.object_id = sys.index_columns.object_id AND sys.indexes.index_id = sys.index_columns.index_id
WHERE sys.objects.type = 'U' and sys.indexes.type IN (1,2);