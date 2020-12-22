using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Configuration;
using System.Text.RegularExpressions;
using System.IO;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Cfg;
using Foods;
using System.Globalization;
using DataAccess;


namespace Foods
{
    public partial class rpt_Purchase : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["D"].ConnectionString);
        string Id,PurId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var check = Session["user"];
                if (check != null)
                {
                    Id = Request.QueryString["PR"];
                    PurId = Request.QueryString["PURID"];

                    FillGrid();
                }
                else
                {
                    Response.Redirect("~/Login.aspx");
                }
            }

        }

        public void FillGrid()
        {
            try
            {
                if (Id == null)
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = " SELECT ROW_NUMBER() OVER(ORDER BY (select 1)) AS ID,* from v_rptpur where MPurID='" + PurId + "' and CompanyId = '" + Session["CompanyID"] + "' and BranchId= '" + Session["BranchID"] + "'";

                        cmd.Connection = con;
                        con.Open();

                        DataTable dt_ = new DataTable();
                        SqlDataAdapter adp = new SqlDataAdapter(cmd);
                        adp.Fill(dt_);

                        if (dt_.Rows.Count > 0)
                        {

                            TBDCNo.Text = dt_.Rows[0]["DCNO"].ToString();
                            TBDatTim.Text = dt_.Rows[0]["DatTim"].ToString();
                            TBBilNo.Text = dt_.Rows[0]["BiltyNo"].ToString();
                            TBVehlNo.Text = dt_.Rows[0]["VehicalNo"].ToString();
                            TbDriNam.Text = dt_.Rows[0]["DriverNam"].ToString();
                            TBDrMobNo.Text = dt_.Rows[0]["DriverMobilno"].ToString();
                            TBTrans.Text = dt_.Rows[0]["Transporter"].ToString();
                            TBSTion.Text = dt_.Rows[0]["station"].ToString();
                            TBDO.Text = dt_.Rows[0]["DilverOrdr"].ToString();
                            TBFright.Text = dt_.Rows[0]["frieght"].ToString();
                            LBLShpTo.Text = dt_.Rows[0]["suppliername"].ToString();
                            lblTo.Text = dt_.Rows[0]["suppliername"].ToString();

                            GVShowpurItms.DataSource = dt_;
                            GVShowpurItms.DataBind();
                        }

                        con.Close();
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;               
            }
        }
    }
}