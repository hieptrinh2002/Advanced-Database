
-- procedure được sử dụng trong APP

USE Ql_DATHANG_BANHANG
go
--====================================================== LOGIN ==========================================================
create proc ThemTK @idtk int, @username nvarchar(255), @pass char(100), @loaitk int
as
insert into TAIKHOAN(ID_TAI_KHOAN,TEN_DANG_NHAP,MAT_KHAU,LOAI_TK,TRANG_THAI)
values (@idtk,@username,@pass,@loaitk,1)
go

create proc ThemKH @makh char(8), @hoten nvarchar(255), @diachi nvarchar(255), @sdt char(10), @email nvarchar(100)
as
insert into KHACHHANG(MA_KHACH_HANG,HOTEN,DIACHI,SDT,EMAIL,ID_TAI_KHOAN)
values(@makh,@hoten,@diachi,@sdt,@email,null)
go

create proc ThemNV @manv char(8), @hoten nvarchar(255), @diachi nvarchar(255), @sdt char(10), @email char(100)
as
insert into NHANVIEN(MA_NHAN_VIEN,TEN_NHAN_VIEN,DIA_CHI,SDT,ID_TAI_KHOAN,GMAIL)
values(@manv,@hoten,@diachi,@sdt,null,@email)
go

create proc ThemTX @matx char(8), @hoten varchar(255), @sdt char(10), @bienso char(10), @cmnd char(10), @mathue char(5), @email char(100), @khuvuc nvarchar(255)
as
insert into TAIXE(MA_TAI_XE,HOTEN,SDT,BIEN_SO,CMND,MA_THUE,EMAIL,KHU_VUC,TRANG_THAI,STK,ID_TAI_KHOAN)
values(@matx,@hoten,@sdt,@bienso,@cmnd,@mathue,@email,@khuvuc,null,null,null)
go

create proc ThemCH @mach char(8), @tench nvarchar(255), @email char(100), @tp nvarchar(255), @quan nvarchar(255), @sdt char(10), @sochinhanh int, @nguoidaidien nvarchar(50), @masothue char(5)
as
insert into CUAHANG(MA_CUA_HANG,TEN_CUA_HANG,EMAIL,THANH_PHO,QUAN,SDT,SO_CHI_NHANH,NGUOI_DAI_DIEN,MA_SO_THUE,ID_TAI_KHOAN)
values(@mach,@tench,@email,@tp,@quan,@sdt,@sochinhanh,@nguoidaidien,@masothue,null)
go

--============================================================== ADMIN ==========================================================================

create proc capnhapTK @tdn nvarchar(255), @mk char(100)
as
update TAIKHOAN
set MAT_KHAU = @mk where TEN_DANG_NHAP = @tdn
go

create proc capnhapNV @manv char(8), @diachi nvarchar(255), @sdt char(10), @gmail char(100)
as
update NHANVIEN
set DIA_CHI = @diachi, SDT = @sdt, GMAIL = @gmail where MA_NHAN_VIEN = @manv
go

--Tạo 1 tk adm để test
insert into TAIKHOAN(ID_TAI_KHOAN,TEN_DANG_NHAP,MAT_KHAU,LOAI_TK,TRANG_THAI)
values (999999,'ADM','111111',4,1)


-- ================================================================ NHÂN VIÊN ===========================================================================

go

create proc sp_getHopDongByMaHD @MaHD char(8) , @status int 
as
	begin 
		select * from HOPDONG as HD where HD.MA_HOP_DONG = @MaHD and TRANG_THAI = @status
	end
go

create proc UpdateThoiHanHopDong @maHD char(8), @TgKetThuc_new datetime
as
	update HOPDONG
	set TG_KET_THUC = @TgKetThuc_new
	where MA_HOP_DONG = @maHD and TRANG_THAI = 1
go

create proc SelectHopDong_ThoiGian_BD_ThoiGian_KT @ThoiGian_BD datetime , @ThoiGian_KT datetime
as
	select * from HOPDONG as HD
	where @ThoiGian_BD <= HD.TG_BAT_DAU and HD.TG_KET_THUC <= @ThoiGian_KT and TRANG_THAI = 0
go

create proc SlectHopDong_TenCuaHangGanDung @string nvarchar(255) , @TrangThai int
as
begin
	select HD.MA_HOP_DONG,HD.NGAY_TAO,HD.TG_BAT_DAU,HD.TG_KET_THUC,HD.MA_CUA_HANG,HD.STK,HD.MA_NHAN_VIEN,HD.TRANG_THAI
	from HOPDONG HD , CUAHANG CH where CH.MA_CUA_HANG = hd.MA_CUA_HANG AND
	CH.TEN_CUA_HANG Like '%'+@string+'%' AND HD.TRANG_THAI = @TrangThai
end
go

exec SlectHopDong_TenCuaHangGanDung 'EF' , 1
go

--oke
create proc sp_getHopDongByMaCH @MaCH char(8) , @status int 
as
	begin 
		select * from HOPDONG as HD where HD.MA_CUA_HANG = @MaCH and TRANG_THAI = @status
	end
go

create proc  sq_updateStatus_HopDong @maHD char(8) , @TrangThai INT
as
begin
	update HOPDONG 
	set TRANG_THAI = @TrangThai 
	WHERE MA_HOP_DONG = @maHD
	return
end
go

CREATE PROC SP_ThongTinChiNhanh_Ma @MaChiNhanh char(8)
as	
    select HD.MA_HOP_DONG, CH.MA_CUA_HANG,CH.TEN_CUA_HANG,CN.MA_CHI_NHANH,CN.SDT,CN.DIA_CHI,CN.KHUVUC
	from HOPDONG HD, CUAHANG CH, CHINHANH CN
	WHERE HD.MA_HOP_DONG = CN.MA_HOP_DONG AND CN.MA_CUA_HANG = CH.MA_CUA_HANG 
	AND HD.TRANG_THAI = 1  AND CN.MA_CHI_NHANH = @MaChiNhanh
go

create procedure gia_han_hop_dong
@maHD char(8),
@so_ngay_them int
as
begin transaction
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
begin try
-- kiểm tra mã hợp đồng có tồn tại hay không
		if @maHD not in (select hd.MA_HOP_DONG
		     from HOPDONG hd 
			 where hd.MA_HOP_DONG=@maHD)
            begin
	print N'Không tồn tại hợp đồng này'
	rollback tran
	end

-- lấy ra ngày cuối hợp đồng
	declare @ngay_hien_tai datetime
	set @ngay_hien_tai=( select top 1 hd.TG_KET_THUC
						from HOPDONG hd
						where hd.MA_HOP_DONG=@maHD)
	if @ngay_hien_tai is not null
		begin
		-- bắt đầu chờ
	--WAITFOR DELAY '00:00:15'
-- gia hạn ngày hợp đồng
		set @ngay_hien_tai = (select DATEADD(DAY, @so_ngay_them, @ngay_hien_tai))

-- cập nhật vào hợp đồng
		update HOPDONG
			SET TG_KET_THUC = @ngay_hien_tai
			WHERE MA_HOP_DONG = @maHD;
		end
end try
begin catch
	print N'Gia hạn hợp đồng thất bại'
	rollback tran
end catch

commit tran

go
create function TongDoanhThu(@MACH char(8) , @TGBD datetime , @TGKT datetime)
returns float
as
	begin
		if(dbo.SoDonDatHang(@MACH,@TGBD,@TGKT) = 0)
			return 0
		return 
		(select sum( DDH.TONG_TIEN)
		from DONDATHANG DDH , CHINHANH CN
		where DDH.MA_CHI_NHANH = CN.MA_CHI_NHANH AND CN.MA_CUA_HANG = @MACH 
		and @TGBD <= DDH.NGAY_TAO AND DDH.NGAY_TAO <= @TGKT 
		)
	end
go

create function SoDonDatHang(@MACH char(8) , @TGBD datetime , @TGKT datetime)
returns INT
as
	begin
	declare @ketqua int 
	set @ketqua = (select COUNT(DDH.MA_DON) 
		from DONDATHANG DDH , CHINHANH CN
		where DDH.MA_CHI_NHANH = CN.MA_CHI_NHANH AND CN.MA_CUA_HANG = @MACH
		and @TGBD <= DDH.NGAY_TAO AND DDH.NGAY_TAO <= @TGKT 
		)

	if(@ketqua is NULL or @ketqua = 0)
	    begin
			set @ketqua = 0;
		end
	return @ketqua
	end
go

select COUNT(DDH.MA_DON) 
		from DONDATHANG DDH , CHINHANH CN
		where '01/01/2022' <= DDH.NGAY_TAO AND DDH.NGAY_TAO <= '12/27/2022' and DDH.MA_CHI_NHANH = CN.MA_CHI_NHANH 
		and CN.MA_CUA_HANG = 'CH511465'


select CN.MA_CUA_HANG , CH.TEN_CUA_HANG , count(DDH.MA_DON)
		from DONDATHANG DDH , CHINHANH CN , CUAHANG CH
		where DDH.MA_CHI_NHANH = CN.MA_CHI_NHANH and CH.MA_CUA_HANG = CN.MA_CUA_HANG
		and '01/01/2022' <= DDH.NGAY_TAO AND DDH.NGAY_TAO <= '1/27/2022' 
		and DDH.TINH_TRANG =N'đã giao'
		group by CN.MA_CUA_HANG ,CH.TEN_CUA_HANG


go
CREATE PROC XemThongKe @MaCH char(8) , @TGBD datetime , @TGKT datetime
as
begin
	select CH.MA_CUA_HANG , CH.TEN_CUA_HANG ,
	DBO.SoDonDatHang(CH.MA_CUA_HANG,@TGBD,@TGKT) AS SO_DON , 
	DBO.TongDoanhThu(CH.MA_CUA_HANG,@TGBD,@TGKT) AS DOANH_THU
	,(DBO.TongDoanhThu(CH.MA_CUA_HANG,@TGBD,@TGKT)*0.2) AS TIEN_HOA_HONG
	FROM CUAHANG CH
	where CH.MA_CUA_HANG = @MaCH 
end


--EXEC XemThongKe 'CH533015','01/01/2022' , '12/01/2022'

GO

create function TongDoanhThu_curdate(@MACH char(8))
returns float
as
	begin
	    if(dbo.SoDonDatHang_curdate(@MACH) = 0)
			return 0
		return 
		(select sum( DDH.TONG_TIEN)
		from DONDATHANG DDH , CHINHANH CN
		where DDH.MA_CHI_NHANH = CN.MA_CHI_NHANH AND CN.MA_CUA_HANG = @MACH 
		AND  MONTH(DDH.NGAY_TAO) = MONTH(GETDATE()) AND YEAR(DDH.NGAY_TAO) = YEAR(GETDATE())
		)
	end
go

create function SoDonDatHang_curdate(@MACH char(8))
returns INT
as
	begin
	declare @ketqua int 
	set @ketqua = (select COUNT(DDH.MA_DON) 
		from DONDATHANG DDH , CHINHANH CN
		where DDH.MA_CHI_NHANH = CN.MA_CHI_NHANH AND CN.MA_CUA_HANG = @MACH
		AND  MONTH(DDH.NGAY_TAO) = MONTH(GETDATE()) AND YEAR(DDH.NGAY_TAO) = YEAR(GETDATE())
		)
	if(@ketqua is NULL or @ketqua = 0)
	    begin
			set @ketqua = 0;
		end
	return @ketqua
	end
go


CREATE PROC XemThongKe_curdate @maCH char(8)
as
begin
	select CH.MA_CUA_HANG , CH.TEN_CUA_HANG , 
	DBO.SoDonDatHang_curdate(CH.MA_CUA_HANG) AS SO_DON , 
	DBO.TongDoanhThu_curdate(CH.MA_CUA_HANG) AS DOANH_THU,
	(DBO.TongDoanhThu_curdate(CH.MA_CUA_HANG)*0.2) AS TIEN_HOA_HONG
	FROM CUAHANG CH
	where CH.MA_CUA_HANG = @maCH
end
go


--=================================================================== TAI XẾ ================================================================================

create proc capnhapTX @matx char(8), @sdt char(10), @email char(100), @bienso char(10), @kvhd nvarchar(255)
as
update taixe
set SDT = @sdt, EMAIL=@email, BIEN_SO=@bienso, KHU_VUC=@kvhd where MA_TAI_XE=@matx
go

create proc NhanDH @matx char(8), @madh char(8)
as
update DONDATHANG
set MA_TAI_XE = @matx, TINH_TRANG = N'đã xác nhận' where MA_DON=@madh and TRANG_THAI = 1
go


--==================================================================== CỬA HÀNG ===========================================================================
ALTER PROCEDURE INSERT_MA @MA_CUA_HANG CHAR(8), @TEN_MON NVARCHAR(81), @DON_GIA FLOAT, @TRANG_THAI INT
AS
INSERT INTO MON_AN(MA_MON_AN,MA_CUA_HANG, TEN_MON,TINH_TRANG, DON_GIA,TRANG_THAI) 
VALUES ('MO01234',@MA_CUA_HANG, @TEN_MON,N'có bán', @DON_GIA,1)


go

CREATE PROCEDURE USP_DT_CAPNHAT_GIA_MONAN @MA_MON_AN CHAR(8), @GIA_UPDATE MONEY
AS
BEGIN TRAN
	IF NOT EXISTS (SELECT * FROM MON_AN WHERE MA_MON_AN=@MA_MON_AN)
	BEGIN
		PRINT N'MÓN ĂN KHÔNG TỒN TẠI'
		RETURN 
	END

	DECLARE @GIASP MONEY
	SET @GIASP = (SELECT DON_GIA FROM MON_AN WHERE MA_MON_AN=@MA_MON_AN)
	UPDATE MON_AN
    SET DON_GIA = @GIA_UPDATE  
	WHERE MA_MON_AN=@MA_MON_AN
	IF (@GIA_UPDATE = 0)
	BEGIN
		ROLLBACK TRAN
		RETURN
	END
COMMIT TRAN
GO

-- ĐỐI TÁC CẬP NHẬT SỐ LƯỢNG MÓN ĂN TẠI MÔT CHI NHÀNH
CREATE PROC SP_CH_CAP_NHAT_SL_MON_AN
	@MA_CHI_NHANH CHAR(8),
	@MA_MON_AN CHAR(8),
	@SO_LUONG_THEM INT
AS

	BEGIN TRAN
	    
		IF NOT EXISTS (SELECT * FROM MON_AN_CHI_NHANH MA_CN WHERE MA_CHI_NHANH = @MA_CHI_NHANH AND MA_MON_AN = @MA_MON_AN) --(1)
			BEGIN	
				PRINT @MA_CHI_NHANH +', '+@MA_MON_AN + N' KHÔNG HỢP LỆ !'
				ROLLBACK TRAN
				RETURN 0
			END

		IF @SO_LUONG_THEM <1
			BEGIN	
				PRINT N'SỐ LƯỢNG THÊM KHÔNG HỢP LỆ !'
				ROLLBACK TRAN
				RETURN 0
			END
		DECLARE @SO_LUONG_HIEN_CO INT
		SET @SO_LUONG_HIEN_CO = ( SELECT SO_LUONG FROM MON_AN_CHI_NHANH WHERE MA_CHI_NHANH = @MA_CHI_NHANH AND MA_MON_AN = @MA_MON_AN )
		IF @SO_LUONG_HIEN_CO <1
			BEGIN	
				PRINT ' SỐ LƯỢNG HIỆN CÓ KHÔNG HỢP LỆ !'
				ROLLBACK TRAN
				RETURN 0
			END

		UPDATE MON_AN_CHI_NHANH --(4)
		SET SO_LUONG = @SO_LUONG_HIEN_CO + @SO_LUONG_THEM
		WHERE MA_CHI_NHANH = @MA_CHI_NHANH AND MA_MON_AN = @MA_MON_AN
	COMMIT TRAN
PRINT N'CẬP NHẬT THÀNH CÔNG'
RETURN 1

EXEC dbo.SP_CH_CAP_NHAT_SL_MON_AN 'CN001', 'MA001', 22

GO



--Thêm chi nhánh
CREATE PROCEDURE sp_DT_ThemChiNhanh @machinhanh char(8), @diachi nvarchar(255), @sdt nvarchar(10), @macuahang char(8), @mahopdong char(8), @khuvuc nvarchar(255)
AS

BEGIN TRAN
	BEGIN TRY
	--Kiểm tra địa chỉ có trùng hay không
	IF(EXISTS(SELECT * FROM CHINHANH WHERE MA_CUA_HANG = @macuahang AND DIA_CHI = @diachi))
			begin
			rollback tran
			RETURN  -1
			end

	-- Kiểm tra mã chi nhánh có trùng hay không
	IF(EXISTS(SELECT * FROM CHINHANH WHERE MA_CHI_NHANH = @machinhanh))
			begin
			rollback tran
			RETURN  -1
			end
	
	INSERT INTO CHINHANH(MA_CHI_NHANH, SL_DON_MOINGAY, DIA_CHI, SDT, TINH_TRANG, MA_CUA_HANG, MA_HOP_DONG, KHUVUC)
	VALUES
		(@machinhanh,NULL,@diachi, @sdt, null, @macuahang, @mahopdong, @khuvuc)

	UPDATE CUAHANG
	SET SO_CHI_NHANH = SO_CHI_NHANH + 1
	WHERE MA_CUA_HANG = @macuahang
	END TRY
	BEGIN CATCH
		PRINT N'LỖI HỆ THỐNG'
		ROLLBACK TRAN
		RETURN 1
	END CATCH
COMMIT TRAN
	return 1
GO



--Thêm món ăn
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE INSERT_MA @MA_CUA_HANG CHAR(8), @TEN_MON NVARCHAR(81), @DON_GIA FLOAT
AS
INSERT INTO MON_AN(MA_CUA_HANG, TEN_MON, DON_GIA) VALUES (@MA_CUA_HANG, @TEN_MON, @DON_GIA)

--Thêm hợp đồng

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE THEM_HD @MA_CUA_HANG CHAR(8), @NGAY_TAO  DATETIME,@TG_BAT_DAU DATETIME,@TG_KET_THUC DATETIME
AS
INSERT INTO HOP_DONG(MA_CUA_HANG,NGAY_TAO,TG_BAT_DAU,TG_KET_THUC)
VALUES (@MA_CUA_HANG,@NGAY_TAO,@TG_BAT_DAU,@TG_KET_THUC)



--20120093-NGUYỄN THỊ HỒNG NHUNG
---------------------------------------------------------------------------------------------------------
--KIỂM TRA TÊN ĐĂNG NHẬP
go
create 
--alter
proc kiem_tra_tentk_khach_hang
@maKH nvarchar(8),
@ten_dang_nhap nvarchar(255)
as
begin tran
begin try
if @ten_dang_nhap is null
begin 
print N'Tên đăng nhập không có'
rollback tran

end
if @maKH is null
begin
print N'Mã khách hàng không có'
	rollback tran

	end
if not exists ( select *  from TAIKHOAN tk, KHACHHANG kh where kh.ID_TAI_KHOAN= tk.ID_TAI_KHOAN and tk.TEN_DANG_NHAP=@ten_dang_nhap and kh.MA_KHACH_HANG=@maKH)
begin
	rollback tran

end
else
begin
print N'Trùng khớp'

end
end try
begin catch
 print N'Không trùng khớp tên đăng nhập'
end catch
commit tran
go


---------------------------------------------------------------------------------------------------------
--CẬP NHẬT MẬT KHẨU


create 
--alter
proc cap_nhat_mat_khau
@ten_tk NVARCHAR(255),
@mat_khau CHAR(100),
@mat_khau_moi char(100)
as 
begin tran
SET TRANSACTION ISOLATION LEVEL READ COMMITTED
begin try
if @ten_tk is null
	begin 
	print N'ten tk null'
	rollback tran
	return -1
	end
if not exists ( select tk.TEN_DANG_NHAP from TAIKHOAN tk where tk.TEN_DANG_NHAP=@ten_tk)
	begin
			print N'Sai tên đăng nhập'
			rollback tran
			return -1
	end
if not exists ( select tk.TEN_DANG_NHAP from TAIKHOAN tk where tk.TEN_DANG_NHAP=@ten_tk and tk.MAT_KHAU= @mat_khau)
	begin
			print N'Sai mật khẩu. Vui lòng nhập đúng mật khẩu để sửa đổi thông tin'
			rollback tran
			return -1
	end
update TAIKHOAN
set MAT_KHAU=@mat_khau_moi
where TEN_DANG_NHAP= @ten_tk
print N'Đổi mật khẩu thành công'
end try
begin catch
	print N'Thay đổi mật khẩu thất bại'
	rollback tran
end catch
commit tran
go

go
---------------------------------------------------------------------------------------------------------
--KHÁCH HÀNG XEM DANH SÁCH ĐỐI TÁC
CREATE
--ALTER
PROCEDURE sp_KH_XemDSDoiTac
AS
BEGIN TRAN
	BEGIN TRY
		SELECT TOP 1500 MA_CUA_HANG, TEN_CUA_HANG, EMAIL, THANH_PHO, QUAN, SDT, SO_CHI_NHANH  FROM CUAHANG
		--ĐỂ TEST
		--WAITFOR DELAY '0:0:10'
	END TRY
	BEGIN CATCH
			PRINT N'LỖI HỆ THỐNG'
			ROLLBACK TRAN

	END CATCH
COMMIT TRAN

GO

---------------------------------------------------------------------------------------------------------
--KHÁCH HÀNG XEM MÓN ĂN


create
--alter 
proc sp_KH_xem_MonAN
@MaCH char(8)
as
  select  Mon.MA_MON_AN as N'Mã Món', Mon.TEN_MON as N'Tên Món', Mon.DON_GIA as N'Ðơn giá', Mon.TINH_TRANG as N'Tình Trạng'
  from MON_AN Mon
  where Mon.MA_CUA_HANG=@MaCH and TRANG_THAI=1
go


---------------------------------------------------------------------------------------------------------

--KHÁCH HÀNG XEM THÔNG TIN
create
--alter
proc xem_thong_tin_khach_hang
@MaKH char(8)
as
begin tran
if @MaKH is null

begin
print N'Mã khách hàng null'
rollback tran
return -1
end
 select HOTEN, DIACHI, SDT, EMAIL from KHACHHANG where MA_KHACH_HANG= @MaKH
commit tran

go


---------------------------------------------------------------------------------------------------------
--KHÁCH HÀNG CẬP NHẬT TÀI KHOẢN

create
--alter
proc cap_nhat_tai_khach_hang
@maKH char(8),
@hoten nvarchar(255),
@diachi nvarchar(255),
@sdt nvarchar(12)
as
begin tran
begin try
if @maKH is null
	begin
		print N'Chưa nhập mã khách hàng'
		rollback tran
	end

UPDATE KHACHHANG
SET HOTEN=@hoten, DIACHI=@diachi, SDT=@sdt
WHERE MA_KHACH_HANG=@maKH

end try

begin catch
	print N'Cập nhật thất bại'
	rollback tran
end catch

commit tran
go

---------------------------------------------------------------------------------------------------------
-- THÊM CHI TIẾT VÀO BẢNG TẠM

create table ##chitietdon(
		MaDon nvarchar(8),
		MaMon nvarchar(8),
		SoLuong int, 
		TongTien float
		)
go


create
--alter
proc them_chi_tiet
@madh nvarchar(8),
@mamon nvarchar(8),
@soluong int
as 
IF object_id('tempdb.dbo.##chitietdon') is null
begin
		create table ##chitietdon(
		MaDon nvarchar(8),
		MaMon nvarchar(8),
		SoLuong int, 
		TongTien float
		)

end
if @mamon in (select  MaMon from ##chitietdon where MaDon=@madh)
begin
	UPDATE ##chitietdon
	set SoLuong=@soluong
	where MaMon=@mamon and MaDon=@madh
end
else
begin
 declare @money float
 select @money= mon.DON_GIA from MON_AN mon where mon.MA_MON_AN=@mamon
 select @money =@money * @soluong
 INSERT INTO ##chitietdon
 values(@madh, @mamon, @soluong, @money)
 end
 go


---------------------------------------------------------------------------------------------------------
 --THÊM VÀO BẢNG GỐC
create
--alter
proc them_chi_tiet_don_bang_goc
@maDH nvarchar(8)
as 
IF object_id('tempdb.dbo.##chitietdon') is  not null
begin
INSERT INTO CHITIET_DONHANG
SELECT * FROM ##chitietdon
Where MaDon=@madh
delete from ##chitietdon where MaDon=@maDH
end

go
---------------------------------------------------------------------------------------------------------

--HỦY ĐƠN HÀNG
create 
--alter
proc xoa_don_dat
@maDH nvarchar(8)
as
begin tran
begin try
if @maDH is null
begin 
	print N'Mã đơn hàng null'
	rollback tran
end
if not exists (select * from DONDATHANG where MA_DON =@maDH)
begin 
	print N'Không tồn tại mã đơn'
	rollback tran
end
delete top(1) from DONDATHANG where MA_DON=@maDH and TINH_TRANG=N'Chờ xác nhận'
delete from CHITIET_DONHANG where MA_DON=@maDH
end try

begin catch 
	 print N'Xóa không thành công'
	 rollback tran
end catch
commit tran

go
------------------------------------------

--XEM BẢNG TẠM
create
--alter
proc xem_temp_table
@maDH nvarchar(8)
as
begin tran
IF object_id('tempdb.dbo.##chitietdon') is null
begin
print N'Chưa đặt món'
rollback tran
end
select* from ##chitietdon where MaDon=@maDH
commit tran
go
-------------------------------------
--XÓA DATA BẢNG TẠM
create 
--alter
proc xoa_data_temp_tab
@madh nvarchar(8)
as
begin tran
begin try
 if not exists (select * from ##chitietdon where MaDon=@madh)
 BEGIN
		print N'không tồn tại trong bảng tạm'
		rollback tran
 END
 
  delete from ##chitietdon where MaDon=@madh

end try
begin catch
print N' xóa không thành công'
rollback tran
end catch
commit tran


go
-------------------------------------
	--THÊM VÀO ĐƠN ĐẶT HÀNG
	create
	--alter
	proc them_don_dat_hang
	@maDon nvarchar(8),
	@tien float,
	@diachi nvarchar(255),
	@phivanchuyen float, 
	@makh nvarchar(255),
	@macn nvarchar(8)
	as
	declare @ngay datetime
	select @ngay = GETDATE();
	INSERT INTO DONDATHANG
	values(@maDon, @tien, @diachi, N'chờ xác nhận' ,@phivanchuyen, @makh, null,@macn,1, @ngay)

	go




	select * from DONDATHANG
	order by MA_DON


	select * from CHITIET_DONHANG