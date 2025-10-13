SET IDENTITY_INSERT [dbo].[Products] ON
INSERT INTO [dbo].[Products] ([ProductId], [Name], [Description], [Colour], [Price], [Stock], [IsReturnable], [BrandId]) VALUES (10, N'Shampoo', N'New', N'White', CAST(5.00 AS Decimal(10, 2)), 100, 0, 1)
INSERT INTO [dbo].[Products] ([ProductId], [Name], [Description], [Colour], [Price], [Stock], [IsReturnable], [BrandId]) VALUES (11, N'Water', N'Smart', N'Blue', CAST(1.20 AS Decimal(10, 2)), 100, 0, 2)
SET IDENTITY_INSERT [dbo].[Products] OFF
