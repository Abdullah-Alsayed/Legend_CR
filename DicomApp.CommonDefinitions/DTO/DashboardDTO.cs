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
                double Sum = DeliveredOrders_Count + CanceledOrders_Count;
                double Tota = DeliveredOrders_Count / Sum;
                if (DeliveredOrders_Count != 0)
                    return Convert.ToInt32(Tota * 100);
                else
                    return 0;
            }
        }

        // All Orders
        public int AllOrders_Count { get; set; }
        public string AllOrders_Arow { get; set; }

        public double AllOrders_Percentchanged { get; set; }
        public double AllOrders_Total { get; set; }
        public double AllOrders_Tax { get; set; }
        public double AllOrders_Subtotal { get; set; }
        public double AllOrders_Paid { get; set; }
        public double AllOrders_NotPaid { get; set; }
        public double AllOrders_Sum { get; set; }

        //Delivered Orders
        public int DeliveredOrders_Count { get; set; }
        public double DeliveredOrders_Percentchanged { get; set; }
        public double DelevredOrders_Tax { get; set; }
        public string DeliveredOrders_Arow { get; set; }
        public double DeliveredOrders_Total { get; set; }
        public double DeliveredOrdersSubtotal { get; set; }

        //Pending Orders
        public double PendingOrders_Percentchanged { get; set; }
        public int PendingOrders_Count { get; set; }
        public string PendingOrders_Arow { get; set; }
        public double PendingOrders_Total { get; set; }

        //Canceled Orders
        public int CanceledOrders_Count { get; set; }
        public double CanceledOrders_Percentchanged { get; set; }
        public string CanceledOrders_Arow { get; set; }
        public double CanceledOrders_Tax { get; set; }
        public double CanceledOrders_Total { get; set; }

        //Total Earning
        public string TotalEarning_Arow { get; set; }
        public double TotalEarning_Percentchanged { get; set; }
        public double TotalEarning { get; set; }

        // Total Earning Vendor Charts
        public double Income { get; set; }
        public double ShippingFees { get; set; }
        public double CancellationFees { get; set; }

        // Total Earning Chart
        public double Packaging { get; set; }
        public double Shipping { get; set; }
        public double Storage { get; set; }

        public double TodayOrders_Total { get; set; }
        public double TodayOrders_Tax { get; set; }
        public double TodayOrders_Subtotal { get; set; }
        public double TodayOrders_Paid { get; set; }
        public double TodayOrders_NotPaid { get; set; }



        public int NewOrders_Count { get; set; }
        public double NewOrders_Percentchanged { get; set; }


        //Charts
        public ChartDTO[] Chart_Year { get; set; }
        public ChartDTO[] Chart_TopAccount { get; set; }
        public ChartDTO[] Chart_TopDriver { get; set; }
        public ChartDTO[] Chart_TopArea { get; set; }
        public ChartDTO[] Chart_PackagingStock { get; set; }
        public ChartDTO[] AreaChartDate { get; internal set; }

        //public List<ShipmentDTO> Orders { get; set; }
        public double Revenues { get; set; }
        public object Expenses { get; set; }
    }
}


