using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Core.DataAccess
{
    public class Connection
    {
        public static Entities.JacaEntities GetEntityContext()
        {
            return new Entities.JacaEntities();
        }
    }
}