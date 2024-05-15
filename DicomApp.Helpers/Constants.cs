using System;
using System.Collections.Generic;
using System.Text;

namespace DicomApp.Helpers
{
    public static class Constants
    {
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
            public static int Year = DateTime.Now.Month == 1 ? (DateTime.Now.Year) - 1 : DateTime.Now.Year;
        }
    }
}