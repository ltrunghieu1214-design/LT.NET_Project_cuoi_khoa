IF DB_ID(N'QLHeThong') IS NULL
    EXEC(N'CREATE DATABASE QLHeThong');
GO

USE QLHeThong;
GO

-- 1. Tạo bảng Tài khoản
IF OBJECT_ID(N'dbo.TaiKhoan', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.TaiKhoan
    (
        MaTaiKhoan INT IDENTITY(1,1) PRIMARY KEY,
        TenDangNhap VARCHAR(50) NOT NULL UNIQUE,
        MatKhau VARCHAR(255) NOT NULL,
        Quyen NVARCHAR(30),
        TrangThai BIT DEFAULT 1
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.TaiKhoan WHERE TenDangNhap = 'admin')
BEGIN
    INSERT INTO dbo.TaiKhoan (TenDangNhap, MatKhau, Quyen, TrangThai)
    VALUES ('admin', '123456', N'Admin', 1);
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.TaiKhoan WHERE TenDangNhap = 'bao')
BEGIN
    INSERT INTO dbo.TaiKhoan (TenDangNhap, MatKhau, Quyen, TrangThai)
    VALUES ('bao', 'Bao@123', N'Nhân viên', 1);
END
GO

-- 2. Tạo bảng Nhân viên
IF OBJECT_ID(N'dbo.NhanVien', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.NhanVien
    (
        MaNhanVien INT IDENTITY(1,1) PRIMARY KEY,
        HoTen NVARCHAR(100) NOT NULL,
        NgaySinh DATE,
        GioiTinh NVARCHAR(10),
        SoDienThoai VARCHAR(15),
        ChucVu NVARCHAR(50),
        DiaChi NVARCHAR(200)
    );
END
GO

-- 3. Xóa sạch dữ liệu trùng lặp hiện có (giữ lại 1 dòng đầu tiên cho mỗi SĐT)
;WITH DuplicateSeedEmployees AS
(
    SELECT MaNhanVien,
           ROW_NUMBER() OVER (PARTITION BY LTRIM(RTRIM(SoDienThoai)) ORDER BY MaNhanVien) AS RowNumber
    FROM dbo.NhanVien
    WHERE LTRIM(RTRIM(SoDienThoai)) IN ('0912345678', '0987654321', '0901234567')
)
DELETE FROM DuplicateSeedEmployees
WHERE RowNumber > 1;
GO

-- 4. Tạo ràng buộc chống trùng Số điện thoại
IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.NhanVien')
      AND name = N'UX_NhanVien_SoDienThoai'
)
BEGIN
    CREATE UNIQUE INDEX UX_NhanVien_SoDienThoai
        ON dbo.NhanVien(SoDienThoai)
        WHERE SoDienThoai IS NOT NULL;
END
GO

-- 5. Chèn dữ liệu mẫu (Chỉ chèn nếu SĐT chưa tồn tại)
INSERT INTO dbo.NhanVien (HoTen, NgaySinh, GioiTinh, SoDienThoai, ChucVu, DiaChi)
SELECT seed.HoTen, seed.NgaySinh, seed.GioiTinh, seed.SoDienThoai, seed.ChucVu, seed.DiaChi
FROM (VALUES
    (N'Lê Trung Hiếu', CAST('2006-08-30' AS DATE), N'Nam', '0912345678', N'Quản lý', N'Hà Nội'),
    (N'Phạm Gia Bảo', CAST('2004-08-15' AS DATE), N'Nam', '0987654321', N'Thu ngân', N'Hà Nội'),
    (N'Bùi Thanh Bình', CAST('2003-03-10' AS DATE), N'Nam', '0901234567', N'Đầu bếp', N'Hà Nội')
) AS seed(HoTen, NgaySinh, GioiTinh, SoDienThoai, ChucVu, DiaChi)
WHERE NOT EXISTS
(
    SELECT 1
    FROM dbo.NhanVien AS nv
    WHERE LTRIM(RTRIM(nv.SoDienThoai)) = seed.SoDienThoai
);
GO

SELECT * FROM dbo.NhanVien;
GO