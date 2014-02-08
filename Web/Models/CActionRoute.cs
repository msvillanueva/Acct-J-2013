using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class CActionRoute
    {
        public int ID { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Description { get; set; }
        public int? ParentId { get; set; }
        public int? Order { get; set; }
        public string Icon { get; set; }
        public bool IsActive { get; set; }
        public List<CActionRoute> Children { get; set; }
        public int ChildrenCount { get; set; }
    }
}