using System;
using System.Collections.Generic;
using System.Text;

namespace DicomApp.DAL.DB
{
    public enum GenderEnum
    {
        None,
        Male,
        Female
    }

    public enum GameServiceType
    {
        Push = 1,
        Charge
    }

    public enum TransactionTypeEnum
    {
        Acount,
        Push,
        Change
    }
}
