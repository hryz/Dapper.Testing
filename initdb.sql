DROP DATABASE  IF EXISTS  [AdventureWorks]
GO

CREATE DATABASE [AdventureWorks]
GO

USE [AdventureWorks]
GO

CREATE SCHEMA [Production] AUTHORIZATION [dbo];
GO

CREATE TABLE [Production].[Product](
    [ProductID] [int] IDENTITY (1, 1) NOT NULL PRIMARY KEY ,
    [Name] [nvarchar](15) NOT NULL,
    [ProductNumber] [nvarchar](25) NOT NULL,
    [Color] [nvarchar](15) NULL,
    [SafetyStockLevel] [smallint] NOT NULL,
    [ReorderPoint] [smallint] NOT NULL,
    [StandardCost] [money] NOT NULL,
    [ListPrice] [money] NOT NULL,
    [Size] [nvarchar](5) NULL,
    [SizeUnitMeasureCode] [nchar](3) NULL,
    [WeightUnitMeasureCode] [nchar](3) NULL,
    [Weight] [decimal](8, 2) NULL,
    [DaysToManufacture] [int] NOT NULL,
    [ProductLine] [nchar](2) NULL,
    [Class] [nchar](2) NULL,
    [Style] [nchar](2) NULL,
    [ProductSubcategoryID] [int] NULL,
    [ProductModelID] [int] NULL,
    [SellStartDate] [datetime] NOT NULL,
    [SellEndDate] [datetime] NULL,
    [DiscontinuedDate] [datetime] NULL
) ON [PRIMARY];
GO

CREATE TABLE [Production].[ProductCategory](
    [ProductCategoryID] [int] IDENTITY (1, 1) NOT NULL PRIMARY KEY ,
    [Name] [nvarchar](15) NOT NULL
) ON [PRIMARY];
GO

CREATE TABLE [Production].[ProductSubcategory](
    [ProductSubcategoryID] [int] IDENTITY (1, 1) NOT NULL PRIMARY KEY ,
    [ProductCategoryID] [int] NOT NULL,
    [Name] [nvarchar](15) NOT NULL
) ON [PRIMARY];
GO

ALTER TABLE [Production].[ProductSubcategory] ADD
    CONSTRAINT [FK_ProductSubcategory_ProductCategory_ProductCategoryID] FOREIGN KEY
    (
        [ProductCategoryID]
    ) REFERENCES [Production].[ProductCategory](
        [ProductCategoryID]
    );
GO

ALTER TABLE [Production].[Product] ADD
    CONSTRAINT [FK_Product_ProductSubcategory_ProductSubcategoryID] FOREIGN KEY
    (
        [ProductSubcategoryID]
    ) REFERENCES [Production].[ProductSubcategory](
        [ProductSubcategoryID]
    );
GO