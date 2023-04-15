USE master;
GO

CREATE DATABASE logistics;
GO

USE logistics;
GO

IF DB_ID() > 0
BEGIN
    IF OBJECT_ID('dbo.Vehicles', 'U') IS NULL
    BEGIN
        CREATE TABLE dbo.Vehicles (
            Id INT PRIMARY KEY,
            Type NVARCHAR(50),
            Number NVARCHAR(50),
            MaxCargoWeightKg INT,
            MaxCargoWeightPd FLOAT,
            MaxCargoVolume FLOAT
        );
    END

    IF OBJECT_ID('dbo.Warehouses', 'U') IS NULL
    BEGIN
        CREATE TABLE dbo.Warehouses (
            Id INT PRIMARY KEY
        );
    END

    IF OBJECT_ID('dbo.Invoices', 'U') IS NULL
    BEGIN
        CREATE TABLE dbo.Invoices (
            Id UNIQUEIDENTIFIER PRIMARY KEY,
            RecipientAddress NVARCHAR(100),
            SenderAddress NVARCHAR(100),
            RecipientPhoneNumber NVARCHAR(20),
            SenderPhoneNumber NVARCHAR(20)
        );
    END

    IF OBJECT_ID('dbo.Cargos', 'U') IS NULL
    BEGIN
        CREATE TABLE dbo.Cargos (
            Id UNIQUEIDENTIFIER PRIMARY KEY,
            Code NVARCHAR(50),
            Weight INT,
            Volume FLOAT,
            VehicleId INT NULL,
            WarehouseId INT NULL,
            InvoiceId UNIQUEIDENTIFIER NULL,
            CONSTRAINT FK_Cargos_Vehicles FOREIGN KEY (VehicleId) REFERENCES dbo.Vehicles(Id),
            CONSTRAINT FK_Cargos_Warehouses FOREIGN KEY (WarehouseId) REFERENCES dbo.Warehouses(Id),
            CONSTRAINT FK_Cargos_Invoices FOREIGN KEY (InvoiceId) REFERENCES dbo.Invoices(Id)
        );
    END
END
