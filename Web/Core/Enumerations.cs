using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Web.Core
{
    public class Enumerations
    {
        public enum Role
        {
            Admin = 1,
            SuperUser = 2,
            Encoder = 3
        }

        public enum EntryType
        {
            Normal = 1, 
            CashInBank = 2,
            AdvancesToSubContractual = 3, 
            AdvancesToEmployees = 4,
            AdvancesToAffiliates = 5,
            AdvancesToOthers = 6,
            AccountReceivables = 7,
            Inventories = 8,
            FixedAssets = 9,
            AccumulatedDepreciation = 10,
            Payments = 11,
            OtherCurrentAssets = 12,
            CurrentLiabilities = 13,
            LongTermLiabilities =14,
            CapitalAccounts = 15,
            Income = 16,
            CostOfSales = 17,
            Expenses = 18,
            Cash = 19
        }

        public enum EntryClass
        {
            Direct = 1,
            Administrative = 2
        }

        public enum ActionType
        {
            Insert,
            Update,
            Delete,
            Report,
            Upload,
            Login,
            Logout,
            Error,
            LostPassword,
            WrongPassword,
            Submit,
            Deny,
            OR,
            Print,
            ChangePermission
        }

        public enum ActionTable
        {
            User,
            Bank,
            Payee,
            AccountEntry,
            Project,
            Voucher,
            VoucherEntry,
            SalesInvoice,
            SalesInvoiceEntry,
            Report
        }

        public enum Searchrangetype
        {
            range = 1,
            year = 2,
            month = 3,
            week = 4,
            day = 5
        }

        public static string GetDescription(Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return (attributes.Length > 0) ? attributes[0].Description : value.ToString();
        }

        public static List<KeyValuePair<string, string>> GetValuesAndDescription(Type enumType)
        {
            var kvPairList = new List<KeyValuePair<string, string>>();

            foreach (Enum enumValue in Enum.GetValues(enumType))
            {
                kvPairList.Add(new KeyValuePair<string, string>(enumValue.ToString(), GetDescription(enumValue)));
            }

            return kvPairList;
        }

        public static List<KeyValuePair<int, string>> GetList(Type enumType)
        {
            var _type = enumType.ToString();
            var kvPairList = new List<KeyValuePair<int, string>>();

            foreach (Enum enumValue in Enum.GetValues(enumType))
            {
                int intVal = (int)Enum.Parse(Type.GetType(_type), enumValue.ToString());
                kvPairList.Add(new KeyValuePair<int, string>(intVal, GetDescription(enumValue)));
            }

            return kvPairList;
        }
    }
}