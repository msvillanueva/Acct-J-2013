using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Web.Core
{
    public class Constants
    {
        public static decimal VATPercent()
        {
            return decimal.Parse(ConfigurationManager.AppSettings["VAT"]) / 100;
        }

        public static decimal EWTPercent()
        {
            return decimal.Parse(ConfigurationManager.AppSettings["EWT"]) / 100;
        }

        public static string Checker()
        {
            return ConfigurationManager.AppSettings["Checker"].ToString();
        }

        public static string Approver()
        {
            return ConfigurationManager.AppSettings["Approver"].ToString();
        }

        public static string CoName()
        {
            return "JACA CONSTRUCTION & MGMT CORPORATION";
        }

        public static string CoAddress()
        {
            return "A Corporate Center #29 Jose Abad Santos Ave. Salawag, Dasmariñas, Cavite";
        }

        public static string CoTIN()
        {
            return "219-413-233-000";
        }

        public static string CodeInputTax()
        {
            return ConfigurationManager.AppSettings["CodeInputTax"].ToString();
        }

        public static string CodeExpandedTax()
        {
            return ConfigurationManager.AppSettings["CodeExpandedTax"].ToString();
        }

        public static string CodeDirectExpense()
        {
            return ConfigurationManager.AppSettings["CodeDirectExpense"].ToString();
        }

        public static string CodeInDirectExpense()
        {
            return ConfigurationManager.AppSettings["CodeIndirectExpense"].ToString();
        }

        public static string CodeAdminExpense()
        {
            return ConfigurationManager.AppSettings["CodeAdminExpense"].ToString();
        }

        public static List<string> CodeAssets()
        {
            var assetString = ConfigurationManager.AppSettings["Assets"].ToString();
            var assets = assetString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            return assets.ToList();
        }

        public static List<string> CodeLiabilities()
        {
            var liabilityString = ConfigurationManager.AppSettings["Liabilities"].ToString();
            var liabilities = liabilityString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            return liabilities.ToList();
        }

        public static List<string> CodeIncome()
        {
            var liabilityString = ConfigurationManager.AppSettings["CodeIncome"].ToString();
            var liabilities = liabilityString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            return liabilities.ToList();
        }

        public static List<string> CodeCostOfSales()
        {
            var liabilityString = ConfigurationManager.AppSettings["CodeCostOfSales"].ToString();
            var liabilities = liabilityString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            return liabilities.ToList();
        }

        public static List<string> CodeExpenses()
        {
            var liabilityString = ConfigurationManager.AppSettings["CodeExpenses"].ToString();
            var liabilities = liabilityString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            return liabilities.ToList();
        }
    }
}