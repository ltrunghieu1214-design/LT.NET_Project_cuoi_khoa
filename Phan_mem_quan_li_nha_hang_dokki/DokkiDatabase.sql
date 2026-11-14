-- ============================================================
--  DOKKI RESTAURANT MANAGEMENT SYSTEM - DATABASE SCRIPT
--  Tạo bởi: Hệ thống tự động từ DokkiStore.cs
--  Ngày tạo: 2024-06-24
-- ============================================================

USE master;
GO

-- Tạo database nếu chưa có
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'DokkiRestaurant')
BEGIN
    CREATE DATABASE DokkiRestaurant
        COLLATE Vietnamese_CI_AS;
END
GO

USE DokkiRestaurant;
GO

-- ============================================================
--  XÓA BẢNG CŨ (NẾU CÓ) - Thứ tự quan trọng (FK trước)
-- ============================================================
IF OBJECT_ID('dbo.HoaDonChiTiet', 'U') IS NOT NULL DROP TABLE dbo.HoaDonChiTiet;
IF OBJECT_ID('dbo.HoaDon',        'U') IS NOT NULL DROP TABLE dbo.HoaDon;
IF OBJECT_ID('dbo.DatBan',        'U') IS NOT NULL DROP TABLE dbo.DatBan;
IF OBJECT_ID('dbo.MonAn',         'U') IS NOT NULL DROP TABLE dbo.MonAn;
IF OBJECT_ID('dbo.DanhMuc',       'U') IS NOT NULL DROP TABLE dbo.DanhMuc;
IF OBJECT_ID('dbo.BanAn',         'U') IS NOT NULL DROP TABLE dbo.BanAn;
IF OBJECT_ID('dbo.NhatKyHoatDong','U') IS NOT NULL DROP TABLE dbo.NhatKyHoatDong;
GO

-- ============================================================
--  TẠO CÁC BẢNG
-- ============================================================

-- Bảng DanhMuc (Categories)
CREATE TABLE dbo.DanhMuc (
    MaDanhMuc   NVARCHAR(10)  NOT NULL PRIMARY KEY,
    TenDanhMuc  NVARCHAR(100) NOT NULL,
    SoLuongMon  INT           NOT NULL DEFAULT 0,
    GhiChu      NVARCHAR(255) NULL,
    TrangThai   NVARCHAR(50)  NOT NULL DEFAULT N'Đang phục vụ'
);
GO

-- Bảng MonAn (Foods)
CREATE TABLE dbo.MonAn (
    MaMon       NVARCHAR(10)   NOT NULL PRIMARY KEY,
    TenMon      NVARCHAR(150)  NOT NULL,
    DanhMuc     NVARCHAR(100)  NOT NULL,
    Gia         DECIMAL(18, 0) NOT NULL,
    DonVi       NVARCHAR(50)   NOT NULL,
    TrangThai   NVARCHAR(50)   NOT NULL DEFAULT N'Đang bán'
);
GO

-- Bảng BanAn (Tables)
CREATE TABLE dbo.BanAn (
    SoBan       INT           NOT NULL PRIMARY KEY,
    TenBan      NVARCHAR(20)  NOT NULL,
    TrangThai   NVARCHAR(30)  NOT NULL DEFAULT N'Trống',
    SucChua     INT           NOT NULL DEFAULT 4,
    KhuVuc      NVARCHAR(50)  NOT NULL
);
GO

-- Bảng DatBan (Bookings)
CREATE TABLE dbo.DatBan (
    MaDatBan    NVARCHAR(10)  NOT NULL PRIMARY KEY,
    GioHen      NVARCHAR(10)  NOT NULL,
    TenKhach    NVARCHAR(100) NOT NULL,
    SoDienThoai NVARCHAR(20)  NOT NULL,
    SoNguoi     NVARCHAR(20)  NOT NULL,
    TenBan      NVARCHAR(20)  NOT NULL,
    TrangThai   NVARCHAR(30)  NOT NULL DEFAULT N'Chờ xác nhận'
);
GO

-- Bảng HoaDon (Bills)
CREATE TABLE dbo.HoaDon (
    MaHoaDon       NVARCHAR(10)   NOT NULL PRIMARY KEY,
    GioThanhToan   NVARCHAR(10)   NOT NULL,
    NgayThanhToan  DATETIME       NOT NULL DEFAULT GETDATE(),
    TenBan         NVARCHAR(20)   NOT NULL,
    TenKhach       NVARCHAR(100)  NOT NULL,
    TongTien       DECIMAL(18, 0) NOT NULL,
    TrangThai      NVARCHAR(30)   NOT NULL DEFAULT N'Đã thanh toán'
);
GO

-- Bảng HoaDonChiTiet (Order Details - cho active orders)
CREATE TABLE dbo.HoaDonChiTiet (
    ID          INT IDENTITY(1,1) PRIMARY KEY,
    MaHoaDon    NVARCHAR(10)   NOT NULL,
    MaMon       NVARCHAR(10)   NOT NULL,
    TenMon      NVARCHAR(150)  NOT NULL,
    DonGia      DECIMAL(18, 0) NOT NULL,
    SoLuong     INT            NOT NULL DEFAULT 1,
    ThanhTien   AS (DonGia * SoLuong) PERSISTED,
    CONSTRAINT FK_ChiTiet_HoaDon FOREIGN KEY (MaHoaDon) REFERENCES dbo.HoaDon(MaHoaDon)
);
GO

-- Bảng NhatKyHoatDong (Activity Log)
CREATE TABLE dbo.NhatKyHoatDong (
    ID          INT IDENTITY(1,1) PRIMARY KEY,
    NoiDung     NVARCHAR(500) NOT NULL,
    ThoiGian    DATETIME      NOT NULL DEFAULT GETDATE()
);
GO

-- ============================================================
--  NHẬP DỮ LIỆU - DANH MỤC (30 danh mục)
-- ============================================================
INSERT INTO dbo.DanhMuc (MaDanhMuc, TenDanhMuc, SoLuongMon, GhiChu, TrangThai) VALUES
(N'DM001', N'Bánh gạo & Tokbokki',          12, N'Bánh gạo phô mai, bánh gạo khoai môn, truyền thống',          N'Đang phục vụ'),
(N'DM002', N'Thịt bò & Heo nhúng lẩu',      18, N'Ba chỉ bò Mỹ, bắp bò Úc, ba chỉ heo cuộn sốt',               N'Đang phục vụ'),
(N'DM003', N'Hải sản nhúng lẩu',             14, N'Tôm sú tươi, mực hoa, bạch tuộc baby',                        N'Đang phục vụ'),
(N'DM004', N'Topping & Viên nhúng lẩu',      22, N'Chả cá Busan, viên trứng muối, xúc xích phô mai',             N'Đang phục vụ'),
(N'DM005', N'Gà rán sốt Hàn Quốc',           8, N'Gà sốt Yangnyeom, gà sốt mật ong, gà cay',                    N'Đang phục vụ'),
(N'DM006', N'Món chiên & Khai vị giòn',      16, N'Gimari rong biển, mandu chiên, khoai tây lắc',                N'Đang phục vụ'),
(N'DM007', N'Kimbap & Cơm trộn quầy line',  10, N'Kimbap chiên xù, cơm trộn rong biển cá ngừ',                  N'Đang phục vụ'),
(N'DM008', N'Rau sạch & Nấm tươi',           15, N'Cải thảo, nấm kim châm, nấm đùi gà',                          N'Đang phục vụ'),
(N'DM009', N'Mì gói & Miến khoai lang',       9, N'Sari Ramyeon, miến dẹt Ottogi',                                N'Đang phục vụ'),
(N'DM010', N'Nước sốt lẩu Dokki',             7, N'Sốt Dokki, Ddukmo, Daegu, Gungjung, Rose',                    N'Đang phục vụ'),
(N'DM011', N'Đồ uống & Trà đóng chai',       14, N'Nước trà ngô Dongsuh, nước gạo rang Morning Rice',            N'Đang phục vụ'),
(N'DM012', N'Kem tươi & Tráng miệng',         6, N'Kem vani sốt socola, kem matcha, chè đậu đỏ',                 N'Đang phục vụ'),
(N'DM013', N'Buffet Người lớn 139K',           1, N'Vé buffet tiêu chuẩn 1 người lớn ăn không giới hạn',          N'Đang phục vụ'),
(N'DM014', N'Buffet Trẻ em 69K',              1, N'Trẻ em cao từ 1m đến 1m3',                                    N'Đang phục vụ'),
(N'DM015', N'Vòng phô mai bắp kéo sợi',       4, N'Vòng phô mai Mozzarella hoa hồng viền nồi',                   N'Đang phục vụ'),
(N'DM016', N'Panchan & Kimchi chuẩn vị',      8, N'Kimchi cải thảo cay, củ cải muối vàng',                       N'Đang phục vụ'),
(N'DM017', N'Nước ngọt Dispenser tươi',        5, N'Coca-Cola, Sprite, Fanta cam, Soda chanh',                    N'Đang phục vụ'),
(N'DM018', N'Gia vị & Nước chấm quầy bar',   11, N'Dầu mè thơm, tỏi phi giòn, ớt băm, mè rang',                 N'Đang phục vụ'),
(N'DM019', N'Mì lạnh Hàn Quốc Naengmyeon',   4, N'Mì lạnh nước dùng thanh mát, mì trộn cay',                    N'Đang phục vụ'),
(N'DM020', N'Set đóng gói mang về Takeaway',  5, N'Combo tokbokki 2-4 người tự nấu tại nhà',                     N'Đang phục vụ'),
(N'DM021', N'Thịt nướng đá sốt Galbi',        8, N'Ba chỉ nướng đá, dẻ sườn ướp sốt',                           N'Đang phục vụ'),
(N'DM022', N'Bánh cá đậu đỏ Bungeoppang',     4, N'Bánh cá nhân kem custard, đậu đỏ mềm',                       N'Đang phục vụ'),
(N'DM023', N'Hải sản cao cấp Gọi thêm',       6, N'Bào ngư mini, sò điệp Nhật, tôm càng xanh',                  N'Đang phục vụ'),
(N'DM024', N'Bánh xèo Hàn Quốc Pajeon',       5, N'Bánh xèo hải sản hành lá, bánh xèo kimchi',                  N'Đang phục vụ'),
(N'DM025', N'Canh rong biển & Súp chả cá',   4, N'Canh rong biển thịt bò Miyeokguk nóng hổi',                   N'Đang phục vụ'),
(N'DM026', N'Salad & Trái cây tươi',           7, N'Salad mè rang, dưa hấu, ổi tráng miệng',                     N'Đang phục vụ'),
(N'DM027', N'Trà sữa & Đá xay Hàn Quốc',     6, N'Trà sữa trân châu đường đen, đá xay matcha',                  N'Đang phục vụ'),
(N'DM028', N'Thực đơn Trẻ em Dokki Kids',     5, N'Cơm nắm phô mai mini, xúc xích bạch tuộc',                   N'Đang phục vụ'),
(N'DM029', N'Menu Đặc biệt theo mùa Summer',  6, N'Bingsu xoài tuyết, bingsu dưa lưới',                          N'Tạm ẩn'),
(N'DM030', N'Gia vị đóng chai Dokki Mart',    8, N'Chai sốt lẩu 350ml, bánh gạo tươi đóng túi',                 N'Đang phục vụ');
GO

-- ============================================================
--  NHẬP DỮ LIỆU - MÓN ĂN (35 món)
-- ============================================================
INSERT INTO dbo.MonAn (MaMon, TenMon, DanhMuc, Gia, DonVi, TrangThai) VALUES
(N'MA001', N'Ba chỉ bò Mỹ cuộn',                    N'Thịt bò & Heo nhúng lẩu',    89000,  N'Đĩa 200g',    N'Đang bán'),
(N'MA002', N'Bắp bò Úc thượng hạng',                N'Thịt bò & Heo nhúng lẩu',    99000,  N'Đĩa 180g',    N'Đang bán'),
(N'MA003', N'Ba chỉ heo rút sườn sốt BBQ',          N'Thịt bò & Heo nhúng lẩu',    69000,  N'Đĩa 200g',    N'Đang bán'),
(N'MA004', N'Tôm sú tươi lột vỏ',                   N'Hải sản nhúng lẩu',          109000, N'Đĩa 6 con',   N'Đang bán'),
(N'MA005', N'Mực nang cắt hoa tươi',                N'Hải sản nhúng lẩu',           95000, N'Đĩa 200g',    N'Đang bán'),
(N'MA006', N'Bạch tuộc baby sốt cay',               N'Hải sản nhúng lẩu',           99000, N'Đĩa 200g',    N'Đang bán'),
(N'MA007', N'Tokbokki phô mai bắp',                 N'Bánh gạo & Tokbokki',         59000, N'Phần',        N'Đang bán'),
(N'MA008', N'Tokbokki khoai lang tím',              N'Bánh gạo & Tokbokki',         49000, N'Phần',        N'Đang bán'),
(N'MA009', N'Tokbokki hoa hồng sốt Rose',           N'Bánh gạo & Tokbokki',         55000, N'Phần',        N'Đang bán'),
(N'MA010', N'Tokbokki sợi dài truyền thống',        N'Bánh gạo & Tokbokki',         39000, N'Phần',        N'Đang bán'),
(N'MA011', N'Chả cá xiên que Busan Eomuk',          N'Topping & Viên nhúng lẩu',    45000, N'3 xiên',      N'Đang bán'),
(N'MA012', N'Viên phô mai trứng muối tan chảy',     N'Topping & Viên nhúng lẩu',    59000, N'Đĩa 6 viên', N'Đang bán'),
(N'MA013', N'Viên tôm hùm phô mai Mozzarella',      N'Topping & Viên nhúng lẩu',    62000, N'Đĩa 6 viên', N'Đang bán'),
(N'MA014', N'Xúc xích xông khói Dokki',             N'Topping & Viên nhúng lẩu',    49000, N'Đĩa 5 cây',  N'Đang bán'),
(N'MA015', N'Thanh cua Surimi tuyết',               N'Topping & Viên nhúng lẩu',    42000, N'Đĩa 6 thanh',N'Đang bán'),
(N'MA016', N'Gà sốt cay ngọt Yangnyeom',            N'Gà rán sốt Hàn Quốc',         79000, N'Phần 4 miếng',N'Đang bán'),
(N'MA017', N'Gà sốt xì dầu tỏi mật ong',           N'Gà rán sốt Hàn Quốc',         79000, N'Phần 4 miếng',N'Đang bán'),
(N'MA018', N'Rong biển cuộn miến Gimari',           N'Món chiên & Khai vị giòn',    39000, N'Đĩa 5 cuộn', N'Đang bán'),
(N'MA019', N'Mandu chiên giòn nhân thịt',           N'Món chiên & Khai vị giòn',    45000, N'Đĩa 5 cái',  N'Đang bán'),
(N'MA020', N'Khoai tây chiên bơ tỏi lắc phô mai',  N'Món chiên & Khai vị giòn',    35000, N'Phần',        N'Đang bán'),
(N'MA021', N'Kimbap chiên xù xốt cay Mayo',         N'Kimbap & Cơm trộn quầy line', 49000, N'Đĩa 8 miếng',N'Đang bán'),
(N'MA022', N'Nấm kim châm tươi Hàn Quốc',           N'Rau sạch & Nấm tươi',         25000, N'Đĩa 150g',   N'Đang bán'),
(N'MA023', N'Nấm đùi gà baby thái lát',             N'Rau sạch & Nấm tươi',         29000, N'Đĩa 150g',   N'Đang bán'),
(N'MA024', N'Cải thảo thảo mộc Đà Lạt',            N'Rau sạch & Nấm tươi',         19000, N'Đĩa 200g',   N'Đang bán'),
(N'MA025', N'Mì khô không vị Sari Ramyeon',         N'Mì gói & Miến khoai lang',    18000, N'Gói 110g',   N'Đang bán'),
(N'MA026', N'Miến dẹt tươi khoai lang Ottogi',      N'Mì gói & Miến khoai lang',    32000, N'Đĩa 150g',   N'Đang bán'),
(N'MA027', N'Nước trà ngô rang Dongsuh',            N'Đồ uống & Trà đóng chai',     29000, N'Chai 500ml', N'Đang bán'),
(N'MA028', N'Nước gạo rang Morning Rice',           N'Đồ uống & Trà đóng chai',     32000, N'Chai 500ml', N'Đang bán'),
(N'MA029', N'Trà đào sả hạt chia tươi',            N'Đồ uống & Trà đóng chai',     35000, N'Ly 700ml',   N'Tạm hết'),
(N'MA030', N'Kem tươi vani Dokki rưới sốt socola', N'Kem tươi & Tráng miệng',      25000, N'Cốc',         N'Đang bán'),
(N'MA031', N'Vòng phô mai ngô ngọt kéo sợi',       N'Vòng phô mai bắp kéo sợi',    69000, N'Vòng',        N'Đang bán'),
(N'MA032', N'Kimchi cải thảo chuẩn vị Hàn',        N'Panchan & Kimchi chuẩn vị',   20000, N'Đĩa 100g',   N'Đang bán'),
(N'MA033', N'Củ cải vàng muối chua ngọt',           N'Panchan & Kimchi chuẩn vị',   15000, N'Đĩa 100g',   N'Đang bán'),
(N'MA034', N'Bánh cá kem custard nóng',             N'Bánh cá đậu đỏ Bungeoppang',  35000, N'2 con',       N'Đang bán'),
(N'MA035', N'Mì lạnh Naengmyeon giải nhiệt',        N'Mì lạnh Hàn Quốc Naengmyeon', 59000, N'Bát to',      N'Đang bán');
GO

-- ============================================================
--  NHẬP DỮ LIỆU - BÀN ĂN (28 bàn)
-- ============================================================
INSERT INTO dbo.BanAn (SoBan, TenBan, TrangThai, SucChua, KhuVuc) VALUES
( 1, N'BÀN 01', N'Trống',          4, N'Khu vực Tầng 1'),
( 2, N'BÀN 02', N'Trống',          4, N'Khu vực Tầng 1'),
( 3, N'BÀN 03', N'Đang phục vụ',  4, N'Khu vực Tầng 1'),
( 4, N'BÀN 04', N'Trống',          4, N'Khu vực Tầng 1'),
( 5, N'BÀN 05', N'Đã đặt',        4, N'Khu vực Tầng 1'),
( 6, N'BÀN 06', N'Đang phục vụ',  4, N'Khu vực Tầng 1'),
( 7, N'BÀN 07', N'Trống',          4, N'Khu vực Tầng 1'),
( 8, N'BÀN 08', N'Trống',          4, N'Khu vực Tầng 1'),
( 9, N'BÀN 09', N'Đang phục vụ',  4, N'Khu vực Tầng 1'),
(10, N'BÀN 10', N'Trống',          4, N'Khu vực Tầng 1'),
(11, N'BÀN 11', N'Trống',          4, N'Khu vực Tầng 1'),
(12, N'BÀN 12', N'Đã đặt',        4, N'Khu vực Tầng 1'),
(13, N'BÀN 13', N'Trống',          4, N'Khu vực Tầng 1'),
(14, N'BÀN 14', N'Trống',          4, N'Khu vực Tầng 1'),
(15, N'BÀN 15', N'Đang phục vụ',  4, N'Khu vực Tầng 2'),
(16, N'BÀN 16', N'Trống',          4, N'Khu vực Tầng 2'),
(17, N'BÀN 17', N'Trống',          4, N'Khu vực Tầng 2'),
(18, N'BÀN 18', N'Đã đặt',        4, N'Khu vực Tầng 2'),
(19, N'BÀN 19', N'Trống',          4, N'Khu vực Tầng 2'),
(20, N'BÀN 20', N'Trống',          4, N'Khu vực Tầng 2'),
(21, N'BÀN 21', N'Đang phục vụ',  4, N'Khu vực Tầng 2'),
(22, N'BÀN 22', N'Trống',          4, N'Khu vực Tầng 2'),
(23, N'BÀN 23', N'Đã đặt',        4, N'Khu vực Tầng 2'),
(24, N'BÀN 24', N'Đang phục vụ',  4, N'Khu vực Tầng 2'),
(25, N'BÀN 25', N'Trống',          4, N'Khu vực Tầng 2'),
(26, N'BÀN 26', N'Trống',          4, N'Khu vực Tầng 2'),
(27, N'BÀN 27', N'Đã đặt',        4, N'Khu vực Tầng 2'),
(28, N'BÀN 28', N'Trống',          4, N'Khu vực Tầng 2');
GO

-- ============================================================
--  NHẬP DỮ LIỆU - ĐẶT BÀN (25 lượt đặt)
-- ============================================================
INSERT INTO dbo.DatBan (MaDatBan, GioHen, TenKhach, SoDienThoai, SoNguoi, TenBan, TrangThai) VALUES
(N'DB001', N'11:30', N'Nguyễn Minh Tuấn',   N'0912 345 678', N'4 người', N'Bàn 02', N'Đã nhận bàn'),
(N'DB002', N'11:45', N'Trần Thị Mai Anh',   N'0988 765 432', N'2 người', N'Bàn 07', N'Đã nhận bàn'),
(N'DB003', N'12:00', N'Lê Hoàng Long',      N'0903 112 233', N'6 người', N'Bàn 15', N'Đã nhận bàn'),
(N'DB004', N'12:15', N'Phạm Hải Đăng',      N'0977 445 566', N'4 người', N'Bàn 04', N'Đã nhận bàn'),
(N'DB005', N'12:30', N'Vũ Thùy Dương',      N'0934 998 877', N'8 người', N'Bàn 08', N'Đã nhận bàn'),
(N'DB006', N'13:00', N'Hoàng Kim Ngân',     N'0915 223 344', N'2 người', N'Bàn 11', N'Đã nhận bàn'),
(N'DB007', N'13:15', N'Đặng Quốc Huy',      N'0966 332 211', N'3 người', N'Bàn 19', N'Đã nhận bàn'),
(N'DB008', N'17:30', N'Bùi Thanh Hương',    N'0909 554 433', N'4 người', N'Bàn 21', N'Đang phục vụ'),
(N'DB009', N'18:00', N'Ngô Thành Đạt',      N'0982 123 789', N'5 người', N'Bàn 25', N'Đang phục vụ'),
(N'DB010', N'18:30', N'Nguyễn Thu Trang',   N'0945 678 123', N'4 người', N'Bàn 12', N'Đã xác nhận'),
(N'DB011', N'18:45', N'Trịnh Thảo Ly',      N'0918 234 567', N'6 người', N'Bàn 05', N'Đã xác nhận'),
(N'DB012', N'19:00', N'Vũ Đức Anh',         N'0976 543 210', N'4 người', N'Bàn 18', N'Đã xác nhận'),
(N'DB013', N'19:15', N'Lâm Mỹ Duyên',      N'0932 876 543', N'2 người', N'Bàn 23', N'Đã xác nhận'),
(N'DB014', N'19:30', N'Đỗ Khánh Linh',      N'0904 321 987', N'8 người', N'Bàn 27', N'Đã xác nhận'),
(N'DB015', N'19:30', N'Cao Văn Hùng',       N'0989 654 321', N'4 người', N'Bàn 01', N'Chờ xác nhận'),
(N'DB016', N'19:45', N'Phan Gia Khiêm',     N'0913 789 456', N'3 người', N'Bàn 03', N'Chờ xác nhận'),
(N'DB017', N'20:00', N'Dương Bảo Ngọc',     N'0967 890 123', N'6 người', N'Bàn 06', N'Chờ xác nhận'),
(N'DB018', N'20:00', N'Tạ Quang Dũng',      N'0938 456 789', N'4 người', N'Bàn 09', N'Chờ xác nhận'),
(N'DB019', N'20:15', N'Mai Phương Thảo',    N'0971 234 890', N'2 người', N'Bàn 10', N'Chờ xác nhận'),
(N'DB020', N'20:30', N'Hà Trọng Nhân',      N'0908 901 234', N'5 người', N'Bàn 13', N'Chờ xác nhận'),
(N'DB021', N'20:30', N'Nguyễn Thị Yến',     N'0943 567 890', N'4 người', N'Bàn 14', N'Chờ xác nhận'),
(N'DB022', N'20:45', N'Lê Tuấn Kiệt',       N'0985 678 901', N'2 người', N'Bàn 16', N'Chờ xác nhận'),
(N'DB023', N'21:00', N'Trần Diệu Hoa',      N'0917 890 123', N'4 người', N'Bàn 17', N'Chờ xác nhận'),
(N'DB024', N'21:00', N'Đoàn Minh Trí',      N'0935 012 345', N'6 người', N'Bàn 20', N'Chờ xác nhận'),
(N'DB025', N'21:15', N'Quách Ánh Nguyệt',   N'0979 123 456', N'3 người', N'Bàn 22', N'Chờ xác nhận');
GO

-- ============================================================
--  NHẬP DỮ LIỆU - HÓA ĐƠN (25 hóa đơn)
-- ============================================================
INSERT INTO dbo.HoaDon (MaHoaDon, GioThanhToan, NgayThanhToan, TenBan, TenKhach, TongTien, TrangThai) VALUES
(N'HD1086', N'13:45', '2024-06-24 13:45:00', N'Bàn 08', N'Nguyễn Minh Anh',    680000,  N'Đã thanh toán'),
(N'HD1085', N'13:30', '2024-06-24 13:30:00', N'Bàn 15', N'Trần Hoàng Nam',    1240000,  N'Đã thanh toán'),
(N'HD1084', N'13:22', '2024-06-24 13:22:00', N'Bàn 03', N'Lê Ngọc Hà',         420000,  N'Đã thanh toán'),
(N'HD1083', N'13:10', '2024-06-24 13:10:00', N'Bàn 21', N'Phạm Gia Huy',        920000,  N'Đã thanh toán'),
(N'HD1082', N'12:55', '2024-06-24 12:55:00', N'Bàn 07', N'Vũ Mai Phương',       780000,  N'Đã thanh toán'),
(N'HD1081', N'12:48', '2024-06-24 12:48:00', N'Bàn 12', N'Hoàng Tuấn Kiệt',   1560000,  N'Đã thanh toán'),
(N'HD1080', N'12:40', '2024-06-24 12:40:00', N'Bàn 04', N'Đặng Thùy Dung',      550000,  N'Đã thanh toán'),
(N'HD1079', N'12:35', '2024-06-24 12:35:00', N'Bàn 18', N'Bùi Quốc Khánh',      890000,  N'Đã thanh toán'),
(N'HD1078', N'12:28', '2024-06-24 12:28:00', N'Bàn 02', N'Trương Mỹ Linh',      630000,  N'Đã thanh toán'),
(N'HD1077', N'12:15', '2024-06-24 12:15:00', N'Bàn 09', N'Đỗ Hải Đăng',        1120000,  N'Đã thanh toán'),
(N'HD1076', N'12:05', '2024-06-24 12:05:00', N'Bàn 14', N'Nguyễn Kim Oanh',     470000,  N'Đã thanh toán'),
(N'HD1075', N'11:58', '2024-06-24 11:58:00', N'Bàn 22', N'Phan Nhật Minh',      740000,  N'Đã thanh toán'),
(N'HD1074', N'11:50', '2024-06-24 11:50:00', N'Bàn 06', N'Vương Thục Quyên',    980000,  N'Đã thanh toán'),
(N'HD1073', N'11:42', '2024-06-24 11:42:00', N'Bàn 11', N'Dương Trung Hiếu',    590000,  N'Đã thanh toán'),
(N'HD1072', N'11:35', '2024-06-24 11:35:00', N'Bàn 17', N'Lý Khánh Ngọc',      1340000,  N'Đã thanh toán'),
(N'HD1071', N'11:25', '2024-06-24 11:25:00', N'Bàn 01', N'Hồ Thành Luân',       820000,  N'Đã thanh toán'),
(N'HD1070', N'11:18', '2024-06-24 11:18:00', N'Bàn 19', N'Tạ Bích Vân',         660000,  N'Đã thanh toán'),
(N'HD1069', N'11:10', '2024-06-24 11:10:00', N'Bàn 25', N'Mai Văn Đức',        1450000,  N'Đã thanh toán'),
(N'HD1068', N'10:55', '2024-06-24 10:55:00', N'Bàn 10', N'Cao Thu Thủy',        510000,  N'Đã thanh toán'),
(N'HD1067', N'10:45', '2024-06-24 10:45:00', N'Bàn 05', N'Đoàn Tiến Dũng',      730000,  N'Đã thanh toán'),
(N'HD1066', N'10:30', '2024-06-24 10:30:00', N'Bàn 16', N'Nguyễn Diệu Nhi',     880000,  N'Đã thanh toán'),
(N'HD1065', N'10:15', '2024-06-24 10:15:00', N'Bàn 13', N'Lâm Bá Phúc',         610000,  N'Đã thanh toán'),
(N'HD1064', N'10:00', '2024-06-24 10:00:00', N'Bàn 20', N'Võ Thị Hoài Thu',    1180000,  N'Đã thanh toán'),
(N'HD1063', N'09:45', '2024-06-24 09:45:00', N'Bàn 24', N'Trần Đình Khang',     450000,  N'Đã thanh toán'),
(N'HD1062', N'09:30', '2024-06-24 09:30:00', N'Bàn 23', N'Bùi Ngọc Ánh',        790000,  N'Đã thanh toán');
GO

-- ============================================================
--  NHẬP DỮ LIỆU - NHẬT KÝ HOẠT ĐỘNG (20 bản ghi)
-- ============================================================
INSERT INTO dbo.NhatKyHoatDong (NoiDung, ThoiGian) VALUES
(N'[Bàn 08] Yêu cầu gọi thêm ba chỉ bò Mỹ (x2)',                    '2024-06-24 14:05:00'),
(N'[Bàn 15] Thanh toán hóa đơn HD1085: 1.240.000 đ',                 '2024-06-24 13:50:00'),
(N'[Kho] Cảnh báo: Sắp hết Ba chỉ bò Mỹ (còn 4kg)',                  '2024-06-24 13:35:00'),
(N'[Bàn 04] Khách đặt trước DB004 đã nhận bàn',                       '2024-06-24 13:20:00'),
(N'[Bàn 12] Đổi bàn sang Bàn 18 theo yêu cầu khách',                 '2024-06-24 13:02:00'),
(N'[Bàn 21] Gọi thanh toán qua chuyển khoản QR',                     '2024-06-24 12:45:00'),
(N'[Quầy bar] Bổ sung thêm 20 xiên chả cá Busan',                    '2024-06-24 12:30:00'),
(N'[Đặt bàn] Tiếp nhận đặt bàn mới từ Đỗ Khánh Linh (DB014)',        '2024-06-24 12:15:00'),
(N'[Bếp] Đã chuẩn bị xong 10 suất Tokbokki sốt Rose',                '2024-06-24 12:00:00'),
(N'[Bàn 07] Yêu cầu tiếp thêm nước dùng sốt Dokki cay',              '2024-06-24 11:45:00'),
(N'[Bàn 02] Khách đoàn 4 người check-in thành công',                  '2024-06-24 11:30:00'),
(N'[Kho] Nhập kho thành công lô hàng Tôm sú tươi (15kg)',             '2024-06-24 11:15:00'),
(N'[Kho] Cảnh báo: Trà đào sả hạt chia tạm thời hết hàng',           '2024-06-24 11:00:00'),
(N'[Đặt bàn] Xác nhận lịch hẹn DB012 cho khách Vũ Đức Anh',          '2024-06-24 10:45:00'),
(N'[Thu ngân] Bắt đầu ca trưa - Kiểm tra két tiền đầu ca',           '2024-06-24 10:30:00'),
(N'[Quầy line] Kiểm tra vệ sinh an toàn thực phẩm hoàn tất',         '2024-06-24 10:15:00'),
(N'[Bếp] Hoàn thành nấu sốt lẩu Ddukmo & Gungjung',                  '2024-06-24 10:00:00'),
(N'[Quản lý] Họp giao ban đầu ngày ca sáng chi nhánh',               '2024-06-24 09:45:00'),
(N'[Đặt bàn] Nhận đặt trước 5 lượt cho khung giờ tối',               '2024-06-24 09:30:00'),
(N'[Hệ thống] Mở ca hoạt động ngày 24/06/2024',                       '2024-06-24 09:00:00');
GO

-- ============================================================
--  KIỂM TRA DỮ LIỆU
-- ============================================================
SELECT N'DanhMuc'        AS [Bảng], COUNT(*) AS [Số bản ghi] FROM dbo.DanhMuc
UNION ALL
SELECT N'MonAn',                    COUNT(*)                  FROM dbo.MonAn
UNION ALL
SELECT N'BanAn',                    COUNT(*)                  FROM dbo.BanAn
UNION ALL
SELECT N'DatBan',                   COUNT(*)                  FROM dbo.DatBan
UNION ALL
SELECT N'HoaDon',                   COUNT(*)                  FROM dbo.HoaDon
UNION ALL
SELECT N'NhatKyHoatDong',           COUNT(*)                  FROM dbo.NhatKyHoatDong;
GO

PRINT N'✅ DokkiRestaurant database đã được tạo và nhập dữ liệu thành công!';
GO
