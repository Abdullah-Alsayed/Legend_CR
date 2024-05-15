
delete from RoleAppService
GO
DBCC CHECKIDENT ('RoleAppService', RESEED, 0)
GO

insert into AppServiceGroup (Name, AppServiceClassID)
values ('General', 11)

insert into AppService (Name, ShowToUser, Title, AppServiceClassID, AppServiceGroupID)
values ('MenuVendor_ReportsShipments', 0, 'MenuVendor_ReportsShipments', 11, 38)
, ('MenuVendor_ReportsInvoices', 0, 'MenuVendor_ReportsInvoices', 11, 38)
, ('MenuVendor_ReportsStock', 0, 'MenuVendor_ReportsStock', 11, 38)
, ('MenuVendor_ReportsProblem', 0, 'MenuVendor_ReportsProblem', 11, 38)
, ('MenuVendor_AddShipment', 0, 'MenuVendor_ShipmentList', 11, 38)
, ('MenuVendor_PickupDelivery', 0, 'MenuVendor_PickupDelivery', 11, 38)
, ('MenuVendor_PickupStock', 0, 'MenuVendor_PickupStock', 11, 38)
, ('MenuVendor_PickupList', 0, 'MenuVendor_PickupList', 11, 38)
, ('MenuVendor_Products', 0, 'MenuVendor_Products', 11, 38)
, ('MenuVendor_ShippingCalculator', 0, 'MenuVendor_ShippingCalculator', 11, 38)
, ('MenuVendor_ContactUs', 0, 'MenuVendor_ContactUs', 11, 38)
, ('Vendor_ReportsShipments', 0, 'Vendor_ReportsShipments', 11, 38)
, ('Vendor_ReportsInvoices', 0, 'Vendor_ReportsInvoices', 11, 38)
, ('Vendor_ReportsStock', 0, 'Vendor_ReportsStock', 11, 38)
, ('Vendor_ReportsProblem', 0, 'Vendor_ReportsProblem', 11, 38)
, ('Vendor_ShipmentList', 0, 'Vendor_ShipmentList', 11, 38)
, ('Vendor_AddShipment', 0, 'Vendor_AddShipment', 11, 38)
, ('Vendor_ProblemReply', 0, 'Vendor_ProblemReply', 11, 38)
, ('Vendor_PickupDelivery', 0, 'Vendor_PickupDelivery', 11, 38)
, ('Vendor_PickupStock', 0, 'Vendor_PickupStock', 11, 38)
, ('Vendor_PickupList', 0, 'Vendor_PickupList', 11, 38)
, ('Vendor_ShippingCalculator', 0, 'Vendor_ShippingCalculator', 11, 38)
, ('Vendor_ContactUs', 0, 'Vendor_ContactUs', 11, 38)


insert into RoleAppService (AppServiceID, RoleID, Enabled)
values (4, 4, 1) , (6, 4, 1)


insert into RoleAppService (AppServiceID, RoleID, Enabled)
select ID, 4, 1 from AppService
where AppServiceGroupID = 38