using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de MessageBox
/// </summary>
public static class MessageBox
{
    public static void Show(System.Web.UI.Page Page, String Message)
    {
        Page.ClientScript.RegisterStartupScript(
        Page.GetType(),
        "MessageBox",
        "<script language='javascript'>alert('" + Message + "');</script>"
     );
	}
}