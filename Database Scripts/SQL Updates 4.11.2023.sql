--insert into RoleAppService 
--(AppServiceID, Enabled, RoleID, [CreationDate], [ModificationDate]) 
--values  (129,1,4, '2023-10-13 19:26:48.580', '2023-10-13 19:26:48.580') 

--insert into RoleAppService 
--(AppServiceID, Enabled, RoleID, [CreationDate], [ModificationDate]) 
--values  (130,1,4, '2023-10-13 19:26:48.580', '2023-10-13 19:26:48.580') 

--insert into RoleAppService 
--(AppServiceID, Enabled, RoleID, [CreationDate], [ModificationDate]) 
--values  (131,1,4, '2023-10-13 19:26:48.580', '2023-10-13 19:26:48.580') 

--insert into RoleAppService 
--(AppServiceID, Enabled, RoleID, [CreationDate], [ModificationDate]) 
--values  (132,1,4, '2023-10-13 19:26:48.580', '2023-10-13 19:26:48.580') 

--insert into RoleAppService 
--(AppServiceID, Enabled, RoleID) 
--values  (71, 1, 4) 
--, (70,1,4) , (68 ,1 ,4) , (67 ,1 ,4)

alter table Shipment
add ShipToRefundId int null
, RefundCash float null
, RefundFees float null
go

insert into Status(Name)
values 
(N'Ready For Refund | جاهز للإسترداد')
, (N'Assigned For Refund | تم التكليف للإسترداد')
, (N'Out For Refund | في الطريق للإسترداد')
, (N'Refunded | تم الإسترداد')
, (N'Cancelled Refund | إسترداد لاغي')

alter table AccountTransaction
add RefundCash float null
, RefundFees float null
go


insert into AppService(Name, Title, AppServiceClassID, AppServiceGroupID)
values ('ShipmentRefund','Refund Shipment',6,23)