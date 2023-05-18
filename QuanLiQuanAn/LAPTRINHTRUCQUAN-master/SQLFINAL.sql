Create database QUANLIQUANANGK
GO
use QUANLIQUANANGK
GO
CREATE TABLE FoodCategory
(
	id INT IDENTITY PRIMARY KEY,
	name NVARCHAR(100) NOT NULL DEFAULT N'Chưa đặt tên',
	deleteFoodCategory Float not null default 0,

)
GO
CREATE TABLE Food
(
	id INT IDENTITY PRIMARY KEY,
	name NVARCHAR(100) NOT NULL DEFAULT N'Chưa đặt tên',
	idCategory INT NOT NULL,
	price FLOAT NOT NULL DEFAULT 0,
	size Nvarchar(20) ,
	imageFood Nvarchar(1000) ,
	deleteFood Float not null default 0,
	dateInsertFood DATE NOT NULL DEFAULT GETDATE(),
	dateDeleteFood DATE,
	FOREIGN KEY (idCategory) REFERENCES FoodCategory(id)
)
go
CREATE TABLE TableFood
(
	id INT IDENTITY PRIMARY KEY,
	idbill INT not null default 0,
	name NVARCHAR(100) NOT NULL DEFAULT N'Bàn chưa có tên',
	status NVARCHAR(100) NOT NULL DEFAULT N'Trống'	-- Trống || Có người
)
go


create table Users
(
	USERNAME NVARCHAR(100) PRIMARY KEY,
	PASSWORD NVARCHAR(1000) NOT NULL DEFAULT 0,
	TYPE INT NOT NULL DEFAULT 0, -- 0:STAFF && 1:ADMIN
	HOTEN NVARCHAR(200) NOT NULL ,
	GIOITINH NVARCHAR(20) NOT NULL ,
	NGAYSINH DATE NOT NULL DEFAULT GETDATE(),
	SDT NVARCHAR(20) NOT NULL,
	EMAIL NVARCHAR(100) NOT NULL,
	DIACHI NVARCHAR(300) NOT NULL,
	IMAGEUSER NVARCHAR(1000) NOT NULL DEFAULT N'pack://application:,,,/Image/pngtree-vector-businessman-icon-png-image_924876.jpg',
	DANGNHAP INT NOT NULL DEFAULT 0,
)
GO

SET DATEFORMAT DMY
Insert into Users(UserName,PASSWORD,TYPE,HOTEN,GIOITINH,NGAYSINH,SDT,EMAIL,DIACHI,IMAGEUSER) values(N'Admin',N'QWRtaW4=',1,N'Nguyễn Minh Chánh',N'Nam','03/08/2003','0344033842','nguyenminhchanh3842@gmail.com',N'Củ Chi','pack://application:,,,/Image/Tieuvu.jpg')
go

CREATE TABLE Bill
(
	id INT IDENTITY PRIMARY KEY,
	DateCheckIn DATE NOT NULL DEFAULT GETDATE(),
	DateCheckOut DATE,
	idTable INT,
	status INT NOT NULL DEFAULT 0 -- 1: đã thanh toán && 0: chưa thanh toán,
	, Total float not null default 0,
	PrintHD INT,
	
	FOREIGN KEY (idTable) REFERENCES TableFood(id)
)

CREATE TABLE BillInfo
(
	id INT IDENTITY PRIMARY KEY,
	idBill INT NOT NULL,
	idFood INT NOT NULL,
	count INT NOT NULL DEFAULT 0,
	deleteBillInfo int not null default 0
	
	FOREIGN KEY (idBill) REFERENCES dbo.Bill(id),
	FOREIGN KEY (idFood) REFERENCES dbo.Food(id)
)


--Thêm --
DECLARE @i INT = 0

WHILE @i <= 10
BEGIN
	INSERT dbo.TableFood ( name)VALUES  ( N'Bàn ' + CAST(@i AS nvarchar(100)))
	SET @i = @i + 1
END


UPDATE TableFood
SET NAME =  N'Bàn ' + CAST(id AS nvarchar(100))
where (ID >=1 and ID<=11)

UPDATE TableFood SET STATUS = N'Có người' WHERE id = 9




-- thêm bill
--create proc
USE QUANLIQUANANGK
go


--create proc
GO
SET DATEFORMAT dmy
GO

--INCOME BY TABLE PROC
CREATE PROC IncomeByTable
(@m INT,
@y INT)
AS
BEGIN
IF(@m = -1 AND @y = -1)
	BEGIN
	    SELECT name AS N'Bàn', sum N'Tổng tiền' FROM dbo.TableFood JOIN
		(SELECT TableFood.id, SUM(Total) AS sum FROM dbo.Bill
		JOIN dbo.TableFood
		ON TableFood.id = Bill.idTable
		GROUP BY TableFood.id) AS a
		ON a.id = TableFood.id
		ORDER BY a.sum DESC
	END
ELSE IF(@m = -1 AND @y <> -1)
	BEGIN	
		SELECT name AS N'Bàn', sum N'Tổng tiền' FROM dbo.TableFood JOIN
		(SELECT TableFood.id, SUM(Total) AS sum FROM dbo.Bill
		JOIN dbo.TableFood
		ON TableFood.id = Bill.idTable
		WHERE YEAR(DateCheckOut) = @y
		GROUP BY TableFood.id) AS a
		ON a.id = TableFood.id
		ORDER BY a.sum DESC
	END
ELSE IF (@m <> -1 AND @y = -1)
	BEGIN
	    PRINT 'ERROR: YEAR MISSING'
	END
ELSE
	BEGIN
	    SELECT name AS N'Bàn', sum N'Tổng tiền' FROM dbo.TableFood JOIN
		(SELECT TableFood.id, SUM(Total) AS sum FROM dbo.Bill
		JOIN dbo.TableFood
		ON TableFood.id = Bill.idTable
		WHERE YEAR(DateCheckOut) = @y AND MONTH(DateCheckOut) = @m
		GROUP BY TableFood.id) AS a
		ON a.id = TableFood.id
		ORDER BY a.sum DESC
	END
END
GO
-- CREATE INCOME BY FOODCATEGORY PROC
CREATE PROC IncomeByFoodCategory (@m INT , @y INT)
AS
BEGIN
	  
		IF(@m = -1 and @y = -1)
			BEGIN
					SELECT name AS 'Category', total AS N'Total' FROM dbo.FoodCategory JOIN (
					SELECT idCategory, sum(price * a.count) AS total  FROM dbo.Food
					JOIN (
					SELECT idFood, count FROM dbo.Bill
					JOIN dbo.BillInfo
					ON BillInfo.idBill = Bill.id
					WHERE Bill.status = 1 or Bill.status = 2) as a
					ON a.idFood = Food.id
					GROUP BY idCategory) AS b
					ON b.idCategory = FoodCategory.id
			END
		ELSE IF (@m = -1 and @y <> -1)
			BEGIN
			SELECT name AS 'Category', total AS N'Total' FROM dbo.FoodCategory JOIN (
					SELECT idCategory, sum(price * a.count) AS total  FROM dbo.Food
					JOIN (
					SELECT idFood, count FROM dbo.Bill
					JOIN dbo.BillInfo
					ON BillInfo.idBill = Bill.id
					WHERE Year(DateCheckOut) = @y and (Bill.status = 1 or Bill.status = 2)) as a
					ON a.idFood = Food.id
					GROUP BY idCategory) AS b
					ON b.idCategory = FoodCategory.id
			END
		ELSE IF (@m <> -1 AND @y = -1)
			BEGIN
				PRINT 'ERROR: YEAR MISSING'
			END
		ELSE
			BEGIN
			SELECT name AS 'Category', total AS N'Total' FROM dbo.FoodCategory JOIN (
				SELECT idCategory, sum(price * a.count) AS total  FROM dbo.Food
				JOIN (
				SELECT idFood, count FROM dbo.Bill
				JOIN dbo.BillInfo
				ON BillInfo.idBill = Bill.id
				WHERE MONTH(DateCheckOut) = @m AND YEAR(DateCheckOut) = @y and (Bill.status = 1 or Bill.status = 2)) AS a
				ON a.idFood = Food.id
				GROUP BY idCategory) AS b
				ON b.idCategory = FoodCategory.id
			END
END
GO
-- CREATE DATE INCOME PROC
CREATE PROC DayIncome @d DATE
AS
BEGIN
    SELECT SUM(Total) FROM dbo.Bill
WHERE DateCheckOut = @d
END
GO
--CREATE TOP_SALES
CREATE PROC TOP_SALES @TOP INT
AS 
BEGIN
    SELECT TOP (@TOP) WITH TIES F.name AS N'Sản phẩm', A.Total AS N'Số lượng' FROM Food F
JOIN
(SELECT BI.idFood, SUM(BI.count) as Total FROM BillInfo BI
JOIN Bill B
ON B.id = BI.idBill
WHERE B.status = 1 OR B.status = 2
GROUP BY BI.idFood) AS A
ON F.id = A.idFood
ORDER BY A.Total DESC
END

GO
--CREATE MONTH INCOME PROC
CREATE PROC MonthIncome @m INT
AS
BEGIN
    SELECT SUM(Total) FROM dbo.Bill
	WHERE MONTH(DateCheckOut) = @m AND (status = 1 OR status = 2) 
END
GO
-- ALTER INCOME BY TABLE PROC
ALTER PROC IncomeByTable (@s DATETIME,@e DATETIME)
AS
BEGIN
DECLARE @n DATETIME
SELECT @n = DATEADD(DAY,100,GETDATE())
SELECT @s = ISNULL(@s, @n)
SELECT @e = ISNULL(@e, @n)
IF(@s = @n AND @e = @n)
	BEGIN
	    SELECT name AS N'Bàn', sum N'Tổng tiền' FROM dbo.TableFood JOIN
		(SELECT TableFood.id, SUM(Total) AS sum FROM dbo.Bill
		JOIN dbo.TableFood
		ON TableFood.id = Bill.idTable
		GROUP BY TableFood.id ) AS a
		ON a.id = TableFood.id
		ORDER BY a.sum DESC
	END
ELSE IF(@s = @n AND @e <> @n)
	BEGIN	
		SELECT name AS N'Bàn', sum N'Tổng tiền' FROM dbo.TableFood JOIN
		(SELECT TableFood.id, SUM(Total) AS sum FROM dbo.Bill
		JOIN dbo.TableFood
		ON TableFood.id = Bill.idTable
		WHERE DateCheckOut <= @e
		GROUP BY TableFood.id) AS a
		ON a.id = TableFood.id
		ORDER BY a.sum DESC
	END
ELSE IF (@s <> @n AND @e = @n)
	BEGIN
	    SELECT name AS N'Bàn', sum N'Tổng tiền' FROM dbo.TableFood JOIN
		(SELECT TableFood.id, SUM(Total) AS sum FROM dbo.Bill
		JOIN dbo.TableFood
		ON TableFood.id = Bill.idTable
		WHERE DateCheckOut >= @s
		GROUP BY TableFood.id) AS a
		ON a.id = TableFood.id
		ORDER BY a.sum DESC
	END
ELSE
	BEGIN
	    SELECT name AS N'Bàn', sum N'Tổng tiền' FROM dbo.TableFood JOIN
		(SELECT TableFood.id, SUM(Total) AS sum FROM dbo.Bill
		JOIN dbo.TableFood
		ON TableFood.id = Bill.idTable
		WHERE DateCheckOut >= @s AND DateCheckOut <= @e
		GROUP BY TableFood.id) AS a
		ON a.id = TableFood.id
		ORDER BY a.sum DESC
	END
END

GO
--ALTER TOP_SALES
ALTER PROC TOP_SALES @TOP INT
AS 
BEGIN
    SELECT TOP (@TOP) WITH TIES F.name AS N'Sản phẩm', A.Total AS N'Số lượng' FROM Food F
JOIN
(SELECT BI.idFood, SUM(BI.count) as Total FROM BillInfo BI
JOIN Bill B
ON B.id = BI.idBill
WHERE B.status = 1 OR B.status = 2
GROUP BY BI.idFood) AS A
ON F.id = A.idFood
ORDER BY A.Total DESC
END

GO
--ALTER CREATE MONTH INCOME PROC
ALTER PROC MonthIncome @m INT
AS
BEGIN
    SELECT SUM(Total) FROM dbo.Bill
	WHERE MONTH(DateCheckOut) = @m AND (status = 1 OR status = 2) 
END

GO
--INCOME BY TABLE PROC
ALTER PROC IncomeByTable (@s DATETIME,@e DATETIME)
AS
BEGIN
DECLARE @n DATETIME
SELECT @n = DATEADD(DAY,100,GETDATE())
SELECT @s = ISNULL(@s, @n)
SELECT @e = ISNULL(@e, @n)
IF(@s = @n AND @e = @n)
	BEGIN
	    SELECT name AS N'Bàn', sum N'Tổng tiền' FROM dbo.TableFood JOIN
		(SELECT TableFood.id, SUM(Total) AS sum FROM dbo.Bill
		JOIN dbo.TableFood
		ON TableFood.id = Bill.idTable
		WHERE (Bill.status = 1 or Bill.status = 2)
		GROUP BY TableFood.id) AS a
		ON a.id = TableFood.id
		ORDER BY a.sum DESC
	END
ELSE IF(@s = @n AND @e <> @n)
	BEGIN	
		SELECT name AS N'Bàn', sum N'Tổng tiền' FROM dbo.TableFood JOIN
		(SELECT TableFood.id, SUM(Total) AS sum FROM dbo.Bill
		JOIN dbo.TableFood
		ON TableFood.id = Bill.idTable
		WHERE DateCheckOut <= @e AND (Bill.status = 1 or Bill.status = 2)
		GROUP BY TableFood.id) AS a
		ON a.id = TableFood.id
		ORDER BY a.sum DESC
	END
ELSE IF (@s <> @n AND @e = @n)
	BEGIN
	    SELECT name AS N'Bàn', sum N'Tổng tiền' FROM dbo.TableFood JOIN
		(SELECT TableFood.id, SUM(Total) AS sum FROM dbo.Bill
		JOIN dbo.TableFood
		ON TableFood.id = Bill.idTable
		WHERE DateCheckOut >= @s AND (Bill.status = 1 or Bill.status = 2)
		GROUP BY TableFood.id) AS a
		ON a.id = TableFood.id
		ORDER BY a.sum DESC
	END
ELSE
	BEGIN
	    SELECT name AS N'Bàn', sum N'Tổng tiền' FROM dbo.TableFood JOIN
		(SELECT TableFood.id, SUM(Total) AS sum FROM dbo.Bill
		JOIN dbo.TableFood
		ON TableFood.id = Bill.idTable
		WHERE DateCheckOut >= @s AND DateCheckOut <= @e AND (Bill.status = 1 or Bill.status = 2)
		GROUP BY TableFood.id) AS a
		ON a.id = TableFood.id
		ORDER BY a.sum DESC
	END
END
GO
--ALTER INCOMEBYFOODCATEGORY
ALTER PROC IncomeByFoodCategory (@s DATETIME , @e DATETIME)
AS
BEGIN
		DECLARE @n DATETIME
		SELECT @n = DATEADD(DAY,100,GETDATE())
		SELECT @s = ISNULL(@s, @n)
		SELECT @e = ISNULL(@e, @n)
		IF(@s = @n and @e = @n)
			BEGIN
					SELECT name AS 'Category', total AS N'Total' FROM dbo.FoodCategory JOIN (
					SELECT idCategory, sum(price * a.count) AS total  FROM dbo.Food
					JOIN (
					SELECT idFood, count FROM dbo.Bill
					JOIN dbo.BillInfo
					ON BillInfo.idBill = Bill.id
					WHERE Bill.status = 1 or Bill.status = 2) as a
					ON a.idFood = Food.id
					GROUP BY idCategory) AS b
					ON b.idCategory = FoodCategory.id

					PRINT 'null null'
			END
		ELSE IF (@s = @n and @e <> @n)
			BEGIN
			SELECT name AS 'Category', total AS N'Total' FROM dbo.FoodCategory JOIN (
					SELECT idCategory, sum(price * a.count) AS total  FROM dbo.Food
					JOIN (
					SELECT idFood, count FROM dbo.Bill
					JOIN dbo.BillInfo
					ON BillInfo.idBill = Bill.id
					WHERE DateCheckOut <= @e and (Bill.status = 1 or Bill.status = 2)) as a
					ON a.idFood = Food.id
					GROUP BY idCategory) AS b
					ON b.idCategory = FoodCategory.id

					PRINT 'null notnull'
			END
		ELSE IF (@s <> @n AND @e = @n)
			BEGIN
				PRINT N'ERROR: YEAR MISSING'
			END
		ELSE
			BEGIN
			SELECT name AS 'Category', total AS N'Total' FROM dbo.FoodCategory JOIN (
				SELECT idCategory, sum(price * a.count) AS total  FROM dbo.Food
				JOIN (
				SELECT idFood, count FROM dbo.Bill
				JOIN dbo.BillInfo
				ON BillInfo.idBill = Bill.id
				WHERE DateCheckOut >= @s and (Bill.status = 1 or Bill.status = 2)) AS a
				ON a.idFood = Food.id
				GROUP BY idCategory) AS b
				ON b.idCategory = FoodCategory.id
				PRINT 'notnull null'
			END
END
GO 



