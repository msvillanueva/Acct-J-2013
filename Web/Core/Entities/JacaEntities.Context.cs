﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Web.Core.Entities
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class JacaEntities : DbContext
    {
        public JacaEntities()
            : base("name=JacaEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<AccountEntry> AccountEntries { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<SystemLog> SystemLogs { get; set; }
        public DbSet<ActionRoute> ActionRoutes { get; set; }
        public DbSet<Payee> Payees { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<VoucherEntry> VoucherEntries { get; set; }
        public DbSet<SalesInvoice> SalesInvoices { get; set; }
        public DbSet<SalesInvoiceEntry> SalesInvoiceEntries { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<ReportPermission> ReportPermissions { get; set; }
        public DbSet<AccountEntryType> AccountEntryTypes { get; set; }
    }
}