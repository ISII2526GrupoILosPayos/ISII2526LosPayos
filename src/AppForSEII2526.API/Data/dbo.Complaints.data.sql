SET IDENTITY_INSERT [dbo].[Complaints] ON
INSERT INTO [dbo].[Complaints] ([Id], [Description], [ComplaintDate], [Processed], [CustomerId], [TypeId]) VALUES (3, N'Hola', N'2025-10-26 00:00:00', 0, N'1', 1)
INSERT INTO [dbo].[Complaints] ([Id], [Description], [ComplaintDate], [Processed], [CustomerId], [TypeId]) VALUES (6, N'Adios', N'2024-10-25 00:00:00', 0, N'1', 2)
INSERT INTO [dbo].[Complaints] ([Id], [Description], [ComplaintDate], [Processed], [CustomerId], [TypeId]) VALUES (8, N'hasta', N'2000-01-01 00:00:00', 1, N'1', 1)
INSERT INTO [dbo].[Complaints] ([Id], [Description], [ComplaintDate], [Processed], [CustomerId], [TypeId]) VALUES (9, N'Luego', N'2001-01-01 00:00:00', 0, N'1', 1)
SET IDENTITY_INSERT [dbo].[Complaints] OFF
