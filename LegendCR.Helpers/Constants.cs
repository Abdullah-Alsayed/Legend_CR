using System;
using System.Collections.Generic;
using System.Text;

namespace LegendCR.Helpers
{
    public static class Constants
    {
        public const string System = "System";
        public const string Permission = "Permission";
        public const string Issuer = "E-commerceApi";
        public const string HostName = "localhost:5001";
        public const string connectionString = "ArrowDown.png";

        public static class ActionType
        {
            public const string View = "View";
            public const string PartialView = "PartialView";
            public const string Table = "Table";
            public const string Print = "Print";
        }

        public static class ArrowType
        {
            public const string Up = "ArowUp.png";
            public const string Down = "ArrowDown.png";
        }

        public static class Datetime
        {
            public static int lastMonth = DateTime.Now.Month == 1 ? 12 : DateTime.Now.Month;
            public static int Year =
                DateTime.Now.Month == 1 ? (DateTime.Now.Year) - 1 : DateTime.Now.Year;
        }

        public static class Languages
        {
            public const string Arabic = "Arabic";
            public const string English = "English";
            public const string Ar = "ar-EG";
            public const string En = "en-US";
        }

        public static class Gender
        {
            public const string Male = "Male";
            public const string Female = "Female";
        }

        public static class Roles
        {
            public const string SuperAdmin = "Super Admin";
            public const string User = "User";
            public const string Client = "Client";
        }

        public static class PhotoFolder
        {
            public const string Brands = "Brands";
            public const string Categorys = "Categorys";
            public const string SubCategorys = "SubCategorys";
            public const string ProductPhoto = "ProductPhoto";
            public const string Images = "Images";
            public const string Main = "Main";
            public const string Expense = "Expense";
            public const string Slider = "Slider";
            public const string Products = "Products";
        }

        public static class NotificationIcons
        {
            public const string Add = "ADD.png";
            public const string Edit = "Edit.png";
            public const string Delete = "Delete.png";
        }

        public static class Regex
        {
            public const string Color = "^#?([a-f0-9]{6}|[a-f0-9]{3})$";
            public const string PhoneNumber =
                "^(\\+\\d{1,2}\\s?)?(1\\s?)?((\\(\\d{3}\\))|\\d{3})[\\s.-]?\\d{3}[\\s.-]?\\d{4}$";
        }
    }
}
