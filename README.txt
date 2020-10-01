QuickReports README:

--------------------------------------- DESCRIPTION ---------------------------------------------

QuickReports is a pet project designed to be a quick and easy reporting tool for product sales.


--------------------------------------- GET STARTED ---------------------------------------------

Before you start, run the following SQL script in your Sql Server Management Studio

Database script:

CREATE DATABASE EmployeeDB;
USE EmployeeDB;

CREATE TABLE Employees(
	ID INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	FirstName NVARCHAR(50) NOT NULL,
	LastName NVARCHAR(50) NOT NULL,
	Gender NVARCHAR(50),
	Salary INT,
	UpdateTimestamp DATETIME NOT NULL
);

CREATE TABLE LoginCredentials(
	UserId INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	UserName NVARCHAR(50) NULL,
	Password NVARCHAR(50) NULL
);

CREATE TABLE Products(
	ProductId INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	Name NVARCHAR(100) NOT NULL,
	Description NVARCHAR(200),
	Price INT NOT NULL
);

CREATE TABLE Sales(
	SaleId INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	ProductId INT FOREIGN KEY REFERENCES Products(ProductId),
	EmployeeId INT FOREIGN KEY REFERENCES Employees(ID),
	CustomerFirstName NVARCHAR(50) NOT NULL,
	CustomerLastName NVARCHAR(50) NOT NULL,
	Comment NVARCHAR(100),
	Quantity INT NOT NULL,
	Amount MONEY NOT NULL
);

INSERT INTO Employees (FirstName, LastName, Gender, Salary, UpdateTimeStamp) VALUES ('test', 'test', 'Male', 50000, GETDATE());
INSERT INTO LoginCredentials VALUES ('test', 'test');
INSERT INTO Products (Name, Description, Price) VALUES ('RC300', 'test', 50);
INSERT INTO Sales (ProductId, EmployeeId, CustomerFirstName, CustomerLastName, Comment, Quantity, Amount) VALUES (1, 1, 'john', 'smith', 'test comment', 2, 100);