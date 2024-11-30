using System;

namespace DicomApp.CommonDefinitions.DTO
{
    public class DashboardDTO
    {
        public double Available_Balance { get; set; }
        public int Success_Rate
        {
            get
            {
                double Sum = SoldAdvertisements_Count + RejectAdvertisements_Count;
                double Tota = SoldAdvertisements_Count / Sum;
                if (SoldAdvertisements_Count != 0)
                    return Convert.ToInt32(Tota * 100);
                else
                    return 0;
            }
        }

        // All Advertisements
        public int AllAdvertisements_Count { get; set; }
        public string AllAdvertisements_Arow { get; set; }

        public double AllAdvertisements_Percentchanged { get; set; }
        public double AllAdvertisements_Total { get; set; }
        public double AllAdvertisements_Tax { get; set; }
        public double AllAdvertisements_Subtotal { get; set; }
        public double AllAdvertisements_Paid { get; set; }
        public double AllAdvertisements_NotPaid { get; set; }
        public double AllAdvertisements_Sum { get; set; }

        //Sold Advertisements
        public int SoldAdvertisements_Count { get; set; }
        public double SoldAdvertisements_Percentchanged { get; set; }
        public double DelevredAdvertisements_Tax { get; set; }
        public string SoldAdvertisements_Arow { get; set; }
        public double SoldAdvertisements_Total { get; set; }
        public double SoldAdvertisementsSubtotal { get; set; }

        //Pending Advertisements
        public double PendingAdvertisements_Percentchanged { get; set; }
        public int PendingAdvertisements_Count { get; set; }
        public string PendingAdvertisements_Arow { get; set; }
        public double PendingAdvertisements_Total { get; set; }

        //Reject Advertisements
        public int RejectAdvertisements_Count { get; set; }
        public double RejectAdvertisements_Percentchanged { get; set; }
        public string RejectAdvertisements_Arow { get; set; }
        public double RejectAdvertisements_Tax { get; set; }
        public double RejectAdvertisements_Total { get; set; }

        //Total Earning
        public string TotalEarning_Arow { get; set; }
        public double TotalEarning_Percentchanged { get; set; }
        public double TotalEarning { get; set; }

        public double RatingAverage { get; set; }

        // Total Earning Vendor Charts
        public double Income { get; set; }
        public double ShippingFees { get; set; }
        public double CancellationFees { get; set; }

        // Total Earning Chart
        public double Account { get; set; }
        public double Push { get; set; }
        public double Change { get; set; }

        public double TodayAdvertisements_Total { get; set; }
        public double TodayAdvertisements_Tax { get; set; }
        public double TodayAdvertisements_Subtotal { get; set; }
        public double TodayAdvertisements_Paid { get; set; }
        public double TodayAdvertisements_NotPaid { get; set; }

        public int NewAdvertisements_Count { get; set; }
        public double NewAdvertisements_Percentchanged { get; set; }

        //Charts
        public ChartDTO[] Chart_Year { get; set; }
        public ChartDTO[] Chart_TopBuyer { get; set; }
        public ChartDTO[] Chart_TopDriver { get; set; }
        public ChartDTO[] Chart_TopGames { get; set; }
        public ChartDTO[] Chart_PackagingStock { get; set; }
        public ChartDTO[] AreaChartDate { get; internal set; }
        public ChartDTO[] Chart_Genders { get; set; }

        //public List<ShipmentDTO> Advertisements { get; set; }
        public double Revenues { get; set; }
        public object Expenses { get; set; }

        public int TransactionSource_Success { get; set; }
        public int TransactionSource_UnSuccess { get; set; }

        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
