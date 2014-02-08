using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Core;

namespace Web.Models
{
    public class CUser
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Middlename { get; set; }
        public string Email { get; set; }
        public int Role { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? DateLastLogin { get; set; }
        public bool IsActive { get; set; }

        public string RoleName
        {
            get
            {
                return this.Role != 0 ? ((Enumerations.Role)this.Role).ToString() : null;
            }
        }

        public string Status
        {
            get
            {
                return this.IsActive ? "Active" : "Inactive";
            }
            set
            {
                this.IsActive = value == "Active" ? true : false;
            }
        }
    }
}