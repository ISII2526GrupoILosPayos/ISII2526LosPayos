
SET IDENTITY_INSERT [dbo].[PaymentMethods] ON
INSERT INTO [dbo].[PaymentMethods] ([Id], [UserId], [Discriminator], [TelephoneNumber], [CreditCardNumber], [ExpirationDate], [Email]) VALUES (3, N'1', N'Bizum', 65432123, N'200000000', N'2050-12-12 00:00:00', N'pau@gmail.com')
SET IDENTITY_INSERT [dbo].[PaymentMethods] OFF

