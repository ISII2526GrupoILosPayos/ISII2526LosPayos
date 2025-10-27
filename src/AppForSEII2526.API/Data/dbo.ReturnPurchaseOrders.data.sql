SET IDENTITY_INSERT [dbo].[ReturnPurchaseOrders] ON
INSERT INTO [dbo].[ReturnPurchaseOrders] ([Id], [Name], [TotalPrice], [NewTotalPrice], [MoneyToReturn], [Date], [Rating], [PaymentMethodId], [CustomerId]) VALUES (16, N'Devolucion1', CAST(20.00 AS Decimal(10, 2)), CAST(25.00 AS Decimal(10, 2)), CAST(10.00 AS Decimal(10, 2)), N'2002-11-27 00:00:00', NULL, 3, N'1')
SET IDENTITY_INSERT [dbo].[ReturnPurchaseOrders] OFF
