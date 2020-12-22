using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Configuration;
using System.Text.RegularExpressions;
using System.IO;

using NHibernate;
using NHibernate.Criterion;
using Foods;
using DataAccess;

namespace Foods
{
    public partial class rpt_BillSumm : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["D"].ConnectionString);
        DataTable dt_;
        DBConnection db = new DBConnection();
        string SalmanID;

        protected void Page_Load(object sender, EventArgs e)
        {
            var check = Session["user"];
            if (check != null)
            {
                SalmanID = Request.QueryString["MDNID"];
                FillGrid();
            }
            else
            {
                Response.Redirect("~/Login.aspx");
            }
        }

        public void FillGrid()
        {
            try
            {
                dt_ = new DataTable();
                dt_ = DBConnection.GetQueryData(" SELECT ROW_NUMBER() OVER(ORDER BY (select 1)) AS ID, * from v_billsumm where CreatedBy ='haris' and MSal_dat='2019-04-09'");

                if (dt_.Rows.Count > 0)
                {

                    Repeater1.DataSource = dt_;
                    Repeater1.DataBind();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                GridView gv = (GridView)e.Item.FindControl("GVCashMemo");
                Label lblmsalno = (Label)e.Item.FindControl("lblmsalno");
                Label lblttlqty = (Label)e.Item.FindControl("lblttlqty");
                Label lblamount = (Label)e.Item.FindControl("lblamount");
                Label lblsaldisc = (Label)e.Item.FindControl("lblsaldisc");

                if (gv != null)
                {
                    dt_ = new DataTable();
                    dt_ = DBConnection.GetQueryData(" SELECT ROW_NUMBER() OVER(ORDER BY (select 1)) AS ID,* from v_billsumm where CreatedBy ='haris' and MSal_dat='2019-04-09' and MSal_sono='" + lblmsalno.Text.Trim() + "'");

                    if (dt_.Rows.Count > 0)
                    {

                        DataRowView drv = (DataRowView)e.Item.DataItem;
                        gv.DataSource = dt_; //(Convert.ToInt32(drv["ID"]));
                        gv.DataBind();
                    }

                    float Gttlqty= 0;
                    float Gttl = 0;
                    float saldisc = 0;
                    for (int k = 0; k < gv.Rows.Count; k++)
                    {
                        Label lblqty = (Label)gv.Rows[k].FindControl("lblqty");
                        Label lblsalcst = (Label)gv.Rows[k].FindControl("lblsalcst");
                        Label lbldiscamt = (Label)gv.Rows[k].FindControl("lbldiscamt");
                        Label lblttl = (Label)gv.Rows[k].FindControl("lblttl");
                        

                        Gttlqty += Convert.ToSingle(lblqty.Text);
                        Gttl += Convert.ToSingle(lblttl.Text);
                        saldisc += Convert.ToSingle(lbldiscamt.Text);

                    }

                    lblttlqty.Text = Gttlqty.ToString();
                    lblamount.Text = Gttl.ToString();
                    lblsaldisc.Text = saldisc.ToString();
                }
            }
        }
        private decimal Total = (decimal)0.0;

        protected void GVCashMemo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Total += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Qty"));
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                //e.Row.Cells[1].Text = String.Format("{0:0}", "<b>" + Total + "</b>");
            }
        }
    }
}