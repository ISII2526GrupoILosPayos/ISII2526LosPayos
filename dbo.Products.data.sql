SET IDENTITY_INSERT [dbo].[Products] ON
INSERT INTO [dbo].[Products] ([ProductId], [Name], [Description], [Colour], [Price], [Stock], [IsReturnable], [BrandId]) VALUES (4, N'Camiseta', N'Camiseta', N'Blue', CAST(15.00 AS Decimal(10, 2)), 10, 1, 1)
INSERT INTO [dbo].[Products] ([ProductId], [Name], [Description], [Colour], [Price], [Stock], [IsReturnable], [BrandId]) VALUES (5, N'Pantalon', N'Pantalon', N'Negro', CAST(30.00 AS Decimal(10, 2)), 30, 1, 2)
SET IDENTITY_INSERT [dbo].[Products] OFF
