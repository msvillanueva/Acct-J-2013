using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Core.DataAccess;

namespace Web.Models
{
    public class CSystemLog
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string Description { get; set; }
        public string Action { get; set; }
        public string Table { get; set; }
        public DateTime Date { get; set; }

        public CUser User
        {
            get
            {
                if (this.User != null)
                {
                    return LUser.GetUser(this.UserID);
                }
                return null;
            }
        }
    }
}