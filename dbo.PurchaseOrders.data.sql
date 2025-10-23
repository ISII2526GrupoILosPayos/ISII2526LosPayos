SET IDENTITY_INSERT [dbo].[PurchaseOrders] ON
INSERT INTO [dbo].[PurchaseOrders] ([Id], [City], [TotalPrice], [Date], [Description], [NameSurname], [PostalCode], [Rating], [Street], [ApplicationUserId], [PaymentMethodId], [State]) VALUES (30, N'Albacete', CAST(20.00 AS Decimal(10, 2)), N'2025-10-10 00:00:00', N'Two Shirts', N'Pau Femenia', N'02006', 4, N'Campus', N'1', 3, N'Albacete')
SET IDENTITY_INSERT [dbo].[PurchaseOrders] OFF
