SET IDENTITY_INSERT [dbo].[PurchaseOrders] ON
INSERT INTO [dbo].[PurchaseOrders] ([Id], [City], [TotalPrice], [Date], [Description], [NameSurname], [PostalCode], [Rating], [Street], [ApplicationUserId], [PaymentMethodId], [State]) VALUES (30, N'Albacete', CAST(20.00 AS Decimal(10, 2)), N'2025-10-10 00:00:00', N'Two Shirts', N'Pau Femenia', N'02006', 4, N'Campus', N'1', 3, N'Albacete')
INSERT INTO [dbo].[PurchaseOrders] ([Id], [City], [TotalPrice], [Date], [Description], [NameSurname], [PostalCode], [Rating], [Street], [ApplicationUserId], [PaymentMethodId], [State]) VALUES (2, N'Villarrobledo', CAST(150.00 AS Decimal(10, 2)), N'2025-07-15 00:00:00', N'Food', N'Paco Porras', N'02600', 10, N'Reyes Catolicos', N'1', 4, N'Done')
SET IDENTITY_INSERT [dbo].[PurchaseOrders] OFF
