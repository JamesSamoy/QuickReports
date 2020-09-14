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

INSERT INTO Employees (FirstName, LastName, Gender, Salary, UpdateTimeStamp) VALUES ('test', 'test', 'Male', 50000, GETDATE());
INSERT INTO LoginCredentials VALUES ('test', 'test');