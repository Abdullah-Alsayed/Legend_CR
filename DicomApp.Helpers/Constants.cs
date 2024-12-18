﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DicomApp.Helpers
{
    public static class SystemConstants
    {
        public static readonly int? System;

        public static class Permission
        {
            //Category
            public const string CategoryList = "CategoryList";
            public const string AddCategory = "AddCategory";
            public const string EditCategory = "EditCategory";
            public const string DeleteCategory = "DeleteCategory";

            //Game
            public const string ListGame = "ListGame";
            public const string AddGame = "AddGame";
            public const string EditGame = "EditGame";
            public const string GameDelete = "DeleteGame";

            //Role
            public const string ListRole = "ListRole";
            public const string EditRoleAppService = "EditRoleAppService";
            public const string EditRole = "EditRole";
            public const string AddRole = "AddRole";
            public const string DeleteRole = "DeleteRole";

            //Advertisement
            public const string TrackAdvertisement = "TrackAdvertisement";
            public const string AddAdvertisement = "AddAdvertisement";
            public const string EditAdvertisement = "EditAdvertisement";
            public const string PrintAdvertisement = "PrintAdvertisement";
            public const string ListAdvertisement = "ListAdvertisement";
            public const string EditBasicAdvertisement = "EditBasicAdvertisement";
            public const string RejectAdvertisement = "RejectAdvertisement";
            public const string ChangStatusAdvertisement = "ChangStatusAdvertisement";

            //GamerService
            public const string AddGamerService = "AddGamerService";
            public const string EditGamerService = "EditGamerService";
            public const string ChangStatusGamerService = "ChangStatusGamerService";
            public const string RejectGamerService = "RejectGamerService";
            public const string EditBasicGamerService = "EditBasicGameService";
            public const string PrintGamerService = "PrintGamerService";
            public const string ListGamerService = "ListGamerService";

            //Invoice
            public const string ListInvoice = "ListInvoice";
            public const string AddInvoice = "AddInvoice";
            public const string RefundInvoice = "RefundInvoice";
            public const string ListTransaction = "ListTransaction";

            //User
            public const string ListStaff = "ListStaff";
            public const string ListGamer = "ListGamer";
            public const string AddGamer = "AddGamer";
            public const string DeleteStaff = "DeleteStaff";
            public const string AddStaff = "AddStaff";
            public const string GetUserData = "GetUserData";
            public const string EditStaff = "EditStaff";

            // Gamer
            public const string Main = "Main";

            //Testimonial
            public const string ListTestimonial = "ListTestimonial";

            //GameCharge
            public const string AddGameCharge = "AddGameCharge";
            public const string EditGameCharge = "EditGameCharge";
            public const string GameChargeDelete = "GameChargeDelete";

            public const string History = "History";
        }

        public static class Languages
        {
            public const string Arabic = "ar-EG";
            public const string English = "en-US";
        }

        public static class Claims
        {
            public const string UserID = "UserID";
            public const string Name = "Name";
            public const string Email = "Email";
            public const string RoleID = "RoleID";
            public const string RoleName = "RoleName";
            public const string Language = "Language";
        }

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

        public static class Role
        {
            public static string SuperAdmin = "SuperAdmin";
            public static string Admin = "Admin";
            public static string Gamer = "Gamer";
        }

        public static class Imges
        {
            public static string DefaultGames = "DefaultGames.png";
            public static string Admin = "Admin";
            public static string Gamer = "Gamer";
            public static string Default = "Fake-Img.png";
        }

        public static class Message
        {
            public static string Success = "operationSuccess";
            public static string Field = "field";
            public static string NotFound = "notFound";
            public static string CantUpdatePrice = "youCantUpdatePrice";
        }

        public static class Layout
        {
            public static string Dashboard = "Dashboard";
            public static string Gamer = "gamer";
        }
    }
}
