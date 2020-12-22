using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace menu
{
    public partial class GV_DSR : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["D"].ConnectionString);
        string DSRID, EID;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    lblcom.Text = "Ham Brothers";

                    DSRID = Request.QueryString["DSRID"];
                    EID = Request.QueryString["EID"];


                    if (con.State != ConnectionState.Open)
                    {
                        con.Close();                        
                    }
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(" select Username,tbl_Mdsr.CustomerID,CustomerName,tbl_Mdsr.dsrid,dsrdat from tbl_Mdsr " +
                        " inner join Customers_ on tbl_Mdsr.CustomerID = Customers_.CustomerID " +
                        " where Username ='" + EID + "' and dsrdat='" + DSRID + "'", con);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "tbl_Mdsr");
                    Repeater1.DataSource = ds.Tables[0];
                    Repeater1.DataBind();
                    con.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                //if (con.State != ConnectionState.Open)
                //{
                //    con.Close();
                //}
                //con.Open();
                SqlDataAdapter da = new SqlDataAdapter(" select CustomerID,tbl_ddsr.productid,Productname,Qty,PckSize,Amt, " +
                    " cartons = (ltrim(cast(cast((Qty/PckSize)  as decimal(18,2)) AS int))) + '  Cartons', " +
                    " Items =(Qty - (ltrim(cast(cast((Qty/PckSize)  as decimal(18,1)) AS int)) * PckSize)), " +
                    " outstan,recvry,dsrrmk,ttlamt from tbl_Mdsr inner join tbl_ddsr on " +
                    " tbl_Mdsr.dsrid = tbl_ddsr.dsrid  inner join Products on " +
                    " tbl_ddsr.ProductID = Products.ProductID  where Username ='" + ((System.Data.DataRowView)(e.Item.DataItem)).Row[0] + "' and CustomerID='" + ((System.Data.DataRowView)(e.Item.DataItem)).Row[1] + "' and Isdon = 0 and dsrdat='" + DSRID + "' ", con);
                DataSet ds = new DataSet();
                da.Fill(ds, "tbl_Mdsr");
                GridView gd = (GridView)e.Item.FindControl("GridView1");
                gd.DataSource = ds.Tables[0];
                gd.DataBind();

               
                //con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            float GTotal = 0;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lbl_amt = (Label)e.Row.FindControl("lbl_amt");

                GTotal += Convert.ToSingle(lbl_amt.Text);

            }

            if (e.Row.RowType == DataControlRowType.Footer) 
            {   
                Label lbl_ttlamt = (Label)e.Row.FindControl("lbl_ttlamt");

                lbl_ttlamt.Text = GTotal.ToString();
            }
        }

    }
}