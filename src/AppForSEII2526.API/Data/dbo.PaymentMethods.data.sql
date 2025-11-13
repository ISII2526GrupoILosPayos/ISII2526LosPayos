SET IDENTITY_INSERT [dbo].[PaymentMethods] ON
INSERT INTO [dbo].[PaymentMethods] ([Id], [UserId], [Discriminator], [TelephoneNumber], [CreditCardNumber], [ExpirationDate], [Email]) VALUES (3, N'1', N'Bizum', 65432123, N'200000000', N'2050-12-12 00:00:00', N'pau@gmail.com')
INSERT INTO [dbo].[PaymentMethods] ([Id], [UserId], [Discriminator], [TelephoneNumber], [CreditCardNumber], [ExpirationDate], [Email]) VALUES (4, N'1', N'CreditCard', 602453356, N'200000111', N'2032-08-17 00:00:00', N'luis@gmail.com')
INSERT INTO [dbo].[PaymentMethods] ([Id], [UserId], [Discriminator], [TelephoneNumber], [CreditCardNumber], [ExpirationDate], [Email]) VALUES (5, N'2', N'PayPal', 666777889, N'200000125', N'2029-03-25 00:00:00', N'kykybusiness@gmail.com')
SET IDENTITY_INSERT [dbo].[PaymentMethods] OFF
