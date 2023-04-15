BEGIN TRANSACTION;

INSERT INTO Vehicles (Id, Type, Number, MaxCargoWeightKg, MaxCargoWeightPd, MaxCargoVolume)
VALUES
    (1, 'Car', 'CAR001', 1000, 2204.62, 10),
    (2, 'Ship', 'SHIP001', 50000, 110231.13, 100000),
    (3, 'Plane', 'PLANE001', 5000, 11023.11, 500),
    (4, 'Train', 'TRAIN001', 20000, 44092.45, 5000);

INSERT INTO Warehouses (Id) VALUES (1), (2);
    
INSERT INTO Invoices (Id, RecipientAddress, SenderAddress, RecipientPhoneNumber, SenderPhoneNumber)
VALUES
    (NEWID(), 'Recipient1', 'Sender1', '123456789', '987654321'),
    (NEWID(), 'Recipient2', 'Sender2', '987654321', '123456789'),
    (NEWID(), 'Recipient3', 'Sender3', '123123123', '456456456'),
    (NEWID(), 'Recipient4', 'Sender4', '789789789', '321321321'),
    (NEWID(), 'Recipient5', 'Sender5', '111111111', '222222222'),
    (NEWID(), 'Recipient6', 'Sender6', '333333333', '444444444'),
    (NEWID(), 'Recipient7', 'Sender7', '555555555', '666666666'),
    (NEWID(), 'Recipient8', 'Sender8', '777777777', '888888888');

WITH InvoiceCTE AS (
    SELECT Id, ROW_NUMBER() OVER (ORDER BY Id) AS RowNum
    FROM Invoices
)
INSERT INTO Cargos (Id, Code, Weight, Volume, VehicleId, WarehouseId, InvoiceId)
VALUES
    (NEWID(), 'CARCARGO1', 500, 2, (SELECT Id FROM Vehicles WHERE Type = 'Car'), NULL, (SELECT Id FROM InvoiceCTE WHERE RowNum = 1)),
    (NEWID(), 'SHIPCARGO1', 10000, 20, (SELECT Id FROM Vehicles WHERE Type = 'Ship'), NULL, (SELECT Id FROM InvoiceCTE WHERE RowNum = 2)),
    (NEWID(), 'PLANECARGO1', 1000, 1, (SELECT Id FROM Vehicles WHERE Type = 'Plane'), NULL, (SELECT Id FROM InvoiceCTE WHERE RowNum = 3)),
    (NEWID(), 'TRAINCARGO1', 3000, 5, (SELECT Id FROM Vehicles WHERE Type = 'Train'), NULL, (SELECT Id FROM InvoiceCTE WHERE RowNum = 4)),
    (NEWID(), 'WAREHOUSECARGO1', 100, 1, NULL, (SELECT TOP 1 Id FROM Warehouses ORDER BY NEWID()), (SELECT Id FROM InvoiceCTE WHERE RowNum = 5)),
    (NEWID(), 'WAREHOUSECARGO2', 200, 2, NULL, (SELECT TOP 1 Id FROM Warehouses ORDER BY NEWID()), (SELECT Id FROM InvoiceCTE WHERE RowNum = 6)),
    (NEWID(), 'WAREHOUSECARGO3', 300, 3, NULL, (SELECT TOP 1 Id FROM Warehouses ORDER BY NEWID()), (SELECT Id FROM InvoiceCTE WHERE RowNum = 7)),
    (NEWID(), 'WAREHOUSECARGO4', 400, 4, NULL, (SELECT TOP 1 Id FROM Warehouses ORDER BY NEWID()), (SELECT Id FROM InvoiceCTE WHERE RowNum = 8));

COMMIT;