CREATE DATABASE QLHeThong;
GO

USE QLHeThong;
GO

CREATE TABLE TaiKhoan
(
    MaTaiKhoan INT IDENTITY(1,1) PRIMARY KEY,
    TenDangNhap VARCHAR(50) NOT NULL UNIQUE,
    MatKhau VARCHAR(255) NOT NULL,
    Quyen NVARCHAR(30),
    TrangThai BIT DEFAULT 1
);
GO

INSERT INTO TaiKhoan
(TenDangNhap, MatKhau, Quyen, TrangThai)
VALUES
('admin', '123456', N'Admin', 1);
GO
--Quan lý Nhân viên
CREATE TABLE NhanVien
(
    MaNhanVien INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    NgaySinh DATE,
    GioiTinh NVARCHAR(10),
    SoDienThoai VARCHAR(15),
    ChucVu NVARCHAR(50),
    DiaChi NVARCHAR(200)
);
GO
INSERT INTO NhanVien
(HoTen, NgaySinh, GioiTinh, SoDienThoai, ChucVu, DiaChi)
VALUES
(N'Lê Trung Hiếu', '2006-08-30', N'Nam', '0912345678',
 N'Quản lý', N'Hà Nội'),

(N'Pham Gia Bảo', '2004-08-15', N'Nam', '0987654321',
 N'Thu ngân', N'Hà Nội'),

(N'Bùi Thanh Bình', '2003-03-10', N'Nam', '0901234567',
 N'Ðầu bếp', N'Hà Nội');
go

SELECT * FROM NhanVien;
TRUNCATE TABLE NhanVien;