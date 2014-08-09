using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aspose.Cells;
using Web.Core.DataAccess;
using Web.Models;

namespace Web.Core
{
    public class WorkbookGenerator
    {
        public static Workbook GenerateCollectionsReport(string dateFrom, string dateTo)
        {
            var _dateFrom = Convert.ToDateTime(dateFrom);
            var _dateTo = Convert.ToDateTime(dateTo).AddHours(23).AddMinutes(59);

            var invoices = LSalesInvoice.GetSalesInvoices(_dateFrom, _dateTo, true)
                .ToList();
            decimal totalAmount = 0;

            Workbook workbook = new Workbook();
            Worksheet sheet = workbook.Worksheets[0];
            Cells cells = workbook.Worksheets[0].Cells;
            int col = 0;
            int row = 0;

            row = 5;
            cells[row, col++].PutValue("OR NO");
            cells[row, col++].PutValue("OR DATE");
            cells[row, col++].PutValue("INVOICE NO");
            cells[row, col++].PutValue("CUSTOMER");
            cells[row, col++].PutValue("AMOUNT COLLECTED");
            cells[row++, col++].PutValue("EWT DEDUCTED");

            foreach (var invoice in invoices)
            {
                col = 0;
                cells[row, col++].PutValue(invoice.ORNo);
                cells[row, col++].PutValue(((DateTime)invoice.DateCollected).ToShortDateString());
                cells[row, col++].PutValue(invoice.ID.ToString());
                cells[row, col++].PutValue(invoice.Customer);
                var ewt = invoice.NetAmount * Constants.EWTPercent();
                cells[row, col++].PutValue(Utility.To2Dec(invoice.TotalAmountDue - ewt));
                cells[row++, col++].PutValue(Utility.To2Dec(ewt));
                totalAmount += invoice.TotalAmountDue;
            }
            col = 0;
            cells[row, col++].PutValue("TOTAL");
            col = 4;
            cells[row++, col++].PutValue(Utility.To2Dec(totalAmount));
            cells[row, col].PutValue("");

            sheet.AutoFitColumns();

            row = 0;
            col = 0;
            cells[row++, col].PutValue(Constants.CoName().ToUpper());
            cells[row++, col].PutValue("SUMMARY OF COLLECTION");
            string dateRange = _dateFrom.Month == _dateTo.Month ?
                "FOR THE MONTH OF " + _dateFrom.ToString("MMMM").ToUpper() + ", " + _dateFrom.Year.ToString() :
                "FROM " + dateFrom + " TO " + dateTo;
            cells[row++, col].PutValue(dateRange);
            cells[row++, col].PutValue("");

            return workbook;
        }

        public static Workbook GenerateSalesReport(string dateFrom, string dateTo)
        {
            var _dateFrom = Convert.ToDateTime(dateFrom);
            var _dateTo = Convert.ToDateTime(dateTo).AddHours(23).AddMinutes(59);

            var invoices = LSalesInvoice.GetSalesInvoices(false)
                .Where(a => a.DateAdded >= _dateFrom && a.DateAdded <= _dateTo)
                .ToList();
            decimal totalAmount = 0;
            decimal totalVatable = 0;
            decimal totalTax = 0;

            Workbook workbook = new Workbook();
            Worksheet sheet = workbook.Worksheets[0];
            Cells cells = workbook.Worksheets[0].Cells;
            int col = 0;
            int row = 0;

            row = 5;
            cells[row, col++].PutValue("INV NO");
            cells[row, col++].PutValue("INV DATE");
            cells[row, col++].PutValue("CUSTOMER");
            cells[row, col++].PutValue("PROJECT");
            cells[row, col++].PutValue("ITEM DESCRIPTION");
            cells[row, col++].PutValue("INV AMOUNT");
            cells[row, col++].PutValue("VATABLE AMOUNT");
            cells[row++, col++].PutValue("OUTPUT TAX");

            foreach (var invoice in invoices)
            {
                col = 0;
                cells[row, col++].PutValue(invoice.ID.ToString());
                cells[row, col++].PutValue(((DateTime)invoice.DateAdded).ToShortDateString());
                cells[row, col++].PutValue(invoice.Customer);
                cells[row, col++].PutValue(invoice.ProjectName);
                cells[row, col++].PutValue(invoice.Business);
                cells[row, col++].PutValue(Utility.To2Dec(invoice.TotalAmountDue));
                cells[row, col++].PutValue(Utility.To2Dec(invoice.TotalAmount));
                cells[row++, col++].PutValue(Utility.To2Dec(invoice.VATAmount));
                totalAmount += invoice.TotalAmountDue;
                totalVatable += invoice.TotalAmount;
                totalTax += invoice.VATAmount;
            }
            col = 0;
            cells[row, col++].PutValue("TOTAL");
            col = 5;
            cells[row, col++].PutValue(Utility.To2Dec(totalAmount));
            cells[row, col++].PutValue(Utility.To2Dec(totalVatable));
            cells[row++, col++].PutValue(Utility.To2Dec(totalTax));
            cells[row, col].PutValue("");

            sheet.AutoFitColumns();

            row = 0;
            col = 0;
            cells[row++, col].PutValue(Constants.CoName().ToUpper());
            cells[row++, col].PutValue("SUMMARY OF SALES");
            string dateRange = _dateFrom.Month == _dateTo.Month ?
                "FOR THE MONTH OF " + _dateFrom.ToString("MMMM").ToUpper() + ", " + _dateFrom.Year.ToString() :
                "FROM " + dateFrom + " TO " + dateTo;
            cells[row++, col].PutValue(dateRange);
            cells[row++, col].PutValue("");

            return workbook;
        }

        public static Workbook GenerateExpenseReport(CProject project, string dateFrom, string dateTo)
        {
            var _dateFrom = Convert.ToDateTime(dateFrom);
            var _dateTo = Convert.ToDateTime(dateTo).AddHours(23).AddMinutes(59);

            var entries = LVoucherEntry.GetRptEntriesPerProject(project.ID, _dateFrom, _dateTo);

            var directEntryType = LAccountEntryType.GetTypeByCode(Constants.CodeDirectExpense());
            var adminEntryType = LAccountEntryType.GetTypeByCode(Constants.CodeAdminExpense());

            var directExpense = new List<CVoucherEntry>();
            directExpense.AddRange(entries.Where(a => a.AccountEntryTypeID == directEntryType.ID).ToList());

            var adminExpense = new List<CVoucherEntry>();
            adminExpense.AddRange(entries.Where(a => a.AccountEntryTypeID == adminEntryType.ID).ToList());

            decimal totalDirect = 0;
            decimal totalAdministrative = 0;

            Workbook workbook = new Workbook();
            Worksheet sheet = workbook.Worksheets[0];
            Cells cells = workbook.Worksheets[0].Cells;
            int col = 0;
            int row = 0;

            row = 4;
            cells[row++, 0].PutValue("Direct Expenses");
            col = 1;

            var distDirect = directExpense
                .OrderBy(a => a.AccountEntryName)
                .Select(a => a.AccountEntryName)
                .Distinct()
                .ToList();

            var distAdmin = adminExpense
                .OrderBy(a => a.AccountEntryName)
                .Select(a => a.AccountEntryName)
                .Distinct()
                .ToList();

            foreach (var direct in distDirect)
            {
                var entry = directExpense.Where(a => a.AccountEntryName == direct).ToList();
                var amount = entry.Sum(a => a.Debit);
                if (amount != 0)
                {
                    cells[row, 1].PutValue(direct);
                    cells[row++, 3].PutValue(Utility.To2Dec(amount));
                    totalDirect = totalDirect + (amount);
                }
            }
            cells[row, 0].PutValue("Total Direct Expense:");
            cells[row++, 3].PutValue(Utility.To2Dec(totalDirect));
            row++;

            cells[row++, 0].PutValue("Administrative Expenses");
            col = 1;
            foreach (var admin in distAdmin)
            {
                var entry = adminExpense.Where(a => a.AccountEntryName == admin).ToList();
                var amount = entry.Sum(a => a.Debit);
                if (amount != 0)
                {
                    cells[row, 1].PutValue(admin);
                    cells[row++, 3].PutValue(Utility.To2Dec(amount));
                    totalAdministrative = totalAdministrative + (amount);
                }
            }
            cells[row, 0].PutValue("Total Administrative Expense:");
            cells[row++, 3].PutValue(Utility.To2Dec(totalAdministrative));
            row++;
            cells[row, 0].PutValue("Total Expenses:");
            cells[row++, 3].PutValue(Utility.To2Dec(totalDirect + totalAdministrative));

            cells[row, col].PutValue("");

            sheet.AutoFitColumns();

            row = 0;
            col = 0;
            cells[row++, col].PutValue(Constants.CoName().ToUpper());
            cells[row++, col].PutValue("SUMMARY OF EXPENSES FOR (" + project.Name.ToUpper() + ")");
            string dateRange = _dateFrom.Month == _dateTo.Month ?
                "FOR THE MONTH OF " + _dateFrom.ToString("MMMM").ToUpper() + ", " + _dateFrom.Year.ToString() :
                "FROM " + dateFrom + " TO " + dateTo;
            cells[row++, col].PutValue(dateRange);
            cells[row++, col].PutValue("");

            return workbook;
        }

        public static Workbook GenerateTrialBalanceReport(string dateTo)
        {
            var yearNow = Convert.ToDateTime(dateTo).Year;
            var chartOfAccounts = LAccountEntryType.GetTypes();
            var accountEntries = LAccountEntry.GetEntries();
            var tbCodes = new List<string>() { 
                "1000","1200","1300","1400","2000","2100","3000","4000","5000","6000"
            };

            var tbEntryTypes = chartOfAccounts.Where(a => tbCodes.Contains(a.Code)).ToList();
            var entries = LVoucherEntry.GetEntries();

            var currentEntries = entries.Where(a => a.DateAdded.Year == yearNow).ToList();
            var pastEnries = entries.Where(a => a.DateAdded.Year < yearNow).ToList();

            decimal totalPastDebit = 0;
            decimal totalPastCredit = 0;
            decimal totalCurrentDebit = 0;
            decimal totalCurrentCredit = 0;

            Workbook workbook = new Workbook();
            Worksheet sheet = workbook.Worksheets[0];
            Cells cells = workbook.Worksheets[0].Cells;
            int col = 0;
            int row = 0;

            Cell cellToStyle;
            Style styleCenterText;


            row = 4;
            cells.Merge(row, 6, 1, 2);
            cells.Merge(row, 4, 1, 2);
            cells.Merge(row, 2, 1, 2);
            cells[row, 2].PutValue("Beginning Balance");
            cellToStyle = cells[row, 2];
            styleCenterText = cellToStyle.GetStyle();
            styleCenterText.HorizontalAlignment = TextAlignmentType.Center;
            cellToStyle.SetStyle(styleCenterText);
            cells[row, 4].PutValue("Current Year");
            cellToStyle = cells[row, 4];
            styleCenterText = cellToStyle.GetStyle();
            styleCenterText.HorizontalAlignment = TextAlignmentType.Center;
            cellToStyle.SetStyle(styleCenterText);
            cells[row, 6].PutValue("Trial Balance");
            cellToStyle = cells[row, 6];
            styleCenterText = cellToStyle.GetStyle();
            styleCenterText.HorizontalAlignment = TextAlignmentType.Center;
            cellToStyle.SetStyle(styleCenterText);
            row = 5;
            cells[row, 2].PutValue("DR");
            cells[row, 3].PutValue("CR");
            cells[row, 4].PutValue("DR");
            cells[row, 5].PutValue("CR");
            cells[row, 6].PutValue("DR");
            cells[row, 7].PutValue("CR");
            row++;

            col = 0;
            foreach (var entryType in tbEntryTypes)
            {
                cells[row, 0].PutValue(entryType.Code);
                cells[row, 1].PutValue(entryType.Type);
                var pEntry = pastEnries.Where(a => a.AccountEntryTypeID == entryType.ID).ToList();
                var cEntry = currentEntries.Where(a => a.AccountEntryTypeID == entryType.ID).ToList();

                cells[row, 2].PutValue(pEntry.Sum(a => a.Debit));
                cells[row, 3].PutValue(pEntry.Sum(a => a.Credit));
                cells[row, 4].PutValue(cEntry.Sum(a => a.Debit));
                cells[row, 5].PutValue(cEntry.Sum(a => a.Credit));
                cells[row, 6].PutValue(pEntry.Sum(a => a.Debit) + cEntry.Sum(a => a.Debit));
                cells[row, 7].PutValue(pEntry.Sum(a => a.Credit) + cEntry.Sum(a => a.Credit));

                totalPastDebit += pEntry.Sum(a => a.Debit);
                totalPastCredit += pEntry.Sum(a => a.Credit);
                totalCurrentDebit += cEntry.Sum(a => a.Debit);
                totalCurrentCredit += cEntry.Sum(a => a.Credit);
                row++;

                //child entries
                foreach (var childEntryType in chartOfAccounts.Where(a => a.ParentID == entryType.ID).ToList())
                {
                    cells[row, 0].PutValue(childEntryType.Code);
                    cells[row, 1].PutValue("   " + childEntryType.Type);
                    var _pEntry = pastEnries.Where(a => a.AccountEntryTypeID == childEntryType.ID).ToList();
                    var _cEntry = currentEntries.Where(a => a.AccountEntryTypeID == childEntryType.ID).ToList();

                    cells[row, 2].PutValue(_pEntry.Sum(a => a.Debit));
                    cells[row, 3].PutValue(_pEntry.Sum(a => a.Credit));
                    cells[row, 4].PutValue(_cEntry.Sum(a => a.Debit));
                    cells[row, 5].PutValue(_cEntry.Sum(a => a.Credit));
                    cells[row, 6].PutValue(_pEntry.Sum(a => a.Debit) + _cEntry.Sum(a => a.Debit));
                    cells[row, 7].PutValue(_pEntry.Sum(a => a.Credit) + _cEntry.Sum(a => a.Credit));

                    totalPastDebit += _pEntry.Sum(a => a.Debit);
                    totalPastCredit += _pEntry.Sum(a => a.Credit);
                    totalCurrentDebit += _cEntry.Sum(a => a.Debit);
                    totalCurrentCredit += _cEntry.Sum(a => a.Credit);
                    row++;
                }
            }
            row++;
            cells[row, 0].PutValue("Total");
            cells[row, 2].PutValue(Utility.To2Dec(totalPastDebit));
            cells[row, 3].PutValue(Utility.To2Dec(totalPastCredit));
            cells[row, 4].PutValue(Utility.To2Dec(totalCurrentDebit));
            cells[row, 5].PutValue(Utility.To2Dec(totalCurrentCredit));
            cells[row, 6].PutValue(Utility.To2Dec(totalPastDebit + totalCurrentDebit));
            cells[row, 7].PutValue(Utility.To2Dec(totalPastCredit + totalCurrentCredit));
            row++;
            row++;

            cells[row, col].PutValue("");

            sheet.AutoFitColumns();

            row = 0;
            col = 0;
            cells[row++, col].PutValue(Constants.CoName().ToUpper());
            cells[row++, col].PutValue("TRIAL BALANCE");
            string dateRange = "FOR THE YEAR OF " + yearNow.ToString();
            cells[row++, col].PutValue(dateRange);
            cells[row++, col].PutValue("");

            sheet.Cells.SetColumnWidth(2, 13);
            sheet.Cells.SetColumnWidth(3, 13);
            sheet.Cells.SetColumnWidth(4, 13);
            sheet.Cells.SetColumnWidth(5, 13);
            sheet.Cells.SetColumnWidth(6, 13);
            sheet.Cells.SetColumnWidth(7, 13);

            return workbook;
        }

        public static Workbook GenerateBalanceSheetReport(string dateFrom, string dateTo)
        {
            var _dateFrom = Convert.ToDateTime(dateFrom);
            var _dateTo = Convert.ToDateTime(dateTo).AddHours(23).AddMinutes(59);

            var voucherEntries = LVoucherEntry.GetEntries(_dateFrom, _dateTo);
            var receivables = LSalesInvoice.GetSalesInvoices(_dateFrom, _dateTo, true);
            var assets = Web.Core.Constants.CodeAssets();
            var liabilities = Web.Core.Constants.CodeLiabilities();
            var chartOfAccounts = LAccountEntryType.GetTypes();
            var accountEntries = LAccountEntry.GetEntries();
            var invoiceEntries = LSalesInvoiceEntry.GetEntries(_dateFrom, _dateTo);

            decimal totalAssets = 0;
            decimal totalLiabilities = 0;

            Workbook workbook = new Workbook();
            Worksheet sheet = workbook.Worksheets[0];
            Cells cells = workbook.Worksheets[0].Cells;
            int col = 0;
            int row = 0;

            row = 4;
            cells.Merge(4, 0, 1, 4);
            cells[row, 0].PutValue("ASSETS");
            row++;

            if (assets.Count() > 0)
            {
                foreach(var asset in assets)
                {
                    var type = chartOfAccounts.FirstOrDefault(a => a.Code.ToLower().Trim() == asset);
                    if (type != null)
                    {
                        
                        decimal amount = 0;
                        cells[row, 0].PutValue(type.Code);
                        cells[row, 1].PutValue(type.Type);
                        cells.Merge(row, 1, 1, 2);
                        row++;

                        var subAssets = chartOfAccounts.Where(a => a.ParentID == type.ID).ToList();

                        if (subAssets.Count > 0)
                        {
                            foreach (var subAsset in subAssets)
                            {
                                cells[row, 1].PutValue(subAsset.Code + " - " + subAsset.Type);
                                cells.Merge(row, 1, 1, 3);
                                row++;

                                var accountEntriesPerType = accountEntries.Where(a => a.AcctEntryType.ID == subAsset.ID).ToList();

                                foreach (var accountEntryPerType in accountEntriesPerType)
                                {
                                    var voucherEntries_ = voucherEntries.Where(a => a.AccountEntryID == accountEntryPerType.ID);
                                    var debits = voucherEntries_.Sum(a => a.Debit);
                                    var credits = voucherEntries_.Sum(a => a.Credit);
                                    amount = amount + (debits - credits);
                                    var invoiceEntries_ = invoiceEntries.Where(a => a.AccountEntryID == accountEntryPerType.ID);
                                    var invoiceAmounts = invoiceEntries_.Sum(a => a.Amount);
                                    amount = amount + invoiceAmounts;
                                    totalAssets = totalAssets + amount;

                                    cells[row, 1].PutValue(accountEntryPerType.Code);
                                    cells[row, 2].PutValue(voucherEntries_.FirstOrDefault().AccountEntryName);
                                    cells[row, 3].PutValue(accountEntryPerType.TypeName);
                                    cells[row, 4].PutValue(Utility.To2Dec(amount));
                                    row++;
                                }
                                row++;
                            }
                            row++;
                        }
                        else
                        {
                            var accountEntriesPerType = accountEntries.Where(a => a.AcctEntryType.ID == type.ID).ToList();

                            foreach (var accountEntryPerType in accountEntriesPerType)
                            {
                                var voucherEntries_ = voucherEntries.Where(a => a.AccountEntryID == accountEntryPerType.ID);
                                if (voucherEntries_.Count() > 0)
                                {
                                    var debits = voucherEntries_.Sum(a => a.Debit);
                                    var credits = voucherEntries_.Sum(a => a.Credit);
                                    amount = amount + (debits - credits);
                                    var invoiceEntries_ = invoiceEntries.Where(a => a.AccountEntryID == accountEntryPerType.ID);
                                    var invoiceAmounts = invoiceEntries_.Sum(a => a.Amount);
                                    amount = amount + invoiceAmounts;
                                    totalAssets = totalAssets + amount;

                                    cells[row, 1].PutValue(accountEntryPerType.Code);
                                    cells[row, 2].PutValue(voucherEntries_.FirstOrDefault().AccountEntryName);
                                    cells[row, 3].PutValue(accountEntryPerType.TypeName);
                                    cells[row, 4].PutValue(Utility.To2Dec(amount));
                                }
                                row++;
                            }
                            row++;
                        }

                        //cells[row, 0].PutValue(type.Code);
                        //cells[row, 1].PutValue(type.Type);
                        //cells[row, 3].PutValue(Utility.To2Dec(amount));
                        //row++;
                        //totalAssets = totalAssets + amount;
                    }
                }
            }
            cells[row, 0].PutValue("Total Assets:");
            cells.Merge(row, 0, 1, 3);
            cells[row, 4].PutValue(Utility.To2Dec(totalAssets));
            row++;
            row++; 
            row++;

            cells.Merge(row, 0, 1, 4);
            cells[row, 0].PutValue("LIABILITIES AND EQUITY");
            row++;

            //if (liabilities.Count() > 0)
            //{
            //    foreach (var liability in liabilities)
            //    {
            //        var type = chartOfAccounts.FirstOrDefault(a => a.Code.ToLower().Trim() == liability);
            //        if (type != null)
            //        {
            //            var accountEntriesPerType = accountEntries.Where(a => a.AcctEntryType.ID == type.ID || a.AcctEntryType.ParentID == type.ID).ToList();
            //            decimal amount = 0;
            //            foreach (var accountEntryPerType in accountEntriesPerType)
            //            {
            //                var voucherEntries_ = voucherEntries.Where(a => a.AccountEntryID == accountEntryPerType.ID);
            //                var debits = voucherEntries_.Sum(a => a.Debit);
            //                var credits = voucherEntries_.Sum(a => a.Credit);
            //                amount = amount + (debits - credits);
            //                var invoiceEntries_ = invoiceEntries.Where(a => a.AccountEntryID == accountEntryPerType.ID);
            //                var invoiceAmounts = invoiceEntries_.Sum(a => a.Amount);
            //                amount = amount + invoiceAmounts;
            //            }
            //            cells[row, 0].PutValue(type.Code);
            //            cells[row, 1].PutValue(type.Type);
            //            cells[row, 3].PutValue(Utility.To2Dec(amount));
            //            row++;
            //            totalLiabilities = totalLiabilities + amount;
            //        }
            //    }
            //}

            if (liabilities.Count() > 0)
            {
                foreach (var liability in liabilities)
                {
                    var type = chartOfAccounts.FirstOrDefault(a => a.Code.ToLower().Trim() == liability);
                    if (type != null)
                    {
                        decimal amount = 0;
                        cells[row, 0].PutValue(type.Code);
                        cells[row, 1].PutValue(type.Type);
                        cells.Merge(row, 1, 1, 2);
                        row++;

                        var accountEntriesPerType = accountEntries.Where(a => a.AcctEntryType.ID == type.ID).ToList();

                        foreach (var accountEntryPerType in accountEntriesPerType)
                        {
                            var voucherEntries_ = voucherEntries.Where(a => a.AccountEntryID == accountEntryPerType.ID);
                            var debits = voucherEntries_.Sum(a => a.Debit);
                            var credits = voucherEntries_.Sum(a => a.Credit);
                            amount = amount + (debits - credits);
                            var invoiceEntries_ = invoiceEntries.Where(a => a.AccountEntryID == accountEntryPerType.ID);
                            var invoiceAmounts = invoiceEntries_.Sum(a => a.Amount);
                            amount = amount + invoiceAmounts;
                            totalLiabilities = totalLiabilities + amount;

                            cells[row, 1].PutValue(accountEntryPerType.Code);
                            cells[row, 2].PutValue(voucherEntries_.FirstOrDefault().AccountEntryName);
                            cells[row, 3].PutValue(accountEntryPerType.TypeName);
                            cells[row, 4].PutValue(Utility.To2Dec(amount));
                            row++;
                        }
                        row++;
                    }
                }
            }



            row++;

            cells[row, 0].PutValue("Total Liabilities and Equity:");
            cells.Merge(row, 0, 1, 3);
            cells[row, 4].PutValue(Utility.To2Dec(totalLiabilities));
            row++;

            cells[row, col].PutValue("");

            sheet.AutoFitColumns();

            row = 0;
            col = 0;
            cells[row++, col].PutValue(Constants.CoName().ToUpper());
            cells[row++, col].PutValue("BALANCE SHEET");
            string dateRange = _dateFrom.Month == _dateTo.Month ?
                "FOR THE MONTH OF " + _dateFrom.ToString("MMMM").ToUpper() + ", " + _dateFrom.Year.ToString() :
                "FROM " + dateFrom + " TO " + dateTo;
            cells[row++, col].PutValue(dateRange);
            cells[row++, col].PutValue("");

            return workbook;
        }

        public static Workbook GeneratePostDatedCheckReport()
        {
            var payables = LVoucher.GetPostDatedChecks();
            decimal totalCheckAmount = 0;

            Workbook workbook = new Workbook();
            Worksheet sheet = workbook.Worksheets[0];
            Cells cells = workbook.Worksheets[0].Cells;
            int col = 0;
            int row = 0;

            row = 4;
            cells[row, 0].PutValue("Date Created");
            cells[row, 1].PutValue("Payee");
            cells[row, 2].PutValue("Date of Check");
            cells[row, 3].PutValue("Check No.");
            cells[row, 4].PutValue("Amount of Check");
            row++;

            foreach(var p in payables.OrderBy(a => a.DateAdded))
            {
                cells[row, 0].PutValue(p.DateAdded.ToShortDateString());
                cells[row, 1].PutValue(p.PayeeName);
                cells[row, 2].PutValue(p.CheckDate != null ? ((DateTime)p.CheckDate).ToShortDateString() : "");
                cells[row, 3].PutValue(p.CheckNo);
                cells[row, 4].PutValue(Utility.To2Dec(p.CheckAmount));
                totalCheckAmount = totalCheckAmount + (p.CheckAmount != null ? (decimal)p.CheckAmount : 0);
                row++;
            }
            row++;
            cells[row, 0].PutValue("Total Amount");
            cells[row, 4].PutValue(Utility.To2Dec(totalCheckAmount));
            row++;

            cells[row, col].PutValue("");

            sheet.AutoFitColumns();

            row = 0;
            col = 0;
            cells[row++, col].PutValue(Constants.CoName().ToUpper());
            cells[row++, col].PutValue("POST DATED CHECK");
            string dateRange = "AS OF " + DateTime.Now.ToShortDateString();
            cells[row++, col].PutValue(dateRange);
            cells[row++, col].PutValue("");

            return workbook;
        }

        public static Workbook GenerateInputTaxReport(string dateFrom, string dateTo)
        {
            var _dateFrom = Convert.ToDateTime(dateFrom);
            var _dateTo = Convert.ToDateTime(dateTo).AddHours(23).AddMinutes(59);

            var code = Constants.CodeInputTax();
            if (code == null)
                return null;

            var entries = LVoucherEntry.GetEntries(_dateFrom, _dateTo);
            var inputTaxes = entries.Where(a => a.AccountEntryCode == code).ToList();

            decimal totalTaxAmount = 0;

            Workbook workbook = new Workbook();
            Worksheet sheet = workbook.Worksheets[0];
            Cells cells = workbook.Worksheets[0].Cells;
            int col = 0;
            int row = 0;

            row = 4;
            cells[row, 0].PutValue("Date Created");
            cells[row, 1].PutValue("Payee");
            cells[row, 2].PutValue("Date of Check");
            cells[row, 3].PutValue("Check No.");
            cells[row, 4].PutValue("Tax Amount");
            row++;

            foreach (var e in inputTaxes.OrderBy(a => a.DateAdded))
            {
                var amount = e.Debit - e.Credit;
                cells[row, 0].PutValue(e.DateAdded.ToShortDateString());
                cells[row, 1].PutValue(e.PayeeName);
                cells[row, 2].PutValue(e.CheckDate != null ? ((DateTime)e.CheckDate).ToShortDateString() : "");
                cells[row, 3].PutValue(e.CheckNo);
                cells[row, 4].PutValue(Utility.To2Dec(amount));
                totalTaxAmount = totalTaxAmount + (amount);
                row++;
            }
            row++;
            cells[row, 0].PutValue("Total Amount");
            cells[row, 4].PutValue(Utility.To2Dec(totalTaxAmount));
            row++;

            cells[row, col].PutValue("");

            sheet.AutoFitColumns();

            row = 0;
            col = 0;
            cells[row++, col].PutValue(Constants.CoName().ToUpper());
            cells[row++, col].PutValue("INPUT TAX REPORT");
            string dateRange = _dateFrom.Month == _dateTo.Month ?
                "FOR THE MONTH OF " + _dateFrom.ToString("MMMM").ToUpper() + ", " + _dateFrom.Year.ToString() :
                "FROM " + dateFrom + " TO " + dateTo;
            cells[row++, col].PutValue(dateRange);
            cells[row++, col].PutValue("");

            return workbook;
        }

        public static Workbook GenerateExpandedTaxReport(string dateFrom, string dateTo)
        {
            var _dateFrom = Convert.ToDateTime(dateFrom);
            var _dateTo = Convert.ToDateTime(dateTo).AddHours(23).AddMinutes(59);

            var code = Constants.CodeExpandedTax();
            if (code == null)
                return null;

            var entries = LVoucherEntry.GetEntries(_dateFrom, _dateTo);
            var inputTaxes = entries.Where(a => a.AccountEntryCode == code).ToList();

            decimal totalTaxAmount = 0;

            Workbook workbook = new Workbook();
            Worksheet sheet = workbook.Worksheets[0];
            Cells cells = workbook.Worksheets[0].Cells;
            int col = 0;
            int row = 0;

            row = 4;
            cells[row, 0].PutValue("Date Created");
            cells[row, 1].PutValue("Payee");
            cells[row, 2].PutValue("Date of Check");
            cells[row, 3].PutValue("Check No.");
            cells[row, 4].PutValue("Tax Amount");
            row++;

            foreach (var e in inputTaxes.OrderBy(a => a.DateAdded))
            {
                cells[row, 0].PutValue(e.DateAdded.ToShortDateString());
                cells[row, 1].PutValue(e.PayeeName);
                cells[row, 2].PutValue(e.CheckDate != null ? ((DateTime)e.CheckDate).ToShortDateString() : "");
                cells[row, 3].PutValue(e.CheckNo);
                cells[row, 4].PutValue(Utility.To2Dec(e.Credit));
                totalTaxAmount = totalTaxAmount + (e.Credit);
                row++;
            }
            row++;
            cells[row, 0].PutValue("Total Amount");
            cells[row, 4].PutValue(Utility.To2Dec(totalTaxAmount));
            row++;

            cells[row, col].PutValue("");

            sheet.AutoFitColumns();

            row = 0;
            col = 0;
            cells[row++, col].PutValue(Constants.CoName().ToUpper());
            cells[row++, col].PutValue("EXPANDED REPORT");
            string dateRange = _dateFrom.Month == _dateTo.Month ?
                "FOR THE MONTH OF " + _dateFrom.ToString("MMMM").ToUpper() + ", " + _dateFrom.Year.ToString() :
                "FROM " + dateFrom + " TO " + dateTo;
            cells[row++, col].PutValue(dateRange);
            cells[row++, col].PutValue("");

            return workbook;
        }

        public static Workbook GenerateIncomeStatementReport(string dateFrom, string dateTo)
        {
            var _dateFrom = Convert.ToDateTime(dateFrom);
            var _dateTo = Convert.ToDateTime(dateTo).AddHours(23).AddMinutes(59);

            var voucherEntries = LVoucherEntry.GetEntries(_dateFrom, _dateTo);
            var receivables = LSalesInvoice.GetSalesInvoices(_dateFrom, _dateTo, true);
            var income = Web.Core.Constants.CodeIncome();
            var costOfSales = Web.Core.Constants.CodeCostOfSales();
            var expenses = Web.Core.Constants.CodeExpenses();
            var chartOfAccounts = LAccountEntryType.GetTypes();
            var accountEntries = LAccountEntry.GetEntries();
            var invoiceEntries = LSalesInvoiceEntry.GetEntries(_dateFrom, _dateTo);

            Workbook workbook = new Workbook();
            Worksheet sheet = workbook.Worksheets[0];
            Cells cells = workbook.Worksheets[0].Cells;
            int col = 0;
            int row = 0;

            row = 4;

            decimal totalIncome = 0;
            decimal totalCostOfSales = 0;
            decimal totalExpenses = 0;

            if (income.Count() > 0)
            {
                foreach (var i in income)
                {
                    var type = chartOfAccounts.FirstOrDefault(a => a.Code.ToLower().Trim() == i);
                    if (type != null)
                    {
                        var accountEntriesPerType = accountEntries.Where(a => a.AcctEntryType.ID == type.ID || a.AcctEntryType.ParentID == type.ID).ToList();
                        decimal amount = 0;
                        foreach (var accountEntryPerType in accountEntriesPerType)
                        {
                            var voucherEntries_ = voucherEntries.Where(a => a.AccountEntryID == accountEntryPerType.ID);
                            var debits = voucherEntries_.Sum(a => a.Debit);
                            var credits = voucherEntries_.Sum(a => a.Credit);
                            amount = amount + (debits - credits);
                            var invoiceEntries_ = invoiceEntries.Where(a => a.AccountEntryID == accountEntryPerType.ID);
                            var invoiceAmounts = invoiceEntries_.Sum(a => a.Amount);
                            amount = amount + invoiceAmounts;
                        }
                        cells[row, 0].PutValue(type.Code);
                        cells[row, 1].PutValue(type.Type);
                        cells[row, 3].PutValue(Utility.To2Dec(amount));
                        row++;
                        totalIncome = totalIncome + amount;
                    }
                }

            }
            row++;

            if (costOfSales.Count() > 0)
            {
                foreach (var i in costOfSales)
                {
                    var type = chartOfAccounts.FirstOrDefault(a => a.Code.ToLower().Trim() == i);
                    if (type != null)
                    {
                        var accountEntriesPerType = accountEntries.Where(a => a.AcctEntryType.ID == type.ID || a.AcctEntryType.ParentID == type.ID).ToList();
                        decimal amount = 0;
                        foreach (var accountEntryPerType in accountEntriesPerType)
                        {
                            var voucherEntries_ = voucherEntries.Where(a => a.AccountEntryID == accountEntryPerType.ID);
                            var debits = voucherEntries_.Sum(a => a.Debit);
                            var credits = voucherEntries_.Sum(a => a.Credit);
                            amount = amount + (debits - credits);
                            var invoiceEntries_ = invoiceEntries.Where(a => a.AccountEntryID == accountEntryPerType.ID);
                            var invoiceAmounts = invoiceEntries_.Sum(a => a.Amount);
                            amount = amount + invoiceAmounts;
                        }
                        cells[row, 0].PutValue(type.Code);
                        cells[row, 1].PutValue(type.Type);
                        cells[row, 3].PutValue(Utility.To2Dec(amount));
                        row++;
                        totalCostOfSales = totalCostOfSales + amount;
                    }
                }

            }
            row++;

            cells.Merge(row, 0, 1, 3);
            cells[row, 0].PutValue("Gross Margin");
            cells[row, 3].PutValue(Utility.To2Dec(totalIncome - totalCostOfSales));
            row++;
            row++;

            if (expenses.Count() > 0)
            {
                foreach (var i in expenses)
                {
                    var type = chartOfAccounts.FirstOrDefault(a => a.Code.ToLower().Trim() == i);
                    if (type != null)
                    {
                        cells[row, 0].PutValue(i);
                        cells[row, 1].PutValue(type.Type);
                        row++;

                        var children = chartOfAccounts.Where(a => a.ParentID == type.ID).ToList();
                        foreach (var child in children)
                        {
                            var accountEntriesPerType = accountEntries.Where(a => a.AcctEntryType.ID == child.ID).ToList();
                            decimal amount = 0;
                            foreach (var accountEntryPerType in accountEntriesPerType)
                            {
                                var voucherEntries_ = voucherEntries.Where(a => a.AccountEntryID == accountEntryPerType.ID);
                                var debits = voucherEntries_.Sum(a => a.Debit);
                                var credits = voucherEntries_.Sum(a => a.Credit);
                                amount = amount + (debits - credits);
                                var invoiceEntries_ = invoiceEntries.Where(a => a.AccountEntryID == accountEntryPerType.ID);
                                var invoiceAmounts = invoiceEntries_.Sum(a => a.Amount);
                                amount = amount + invoiceAmounts;
                            }
                            cells[row, 1].PutValue(child.Type);
                            cells[row, 2].PutValue(Utility.To2Dec(amount));
                            row++;
                            totalExpenses = totalExpenses + amount;
                        }
                    }
                }
            }
            row++;
            cells.Merge(row, 0, 1, 3);
            cells[row, 0].PutValue("Total Expenses");
            cells[row, 3].PutValue(Utility.To2Dec(totalExpenses));
            row++;
            row++;

            cells.Merge(row, 0, 1, 3);
            cells[row, 0].PutValue("Net Income / (loss) from operation");
            cells[row, 3].PutValue(Utility.To2Dec(totalIncome - totalCostOfSales - totalExpenses));
            row++;


            cells[row, col].PutValue("");

            sheet.AutoFitColumns();

            row = 0;
            col = 0;
            cells[row++, col].PutValue(Constants.CoName().ToUpper());
            cells[row++, col].PutValue("INCOME STATEMENT");
            string dateRange = _dateFrom.Month == _dateTo.Month ?
                "FOR THE MONTH OF " + _dateFrom.ToString("MMMM").ToUpper() + ", " + _dateFrom.Year.ToString() :
                "FROM " + dateFrom + " TO " + dateTo;
            cells[row++, col].PutValue(dateRange);
            cells[row++, col].PutValue("");

            return workbook;
        }

    }
}