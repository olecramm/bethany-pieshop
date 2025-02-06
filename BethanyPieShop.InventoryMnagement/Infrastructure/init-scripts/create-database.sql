-- Create the database
CREATE DATABASE BethanyPieShop

-- Switch to the new database
USE BethanyPieShop;

-- Create ProductType table
CREATE TABLE ProductType (
    ProductTypeID INT PRIMARY KEY IDENTITY(1,1),
    ProductType NVARCHAR(100) NOT NULL
);

-- Create Currency table
CREATE TABLE Currency (
    CurrencyID INT PRIMARY KEY IDENTITY(1,1),
    Currency NVARCHAR(50) NOT NULL
);

-- Create UnitType table
CREATE TABLE UnitType (
    UnitTypeID INT PRIMARY KEY IDENTITY(1,1),
    UnitType NVARCHAR(50) NOT NULL
);

-- Create Product table
CREATE TABLE Product (
    ProductID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(50) NOT NULL,
    Description NVARCHAR(140),
    AmountInStock INT NOT NULL,
    Price DECIMAL(18,2) NOT NULL,
    CurrencyID INT NOT NULL,
    UnitTypeID INT NOT NULL,
    ProductTypeID INT NOT NULL,
    MaxAmountInStock INT NOT NULL,
    
    -- Foreign key constraints
    CONSTRAINT FK_Product_Currency FOREIGN KEY (CurrencyID) REFERENCES Currency(CurrencyID),
    CONSTRAINT FK_Product_UnitType FOREIGN KEY (UnitTypeID) REFERENCES UnitType(UnitTypeID),
    CONSTRAINT FK_Product_ProductType FOREIGN KEY (ProductTypeID) REFERENCES ProductType(ProductTypeID)
);