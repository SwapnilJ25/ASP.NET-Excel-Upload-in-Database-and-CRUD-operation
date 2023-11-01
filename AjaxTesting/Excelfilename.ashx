<%@ WebHandler Language="C#" Class="Excelfilename" %>

using System;
using System.Web;

public class Excelfilename : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        if (context.Request.Files.Count > 0)
        {
            HttpFileCollection files = context.Request.Files;
            for (int i = 0; i < files.Count; i++)
            {
                HttpPostedFile file = files[i];
                string fname = context.Server.MapPath("/uploaded_file/" + file.FileName);
                file.SaveAs(fname);
            }
            context.Response.ContentType = "text/plain";
            context.Response.Write("Successfully");
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}