--------------------------
USE Ql_DATHANG_BANHANG
GO

ALTER DATABASE Ql_DATHANG_BANHANG
ADD FILEGROUP FG4

ALTER DATABASE Ql_DATHANG_BANHANG
ADD FILEGROUP FG5

ALTER DATABASE Ql_DATHANG_BANHANG
ADD FILEGROUP FG6

ALTER DATABASE Ql_DATHANG_BANHANG
ADD FILEGROUP FG7

ALTER DATABASE Ql_DATHANG_BANHANG
ADD FILEGROUP FG8

ALTER DATABASE Ql_DATHANG_BANHANG
ADD FILE (
	NAME = FG4_1,                                  -- Tên file
	FILENAME = 'D:\PartitionDB\FG4\DBPartition_4.ndf',-- đường dẫn vật lý 
	SIZE = 1MB,										  -- size of file (MB)
	MAXSIZE = UNLIMITED,							  -- số MB tối đa (chế độ không giới hạn )
	FILEGROWTH = 1									  -- số byte tự thêm vào khi vượt qua SIZE hiện tại
) TO FILEGROUP FG4
go
ALTER DATABASE Ql_DATHANG_BANHANG
ADD FILE (
	NAME = FG5_3,
	FILENAME = 'D:\PartitionDB\FG5\DBPartition_5.ndf',
	SIZE = 1MB,
	MAXSIZE = UNLIMITED,
	FILEGROWTH = 1
) TO FILEGROUP FG5

ALTER DATABASE Ql_DATHANG_BANHANG
ADD FILE (
	NAME = FG6_5,
	FILENAME = 'D:\PartitionDB\FG6\DBPartition_6.ndf',
	SIZE = 1MB,
	MAXSIZE = UNLIMITED,
	FILEGROWTH = 1
) TO FILEGROUP FG6

ALTER DATABASE Ql_DATHANG_BANHANG
ADD FILE (
	NAME = FG7_7,
	FILENAME = 'D:\PartitionDB\FG7\DBPartition_7.ndf',
	SIZE = 1MB,
	MAXSIZE = UNLIMITED,
	FILEGROWTH = 1
) TO FILEGROUP FG7

ALTER DATABASE Ql_DATHANG_BANHANG
ADD FILE (
	NAME = FG8_9,
	FILENAME = 'D:\PartitionDB\FG8\DBPartition_8.ndf',
	SIZE = 1MB,
	MAXSIZE = UNLIMITED,
	FILEGROWTH = 1
) TO FILEGROUP FG8


-- check
SELECT NAME AS [DB FILENAME],PHYSICAL_NAME AS [BD File Path]
FROM sys.database_files
WHERE type_desc='ROWS'


--B2 : TẠO PARTITION FUNCTION ( chia theo ngày )
CREATE PARTITION FUNCTION dateCreatedPartitions(datetime)
AS RANGE LEFT  --/RIGHT
FOR VALUES('2020-01-01','2021-01-01','2022-01-01','2023-01-01','2024-01-01')
go

--B3:  TẠO LƯỢC ĐỒ PARTITION SCHEME ÁNH XẠ CÁC PHÂN MẢNH CỦA BẢNG VÀO CÁC FILEGROUP

CREATE PARTITION SCHEME dateCreatedPartitionsScheme
AS PARTITION dateCreatedPartitions
TO (FG4,FG5,FG6,FG7,FG8,[PRIMARY])	
go

--B4 : TẠO CLUSTER INDEX TRÊN CỘT CHIA
-- xóa khóa ngoại bảng muốn phân mảnh (NẾU CÓ)
--ALTER TABLE DONDATHANG
--DROP CONSTRAINT DONDATHANG_PK

-- tạo khóa chính với non-ClusterIndex
ALTER TABLE DONDATHANG
ADD PRIMARY KEY NONCLUSTERED(MA_DON ASC)
ON [PRIMARY]

CREATE PARTITION SCHEME dateCreatedPartitionsScheme
AS PARTITION dateCreatedPartitions
TO (FG4,FG5,FG6,FG7,FG8,[PRIMARY]) 
go
-- tạo thuộc tính cluster index trên (NGAY_TAO)
CREATE CLUSTERED INDEX IX_NGAY_TAO_DATE
ON DONDATHANG
(
	NGAY_TAO
) ON dateCreatedPartitionsScheme(NGAY_TAO)

go

SELECT 
	p.partition_number AS partition_number,
	f.name AS file_group, 
	p.rows AS row_count
FROM sys.partitions p
JOIN sys.destination_data_spaces dds ON p.partition_number = dds.destination_id
JOIN sys.filegroups f ON dds.data_space_id = f.data_space_id
WHERE OBJECT_NAME(OBJECT_ID) = 'DONDATHANG'
order by partition_number
 