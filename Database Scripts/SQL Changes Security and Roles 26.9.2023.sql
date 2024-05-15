alter table CommonUser
add HashedPassword nvarchar(max) null
go

INSERT INTO Role (Name)
VALUES ('Super Admin')
go

--update CommonUser set RoleID = 10014 
--where ID = 7
--go

CREATE TABLE AppServiceClass
	(
	ID int NOT NULL IDENTITY (1, 1),
	Name nvarchar(250) NULL,
	Description nvarchar(MAX) NULL,
	CreatedBy int NULL,
	CreatedAt datetime NOT NULL,
	IsDeleted bit NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE AppServiceClass ADD CONSTRAINT
	DF_AppServiceClass_CreatedAt DEFAULT (getdate()) FOR CreatedAt
GO
ALTER TABLE AppServiceClass ADD CONSTRAINT
	DF_AppServiceClass_IsDeleted DEFAULT ((0)) FOR IsDeleted
GO
ALTER TABLE AppServiceClass ADD CONSTRAINT
	PK_AppServiceClass PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE AppServiceClass SET (LOCK_ESCALATION = TABLE)
GO


CREATE TABLE AppServiceGroup
	(
	ID int NOT NULL IDENTITY (1, 1),
	Name nvarchar(250) NULL,
	Description nvarchar(MAX) NULL,
	AppServiceClassID int NULL,
	CreatedBy int NULL,
	CreatedAt datetime NOT NULL,
	IsDeleted bit NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE AppServiceGroup ADD CONSTRAINT
	DF_AppServiceGroup_CreatedAt DEFAULT (getdate()) FOR CreatedAt
GO
ALTER TABLE AppServiceGroup ADD CONSTRAINT
	DF_AppServiceGroup_IsDeleted DEFAULT ((0)) FOR IsDeleted
GO
ALTER TABLE AppServiceGroup ADD CONSTRAINT
	PK_AppServiceGroup PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE AppServiceGroup SET (LOCK_ESCALATION = TABLE)
GO

ALTER TABLE AppServiceClass SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE AppServiceGroup ADD CONSTRAINT
	FK_AppServiceGroup_AppServiceClass FOREIGN KEY
	(
	AppServiceClassID
	) REFERENCES AppServiceClass
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE AppServiceGroup SET (LOCK_ESCALATION = TABLE)
GO

alter table AppService
add AppServiceClassID int null, AppServiceGroupID int null

ALTER TABLE AppService ADD CONSTRAINT
	FK_AppService_AppServiceClass FOREIGN KEY
	(AppServiceClassID) REFERENCES AppServiceClass
	(ID) 
	 ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
GO

ALTER TABLE AppService ADD CONSTRAINT
	FK_AppService_AppServiceGroup FOREIGN KEY
	(AppServiceGroupID) REFERENCES AppServiceGroup
	(ID) 
	 ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
GO



-----------------------------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------------------------

delete from RoleAppService
GO
DBCC CHECKIDENT ('RoleAppService', RESEED, 0)
GO

delete from AppService
GO
DBCC CHECKIDENT ('AppService', RESEED, 0)
GO

alter table AppService
alter column ClassName nvarchar(150) null


------------------------------------- Add New Data -----------------------

SET IDENTITY_INSERT [dbo].[AppServiceClass] ON 
GO
INSERT [dbo].[AppServiceClass] ([ID], [Name], [Description], [CreatedBy], [CreatedAt], [IsDeleted]) VALUES (1, N'Red Dashboard', NULL, NULL, CAST(N'2023-09-28T12:44:42.250' AS DateTime), 0)
GO
INSERT [dbo].[AppServiceClass] ([ID], [Name], [Description], [CreatedBy], [CreatedAt], [IsDeleted]) VALUES (2, N'Account Dashboard', NULL, NULL, CAST(N'2023-09-28T12:44:42.250' AS DateTime), 0)
GO
INSERT [dbo].[AppServiceClass] ([ID], [Name], [Description], [CreatedBy], [CreatedAt], [IsDeleted]) VALUES (3, N'Reports', NULL, NULL, CAST(N'2023-09-28T12:44:42.250' AS DateTime), 0)
GO
INSERT [dbo].[AppServiceClass] ([ID], [Name], [Description], [CreatedBy], [CreatedAt], [IsDeleted]) VALUES (4, N'Red Management', NULL, NULL, CAST(N'2023-09-28T12:44:42.250' AS DateTime), 0)
GO
INSERT [dbo].[AppServiceClass] ([ID], [Name], [Description], [CreatedBy], [CreatedAt], [IsDeleted]) VALUES (5, N'Pickup Management', NULL, NULL, CAST(N'2023-09-28T12:44:42.250' AS DateTime), 0)
GO
INSERT [dbo].[AppServiceClass] ([ID], [Name], [Description], [CreatedBy], [CreatedAt], [IsDeleted]) VALUES (6, N'Shipping Management', NULL, NULL, CAST(N'2023-09-28T12:44:42.250' AS DateTime), 0)
GO
INSERT [dbo].[AppServiceClass] ([ID], [Name], [Description], [CreatedBy], [CreatedAt], [IsDeleted]) VALUES (7, N'Warehouse', NULL, NULL, CAST(N'2023-09-28T12:44:42.250' AS DateTime), 0)
GO
INSERT [dbo].[AppServiceClass] ([ID], [Name], [Description], [CreatedBy], [CreatedAt], [IsDeleted]) VALUES (8, N'Invoicing', NULL, NULL, CAST(N'2023-09-28T12:44:42.250' AS DateTime), 0)
GO
INSERT [dbo].[AppServiceClass] ([ID], [Name], [Description], [CreatedBy], [CreatedAt], [IsDeleted]) VALUES (9, N'Products Management', NULL, NULL, CAST(N'2023-09-28T12:44:42.250' AS DateTime), 0)
GO
INSERT [dbo].[AppServiceClass] ([ID], [Name], [Description], [CreatedBy], [CreatedAt], [IsDeleted]) VALUES (10, N'Complains Management', NULL, NULL, CAST(N'2023-09-28T12:44:42.250' AS DateTime), 0)
GO
INSERT [dbo].[AppServiceClass] ([ID], [Name], [Description], [CreatedBy], [CreatedAt], [IsDeleted]) VALUES (11, N'General', NULL, NULL, CAST(N'2023-09-29T16:58:07.267' AS DateTime), 0)
GO
SET IDENTITY_INSERT [dbo].[AppServiceClass] OFF
GO
SET IDENTITY_INSERT [dbo].[AppServiceGroup] ON 
GO
INSERT [dbo].[AppServiceGroup] ([ID], [Name], [Description], [AppServiceClassID], [CreatedBy], [CreatedAt], [IsDeleted]) VALUES (1, N'Dashboard', NULL, 1, NULL, CAST(N'2023-09-29T19:30:08.070' AS DateTime), 0)
GO
INSERT [dbo].[AppServiceGroup] ([ID], [Name], [Description], [AppServiceClassID], [CreatedBy], [CreatedAt], [IsDeleted]) VALUES (2, N'Dashboard', NULL, 2, NULL, CAST(N'2023-09-29T19:30:22.510' AS DateTime), 0)
GO
INSERT [dbo].[AppServiceGroup] ([ID], [Name], [Description], [AppServiceClassID], [CreatedBy], [CreatedAt], [IsDeleted]) VALUES (3, N'Shipments Report', NULL, 3, NULL, CAST(N'2023-09-29T19:48:41.107' AS DateTime), 0)
GO
INSERT [dbo].[AppServiceGroup] ([ID], [Name], [Description], [AppServiceClassID], [CreatedBy], [CreatedAt], [IsDeleted]) VALUES (4, N'Pickups Report', NULL, 3, NULL, CAST(N'2023-09-29T19:51:02.773' AS DateTime), 0)
GO
INSERT [dbo].[AppServiceGroup] ([ID], [Name], [Description], [AppServiceClassID], [CreatedBy], [CreatedAt], [IsDeleted]) VALUES (5, N'Invoices Report', NULL, 3, NULL, CAST(N'2023-09-29T19:56:56.477' AS DateTime), 0)
GO
INSERT [dbo].[AppServiceGroup] ([ID], [Name], [Description], [AppServiceClassID], [CreatedBy], [CreatedAt], [IsDeleted]) VALUES (6, N'Stock Report', NULL, 3, NULL, CAST(N'2023-09-29T19:59:12.327' AS DateTime), 0)
GO
INSERT [dbo].[AppServiceGroup] ([ID], [Name], [Description], [AppServiceClassID], [CreatedBy], [CreatedAt], [IsDeleted]) VALUES (7, N'Vendor Report', NULL, 3, NULL, CAST(N'2023-09-29T19:59:29.300' AS DateTime), 0)
GO
INSERT [dbo].[AppServiceGroup] ([ID], [Name], [Description], [AppServiceClassID], [CreatedBy], [CreatedAt], [IsDeleted]) VALUES (8, N'Couriers Report', NULL, 3, NULL, CAST(N'2023-09-29T19:59:43.200' AS DateTime), 0)
GO
INSERT [dbo].[AppServiceGroup] ([ID], [Name], [Description], [AppServiceClassID], [CreatedBy], [CreatedAt], [IsDeleted]) VALUES (9, N'Packing Report', NULL, 3, NULL, CAST(N'2023-09-29T20:00:07.557' AS DateTime), 0)
GO
INSERT [dbo].[AppServiceGroup] ([ID], [Name], [Description], [AppServiceClassID], [CreatedBy], [CreatedAt], [IsDeleted]) VALUES (10, N'Customer FollowUp Report', NULL, 3, NULL, CAST(N'2023-09-29T20:00:33.990' AS DateTime), 0)
GO
INSERT [dbo].[AppServiceGroup] ([ID], [Name], [Description], [AppServiceClassID], [CreatedBy], [CreatedAt], [IsDeleted]) VALUES (11, N'Complains Report', NULL, 3, NULL, CAST(N'2023-09-29T20:00:58.303' AS DateTime), 0)
GO
INSERT [dbo].[AppServiceGroup] ([ID], [Name], [Description], [AppServiceClassID], [CreatedBy], [CreatedAt], [IsDeleted]) VALUES (12, N'Staff', NULL, 4, NULL, CAST(N'2023-09-29T20:28:58.157' AS DateTime), 0)
GO
INSERT [dbo].[AppServiceGroup] ([ID], [Name], [Description], [AppServiceClassID], [CreatedBy], [CreatedAt], [IsDeleted]) VALUES (13, N'Vendors', NULL, 4, NULL, CAST(N'2023-09-29T20:29:03.557' AS DateTime), 0)
GO
INSERT [dbo].[AppServiceGroup] ([ID], [Name], [Description], [AppServiceClassID], [CreatedBy], [CreatedAt], [IsDeleted]) VALUES (14, N'Packing', NULL, 4, NULL, CAST(N'2023-09-29T20:29:14.377' AS DateTime), 0)
GO
INSERT [dbo].[AppServiceGroup] ([ID], [Name], [Description], [AppServiceClassID], [CreatedBy], [CreatedAt], [IsDeleted]) VALUES (15, N'Areas', NULL, 4, NULL, CAST(N'2023-09-29T20:29:25.820' AS DateTime), 0)
GO
INSERT [dbo].[AppServiceGroup] ([ID], [Name], [Description], [AppServiceClassID], [CreatedBy], [CreatedAt], [IsDeleted]) VALUES (16, N'Routes', NULL, 4, NULL, CAST(N'2023-09-29T20:29:34.910' AS DateTime), 0)
GO
INSERT [dbo].[AppServiceGroup] ([ID], [Name], [Description], [AppServiceClassID], [CreatedBy], [CreatedAt], [IsDeleted]) VALUES (17, N'Branches', NULL, 4, NULL, CAST(N'2023-09-29T20:29:57.130' AS DateTime), 0)
GO
INSERT [dbo].[AppServiceGroup] ([ID], [Name], [Description], [AppServiceClassID], [CreatedBy], [CreatedAt], [IsDeleted]) VALUES (18, N'Problem Types', NULL, 4, NULL, CAST(N'2023-09-29T20:30:27.753' AS DateTime), 0)
GO
INSERT [dbo].[AppServiceGroup] ([ID], [Name], [Description], [AppServiceClassID], [CreatedBy], [CreatedAt], [IsDeleted]) VALUES (19, N'Add Delivery Pickup', NULL, 5, NULL, CAST(N'2023-09-30T15:51:21.200' AS DateTime), 0)
GO
INSERT [dbo].[AppServiceGroup] ([ID], [Name], [Description], [AppServiceClassID], [CreatedBy], [CreatedAt], [IsDeleted]) VALUES (20, N'Add Stock Pickup', NULL, 5, NULL, CAST(N'2023-09-30T15:51:31.153' AS DateTime), 0)
GO
INSERT [dbo].[AppServiceGroup] ([ID], [Name], [Description], [AppServiceClassID], [CreatedBy], [CreatedAt], [IsDeleted]) VALUES (21, N'Pickups', NULL, 5, NULL, CAST(N'2023-09-30T15:51:59.623' AS DateTime), 0)
GO
INSERT [dbo].[AppServiceGroup] ([ID], [Name], [Description], [AppServiceClassID], [CreatedBy], [CreatedAt], [IsDeleted]) VALUES (22, N'Assign Pickups', NULL, 5, NULL, CAST(N'2023-09-30T15:52:16.753' AS DateTime), 0)
GO
INSERT [dbo].[AppServiceGroup] ([ID], [Name], [Description], [AppServiceClassID], [CreatedBy], [CreatedAt], [IsDeleted]) VALUES (23, N'Add Shipment', NULL, 6, NULL, CAST(N'2023-09-30T16:22:34.700' AS DateTime), 0)
GO
INSERT [dbo].[AppServiceGroup] ([ID], [Name], [Description], [AppServiceClassID], [CreatedBy], [CreatedAt], [IsDeleted]) VALUES (24, N'Shipments', NULL, 6, NULL, CAST(N'2023-09-30T16:24:07.820' AS DateTime), 0)
GO
INSERT [dbo].[AppServiceGroup] ([ID], [Name], [Description], [AppServiceClassID], [CreatedBy], [CreatedAt], [IsDeleted]) VALUES (25, N'Assign Shipments', NULL, 6, NULL, CAST(N'2023-09-30T16:26:48.473' AS DateTime), 0)
GO
INSERT [dbo].[AppServiceGroup] ([ID], [Name], [Description], [AppServiceClassID], [CreatedBy], [CreatedAt], [IsDeleted]) VALUES (26, N'Receive Delivery Pickups', NULL, 7, NULL, CAST(N'2023-10-02T15:27:20.710' AS DateTime), 0)
GO
INSERT [dbo].[AppServiceGroup] ([ID], [Name], [Description], [AppServiceClassID], [CreatedBy], [CreatedAt], [IsDeleted]) VALUES (27, N'Receive Stock Pickups', NULL, 7, NULL, CAST(N'2023-10-02T15:27:33.183' AS DateTime), 0)
GO
INSERT [dbo].[AppServiceGroup] ([ID], [Name], [Description], [AppServiceClassID], [CreatedBy], [CreatedAt], [IsDeleted]) VALUES (28, N'Receive Returns', NULL, 7, NULL, CAST(N'2023-10-02T15:27:48.900' AS DateTime), 0)
GO
INSERT [dbo].[AppServiceGroup] ([ID], [Name], [Description], [AppServiceClassID], [CreatedBy], [CreatedAt], [IsDeleted]) VALUES (29, N'Warehouse Shipments', NULL, 7, NULL, CAST(N'2023-10-02T15:28:07.897' AS DateTime), 0)
GO
INSERT [dbo].[AppServiceGroup] ([ID], [Name], [Description], [AppServiceClassID], [CreatedBy], [CreatedAt], [IsDeleted]) VALUES (30, N'Warehouse Stock', NULL, 7, NULL, CAST(N'2023-10-02T15:28:25.057' AS DateTime), 0)
GO
INSERT [dbo].[AppServiceGroup] ([ID], [Name], [Description], [AppServiceClassID], [CreatedBy], [CreatedAt], [IsDeleted]) VALUES (31, N'Courier''s Orders', NULL, 7, NULL, CAST(N'2023-10-02T15:28:50.983' AS DateTime), 0)
GO
INSERT [dbo].[AppServiceGroup] ([ID], [Name], [Description], [AppServiceClassID], [CreatedBy], [CreatedAt], [IsDeleted]) VALUES (32, N'Receive Cash', NULL, 8, NULL, CAST(N'2023-10-02T15:50:42.680' AS DateTime), 0)
GO
INSERT [dbo].[AppServiceGroup] ([ID], [Name], [Description], [AppServiceClassID], [CreatedBy], [CreatedAt], [IsDeleted]) VALUES (33, N'Treasury', NULL, 8, NULL, CAST(N'2023-10-02T15:50:53.060' AS DateTime), 0)
GO
INSERT [dbo].[AppServiceGroup] ([ID], [Name], [Description], [AppServiceClassID], [CreatedBy], [CreatedAt], [IsDeleted]) VALUES (34, N'Transfer Cash', NULL, 8, NULL, CAST(N'2023-10-02T15:50:58.237' AS DateTime), 0)
GO
INSERT [dbo].[AppServiceGroup] ([ID], [Name], [Description], [AppServiceClassID], [CreatedBy], [CreatedAt], [IsDeleted]) VALUES (35, N'Invoices', NULL, 8, NULL, CAST(N'2023-10-02T15:51:05.877' AS DateTime), 0)
GO
INSERT [dbo].[AppServiceGroup] ([ID], [Name], [Description], [AppServiceClassID], [CreatedBy], [CreatedAt], [IsDeleted]) VALUES (36, N'Products', NULL, 9, NULL, CAST(N'2023-10-02T16:10:02.137' AS DateTime), 0)
GO
INSERT [dbo].[AppServiceGroup] ([ID], [Name], [Description], [AppServiceClassID], [CreatedBy], [CreatedAt], [IsDeleted]) VALUES (37, N'Complains', NULL, 10, NULL, CAST(N'2023-10-02T16:16:04.510' AS DateTime), 0)
GO
SET IDENTITY_INSERT [dbo].[AppServiceGroup] OFF
GO
SET IDENTITY_INSERT [dbo].[AppService] ON 
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (3, 0, NULL, CAST(N'2023-09-29T19:26:48.580' AS DateTime), 0, CAST(N'2023-09-29T19:26:48.580' AS DateTime), NULL, N'Menu_RedDashboard', 1, N'Menu - Red Dashboard', NULL, 0, NULL, 1, 1)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (4, 0, NULL, CAST(N'2023-09-29T19:31:40.827' AS DateTime), 0, CAST(N'2023-09-29T19:31:40.827' AS DateTime), NULL, N'Menu_AccountDashboard', 1, N'Menu - Account Dashboard', NULL, 0, NULL, 2, 2)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (5, 0, NULL, CAST(N'2023-09-29T19:37:54.240' AS DateTime), 0, CAST(N'2023-09-29T19:37:54.240' AS DateTime), NULL, N'RedDashboard', 1, N'View Red Dashboard', NULL, 0, NULL, 1, 1)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (6, 0, NULL, CAST(N'2023-09-29T19:40:08.603' AS DateTime), 0, CAST(N'2023-09-29T19:40:08.603' AS DateTime), NULL, N'AccountDashboard', 1, N'View Account Dashboard', NULL, 0, NULL, 2, 2)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (7, 0, NULL, CAST(N'2023-09-29T19:49:00.553' AS DateTime), 0, CAST(N'2023-09-29T19:49:00.553' AS DateTime), NULL, N'Menu_ReportsShipments', 1, N'Menu - Reports / Shipments', NULL, 0, NULL, 3, 3)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (8, 0, NULL, CAST(N'2023-09-29T19:49:41.677' AS DateTime), 0, CAST(N'2023-09-29T19:49:41.677' AS DateTime), NULL, N'ReportsShipments', 1, N'View - Reports / Shipments', NULL, 0, NULL, 3, 3)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (11, 0, NULL, CAST(N'2023-09-29T19:52:00.950' AS DateTime), 0, CAST(N'2023-09-29T19:52:00.950' AS DateTime), NULL, N'Menu_ReportsPickups', 1, N'Menu - Reports / Pickups', NULL, 0, NULL, 3, 4)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (12, 0, NULL, CAST(N'2023-09-29T19:52:21.973' AS DateTime), 0, CAST(N'2023-09-29T19:52:21.973' AS DateTime), NULL, N'ReportsPickups', 1, N'View - Reports / Pickups', NULL, 0, NULL, 3, 4)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (13, 0, NULL, CAST(N'2023-09-29T19:58:29.077' AS DateTime), 0, CAST(N'2023-09-29T19:58:29.077' AS DateTime), NULL, N'Menu_ReportsInvoices', 1, N'Menu - Reports / Invoices', NULL, 0, NULL, 3, 5)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (14, 0, NULL, CAST(N'2023-09-29T19:58:55.447' AS DateTime), 0, CAST(N'2023-09-29T19:58:55.447' AS DateTime), NULL, N'ReportsInvoices', 1, N'View - Reports / Invoices', NULL, 0, NULL, 3, 5)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (15, 0, NULL, CAST(N'2023-09-29T20:04:17.570' AS DateTime), 0, CAST(N'2023-09-29T20:04:17.570' AS DateTime), NULL, N'Menu_ReportsStock', 1, N'Menu - Reports / Stock', NULL, 0, NULL, 3, 6)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (16, 0, NULL, CAST(N'2023-09-29T20:04:41.407' AS DateTime), 0, CAST(N'2023-09-29T20:04:41.407' AS DateTime), NULL, N'ReportsStock', 1, N'View - Reports / Stock', NULL, 0, NULL, 3, 6)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (17, 0, NULL, CAST(N'2023-09-29T20:07:01.553' AS DateTime), 0, CAST(N'2023-09-29T20:07:01.553' AS DateTime), NULL, N'Menu_ReportsCouriers', 1, N'Menu - Reports / Couriers', NULL, 0, NULL, 3, 8)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (18, 0, NULL, CAST(N'2023-09-29T20:07:20.193' AS DateTime), 0, CAST(N'2023-09-29T20:07:20.193' AS DateTime), NULL, N'ReportsCouriers', 1, N'View - Reports / Couriers', NULL, 0, NULL, 3, 8)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (19, 0, NULL, CAST(N'2023-09-29T20:08:56.203' AS DateTime), 0, CAST(N'2023-09-29T20:08:56.203' AS DateTime), NULL, N'Menu_ReportsPacking', 1, N'Menu - Reports / Packing', NULL, 0, NULL, 3, 9)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (20, 0, NULL, CAST(N'2023-09-29T20:09:13.437' AS DateTime), 0, CAST(N'2023-09-29T20:09:13.437' AS DateTime), NULL, N'ReportsPacking', 1, N'View - Reports / Packing', NULL, 0, NULL, 3, 9)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (21, 0, NULL, CAST(N'2023-09-29T20:09:38.973' AS DateTime), 0, CAST(N'2023-09-29T20:09:38.973' AS DateTime), NULL, N'Menu_ReportsCustomerFollowup', 1, N'Menu - Reports / CustomerFollowup', NULL, 0, NULL, 3, 10)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (22, 0, NULL, CAST(N'2023-09-29T20:09:51.930' AS DateTime), 0, CAST(N'2023-09-29T20:09:51.930' AS DateTime), NULL, N'ReportsCustomerFollowup', 1, N'View - Reports / CustomerFollowup', NULL, 0, NULL, 3, 10)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (23, 0, NULL, CAST(N'2023-09-29T20:10:13.190' AS DateTime), 0, CAST(N'2023-09-29T20:10:13.190' AS DateTime), NULL, N'Menu_ReportsComplains', 1, N'Menu - Reports / Complains', NULL, 0, NULL, 3, 11)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (24, 0, NULL, CAST(N'2023-09-29T20:10:33.877' AS DateTime), 0, CAST(N'2023-09-29T20:10:33.877' AS DateTime), NULL, N'ReportsComplains', 1, N'View - Reports / Complains', NULL, 0, NULL, 3, 11)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (25, 0, NULL, CAST(N'2023-09-29T20:32:45.727' AS DateTime), 0, CAST(N'2023-09-29T20:32:45.727' AS DateTime), NULL, N'Menu_Staff', 1, N'Menu - RED Management / Staff', NULL, 0, NULL, 4, 12)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (26, 0, NULL, CAST(N'2023-09-29T20:33:26.030' AS DateTime), 0, CAST(N'2023-09-29T20:33:26.030' AS DateTime), NULL, N'StaffList', 1, N'View Users', NULL, 0, NULL, 4, 12)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (29, 0, NULL, CAST(N'2023-09-29T20:39:52.323' AS DateTime), 0, CAST(N'2023-09-29T20:39:52.323' AS DateTime), NULL, N'StaffAdd', 1, N'Add New User', NULL, 0, NULL, 4, 12)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (30, 0, NULL, CAST(N'2023-09-29T20:43:19.710' AS DateTime), 0, CAST(N'2023-09-29T20:43:19.710' AS DateTime), NULL, N'StaffEdit', 1, N'Edit User', NULL, 0, NULL, 4, 12)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (31, 0, NULL, CAST(N'2023-09-29T20:44:31.263' AS DateTime), 0, CAST(N'2023-09-29T20:44:31.263' AS DateTime), NULL, N'StaffDelete', 1, N'Delete User', NULL, 0, NULL, 4, 12)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (32, 0, NULL, CAST(N'2023-09-29T20:47:17.217' AS DateTime), 0, CAST(N'2023-09-29T20:47:17.217' AS DateTime), NULL, N'StaffChangePassword', 1, N'Change User Password', NULL, 0, NULL, 4, 12)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (33, 0, NULL, CAST(N'2023-09-29T20:50:42.467' AS DateTime), 0, CAST(N'2023-09-29T20:50:42.467' AS DateTime), NULL, N'Menu_Vendors', 1, N'Menu - RED Management / Vendors', NULL, 0, NULL, 4, 13)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (34, 0, NULL, CAST(N'2023-09-29T20:52:12.963' AS DateTime), 0, CAST(N'2023-09-29T20:52:12.963' AS DateTime), NULL, N'VendorsList', 1, N'View Vendors', NULL, 0, NULL, 4, 13)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (35, 0, NULL, CAST(N'2023-09-29T20:58:32.973' AS DateTime), 0, CAST(N'2023-09-29T20:58:32.973' AS DateTime), NULL, N'VendorsAdd', 1, N'Add Vendor', NULL, 0, NULL, 4, 13)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (38, 0, NULL, CAST(N'2023-09-30T14:45:24.500' AS DateTime), 0, CAST(N'2023-09-30T14:45:24.500' AS DateTime), NULL, N'VendorsEdit', 1, N'Edit Vendor', NULL, 0, NULL, 4, 13)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (39, 0, NULL, CAST(N'2023-09-30T14:46:46.633' AS DateTime), 0, CAST(N'2023-09-30T14:46:46.633' AS DateTime), NULL, N'VendorsDelete', 1, N'Delete Vendor', NULL, 0, NULL, 4, 13)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (40, 0, NULL, CAST(N'2023-09-30T14:57:18.800' AS DateTime), 0, CAST(N'2023-09-30T14:57:18.800' AS DateTime), NULL, N'Menu_Packing', 1, N'Menu - RED Management / Packing', NULL, 0, NULL, 4, 14)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (42, 0, NULL, CAST(N'2023-09-30T15:07:06.427' AS DateTime), 0, CAST(N'2023-09-30T15:07:06.427' AS DateTime), NULL, N'PackingList', 1, N'View Packing', NULL, 0, NULL, 4, 14)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (43, 0, NULL, CAST(N'2023-09-30T15:08:25.573' AS DateTime), 0, CAST(N'2023-09-30T15:08:25.573' AS DateTime), NULL, N'PackingAdd', 1, N'Add Packing', NULL, 0, NULL, 4, 14)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (44, 0, NULL, CAST(N'2023-09-30T15:09:37.313' AS DateTime), 0, CAST(N'2023-09-30T15:09:37.313' AS DateTime), NULL, N'PackingEdit', 1, N'Edit Packing', NULL, 0, NULL, 4, 14)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (45, 0, NULL, CAST(N'2023-09-30T15:10:02.473' AS DateTime), 0, CAST(N'2023-09-30T15:10:02.473' AS DateTime), NULL, N'PackingDelete', 1, N'Delete Packing', NULL, 0, NULL, 4, 14)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (46, 0, NULL, CAST(N'2023-09-30T15:16:22.413' AS DateTime), 0, CAST(N'2023-09-30T15:16:22.413' AS DateTime), NULL, N'Menu_Areas', 1, N'Menu - RED Management / Areas', NULL, 0, NULL, 4, 15)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (47, 0, NULL, CAST(N'2023-09-30T15:19:24.910' AS DateTime), 0, CAST(N'2023-09-30T15:19:24.910' AS DateTime), NULL, N'AreaList', 1, N'View Areas', NULL, 0, NULL, 4, 15)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (48, 0, NULL, CAST(N'2023-09-30T15:23:48.567' AS DateTime), 0, CAST(N'2023-09-30T15:23:48.567' AS DateTime), NULL, N'AreaAdd', 1, N'Add Area', NULL, 0, NULL, 4, 15)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (49, 0, NULL, CAST(N'2023-09-30T15:29:09.187' AS DateTime), 0, CAST(N'2023-09-30T15:29:09.187' AS DateTime), NULL, N'AreaEdit', 1, N'Edit Area', NULL, 0, NULL, 4, 15)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (50, 0, NULL, CAST(N'2023-09-30T15:29:23.963' AS DateTime), 0, CAST(N'2023-09-30T15:29:23.963' AS DateTime), NULL, N'AreaDelete', 1, N'Delete Area', NULL, 0, NULL, 4, 15)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (51, 0, NULL, CAST(N'2023-09-30T15:35:07.870' AS DateTime), 0, CAST(N'2023-09-30T15:35:07.870' AS DateTime), NULL, N'Menu_Zones', 1, N'Menu - RED Management / Routes', NULL, 0, NULL, 4, 16)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (52, 0, NULL, CAST(N'2023-09-30T15:35:07.870' AS DateTime), 0, CAST(N'2023-09-30T15:35:07.870' AS DateTime), NULL, N'ZoneList', 1, N'View Routes', NULL, 0, NULL, 4, 16)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (53, 0, NULL, CAST(N'2023-09-30T15:35:07.870' AS DateTime), 0, CAST(N'2023-09-30T15:35:07.870' AS DateTime), NULL, N'ZoneAdd', 1, N'Add Route', NULL, 0, NULL, 4, 16)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (54, 0, NULL, CAST(N'2023-09-30T15:35:07.870' AS DateTime), 0, CAST(N'2023-09-30T15:35:07.870' AS DateTime), NULL, N'ZoneEdit', 1, N'Edit Route', NULL, 0, NULL, 4, 16)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (55, 0, NULL, CAST(N'2023-09-30T15:35:07.870' AS DateTime), 0, CAST(N'2023-09-30T15:35:07.870' AS DateTime), NULL, N'ZoneDelete', 1, N'Delete Route', NULL, 0, NULL, 4, 16)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (56, 0, NULL, CAST(N'2023-09-30T15:39:05.870' AS DateTime), 0, CAST(N'2023-09-30T15:39:05.870' AS DateTime), NULL, N'Menu_Branches', 1, N'Menu - RED Management / Branches', NULL, 0, NULL, 4, 17)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (57, 0, NULL, CAST(N'2023-09-30T15:39:05.870' AS DateTime), 0, CAST(N'2023-09-30T15:39:05.870' AS DateTime), NULL, N'BranchList', 1, N'View Branches', NULL, 0, NULL, 4, 17)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (58, 0, NULL, CAST(N'2023-09-30T15:39:05.870' AS DateTime), 0, CAST(N'2023-09-30T15:39:05.870' AS DateTime), NULL, N'BranchAdd', 1, N'Add Branch', NULL, 0, NULL, 4, 17)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (59, 0, NULL, CAST(N'2023-09-30T15:39:05.870' AS DateTime), 0, CAST(N'2023-09-30T15:39:05.870' AS DateTime), NULL, N'BranchEdit', 1, N'Edit Branch', NULL, 0, NULL, 4, 17)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (60, 0, NULL, CAST(N'2023-09-30T15:39:05.870' AS DateTime), 0, CAST(N'2023-09-30T15:39:05.870' AS DateTime), NULL, N'BranchDelete', 1, N'Delete Branch', NULL, 0, NULL, 4, 17)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (61, 0, NULL, CAST(N'2023-09-30T15:44:43.513' AS DateTime), 0, CAST(N'2023-09-30T15:44:43.513' AS DateTime), NULL, N'Menu_ProblemType', 1, N'Menu - RED Management / Problem Types', NULL, 0, NULL, 4, 18)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (62, 0, NULL, CAST(N'2023-09-30T15:44:43.513' AS DateTime), 0, CAST(N'2023-09-30T15:44:43.513' AS DateTime), NULL, N'ProblemTypeList', 1, N'View Problem Types', NULL, 0, NULL, 4, 18)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (63, 0, NULL, CAST(N'2023-09-30T15:44:43.513' AS DateTime), 0, CAST(N'2023-09-30T15:44:43.513' AS DateTime), NULL, N'ProblemTypeAdd', 1, N'Add Problem Type', NULL, 0, NULL, 4, 18)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (64, 0, NULL, CAST(N'2023-09-30T15:44:43.513' AS DateTime), 0, CAST(N'2023-09-30T15:44:43.513' AS DateTime), NULL, N'ProblemTypeEdit', 1, N'Edit Problem Type', NULL, 0, NULL, 4, 18)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (65, 0, NULL, CAST(N'2023-09-30T15:44:43.513' AS DateTime), 0, CAST(N'2023-09-30T15:44:43.513' AS DateTime), NULL, N'ProblemTypeDelete', 1, N'Delete Problem Type', NULL, 0, NULL, 4, 18)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (66, 0, NULL, CAST(N'2023-09-30T15:55:53.940' AS DateTime), 0, CAST(N'2023-09-30T15:55:53.940' AS DateTime), NULL, N'Menu_PickupDelivery', 1, N'Menu - Pickup Management / Add Delivery Pickup', NULL, 0, NULL, 5, 19)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (67, 0, NULL, CAST(N'2023-09-30T15:55:53.940' AS DateTime), 0, CAST(N'2023-09-30T15:55:53.940' AS DateTime), NULL, N'PickupDeliveryList', 1, N'View Delivery Pickup', NULL, 0, NULL, 5, 19)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (68, 0, NULL, CAST(N'2023-09-30T15:55:53.940' AS DateTime), 0, CAST(N'2023-09-30T15:55:53.940' AS DateTime), NULL, N'PickupDeliveryAdd', 1, N'Add Delivery Pickup', NULL, 0, NULL, 5, 19)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (69, 0, NULL, CAST(N'2023-09-30T15:57:09.140' AS DateTime), 0, CAST(N'2023-09-30T15:57:09.140' AS DateTime), NULL, N'Menu_PickupStock', 1, N'Menu - Pickup Management / Add Stock Pickup', NULL, 0, NULL, 5, 20)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (70, 0, NULL, CAST(N'2023-09-30T15:57:09.140' AS DateTime), 0, CAST(N'2023-09-30T15:57:09.140' AS DateTime), NULL, N'PickupStockList', 1, N'View Stock Pickup', NULL, 0, NULL, 5, 20)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (71, 0, NULL, CAST(N'2023-09-30T15:57:09.140' AS DateTime), 0, CAST(N'2023-09-30T15:57:09.140' AS DateTime), NULL, N'PickupStockAdd', 1, N'Add Stock Pickup', NULL, 0, NULL, 5, 20)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (72, 0, NULL, CAST(N'2023-09-30T16:12:14.473' AS DateTime), 0, CAST(N'2023-09-30T16:12:14.473' AS DateTime), NULL, N'Menu_PickupList', 1, N'Menu - Pickup Management / Add Stock Pickup', NULL, 0, NULL, 5, 21)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (73, 0, NULL, CAST(N'2023-09-30T16:12:14.473' AS DateTime), 0, CAST(N'2023-09-30T16:12:14.473' AS DateTime), NULL, N'PickupsList', 1, N'View Pickups', NULL, 0, NULL, 5, 21)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (74, 0, NULL, CAST(N'2023-09-30T16:12:14.473' AS DateTime), 0, CAST(N'2023-09-30T16:12:14.473' AS DateTime), NULL, N'PickupEdit', 1, N'Edit Pickup', NULL, 0, NULL, 5, 21)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (75, 0, NULL, CAST(N'2023-09-30T16:12:14.473' AS DateTime), 0, CAST(N'2023-09-30T16:12:14.473' AS DateTime), NULL, N'PickupCancel', 1, N'Cancel Pickup', NULL, 0, NULL, 5, 21)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (76, 0, NULL, CAST(N'2023-09-30T16:12:14.473' AS DateTime), 0, CAST(N'2023-09-30T16:12:14.473' AS DateTime), NULL, N'PickupUnAssign', 1, N'UnAssign Pickup', NULL, 0, NULL, 5, 21)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (77, 0, NULL, CAST(N'2023-09-30T16:15:40.570' AS DateTime), 0, CAST(N'2023-09-30T16:15:40.570' AS DateTime), NULL, N'Menu_PickupAssign', 1, N'Menu - Pickup Management / Assign Pickups', NULL, 0, NULL, 5, 22)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (78, 0, NULL, CAST(N'2023-09-30T16:15:40.570' AS DateTime), 0, CAST(N'2023-09-30T16:15:40.570' AS DateTime), NULL, N'AssignPickupList', 1, N'View Assign Pickups', NULL, 0, NULL, 5, 22)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (79, 0, NULL, CAST(N'2023-09-30T16:15:40.570' AS DateTime), 0, CAST(N'2023-09-30T16:15:40.570' AS DateTime), NULL, N'PickupAssign', 1, N'Assign Pickup', NULL, 0, NULL, 5, 22)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (80, 0, NULL, CAST(N'2023-09-30T16:31:15.783' AS DateTime), 0, CAST(N'2023-09-30T16:31:15.783' AS DateTime), NULL, N'Menu_AddShipment', 1, N'Menu - Shipping Management / Add Shipment', NULL, 0, NULL, 6, 23)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (81, 0, NULL, CAST(N'2023-09-30T16:31:15.783' AS DateTime), 0, CAST(N'2023-09-30T16:31:15.783' AS DateTime), NULL, N'ShipmentAdd', 1, N'Add Shipment', NULL, 0, NULL, 6, 23)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (82, 0, NULL, CAST(N'2023-10-02T15:08:46.310' AS DateTime), 0, CAST(N'2023-10-02T15:08:46.310' AS DateTime), NULL, N'ShipmentTrack', 1, N'Track Shipment', NULL, 0, NULL, 6, 24)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (83, 0, NULL, CAST(N'2023-10-02T15:20:46.840' AS DateTime), 0, CAST(N'2023-10-02T15:20:46.840' AS DateTime), NULL, N'Menu_ShipmentList', 1, N'Menu - Shipping Management / Shipments List', NULL, 0, NULL, 6, 24)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (84, 0, NULL, CAST(N'2023-10-02T15:20:46.840' AS DateTime), 0, CAST(N'2023-10-02T15:20:46.840' AS DateTime), NULL, N'ShipmentsList', 1, N'View Shipments List', NULL, 0, NULL, 6, 24)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (85, 0, NULL, CAST(N'2023-10-02T15:20:46.840' AS DateTime), 0, CAST(N'2023-10-02T15:20:46.840' AS DateTime), NULL, N'ShipmentEdit', 1, N'Edit Shipment', NULL, 0, NULL, 6, 24)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (86, 0, NULL, CAST(N'2023-10-02T15:20:46.840' AS DateTime), 0, CAST(N'2023-10-02T15:20:46.840' AS DateTime), NULL, N'ShipmentEditAddress', 1, N'Edit Shipment Address', NULL, 0, NULL, 6, 24)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (87, 0, NULL, CAST(N'2023-10-02T15:20:46.840' AS DateTime), 0, CAST(N'2023-10-02T15:20:46.840' AS DateTime), NULL, N'ShipmentCancel', 1, N'Cancel Shipment', NULL, 0, NULL, 6, 24)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (88, 0, NULL, CAST(N'2023-10-02T15:20:46.840' AS DateTime), 0, CAST(N'2023-10-02T15:20:46.840' AS DateTime), NULL, N'ShipmentCallHisory', 1, N'Edit Shipment Call History', NULL, 0, NULL, 6, 24)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (89, 0, NULL, CAST(N'2023-10-02T15:20:46.840' AS DateTime), 0, CAST(N'2023-10-02T15:20:46.840' AS DateTime), NULL, N'ShipmentPostpone', 1, N'PostPone Shipment', NULL, 0, NULL, 6, 24)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (90, 0, NULL, CAST(N'2023-10-02T15:20:46.840' AS DateTime), 0, CAST(N'2023-10-02T15:20:46.840' AS DateTime), NULL, N'ShipmentPrint', 1, N'Print Shipment', NULL, 0, NULL, 6, 24)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (91, 0, NULL, CAST(N'2023-10-02T15:20:46.840' AS DateTime), 0, CAST(N'2023-10-02T15:20:46.840' AS DateTime), NULL, N'ShipmentReturnToVendor', 1, N'Return Shipment To Vendor', NULL, 0, NULL, 6, 24)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (92, 0, NULL, CAST(N'2023-10-02T15:20:46.840' AS DateTime), 0, CAST(N'2023-10-02T15:20:46.840' AS DateTime), NULL, N'ShipmentAddProblem', 1, N'Report Problem', NULL, 0, NULL, 6, 24)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (93, 0, NULL, CAST(N'2023-10-02T15:20:46.840' AS DateTime), 0, CAST(N'2023-10-02T15:20:46.840' AS DateTime), NULL, N'ShipmentReportProblemToVendor', 1, N'Report Problem To Vendor', NULL, 0, NULL, 6, 24)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (94, 0, NULL, CAST(N'2023-10-02T15:20:46.840' AS DateTime), 0, CAST(N'2023-10-02T15:20:46.840' AS DateTime), NULL, N'ShipmentSolveProblem', 1, N'Solve Problem', NULL, 0, NULL, 6, 24)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (95, 0, NULL, CAST(N'2023-10-02T15:20:46.840' AS DateTime), 0, CAST(N'2023-10-02T15:20:46.840' AS DateTime), NULL, N'ShipmentReplyToCourier', 1, N'Reply To Courier Problem', NULL, 0, NULL, 6, 24)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (96, 0, NULL, CAST(N'2023-10-02T15:20:46.840' AS DateTime), 0, CAST(N'2023-10-02T15:20:46.840' AS DateTime), NULL, N'ShipmentDeleteProblem', 1, N'Delete Problem', NULL, 0, NULL, 6, 24)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (97, 0, NULL, CAST(N'2023-10-02T15:20:46.840' AS DateTime), 0, CAST(N'2023-10-02T15:20:46.840' AS DateTime), NULL, N'ShipmentsUnAssign', 1, N'UnAssign Shipment', NULL, 0, NULL, 6, 24)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (98, 0, NULL, CAST(N'2023-10-02T15:25:28.320' AS DateTime), 0, CAST(N'2023-10-02T15:25:28.320' AS DateTime), NULL, N'Menu_AssignShipments', 1, N'Menu - Shipping Management / Assign Shipments', NULL, 0, NULL, 6, 25)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (99, 0, NULL, CAST(N'2023-10-02T15:25:28.320' AS DateTime), 0, CAST(N'2023-10-02T15:25:28.320' AS DateTime), NULL, N'ShipmentsAssignList', 1, N'View Assign Shipments List', NULL, 0, NULL, 6, 25)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (100, 0, NULL, CAST(N'2023-10-02T15:25:28.320' AS DateTime), 0, CAST(N'2023-10-02T15:25:28.320' AS DateTime), NULL, N'ShipmentsAssign', 1, N'Assign Shipments', NULL, 0, NULL, 6, 25)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (101, 0, NULL, CAST(N'2023-10-02T15:34:39.747' AS DateTime), 0, CAST(N'2023-10-02T15:34:39.747' AS DateTime), NULL, N'Menu_ReceiveDeliveryPickups', 1, N'Menu - Warehouse / Receive Delivery Pickups', NULL, 0, NULL, 7, 26)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (102, 0, NULL, CAST(N'2023-10-02T15:34:39.747' AS DateTime), 0, CAST(N'2023-10-02T15:34:39.747' AS DateTime), NULL, N'ReceiveDeliveryPickups', 1, N'View Delivery Pickups', NULL, 0, NULL, 7, 26)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (103, 0, NULL, CAST(N'2023-10-02T15:34:39.747' AS DateTime), 0, CAST(N'2023-10-02T15:34:39.747' AS DateTime), NULL, N'ConfirmReceivePickups', 1, N'Receive Pickups', NULL, 0, NULL, 7, 26)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (104, 0, NULL, CAST(N'2023-10-02T15:35:35.830' AS DateTime), 0, CAST(N'2023-10-02T15:35:35.830' AS DateTime), NULL, N'Menu_ReceiveStockPickups', 1, N'Menu - Warehouse / Receive Stock Pickups', NULL, 0, NULL, 7, 27)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (105, 0, NULL, CAST(N'2023-10-02T15:35:35.830' AS DateTime), 0, CAST(N'2023-10-02T15:35:35.830' AS DateTime), NULL, N'ReceiveStockPickups', 1, N'View Stock Pickups', NULL, 0, NULL, 7, 27)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (106, 0, NULL, CAST(N'2023-10-02T15:37:57.703' AS DateTime), 0, CAST(N'2023-10-02T15:37:57.703' AS DateTime), NULL, N'Menu_ReceiveReturns', 1, N'Menu - Warehouse / Receive Returns', NULL, 0, NULL, 7, 28)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (107, 0, NULL, CAST(N'2023-10-02T15:37:57.703' AS DateTime), 0, CAST(N'2023-10-02T15:37:57.703' AS DateTime), NULL, N'ReceiveReturns', 1, N'View Returns', NULL, 0, NULL, 7, 28)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (108, 0, NULL, CAST(N'2023-10-02T15:37:57.703' AS DateTime), 0, CAST(N'2023-10-02T15:37:57.703' AS DateTime), NULL, N'ConfirmReceiveReturn', 1, N'Receive Returns', NULL, 0, NULL, 7, 28)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (109, 0, NULL, CAST(N'2023-10-02T15:42:12.020' AS DateTime), 0, CAST(N'2023-10-02T15:42:12.020' AS DateTime), NULL, N'Menu_WarehouseShipments', 1, N'Menu - Warehouse / Shipments', NULL, 0, NULL, 7, 29)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (110, 0, NULL, CAST(N'2023-10-02T15:42:12.020' AS DateTime), 0, CAST(N'2023-10-02T15:42:12.020' AS DateTime), NULL, N'WarehouseShipments', 1, N'View Warehouse Shipments', NULL, 0, NULL, 7, 29)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (111, 0, NULL, CAST(N'2023-10-02T15:42:12.020' AS DateTime), 0, CAST(N'2023-10-02T15:42:12.020' AS DateTime), NULL, N'WarehouseChangeToReady', 1, N'Change Shipments To Ready', NULL, 0, NULL, 7, 29)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (112, 0, NULL, CAST(N'2023-10-02T15:42:12.020' AS DateTime), 0, CAST(N'2023-10-02T15:42:12.020' AS DateTime), NULL, N'WarehouseEditPacking', 1, N'Edit Shipments Packing', NULL, 0, NULL, 7, 29)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (113, 0, NULL, CAST(N'2023-10-02T15:42:12.020' AS DateTime), 0, CAST(N'2023-10-02T15:42:12.020' AS DateTime), NULL, N'WarehouseBack', 1, N'Change Shipments To Pending', NULL, 0, NULL, 7, 29)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (114, 0, NULL, CAST(N'2023-10-02T15:43:40.303' AS DateTime), 0, CAST(N'2023-10-02T15:43:40.303' AS DateTime), NULL, N'Menu_WarehouseStock', 1, N'Menu - Warehouse / Stock', NULL, 0, NULL, 7, 30)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (115, 0, NULL, CAST(N'2023-10-02T15:43:40.303' AS DateTime), 0, CAST(N'2023-10-02T15:43:40.303' AS DateTime), NULL, N'WarehouseStock', 1, N'View Warehouse Stock', NULL, 0, NULL, 7, 30)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (116, 0, NULL, CAST(N'2023-10-02T15:48:10.360' AS DateTime), 0, CAST(N'2023-10-02T15:48:10.360' AS DateTime), NULL, N'Menu_CouriersShipments', 1, N'Menu - Warehouse / Stock', NULL, 0, NULL, 7, 31)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (117, 0, NULL, CAST(N'2023-10-02T15:48:10.360' AS DateTime), 0, CAST(N'2023-10-02T15:48:10.360' AS DateTime), NULL, N'WarehouseCouriersShipments', 1, N'View Courier Orders', NULL, 0, NULL, 7, 31)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (118, 0, NULL, CAST(N'2023-10-02T15:53:11.860' AS DateTime), 0, CAST(N'2023-10-02T15:53:11.860' AS DateTime), NULL, N'Menu_ReceiveCash', 1, N'Menu - Invoicing / Receive Cash', NULL, 0, NULL, 8, 32)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (119, 0, NULL, CAST(N'2023-10-02T15:53:11.860' AS DateTime), 0, CAST(N'2023-10-02T15:53:11.860' AS DateTime), NULL, N'ReceiveCash', 1, N'View Receive Cash', NULL, 0, NULL, 8, 32)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (120, 0, NULL, CAST(N'2023-10-02T15:53:11.860' AS DateTime), 0, CAST(N'2023-10-02T15:53:11.860' AS DateTime), NULL, N'ConfirmReceiveCash', 1, N'Confirm Receive Cash', NULL, 0, NULL, 8, 32)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (121, 0, NULL, CAST(N'2023-10-02T15:53:42.653' AS DateTime), 0, CAST(N'2023-10-02T15:53:42.653' AS DateTime), NULL, N'Menu_Treasury', 1, N'Menu - Invoicing / Treasury', NULL, 0, NULL, 8, 33)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (122, 0, NULL, CAST(N'2023-10-02T15:53:42.653' AS DateTime), 0, CAST(N'2023-10-02T15:53:42.653' AS DateTime), NULL, N'Treasury', 1, N'View Treasury', NULL, 0, NULL, 8, 33)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (123, 0, NULL, CAST(N'2023-10-02T15:55:24.307' AS DateTime), 0, CAST(N'2023-10-02T15:55:24.307' AS DateTime), NULL, N'Menu_TransferCash', 1, N'Menu - Invoicing / Transfer Cash', NULL, 0, NULL, 8, 34)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (124, 0, NULL, CAST(N'2023-10-02T15:55:24.307' AS DateTime), 0, CAST(N'2023-10-02T15:55:24.307' AS DateTime), NULL, N'TransferCash', 1, N'View Transfer Cash', NULL, 0, NULL, 8, 34)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (125, 0, NULL, CAST(N'2023-10-02T15:55:24.307' AS DateTime), 0, CAST(N'2023-10-02T15:55:24.307' AS DateTime), NULL, N'ConfirmTransferCash', 1, N'Confirm Transfer Cash', NULL, 0, NULL, 8, 34)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (126, 0, NULL, CAST(N'2023-10-02T16:05:07.033' AS DateTime), 0, CAST(N'2023-10-02T16:05:07.033' AS DateTime), NULL, N'Menu_Invoices', 1, N'Menu - Invoicing / Invoicing', NULL, 0, NULL, 8, 35)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (127, 0, NULL, CAST(N'2023-10-02T16:05:07.033' AS DateTime), 0, CAST(N'2023-10-02T16:05:07.033' AS DateTime), NULL, N'Invoices', 1, N'View Invoices', NULL, 0, NULL, 8, 35)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (128, 0, NULL, CAST(N'2023-10-02T16:13:52.627' AS DateTime), 0, CAST(N'2023-10-02T16:13:52.627' AS DateTime), NULL, N'Menu_Products', 1, N'Menu - Products Management', NULL, 0, NULL, 9, 36)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (129, 0, NULL, CAST(N'2023-10-02T16:13:52.627' AS DateTime), 0, CAST(N'2023-10-02T16:13:52.627' AS DateTime), NULL, N'ProductList', 1, N'View Products', NULL, 0, NULL, 9, 36)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (130, 0, NULL, CAST(N'2023-10-02T16:13:52.627' AS DateTime), 0, CAST(N'2023-10-02T16:13:52.627' AS DateTime), NULL, N'AddProduct', 1, N'Add Product', NULL, 0, NULL, 9, 36)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (131, 0, NULL, CAST(N'2023-10-02T16:13:52.627' AS DateTime), 0, CAST(N'2023-10-02T16:13:52.627' AS DateTime), NULL, N'EditProduct', 1, N'Edit Product', NULL, 0, NULL, 9, 36)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (132, 0, NULL, CAST(N'2023-10-02T16:13:52.627' AS DateTime), 0, CAST(N'2023-10-02T16:13:52.627' AS DateTime), NULL, N'DeletProduct', 1, N'Delete Product', NULL, 0, NULL, 9, 36)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (133, 0, NULL, CAST(N'2023-10-02T16:17:25.950' AS DateTime), 0, CAST(N'2023-10-02T16:17:25.950' AS DateTime), NULL, N'Menu_Complains', 1, N'Menu - Complains Management', NULL, 0, NULL, 10, 37)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (134, 0, NULL, CAST(N'2023-10-02T16:17:25.950' AS DateTime), 0, CAST(N'2023-10-02T16:17:25.950' AS DateTime), NULL, N'ComplainsList', 1, N'View Complains', NULL, 0, NULL, 10, 37)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (135, 0, NULL, CAST(N'2023-10-02T16:17:25.950' AS DateTime), 0, CAST(N'2023-10-02T16:17:25.950' AS DateTime), NULL, N'AddComplain', 1, N'Add Complain', NULL, 0, NULL, 10, 37)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (136, 0, NULL, CAST(N'2023-10-02T16:17:25.950' AS DateTime), 0, CAST(N'2023-10-02T16:17:25.950' AS DateTime), NULL, N'DeleteComplain', 1, N'Delete Complain', NULL, 0, NULL, 10, 37)
GO
INSERT [dbo].[AppService] ([ID], [AllowAnonymous], [CreatedBy], [CreationDate], [IsDeleted], [ModificationDate], [ModifiedBy], [Name], [ShowToUser], [Title], [ClassName], [AllowLog], [LogMessage], [AppServiceClassID], [AppServiceGroupID]) VALUES (137, 0, NULL, CAST(N'2023-10-02T16:17:25.950' AS DateTime), 0, CAST(N'2023-10-02T16:17:25.950' AS DateTime), NULL, N'SolveComplain', 1, N'Solve Complain', NULL, 0, NULL, 10, 37)
GO
SET IDENTITY_INSERT [dbo].[AppService] OFF
GO
