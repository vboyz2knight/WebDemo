using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;


public partial class sqlBasic : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        XDocument sqlBasic = XDocument.Load(Server.MapPath("SQLBasic.xml"));
        var sqls = from _sql in sqlBasic.Descendants("SQL")
                   select new
                   {
                       Command = _sql.Element("Command"),
                       Descriptions = _sql.Element("Descriptions"),
                       Examples = (from l in _sql.Descendants("example")
                                   select new
                                   {
                                       example = l.Value

                                   })
                   };
        ListView1.DataSource = sqls;
        ListView1.DataBind();
    }
}