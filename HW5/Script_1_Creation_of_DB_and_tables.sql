USE master;
GO

CREATE DATABASE logistics;
GO

USE logistics;
GO

CREATE TABLE dbo.Vehicles (
    Id INT PRIMARY KEY,
    Type VARCHAR(50),
    Number VARCHAR(50),
    MaxCargoWeightKg INT,
    MaxCargoWeightPd FLOAT,
    MaxCargoVolume FLOAT
);

CREATE TABLE dbo.Warehouses (
    Id INT PRIMARY KEY
);

CREATE TABLE dbo.Invoices (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    RecipientAddress VARCHAR(100),
    SenderAddress VARCHAR(100),
    RecipientPhoneNumber VARCHAR(20),
    SenderPhoneNumber VARCHAR(20)
);

CREATE TABLE dbo.Cargos (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    Code VARCHAR(50),
    Weight INT,
    Volume FLOAT,
    VehicleId INT NULL REFERENCES dbo.Vehicles(Id),
    WarehouseId INT NULL REFERENCES dbo.Warehouses(Id),
    InvoiceId UNIQUEIDENTIFIER FOREIGN KEY REFERENCES dbo.Invoices(Id),
    CONSTRAINT FK_Cargos_Vehicles FOREIGN KEY (VehicleId) REFERENCES dbo.Vehicles(Id),
    CONSTRAINT FK_Cargos_Warehouses FOREIGN KEY (WarehouseId) REFERENCES dbo.Warehouses(Id),
    CONSTRAINT FK_Cargos_Invoices FOREIGN KEY (InvoiceId) REFERENCES dbo.Invoices(Id)
);