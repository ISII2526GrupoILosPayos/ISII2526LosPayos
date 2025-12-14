SET IDENTITY_INSERT [dbo].[Products] ON
INSERT INTO [dbo].[Products] ([ProductId], [Name], [Description], [Colour], [Price], [Stock], [IsReturnable], [BrandId]) VALUES (4, N'Camiseta', N'Camiseta', N'Blue', CAST(15.00 AS Decimal(10, 2)), 10, 1, 1)
INSERT INTO [dbo].[Products] ([ProductId], [Name], [Description], [Colour], [Price], [Stock], [IsReturnable], [BrandId]) VALUES (5, N'Pantalon', N'Pantalon', N'Negro', CAST(30.00 AS Decimal(10, 2)), 30, 1, 2)
INSERT INTO [dbo].[Products] ([ProductId], [Name], [Description], [Colour], [Price], [Stock], [IsReturnable], [BrandId]) VALUES (10, N'Shampoo', N'New', N'White', CAST(5.00 AS Decimal(10, 2)), 100, 0, 3)
INSERT INTO [dbo].[Products] ([ProductId], [Name], [Description], [Colour], [Price], [Stock], [IsReturnable], [BrandId]) VALUES (11, N'Water', N'Smart', N'Blue', CAST(1.20 AS Decimal(10, 2)), 100, 0, 4)
INSERT INTO [dbo].[Products] ([ProductId], [Name], [Description], [Colour], [Price], [Stock], [IsReturnable], [BrandId]) VALUES (12, N'PS5', N'Gaming', N'White', CAST(500.00 AS Decimal(10, 2)), 10, 1, 5)
INSERT INTO [dbo].[Products] ([ProductId], [Name], [Description], [Colour], [Price], [Stock], [IsReturnable], [BrandId]) VALUES (13, N'Controller', N'Gaming', N'White', CAST(50.00 AS Decimal(10, 2)), 50, 1, 5)
INSERT INTO [dbo].[Products] ([ProductId], [Name], [Description], [Colour], [Price], [Stock], [IsReturnable], [BrandId]) VALUES (14, N'Gafas', N'Gafas', N'Negro', CAST(230.00 AS Decimal(10, 2)), 1, 1, 2)
INSERT INTO [dbo].[Products] ([ProductId], [Name], [Description], [Colour], [Price], [Stock], [IsReturnable], [BrandId]) VALUES (15, N'Calcetines', N'Calcetines', N'Blanco', CAST(1.00 AS Decimal(10, 2)), 10, 1, 2)
INSERT INTO [dbo].[Products] ([ProductId], [Name], [Description], [Colour], [Price], [Stock], [IsReturnable], [BrandId]) VALUES (16, N'Sudadera', N'Sudadera', N'Azul', CAST(30.00 AS Decimal(10, 2)), 3, 1, 1)
SET IDENTITY_INSERT [dbo].[Products] OFF
