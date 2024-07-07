namespace DicomApp.CommonDefinitions.DTO
{
    public enum MessageKey : int
    {
        UserNotActive,
        InvalidUsernameOrPassword,
        AppUsersDeletedSuccessfully,
        AppUsersInvalid,
        DeletedSuccessfully,
        InvalidData,
        AddedSuccessfully,
        UpdatedSuccessfully,
        QuantityInvalid,
        SaveFaild,
        SerialNumberAlreadyExist,
        UserAlreadyExist,
        UserExceedNumberPackage,
        SelectPackage,
        ForgetPasswordSuccessfully,
        ExistEmail,
        CompanyEmailExist,
        UserLogoFileIsNull
    }

    public enum EnumRole : int
    {
        Admin = 1,
        BranchManger = 2,
        Customer = 3,
        Gamer = 4,
        DeliveryMan = 5,
        Accounting = 6,
        DataEntry = 7,
        WarehouseManager = 8,
        AccountManager = 9,
        Employee = 10,
        Treasury = 10013,
        SuperAdmin = 10014,
    }

    public enum EnumStatus : int
    {
        Ready_For_Delivery = 1,
        Assigned_For_Delivery = 2,
        Out_For_Delivery = 3,
        Delivered = 4,
        Cancelled = 5,
        Ready_For_Pickup = 6,
        Assigned_For_Pickup = 7,
        Picked_Up = 8,
        At_Warehouse = 9,
        Cancelled_Pickup = 10,
        Postponed = 11,
        Archive = 12,
        Paid_To_Vendor = 13,
        Ready_For_Return = 14,
        Assigned_For_Return = 15,
        Out_For_Return = 16,
        Returned = 17,
        Not_Delivered = 18,
        Not_Received = 19,
        Ready_For_Refund = 20,
        Assigned_For_Refund = 21,
        Out_For_Refund = 22,
        Refunded = 23,
        Cancelled_Refund = 24,

        All = 0,
        Current = 101,
        CourierReturn = 102,
        Done = 103
    }

    public enum EnumShipmentService : int
    {
        PickupDelivery = 1,
        Exchange = 2,
        Refund = 3,
        PaymentCollection = 4,
        ExchangeRefund = 5
    }

    public enum EnumShipmentType : int
    {
        PickupDelivery = 1,
        StockDelivery = 2,
        CashCollect = 3,
        CashDelivery = 4
    }

    public enum EnumPickupRequestType : int
    {
        StockPickup = 1,
        DeliveryPickup = 2
    }

    public enum EnumSelectListType : byte
    {
        AccountMultiple = 1,
        StatusMultiple = 2,
        Courier = 3,
        Vendor = 4,
        Status = 5,
        Area = 6,
        PDF = 7,
        Zone = 8,
        Confirmed = 9,
        From = 10,
        To = 11,
        AreaMultiple = 12,
        Category = 13,
        Quantity = 14,
        Role = 15,
        Solved,
        Department,
        Employee,
        Users,
        Game,
        Gamer,
        Level,
        Price,
        ServiceType,
        GameServiceType,
        AcceptAdvertisement,
        AcceptGamerService
    }

    public enum EnumVendorActionName : byte
    {
        OrderReports = 1,
        CustomerFollowup = 2,
        ShipmentList = 3,
        PickupList = 4,
        PickupRequest = 5,
        VendorReport = 6,
    }

    public enum EnumActionName : byte
    {
        PickupOrdersAssigned = 1,
        AssignPickups = 2,
        OrderListPartial = 3,
        OrderListVendorPartial = 4,
        CustomerFollowup = 5,
        AssignOrderListPartial = 6,
        PolicyOrderListPartial = 7,
        AssignOrderListDeliveryPartial = 8,
        AccountReportPartial = 9,
        DriverReportPartial = 10,
        PaidReportPartial = 11,
        ProblemsReportPartial = 12,
        ListPickUpRequestPartial = 13,
        PickupReport = 14,
        PickupRequest = 15,
        PickupRequestVendor = 16,
        VendorReport = 17,
        CouriersReport = 18,
        GameList = 19,
        PackagingReport = 20,
        InvoiceReport = 21,
        Invoices = 22,
        ShipmentsReport = 23,
        InvoiceReportVendor = 24,
        CustomerFollowupReport = 25,
        Treasury = 26,
        ComplainsManagement = 27,
        ComplainsReport = 28
    }

    public enum EnumFollowupType : int
    {
        Ads_Added = 1,
        Advertisement_Updated = 2,
        Problem_Added = 3,
        Problem_Deleted = 4,
        Problem_Solved = 5,
        Replied_To_Courier = 6,
        Reported_To_Vendor = 7,
        Vendor_Replied = 8,
        UpdatePackage = 9,
    }

    public enum DepartmentList : int
    {
        AccountManger = 1,
        Accounting = 2,
        BranchManger = 3,
        Storage = 4,
        CallCenter = 5,
        DataEntry = 6,
    }

    public enum EnumTransactionType : byte
    {
        Withdraw = 0,
        Deposit = 1
    }

    public enum EnumCashTransferType : byte
    {
        CourierTransfer = 1,
        BankTransfer = 2,
        VodafoneCashTransfer = 3,
        InstaPayTransfer = 4
    }

    public enum EnumUserId : int
    {
        Admin = 7
    }

    public enum EnumAccountId : int
    {
        Main_Treasury = 1,
        RED_Main_Account = 2
    }

    public enum EnumRefIdType
    {
        Advertisement,
        Delivery_Pickup,
        Stock_Pickup,
        Cash_Delivery,
        Cash_Collect,
        Shipment_Return,
        Shipment_Refund,
        Account_Transaction,
        Cash_Transfer,
        GamerService,
        Invoice
    }

    public enum EnumProblemType : int
    {
        ProblemType = 0,
        CancelType = 1
    }

    public enum StatusTypeEnum : int
    {
        All = 0,
        InProgress = 1,
        Accept,
        Published,
        Reject,
        Sold,
    }
}
