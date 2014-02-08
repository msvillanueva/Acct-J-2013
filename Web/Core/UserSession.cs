using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Core;
using Web.Models;



public static class UserSession
{
    private const string user = "User";
    private const string username = "Username";
    private const string userid = "0";
    private const string loggedIn = "LoggedIn";
    private const string role = "Role";
    private const string allowedControllers = "AllowedControllers";
    private const string allowedRoutes = "AllowedRoutes";

    public static CUser User
    {
        get
        {
            if (WebContext.ContainsInSession(user))
            {
                return WebContext.GetFromSession(user) as CUser;
            }
            return null;
        }
        set
        {
            WebContext.SetInSession(user, value);
        }
    }
    public static string Username
    {
        get
        {
            if (WebContext.ContainsInSession(username))
            {
                return WebContext.GetFromSession(username).ToString();
            }

            return "";
        }

        set
        {
            WebContext.SetInSession(username, value);
        }
    }
    public static int UserId
    {
        get
        {
            if (WebContext.ContainsInSession(userid))
            {
                return Convert.ToInt16(WebContext.GetFromSession(userid));
            }
            return 0;
        }

        set
        {
            WebContext.SetInSession(userid, value);
        }
    }
    public static bool LoggedIn
    {
        get
        {
            return WebContext.ContainsInSession(loggedIn) && (bool)WebContext.GetFromSession(loggedIn);
        }
        set
        {
            WebContext.SetInSession(loggedIn, value);
        }
    }

    public static bool IsLoggedIn
    {
        get
        {
            return WebContext.ContainsInSession(user) && User != null;
        }
    }

    public static Enumerations.Role Role
    {
        get
        {
            return (Enumerations.Role)WebContext.GetFromSession(role);
        }

        set
        {
            WebContext.SetInSession(role, value);
        }
    }

    public static List<string> AllowedControllers
    {
        get
        {
            return (List<string>)WebContext.GetFromSession(allowedControllers);
        }

        set
        {
            WebContext.SetInSession(allowedControllers, value);
        }
    }

    public static List<CActionRoute> AllowedRoutes
    {
        get
        {
            return (List<CActionRoute>)WebContext.GetFromSession(allowedRoutes);
        }

        set
        {
            WebContext.SetInSession(allowedRoutes, value);
        }
    }
}

