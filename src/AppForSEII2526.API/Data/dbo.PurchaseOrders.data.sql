SET IDENTITY_INSERT [dbo].[PurchaseOrders] ON
INSERT INTO [dbo].[PurchaseOrders] ([Id], [City], [TotalPrice], [Date], [Description], [NameSurname], [PostalCode], [Rating], [Street], [ApplicationUserId], [PaymentMethodId], [State]) VALUES (2, N'Villarrobledo', CAST(100.00 AS Decimal(10, 2)), N'2025-07-15 00:00:00', N'Food', N'Paco Porras', N'02600', 10, N'Reyes Catolicos', N'1', 4, N'Done')
INSERT INTO [dbo].[PurchaseOrders] ([Id], [City], [TotalPrice], [Date], [Description], [NameSurname], [PostalCode], [Rating], [Street], [ApplicationUserId], [PaymentMethodId], [State]) VALUES (30, N'Albacete', CAST(45.00 AS Decimal(10, 2)), N'2025-10-10 00:00:00', N'Two Shirts', N'Pau Femenia', N'02006', 4, N'Campus', N'1', 3, N'Done')
INSERT INTO [dbo].[PurchaseOrders] ([Id], [City], [TotalPrice], [Date], [Description], [NameSurname], [PostalCode], [Rating], [Street], [ApplicationUserId], [PaymentMethodId], [State]) VALUES (32, N'Madrid', CAST(550.00 AS Decimal(10, 2)), N'2025-10-31 00:00:00', N'Gaming', N'Kylian Mbappe', N'28001', 10, N'La Finca', N'2', 5, N'Done')
SET IDENTITY_INSERT [dbo].[PurchaseOrders] OFF
INSERT INTO [dbo].[PurchaseProducts] ([ProductId], [PurchaseOrderId], [Quantity], [Price]) VALUES (5, 30, 2, CAST(20.00 AS Decimal(10, 2)))
SET IDENTITY_INSERT [dbo].[ReturnPurchaseOrders] ON
INSERT INTO [dbo].[ReturnPurchaseOrders] ([Id], [Name], [TotalPrice], [NewTotalPrice], [MoneyToReturn], [Date], [Rating], [PaymentMethodId], [CustomerId]) VALUES (16, N'Devolucion1', CAST(20.00 AS Decimal(10, 2)), CAST(25.00 AS Decimal(10, 2)), CAST(10.00 AS Decimal(10, 2)), N'2002-11-27 00:00:00', NULL, 3, N'1')
SET IDENTITY_INSERT [dbo].[ReturnPurchaseOrders] OFF

INSERT INTO [dbo].[ReturnProducts] (
    [ProductId], [PurchaseOrderId], [Id], [Quantity], [Reason], [ReturnOrderId]
) VALUES (
    5, 30, 1, 2, N'Defective', 16
);
