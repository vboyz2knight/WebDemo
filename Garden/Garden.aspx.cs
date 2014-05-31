using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Garden : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string myXMLfile = Server.MapPath("Garden.xml");
        DataSet ds = new DataSet();
        // Create new FileStream with which to read the schema.
        System.IO.FileStream fsReadXml = new System.IO.FileStream
            (myXMLfile, System.IO.FileMode.Open);
        try
        {
            ds.ReadXml(fsReadXml);
            GardenListView1.DataSource = ds;
            //GardenListView1.DataMember = "vegetable";
        }
        catch (Exception ex)
        {
            //error
        }
        finally
        {
            fsReadXml.Close();
        }
    }
}