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
        Account,
        Push,
        Change,
        None
    }

    public enum TransactionSourceEnum
    {
        Paypal,
        Attachments
    }

    public enum EntityTypeEnum
    {
        Game,
        User,
        Advertisement,
        GameService,
        Invoice
    }

    public enum ActionTypeEnum
    {
        Add,
        Edit,
        Delete,
        ChangeStatus
    }
}
