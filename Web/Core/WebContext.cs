using System.Web;

public static class WebContext
{
    public static HttpFileCollection Files
    {
        get
        {
            return HttpContext.Current.Request.Files;
        }
    }

    public static string FilePath
    {
        get
        {
            return HttpContext.Current.Request.ServerVariables["APPL_PHYSICAL_PATH"];
        }
    }

    public static string RootUrl
    {
        get
        {
            var Port = HttpContext.Current.Request.ServerVariables["SERVER_PORT"];
            if (Port == null || Port == "80" || Port == "443")
                Port = "";
            else
                Port = ":" + Port;

            string Protocol = HttpContext.Current.Request.ServerVariables["SERVER_PORT_SECURE"];
            if (Protocol == null || Protocol == "0")
                Protocol = "http://";
            else
                Protocol = "https://";

            string result = Protocol + HttpContext.Current.Request.ServerVariables["SERVER_NAME"] +
                            Port + HttpContext.Current.Request.ApplicationPath;

            return result;
        }
    }

    public static void ClearSession()
    {
        HttpContext.Current.Session.Clear();
    }

    public static bool ContainsInSession(string key)
    {
        if (HttpContext.Current.Session != null && HttpContext.Current.Session[key] != null)
            return true;
        return false;
    }

    public static void RemoveFromSession(string key)
    {
        HttpContext.Current.Session.Remove(key);
    }

    public static bool ContainsInQueryString(string key)
    {
        if (HttpContext.Current.Request.QueryString[key] != null)
            return true;
        return false;
    }

    public static string GetQueryStringValue(string key)
    {
        return HttpContext.Current.Request.QueryString.Get(key);
    }

    public static bool ContainsInContextItems(string key)
    {
        if (HttpContext.Current.Items[key] != null)
            return true;
        return false;
    }

    public static string GetContextItemValue(string key)
    {
        return (string)HttpContext.Current.Items[key];
    }

    public static void SetInSession(string key, object value)
    {
        if (HttpContext.Current == null || HttpContext.Current.Session == null)
        {
            return;
        }
        HttpContext.Current.Session[key] = value;
    }

    public static object GetFromSession(string key)
    {
        if (HttpContext.Current == null || HttpContext.Current.Session == null)
        {
            return null;
        }
        return HttpContext.Current.Session[key];
    }
}
