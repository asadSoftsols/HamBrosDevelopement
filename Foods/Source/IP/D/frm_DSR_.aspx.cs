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
using System.Globalization;
using System.IO;
using Foods;
using DataAccess;
using System.Web.Services;

namespace Foods

{
    public partial class frm_DSR_ : System.Web.UI.Page
    {
        SqlConnection con = DataAccess.DBConnection.connection();
        DataTable dt_ = null;
        SqlDataAdapter adp;
        int i = 0;
        int chkdetails = 0;
        string query, mdsrId, proid, DDL_Itm, DDL_Unt, TBQty, TBSalRat, TBAmt, TBRmk, lvl;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lvl = Session["Level"].ToString();
                FillGrid();
                SetInitRow();
                BindDLL();
                TBdsrdat.Text = DateTime.Now.ToShortDateString();
                btnUpd.Enabled = false;
                lblOutstan.Enabled = false;

                #region Validation

                lbl_ChkArea.Visible = false;
                lbl_Chktotl.Visible = false;
                lbl_ChkSalman.Visible = false;
                lbl_Chkcust.Visible = false;
                lbl_Chkcust.Visible = false;
                lbl_ChkProNam.Visible = false;
                lbl_Chksalrat.Visible = false;
                lbl_Chkqty.Visible = false;
                lbl_Chksalrat.Visible = false;
                lbl_ChkAmt.Visible = false;
                lbl_Chkunt.Visible = false;
               
                #endregion
            }
        }


        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> GetSearch(string prefixText)
        {
            SqlConnection con = DataAccess.DBConnection.connection();
            SqlDataAdapter da;  
            DataTable dt; 
            DataTable Result = new DataTable();
            //string str = "select ProductName from Products where ProductName like '" + prefixText + "%'";
            string str = "select distinct(ProductName) from tbl_dstk " +
                        " inner join products on tbl_dstk.productid = Products.ProductID where ProductName like '" + prefixText + "%'";

            da = new SqlDataAdapter(str, con);
            dt = new DataTable();
            da.Fill(dt);
            List<string> Output = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
                Output.Add(dt.Rows[i][0].ToString());
            return Output;
        }


        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> GetCust(string prefixText)
        {
            SqlConnection con = DataAccess.DBConnection.connection();
            SqlDataAdapter da;
            DataTable dt;
            DataTable Result = new DataTable();
            //string str = "select ProductName from Products where ProductName like '" + prefixText + "%'";
            string str = " select distinct(CustomerName) from tbl_mdsr " +
                " inner join Customers_ on tbl_Mdsr.CustomerID = Customers_.CustomerID where CustomerName like '" + prefixText + "%'";

            da = new SqlDataAdapter(str, con);
            dt = new DataTable();
            da.Fill(dt);
            List<string> Output = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
                Output.Add(dt.Rows[i][0].ToString());
            return Output;
        } 

        #region methods
        public void FillGrid()
        {
            try
            {
                //Sales Order
                using (SqlCommand cmd = new SqlCommand())
                {
                    lvl = Session["Level"].ToString();
                    if (lvl == "1")
                    {

                        cmd.CommandText = " select top 30 ROW_NUMBER() OVER(ORDER BY (select 1)) AS ID, tbl_Mdsr.dsrid, tbl_Mdsr.CustomerID,Isdon,CustomerName,convert(varchar, dsrdat, 111) as [dsrdat] from tbl_Mdsr inner join Customers_ on tbl_Mdsr.CustomerID = Customers_.cust_acc where tbl_Mdsr.CompanyId = '" + Session["CompanyID"] + "' and tbl_Mdsr.BranchId= '" + Session["BranchID"] + "' order by dsrid desc ";
                    }
                    else
                    {
                        cmd.CommandText = " select top 30 ROW_NUMBER() OVER(ORDER BY (select 1)) AS ID,tbl_Mdsr.dsrid, tbl_Mdsr.CustomerID,Isdon,CustomerName,convert(varchar, dsrdat, 111) as [dsrdat] from tbl_Mdsr inner join Customers_ on tbl_Mdsr.CustomerID = Customers_.cust_acc where tbl_Mdsr.CompanyId = '" + Session["CompanyID"] + "' and tbl_Mdsr.BranchId= '" + Session["BranchID"] + "' and tbl_Mdsr.CreateBy='" + Session["Username"] + "' or Salesman='" + Session["Username"] + "'  order by dsrid desc ";                         
                    }

                    cmd.Connection = con;
                    con.Open();

                    DataTable dtSal_ = new DataTable();

                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dtSal_);

                    GVDSR.DataSource = dtSal_;
                    GVDSR.DataBind();
                    ViewState["DSR"] = dtSal_;

                    con.Close();
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }          
        }

        private void SetInitRow()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            //dt.Columns.Add(new DataColumn("ITEMCODE", typeof(string)));
            dt.Columns.Add(new DataColumn("ITEMNAME", typeof(string)));
            dt.Columns.Add(new DataColumn("ITEM TYPE", typeof(string)));
            dt.Columns.Add(new DataColumn("UNITS", typeof(string)));
            dt.Columns.Add(new DataColumn("QTY", typeof(string)));
            dt.Columns.Add(new DataColumn("SALERATE", typeof(string)));
            dt.Columns.Add(new DataColumn("SALERETURN", typeof(string)));
            dt.Columns.Add(new DataColumn("RECOVERY", typeof(string)));
            dt.Columns.Add(new DataColumn("OUTSTANDING", typeof(string)));
            dt.Columns.Add(new DataColumn("FUROUTSTANDING", typeof(string)));            
            dt.Columns.Add(new DataColumn("AMOUNT", typeof(string)));
            dt.Columns.Add(new DataColumn("REMARKS", typeof(string)));
            dt.Columns.Add(new DataColumn("ddsr", typeof(string)));
            

            dr = dt.NewRow();

            //dr["ITEMCODE"] = string.Empty;
            dr["ITEMNAME"] = string.Empty;
            dr["ITEM TYPE"] = string.Empty;
            dr["UNITS"] = string.Empty;
            dr["QTY"] = "0.00";
            dr["SALERATE"] = "0.00";
            dr["SALERETURN"] = "0.00";
            dr["RECOVERY"] = "0.00";
            dr["OUTSTANDING"] = "0.00";
            dr["FUROUTSTANDING"] = "0.00";
            dr["AMOUNT"] = "0.00";
            dr["REMARKS"] = string.Empty;
            dr["ddsr"] = "0.00";            
            
            dt.Rows.Add(dr);

            //Store the DataTable in ViewState
            ViewState["dt_adItm"] = dt;

            GVPro1.DataSource = dt;
            GVPro1.DataBind();

            int rowIndex1 = 0;
            LinkButton linkbtndel = (LinkButton)GVPro1.Rows[rowIndex1].Cells[0].FindControl("linkbtndel");
            linkbtndel.Visible = false;
        }

        private void AddNewRow()
        {
            int rowIndex = 0;

            if (ViewState["dt_adItm"] != null)
            {
                DataTable dt = (DataTable)ViewState["dt_adItm"];
                DataRow drRow = null;
                if (dt.Rows.Count > 0)
                {
                    for (int i = 1; i <= dt.Rows.Count; i++)
                    {
                        //extract the Controls values

                        //DropDownList DDL_Itm = (DropDownList)GVPro1.Rows[rowIndex].Cells[0].FindControl("DDL_Itm");
                        TextBox TextBox1 = (TextBox)GVPro1.Rows[rowIndex].Cells[0].FindControl("TextBox1");
                        DropDownList DDL_Itmtyp = (DropDownList)GVPro1.Rows[rowIndex].Cells[1].FindControl("DDL_Itmtyp");
                        DropDownList DDL_Unt = (DropDownList)GVPro1.Rows[rowIndex].Cells[2].FindControl("DDL_Unt");
                        TextBox TBQty = (TextBox)GVPro1.Rows[rowIndex].Cells[3].FindControl("TBQty");
                        TextBox TBSalRat = (TextBox)GVPro1.Rows[rowIndex].Cells[4].FindControl("TBSalRat");
                        Label lbSalRtrn = (Label)GVPro1.Rows[rowIndex].Cells[5].FindControl("lbSalRtrn");
                        Label LBRecy = (Label)GVPro1.Rows[rowIndex].Cells[6].FindControl("LBRecy");
                        Label lbOutstan = (Label)GVPro1.Rows[rowIndex].Cells[7].FindControl("lbOutstan");
                        Label lblfurOutstan = (Label)GVPro1.Rows[rowIndex].Cells[8].FindControl("lblfurOutstan");
                        TextBox TBAmt = (TextBox)GVPro1.Rows[rowIndex].Cells[9].FindControl("TBAmt");
                        TextBox TBRmk = (TextBox)GVPro1.Rows[rowIndex].Cells[10].FindControl("TBRmk");
                        //TextBox lblttl = (TextBox)GVPro1.Rows[rowIndex].Cells[8].FindControl("lblttl");
                        Label dsrid = (Label)GVPro1.FooterRow.Cells[11].FindControl("HFDSR");
                        LinkButton linkbtndel = (LinkButton)GVPro1.Rows[rowIndex].Cells[0].FindControl("linkbtndel");
                        linkbtndel.Visible = true;

                        drRow = dt.NewRow();

                        dt.Rows[i - 1]["ITEMNAME"] = TextBox1.Text;  
                        dt.Rows[i - 1]["UNITS"] = DDL_Unt.SelectedValue;
                        dt.Rows[i - 1]["QTY"] = TBQty.Text;
                        dt.Rows[i - 1]["SALERATE"] = TBSalRat.Text;
                        dt.Rows[i - 1]["SALERETURN"] = lbSalRtrn.Text;
                        dt.Rows[i - 1]["RECOVERY"] = LBRecy.Text;
                        dt.Rows[i - 1]["OUTSTANDING"] = lbOutstan.Text;
                        dt.Rows[i - 1]["FUROUTSTANDING"] = lblfurOutstan.Text;
                        dt.Rows[i - 1]["AMOUNT"] = TBAmt.Text;
                        dt.Rows[i - 1]["REMARKS"] = TBRmk.Text;
                        //dt.Rows[i - 1]["ddsr"] = dsrid.Text;                  
                        
                        rowIndex++;

                        //DDL_Unt.Focus();
                    }
                    dt.Rows.Add(drRow);
                    ViewState["dt_adItm"] = dt;

                    GVPro1.DataSource = dt;
                    GVPro1.DataBind();

                }
            }
            else
            {
                Response.Write("ViewState is null");
            }

            //Set Previous Data on Postbacks
            SetPreRow();
        }

        private void SetPreRow()
        {
            try
            {
                //BindDDL();
                int rowIndex = 0;
                           
                if (ViewState["dt_adItm"] != null)
                {
                    DataTable dt = (DataTable)ViewState["dt_adItm"];
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            TextBox TextBox1 = (TextBox)GVPro1.Rows[rowIndex].Cells[0].FindControl("TextBox1");
                            DropDownList DDL_Itmtyp = (DropDownList)GVPro1.Rows[rowIndex].Cells[1].FindControl("DDL_Itmtyp");
                            DropDownList DDL_Unt = (DropDownList)GVPro1.Rows[rowIndex].Cells[2].FindControl("DDL_Unt");
                            TextBox TBQty = (TextBox)GVPro1.Rows[rowIndex].Cells[3].FindControl("TBQty");
                            TextBox TBSalRat = (TextBox)GVPro1.Rows[rowIndex].Cells[4].FindControl("TBSalRat");
                            Label lbSalRtrn = (Label)GVPro1.Rows[rowIndex].Cells[5].FindControl("lbSalRtrn");
                            Label LBRecy = (Label)GVPro1.Rows[rowIndex].Cells[6].FindControl("LBRecy");
                            Label lbOutstan = (Label)GVPro1.Rows[rowIndex].Cells[7].FindControl("lbOutstan");
                            Label lblfurOutstan = (Label)GVPro1.Rows[rowIndex].Cells[8].FindControl("lblfurOutstan");
                            // Label lblttl = (Label)GVPro1.FooterRow.Cells[8].FindControl("lblttl");
                            TextBox TBAmt = (TextBox)GVPro1.Rows[rowIndex].Cells[8].FindControl("TBAmt");
                            Label lbl_Flag = (Label)GVPro1.Rows[i].FindControl("lbl_Flag");
                            TextBox TBRmk = (TextBox)GVPro1.Rows[rowIndex].Cells[9].FindControl("TBRmk");
                            //HiddenField HFDSR = (HiddenField)GVPro1.Rows[rowIndex].Cells[10].FindControl("HFDSR");
                            Label HFDSR = (Label)GVPro1.Rows[rowIndex].Cells[10].FindControl("HFDSR");

                            
                            string itm = dt.Rows[i]["ITEMNAME"].ToString();

                            if (itm != "")
                            {
                                TextBox1.Text = dt.Rows[i]["ITEMNAME"].ToString();

                            }
                            else
                            {
                                TextBox1.Text = "";
                            }

                            DDL_Unt.SelectedValue = dt.Rows[i]["UNITS"].ToString();
                            TBQty.Text = dt.Rows[i]["QTY"].ToString();
                            TBSalRat.Text = dt.Rows[i]["SALERATE"].ToString();
                            lbSalRtrn.Text = dt.Rows[i]["SALERETURN"].ToString();
                            LBRecy.Text = dt.Rows[i]["RECOVERY"].ToString();
                            lbOutstan.Text = dt.Rows[i]["OUTSTANDING"].ToString();
                            lblfurOutstan.Text = dt.Rows[i]["FUROUTSTANDING"].ToString();                            
                            TBAmt.Text = dt.Rows[i]["AMOUNT"].ToString();
                            TBRmk.Text = dt.Rows[i]["REMARKS"].ToString();
                            HFDSR.Text = dt.Rows[i]["ddsr"].ToString();                            

                            string qty = dt.Rows[i]["QTY"].ToString();

                            if (qty != "")
                            {
                                TBQty.Text = dt.Rows[i]["QTY"].ToString();
                            }
                            else
                            {
                                TBQty.Text = "0.00";
                            }


                            string salrat = dt.Rows[i]["SALERATE"].ToString();

                            if (salrat != "")
                            {
                                TBSalRat.Text = dt.Rows[i]["SALERATE"].ToString();
                            }
                            else
                            {
                                TBSalRat.Text = "0.00";
                            }

                            string salratun = dt.Rows[i]["SALERETURN"].ToString();

                            if (salratun != "")
                            {
                                lbSalRtrn.Text = dt.Rows[i]["SALERETURN"].ToString();
                            }
                            else
                            {
                                lbSalRtrn.Text = "0.00";
                            }

                            string recovy = dt.Rows[i]["RECOVERY"].ToString();

                            if (recovy != "")
                            {
                                LBRecy.Text = dt.Rows[i]["RECOVERY"].ToString();
                            }
                            else
                            {
                                LBRecy.Text = "0.00";
                            }

                            string outstand = dt.Rows[i]["OUTSTANDING"].ToString();

                            if (outstand != "")
                            {
                                lbOutstan.Text = dt.Rows[i]["OUTSTANDING"].ToString();
                            }
                            else
                            {
                                lbOutstan.Text = "0.00";
                            }
                            string furoutstand = dt.Rows[i]["FUROUTSTANDING"].ToString();

                            if (furoutstand != "")
                            {
                                lblfurOutstan.Text = dt.Rows[i]["FUROUTSTANDING"].ToString();
                            }
                            else
                            {
                                lblfurOutstan.Text = "0.00";
                            }

                            string netttl = dt.Rows[i]["AMOUNT"].ToString();

                            if (netttl != "")
                            {
                                TBAmt.Text = dt.Rows[i]["AMOUNT"].ToString();
                            }
                            else
                            {
                                TBAmt.Text = "0.00";
                            }

                            //.HFDSR.Text = dt.Rows[i]["dsrid"].ToString();

                            if (TextBox1.Text == "")
                            {
                                lbl_Flag.Text = "0";
                            }
                            else
                            {
                                lbl_Flag.Text = "1";
                            }
                            //ChkCls.Checked = Convert.ToBoolean(dt.Rows[i]["CLOSED"].ToString());

                            rowIndex++;

                           
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //throw;
                lbl_err.Text = ex.Message.ToString();
            }
        }

        private void Update()
        {
            con.Open();

            SqlCommand command = con.CreateCommand();
            SqlTransaction transaction;

            // Start a local transaction.
            transaction = con.BeginTransaction("UpdateTransaction");

            // Must assign both transaction object and connection 
            // to Command object for a pending local transaction
            command.Connection = con;
            command.Transaction = transaction;

            try
            {  

                command.CommandText =
                    " update tbl_Mdsr set dsrdat = '" + TBdsrdat.Text + "',CustomerID = '" + DDL_Cust.SelectedValue.Trim() +
                    "', CreateBy = '" + Session["user"].ToString() + "', furout=" + tbfurOutstan.Text.Trim() + ", CreateAt = '"
                    + DateTime.Today.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) +
                    "', CompanyId = '" + Session["CompanyID"] + "', BranchId ='" + Session["BranchID"]
                    + "', Username='" + Session["user"].ToString() + "', prevbal = '" + lblOutstan.Text + "' where dsrid='" + HFdsrID.Value.Trim()
                    + "' and CompanyId = '" + Session["CompanyID"] + "' and BranchId= '" + Session["BranchID"] + "'";


                command.ExecuteNonQuery();

                foreach (GridViewRow g1 in GVPro1.Rows)
                {
                    //string DDL_Itm = (g1.FindControl("DDL_Itm") as DropDownList).SelectedValue;
                    string TextBox1 = (g1.FindControl("TextBox1") as TextBox).Text;
                    string DDL_Unt = (g1.FindControl("DDL_Unt") as DropDownList).SelectedValue;
                    string TBQty = (g1.FindControl("TBQty") as TextBox).Text;
                    string TBSalRat = (g1.FindControl("TBSalRat") as TextBox).Text;                    
                    string TBAmt = (g1.FindControl("TBAmt") as TextBox).Text;
                    string TBRmk = (g1.FindControl("TBRmk") as TextBox).Text;
                    //string HFDSR = (g1.FindControl("HFDSR") as HiddenField).Value;
                    string HFDSR = (g1.FindControl("HFDSR") as Label).Text;
                    string lbl_Flag = (g1.FindControl("lbl_Flag") as Label).Text;

                    //Detail DSR

                    //For Items
                    dt_ = new DataTable();
                    command.CommandText = " select ProductID from Products where ProductName='" + TextBox1.Trim() + "'";

                    adp = new SqlDataAdapter(command);
                    adp.Fill(dt_);

                    if (dt_.Rows.Count > 0)
                    {
                        proid = dt_.Rows[0]["ProductID"].ToString();
                    }

                    if (HFDSR != "")
                    {
                        command.CommandText =
                            " update tbl_ddsr set  ProductID ='" + proid + "' , untid='" + DDL_Unt + "', Qty = '" + 
                            TBQty + "', salrat='" + TBSalRat + "', salrturn ='" + TBSalRtrn.Text + "', recvry='" +
                            TBRecy.Text + "', outstan ='" + lblOutstan.Text + "', Amt='" + TBAmt + "', dsrrmk='" + TBRmk 
                            + "', ttlamt='" + tbtotal.Text.Trim() + "', CreateBy='" + Session["user"].ToString() + "', CreateAt='" + DateTime.Today.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) + "' where dsrid ='" + HFdsrID.Value.Trim() + "' and ddsr='" + HFDSR + "' and CompanyId = '" + Session["CompanyID"] + "'and BranchId ='" + Session["BranchID"] + "'";
                        command.ExecuteNonQuery();
                    }
                    else
                    {
                        command.CommandText =
                       " INSERT INTO tbl_ddsr (dsrid, ProductTypeID, ProductID, untid, Qty, salrat, salrturn, recvry, " +
                       "  outstan, Amt,  ttlamt, dsrrmk, CompanyId, BranchId, CreateAt, CreateBy) " +
                       " VALUES " +
                       " ('" + HFdsrID.Value.Trim() + "', '', '" + proid + "','" + DDL_Unt + "','" + TBQty + "','" + 
                       TBSalRat + "','" + TBSalRtrn.Text + "', '" + TBRecy.Text.Trim() + "','" + lblOutstan.Text + "','" 
                       + TBAmt + "','" + tbtotal.Text.Trim() + "','" + TBRmk + "','" + Session["CompanyID"] + "','" + 
                       Session["BranchID"] + "','" + DateTime.Now.ToShortDateString() + "','" + Session["user"].ToString() 
                       + "')";
                        command.ExecuteNonQuery();
                    }

                }

                #region Credit Sheets

                command.CommandText = "select CredAmt from tbl_Salcredit where CustomerID='" + DDL_Cust.SelectedValue.Trim() + "'";

                SqlDataAdapter stksalcre = new SqlDataAdapter(command);

                DataTable dtsalcre = new DataTable();
                stksalcre.Fill(dtsalcre);

                if (dtsalcre.Rows.Count > 0)
                {
                    double recv = Convert.ToDouble(lblOutstan.Text.Trim()); //- Convert.ToDouble(TBRecy.Text.Trim());

                    command.CommandText = " Update tbl_Salcredit set CredAmt = '" + recv + "' where CustomerID='" + DDL_Cust.SelectedValue.Trim() + "'";
                    command.ExecuteNonQuery();
                }
                else
                {
                    command.CommandText = " insert into tbl_Salcredit (CustomerID,CredAmt) values('" + DDL_Cust.SelectedValue.Trim() + "','" + lblOutstan.Text.Trim() + "')";
                    command.ExecuteNonQuery();
                }

                #endregion

                // Attempt to commit the transaction.
                transaction.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                Console.WriteLine("  Message: {0}", ex.Message);

                // Attempt to roll back the transaction. 
                try
                {
                    transaction.Rollback();
                }
                catch (Exception ex2)
                {
                    // This catch block will handle any errors that may have occurred 
                    // on the server that would cause the rollback to fail, such as 
                    // a closed connection.
                    Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                    Console.WriteLine("  Message: {0}", ex2.Message);
                }
            }
            finally
            {
                con.Close();
                //FillGrid();
                //ptnSno();
                Response.Redirect("frm_DSR_.aspx");
                //BindDll();
            }

        }
        private int Save()
        {

            con.Open();
            
            int res;

            SqlCommand command = con.CreateCommand();
            SqlTransaction transaction;

            // Start a local transaction.
            transaction = con.BeginTransaction("SampleTransaction");

            // Must assign both transaction object and connection 
            // to Command object for a pending local transaction
            command.Connection = con;
            command.Transaction = transaction;

            try
            {

                #region DSR

                //command.CommandText = " INSERT INTO tbl_Mdsr(dsrdat,CustomerID, CompanyId, BranchId, CreateAt, CreateBy, " +
                //    "   Isdon, Username, Salesman, saleper, prevbal) " +
                //             " VALUES " +
                //             " ('" + TBdsrdat.Text + "','" + DDL_Cust.SelectedValue.Trim() + "','" + Session["CompanyID"] 
                //             + "', '" + Session["BranchID"] + "','" + DateTime.Now + "','" + Session["user"].ToString() 
                //             + "',0,'" + Session["user"].ToString() + "','"  + DDL_Salman.SelectedValue.Trim() + "','" 
                //             + lbl_disc.Text + "','" + lblOutstan.Text.Trim() 
                //             + "')";

                command.CommandText = "sp_Mdsr";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@dsrdat", TBdsrdat.Text);
                command.Parameters.AddWithValue("@CustomerID", DDL_Cust.SelectedValue.Trim());
                command.Parameters.AddWithValue("@CompanyId", Session["CompanyID"]);
                command.Parameters.AddWithValue("@BranchId", Session["BranchID"]);
                command.Parameters.AddWithValue("@Isdone","0");
                command.Parameters.AddWithValue("@CreateAt", DateTime.Now.ToString());
                command.Parameters.AddWithValue("@CreateBy", Session["user"].ToString());
                command.Parameters.AddWithValue("@Username", Session["user"].ToString());
                command.Parameters.AddWithValue("@saleper", lbl_disc.Text);
                command.Parameters.AddWithValue("@prevbal", lblOutstan.Text.Trim());
                command.Parameters.AddWithValue("@Salesman", DDL_Salman.SelectedValue.Trim());
                command.Parameters.AddWithValue("@areaid", DDL_area.SelectedValue.Trim());
                command.Parameters.AddWithValue("@furout", tbfurOutstan.Text.Trim());
                
                command.ExecuteNonQuery();
                command.Parameters.Clear();



                // Master Purchase
                command.CommandText = "select top 1 dsrid from tbl_Mdsr where CompanyId = '" + Session["CompanyID"] + "' and BranchId = '" + Session["BranchID"] + "' order by dsrid desc";
                command.CommandType = CommandType.Text;

                adp = new SqlDataAdapter(command);

                DataTable dt = new DataTable();
                adp.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    mdsrId = dt.Rows[0]["dsrid"].ToString();
                }

                foreach (GridViewRow g1 in GVPro1.Rows)
                {
                    DDL_Unt = (g1.FindControl("DDL_Unt") as DropDownList).SelectedValue;
                    string TextBox1 = (g1.FindControl("TextBox1") as TextBox).Text;
                    TBSalRat = (g1.FindControl("TBSalRat") as TextBox).Text;
                    TBQty = (g1.FindControl("TBQty") as TextBox).Text;                    
                    TBAmt = (g1.FindControl("TBAmt") as TextBox).Text;
                    TBRmk = (g1.FindControl("TBRmk") as TextBox).Text;

                    
                    //For Items

                    DataTable dtpro = new DataTable();
                    command.CommandText = " select ProductID from Products where ProductName='" + TextBox1.Trim() + "'and CompanyId = '" + Session["CompanyID"] + "' and BranchId = '" + Session["BranchID"] + "'";
                    command.CommandType = CommandType.Text;

                    adp = new SqlDataAdapter(command);
                    adp.Fill(dtpro);

                    if (dtpro.Rows.Count > 0)
                    {
                        proid = dtpro.Rows[0]["ProductID"].ToString();
                    }
                    
                    //Detail Purchase,
                    //command.CommandText =
                    //    " INSERT INTO tbl_ddsr (dsrid, ProductTypeID, ProductID, untid, Qty, salrat, salrturn, recvry, outstan, Amt, ttlamt, dsrrmk, CompanyId, BranchId, CreateAt, CreateBy) " +
                    //    " VALUES " +
                    //    " ('" + mdsrId + "', '', '" + proid.Trim() + "','" + DDL_Unt + "','" + TBQty + "','" + TBSalRat + "','" + TBSalRtrn.Text +
                    //    "', '" + TBRecy.Text + "','" + lblOutstan.Text + "','" + TBAmt + "','" + tbtotal.Text + "','" + TBRmk + "','" +
                    //Session["CompanyID"] + "','" + Session["BranchID"] + "','" + DateTime.Now.ToShortDateString() + "','" 
                    //+ Session["user"].ToString() + "')";
                    //command.ExecuteNonQuery();

                    command.Parameters.Clear();
                    command.CommandText = "sp_Ddsr";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@dsrid", mdsrId);
                    command.Parameters.AddWithValue("@ProductTypeID", "");
                    command.Parameters.AddWithValue("@ProductID", proid.Trim());
                    command.Parameters.AddWithValue("@untid", DDL_Unt);
                    command.Parameters.AddWithValue("@Qty", TBQty);
                    command.Parameters.AddWithValue("@salrat", TBSalRat);
                    command.Parameters.AddWithValue("@salrturn", TBSalRtrn.Text);
                    command.Parameters.AddWithValue("@recvry", TBRecy.Text);
                    command.Parameters.AddWithValue("@outstan", lblOutstan.Text);
                    command.Parameters.AddWithValue("@Amt", TBAmt);
                    command.Parameters.AddWithValue("@dsrrmk", TBRmk);
                    command.Parameters.AddWithValue("@CompanyId", Session["CompanyID"]);
                    command.Parameters.AddWithValue("@BranchId", Session["BranchID"]);
                    command.Parameters.AddWithValue("@CreateAt", DateTime.Now.ToShortDateString());
                    command.Parameters.AddWithValue("@CreateBy", Session["user"].ToString());
                    command.Parameters.AddWithValue("@ttlamt", tbtotal.Text.Trim());

                    command.ExecuteNonQuery();

                 

                }
                #endregion

                #region Credit sheet

                command.Parameters.Clear();

                command.CommandText = " select CredAmt from tbl_Salcredit where CustomerID='" + DDL_Cust.SelectedValue.Trim() + "'";
                command.CommandType = CommandType.Text;

                DataTable dtven = new DataTable();
                adp = new SqlDataAdapter(command);
                adp.Fill(dtven);
                command.ExecuteNonQuery();

                if (dtven.Rows.Count > 0)
                {
                    command.Parameters.Clear();
                    command.CommandText = " Update tbl_Salcredit set CredAmt = '" + lblOutstan.Text.Trim() + "' where CustomerID='" + DDL_Cust.SelectedValue.Trim() + "'";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
                else
                {
                    command.Parameters.Clear();
                    command.CommandText = " insert into tbl_Salcredit (CustomerID,CredAmt) values('" + DDL_Cust.SelectedValue.Trim() + "','" + lblOutstan.Text.Trim() + "')";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }

                #endregion
                    
                // Attempt to commit the transaction.
                transaction.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                Console.WriteLine("  Message: {0}", ex.Message);

                // Attempt to roll back the transaction. 
                try
                {
                    transaction.Rollback();
                }
                catch (Exception ex2)
                {
                    // This catch block will handle any errors that may have occurred 
                    // on the server that would cause the rollback to fail, such as 
                    // a closed connection.
                    Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                    Console.WriteLine("  Message: {0}", ex2.Message);
                }
            }
            finally
            {
                con.Close();
                res = 1;
                Response.Redirect("frm_DSR_.aspx");
            }

            return res;
        }

        public void BindDLL()
        {
            //Customer Name

            dt_ = new DataTable();
            //dt_ = DBConnection.GetQueryData("select rtrim('[' + CAST(CustomerID AS VARCHAR(200)) + ']-' + CustomerName ) as [CustomerName], CustomerID from Customers_ where CompanyId = '" + Session["CompanyID"] + "' and BranchId= '" + Session["BranchID"] + "'");
            dt_ = DBConnection.GetQueryData("select * from Customers_ where CompanyId = '" + Session["CompanyID"] + "' and BranchId= '" + Session["BranchID"] + "'");

            DDL_Cust.DataSource = dt_;
            DDL_Cust.DataTextField = "CustomerName";
            DDL_Cust.DataValueField = "cust_acc";
            DDL_Cust.DataBind();
            DDL_Cust.Items.Insert(0, new ListItem("--Select Customer --", "0"));

            
            dt_ = new DataTable();
            dt_ = DBConnection.GetQueryData("select areaid,  " +
                " area_ from tbl_area where CompanyId = '" + Session["CompanyID"] + "' and BranchId= '" + Session["BranchID"] + "'");

            DDL_area.DataSource = dt_;
            DDL_area.DataTextField = "area_";
            DDL_area.DataValueField = "areaid";
            DDL_area.DataBind();
            DDL_area.Items.Insert(0, new ListItem("--Select Area --", "0"));


            DataTable dt_salman = new DataTable();

            if (lvl == "1")
            {
                //dt_salman = DBConnection.GetQueryData("select  *  from Users where Level = 3 and CompanyId = '" + Session["CompanyID"] + "' and BranchId= '" + Session["BranchID"] + "'");
                dt_salman = DBConnection.GetQueryData("select  distinct(salmanid) as [Username],booksalman  from tbl_booksalman where CompanyId = '" + Session["CompanyID"] + "' and BranchId= '" + Session["BranchID"] + "'");
            }
            else if (lvl == "3")
            {
                dt_salman = DBConnection.GetQueryData("select  distinct(salmanid) as [Username],booksalman  from tbl_booksalman where salmanid = '" + Session["Username"] + "' and CompanyId = '" + Session["CompanyID"] + "' and BranchId= '" + Session["BranchID"] + "'");
            }
            else if (lvl == "2")
            {
                dt_salman = DBConnection.GetQueryData("select  salmanid as [Username],booksalman  from tbl_booksalman where bookerid = '" + Session["Username"] + "' and CompanyId = '" + Session["CompanyID"] + "' and BranchId= '" + Session["BranchID"] + "'");
            }

            DDL_Salman.DataSource = dt_salman;
            DDL_Salman.DataTextField = "Username";
            DDL_Salman.DataValueField = "booksalman";
            DDL_Salman.DataBind();
            DDL_Salman.Items.Insert(0, new ListItem("--Select Sales Man--", "0"));

        }

        #endregion
        protected void GVDSR_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                GridViewRow row;

                //string PURID = GVDSR.DataKeys[row.RowIndex].Values[0].ToString();
                //dsrinvoice
                if (e.CommandName == "invoice")
                {
                    row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                    string DSRID = GVDSR.DataKeys[row.RowIndex].Values[0].ToString();
                    string CUST = GVDSR.DataKeys[row.RowIndex].Values[1].ToString();
                    //string CUSTID = GVScrhMSal.DataKeys[row.RowIndex].Values[1].ToString();

                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "onclick", "javascript:window.open( 'rpt_salInv.aspx?ID=SAL&SalID=" + SalID + "','_blank','height=600px,width=600px,scrollbars=1');", true);                
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "onclick", "javascript:window.open( 'Reports/dsrinvoice.aspx?ID=SAL&SalID=" + DSRID + "&CUST=" + CUST + "','_blank','height=600px,width=600px,scrollbars=1');", true);                

                }else if (e.CommandName == "Select")
                {
                    row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                    //string MDSRID = Server.HtmlDecode(GVDSR.Rows[row.RowIndex].Cells[0].Text.ToString());
                    string MDSRID = GVDSR.DataKeys[row.RowIndex].Values[0].ToString();

                    string cmdtxt = "select tbl_Mdsr.CustomerID, tbl_Mdsr.areaid, CustomerName,prevbal, tbl_Mdsr.saleper, recvry, replace(convert(NVARCHAR, dsrdat, 101), ' ', '/') as [dsrdat],tbl_Mdsr.dsrid from tbl_Mdsr inner join tbl_ddsr on " +
                        " tbl_Mdsr.dsrid = tbl_ddsr.dsrid inner join Customers_ on tbl_Mdsr.CustomerID = Customers_.cust_acc  inner join Products on tbl_ddsr.ProductID = Products.ProductID " +
                        " where tbl_Mdsr.CompanyId = '" + Session["CompanyID"] + "' and tbl_Mdsr.BranchId= '" + Session["BranchID"] + "' and tbl_Mdsr.dsrid =" + MDSRID + "";

                    //string cmdtxt = " select a.mPurID, b.dPurId, b.mPurId, a.ven_id, a.VndrAdd, a.VndrCntct,a.PurNo, a.mPurDate, a.CreatedBy, a.CreatedAt, a.cnic, a.ntnno, a.tobePrntd,b.Dpurid, b.ProNam, b.ProDes, b.Qty, b.Total, b.subtotl, b.unit, b.cost, b.protyp,b.grossttal from MPurchase a  inner join DPurchase b on a.mPurID = b.mPurID where a.MPurID =" + MPurID + "";

                    SqlCommand cmdSlct = new SqlCommand(cmdtxt, con);
                    SqlDataAdapter adp = new SqlDataAdapter(cmdSlct);

                    DataTable dt = new DataTable();
                    adp.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        HFdsrID.Value = dt.Rows[0]["dsrid"].ToString();
                        TBdsrdat.Text = dt.Rows[0]["dsrdat"].ToString();
                        DDL_area.SelectedValue = dt.Rows[0]["areaid"].ToString();
                        DDL_Cust.SelectedValue = dt.Rows[0]["CustomerID"].ToString();
                        lbl_disc.Text = dt.Rows[0]["saleper"].ToString();
                        lblOutstan.Text = dt.Rows[0]["prevbal"].ToString();
                        TBRecy.Text = dt.Rows[0]["recvry"].ToString();
                        
                        string cmdDettxt = " select ddsr,'' as [ITEM TYPE],tbl_Ddsr.ProductID, ProductName as [ITEMNAME], " +
                            " ProductName,untid as [UNITS],tbl_Ddsr.dsrid,Qty,Amt as [AMOUNT], salrat as [SALERATE], " +
                            " salrturn as [SALERETURN],recvry as [RECOVERY],outstan as [OUTSTANDING],ttlamt, " +
                            " '' as [FUROUTSTANDING], '' as [AMOUNT],dsrrmk as[REMARKS] from tbl_Ddsr  " +
                            "  inner join tbl_Mdsr  on tbl_Ddsr.dsrid = tbl_Mdsr.dsrid inner join Products on  tbl_Ddsr.ProductID =  Products.ProductID" +
                            "  where tbl_Mdsr.CompanyId = '" + Session["CompanyID"] + "' and  tbl_Mdsr.BranchId='" + Session["BranchID"] + "' and tbl_Ddsr.dsrid = " + MDSRID + "";
                        
                        DataTable dt_Det = new DataTable();
                        dt_Det = DataAccess.DBConnection.GetDataTable(cmdDettxt);

                        if (dt_Det.Rows.Count > 0)
                        {
                            GVPro1.DataSource = dt_Det;
                            GVPro1.DataBind();
                            
                            ViewState["dt_adItm"] = dt_Det;

                            for (int j = 0; j < dt_Det.Rows.Count; j++)
                            {  
                                for (int i = 0; i < GVPro1.Rows.Count; i++)
                                {
                                    Label lbl_pro = (Label)GVPro1.Rows[i].FindControl("lblPurItm");
                                    //DropDownList DDL_Itm = (DropDownList)GVPro1.Rows[i].FindControl("DDL_Itm");
                                    Label lbl_unt = (Label)GVPro1.Rows[i].FindControl("lbl_unt");
                                    DropDownList DDL_Unt = (DropDownList)GVPro1.Rows[i].FindControl("DDL_Unt");
                                    Label HFDSR = (Label)GVPro1.Rows[i].FindControl("HFDSR");
                                    Label lbl_Flag = (Label)GVPro1.Rows[i].FindControl("lbl_Flag");
                                    TextBox TextBox1 = (TextBox)GVPro1.Rows[i].FindControl("TextBox1");

                                 
                                    //lbl_pro.Text = dt_Det.Rows[j]["ITEMNAME"].ToString();                                    
                                    //DDL_Itm.SelectedValue = lbl_pro.Text.Trim();
                                    //lbl_unt.Text = dt_Det.Rows[j]["UNITS"].ToString();                                   
                                    DDL_Unt.SelectedValue = lbl_unt.Text.Trim();
                                    TextBox1.Text = lbl_pro.Text; //dt_Det.Rows[j]["ProductName"].ToString();
                                    //HiddenField HFDSR = (HiddenField)GVPro1.Rows[i].FindControl("HFDSR");
                                    //HFDSR.Text = dt_Det.Rows[j]["ddsr"].ToString();
                                    tbtotal.Text = dt_Det.Rows[j]["ttlamt"].ToString();

                                    lbl_Flag.Text = "1";
                                    btnUpd.Enabled = true;
                                  }
                             }
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);                        
                        lblalert.Text = "No Record Found!!";
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);                
                lblalert.Text = ex.Message;
            }
        }

        protected void GVDSR_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                 string DSRID = (string)GVDSR.DataKeys[e.RowIndex].Values[0].ToString();  
                //string MPurID = Server.HtmlDecode(GVDSR.Rows[e.RowIndex].Cells[0].Text.ToString());
                //string MPurNo = Server.HtmlDecode(GVDSR.Rows[e.RowIndex].Cells[1].Text.ToString());
                //= .DataKeys[row.RowIndex].Values[0].ToString();

                SqlCommand cmd = new SqlCommand();

                cmd = new SqlCommand("sp_del_Dsr", con);
                cmd.Parameters.Add("@mDsrID", SqlDbType.Int).Value = DSRID;
                cmd.Parameters.Add("@CompanyId", SqlDbType.VarChar).Value = Session["CompanyID"];
                cmd.Parameters.Add("@BranchId", SqlDbType.VarChar).Value = Session["BranchID"];
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                Response.Redirect("frm_DSR_.aspx");

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
               
                lblalert.Text = ex.Message;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow g1 in GVPro1.Rows)
            {
                string item = (g1.FindControl("TextBox1") as TextBox).Text;
                if (item == "")
                {
                    v_item.Text = "Enter Item Name";
                }
                else if (DDL_area.SelectedValue == "0")
                {
                    lbl_ChkArea.Text = "Please Select Area!!";
                    lbl_ChkArea.Visible = true;
                    lbl_Chkcust.Visible = false;
                    lbl_ChkSalman.Visible = false;
                    lbl_Chktotl.Visible = false;
                    lbl_ChkProNam.Visible = false;
                    lbl_Chksalrat.Visible = false;
                    lbl_Chkqty.Visible = false;
                    lbl_Chksalrat.Visible = false;
                    lbl_ChkAmt.Visible = false;
                    lbl_Chkunt.Visible = false;
                }
                else if (DDL_Cust.SelectedValue == "0")
                {
                    lbl_Chkcust.Text = "Please Select Customer!!";
                    lbl_Chkcust.Visible = true;
                    lbl_ChkArea.Visible = false;
                    lbl_ChkSalman.Visible = false;
                    lbl_Chktotl.Visible = false;
                    lbl_ChkProNam.Visible = false;
                    lbl_Chksalrat.Visible = false;
                    lbl_Chkqty.Visible = false;
                    lbl_Chksalrat.Visible = false;
                    lbl_ChkAmt.Visible = false;
                    lbl_Chkunt.Visible = false;
                }
                else if (DDL_Salman.SelectedValue == "0")
                {
                    lbl_ChkSalman.Text = "Please Select Sales Man!!";
                    lbl_ChkSalman.Visible = true;
                    lbl_ChkArea.Visible = false;
                    lbl_Chkcust.Visible = false;
                    lbl_Chktotl.Visible = false;
                    lbl_ChkProNam.Visible = false;
                    lbl_Chksalrat.Visible = false;
                    lbl_Chkqty.Visible = false;
                    lbl_Chksalrat.Visible = false;
                    lbl_ChkAmt.Visible = false;
                    lbl_Chkunt.Visible = false;
                }
                else if (tbtotal.Text == "" || tbtotal.Text == "0.00" || tbtotal.Text == "0")
                {
                    lbl_Chktotl.Text = "Please Fill Total with Value!!";
                    lbl_Chktotl.Visible = true;
                    lbl_ChkArea.Visible = false;
                    lbl_Chkcust.Visible = false;
                    lbl_ChkSalman.Visible = false;
                    lbl_ChkProNam.Visible = false;
                    lbl_Chksalrat.Visible = false;
                    lbl_Chkqty.Visible = false;
                    lbl_Chksalrat.Visible = false;
                    lbl_ChkAmt.Visible = false;
                    lbl_Chkunt.Visible = false;
                }
                else
                {
                    
                    lbl_ChkArea.Visible = false;
                    lbl_Chkcust.Visible = false;
                    lbl_ChkSalman.Visible = false;
                    lbl_Chktotl.Visible = false;
                    lbl_ChkProNam.Visible = false;
                    lbl_Chksalrat.Visible = false;
                    lbl_Chkqty.Visible = false;
                    lbl_Chksalrat.Visible = false;
                    lbl_ChkAmt.Visible = false;
                    lbl_Chkunt.Visible = false;

                    chkdetails = 0;

                    for (int j = 0; j < GVPro1.Rows.Count; j++)
                    {
                        TextBox TextBox1 = (TextBox)GVPro1.Rows[j].FindControl("TextBox1");
                        TextBox TBQty = (TextBox)GVPro1.Rows[j].FindControl("TBQty");
                        TextBox TBSalRat = (TextBox)GVPro1.Rows[j].FindControl("TBSalRat");
                        TextBox TBAmt = (TextBox)GVPro1.Rows[j].FindControl("TBAmt");
                        DropDownList DDL_Unt = (DropDownList)GVPro1.Rows[j].FindControl("DDL_Unt");

                        if (DDL_Unt.SelectedValue == "0")
                        {
                            lbl_Chkunt.Text = "Please Select Unit!!";
                            lbl_Chkunt.Visible = true;
                            lbl_ChkArea.Visible = false;
                            lbl_Chkcust.Visible = false;
                            lbl_ChkSalman.Visible = false;
                            lbl_Chktotl.Visible = false;
                            lbl_Chksalrat.Visible = false;
                            lbl_Chkqty.Visible = false;
                            lbl_Chksalrat.Visible = false;
                            lbl_ChkAmt.Visible = false;


                        }
                        else if (TextBox1.Text == "")
                        {
                            lbl_ChkProNam.Text = "Please Select Product!!";
                            lbl_ChkProNam.Visible = true;
                            lbl_ChkArea.Visible = false;
                            lbl_Chkcust.Visible = false;
                            lbl_ChkSalman.Visible = false;
                            lbl_Chktotl.Visible = false;
                            lbl_Chksalrat.Visible = false;
                            lbl_Chkqty.Visible = false;
                            lbl_Chksalrat.Visible = false;
                            lbl_ChkAmt.Visible = false;
                            lbl_Chkunt.Visible = false;
                        }
                        else if (TBQty.Text == "" || TBQty.Text == "0.00" || TBQty.Text == "0")
                        {
                            lbl_Chkqty.Text = "Please Write Quantity!!";
                            lbl_Chkqty.Visible = true;
                            lbl_ChkProNam.Visible = false;
                            lbl_ChkArea.Visible = false;
                            lbl_Chkcust.Visible = false;
                            lbl_ChkSalman.Visible = false;
                            lbl_Chktotl.Visible = false;
                            lbl_Chksalrat.Visible = false;
                            lbl_Chksalrat.Visible = false;
                            lbl_ChkAmt.Visible = false;
                            lbl_Chkunt.Visible = false;
                        }
                        else if (TBSalRat.Text == "" || TBSalRat.Text == "0.00" || TBSalRat.Text == "0")
                        {
                            lbl_Chksalrat.Text = "Please Write Sale Rate!!";
                            lbl_Chksalrat.Visible = true;
                            lbl_ChkProNam.Visible = false;
                            lbl_ChkArea.Visible = false;
                            lbl_Chkcust.Visible = false;
                            lbl_ChkSalman.Visible = false;
                            lbl_Chktotl.Visible = false;
                            lbl_Chkqty.Visible = false;
                            lbl_Chksalrat.Visible = false;
                            lbl_ChkAmt.Visible = false;
                            lbl_Chkunt.Visible = false;
                        }
                        else if (TBAmt.Text == "" || TBAmt.Text == "0.00" || TBAmt.Text == "0")
                        {
                            lbl_ChkAmt.Text = "Please Write Amount!!";
                            lbl_ChkAmt.Visible = true;
                            lbl_ChkProNam.Visible = false;
                            lbl_ChkArea.Visible = false;
                            lbl_Chkcust.Visible = false;
                            lbl_ChkSalman.Visible = false;
                            lbl_Chktotl.Visible = false;
                            lbl_Chkqty.Visible = false;
                            lbl_Chksalrat.Visible = false;
                            lbl_Chksalrat.Visible = false;
                            lbl_Chkunt.Visible = false;
                        }
                        else
                        {
                            chkdetails = 1;
                        }
                    }


                    //Save DSR
                    // {
                    if (chkdetails == 1)
                    {
                        v_item.Text = "";
                        Save();
                    }
                    // }
                }

            }
        }

        protected void linkbtnadd_Click(object sender, EventArgs e)
        {
            AddNewRow();
        }
        protected void linkbtndel_Click(object sender, EventArgs e)
        {
            TextBox TextBox1 = null;
            for (int i = 0; i < GVPro1.Rows.Count; i++)
            {
                Label HFDSR = (Label)GVPro1.Rows[i].FindControl("HFDSR");
                 TextBox1 = (TextBox)GVPro1.Rows[i].FindControl("TextBox1");
                
                SqlCommand cmd = new SqlCommand();

                cmd = new SqlCommand("delete from tbl_ddsr where ddsr = " + HFDSR.Text.Trim() + " and CompanyId = '" + Session["CompanyID"] + "' and BranchId= '" + Session["BranchID"] + "'", con);
                cmd.CommandType = CommandType.Text;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                FillGrid();               
            }

            TextBox1.Focus();
        }
        protected void btnCancl_Click(object sender, EventArgs e)
        {
            Response.Redirect("frm_DSR_.aspx");
        }
        protected void linkmodaldelete_Click(object sender, EventArgs e)
        {

        }
        protected void GVPro1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                dt_ = new DataTable();
                //dt_ = DBConnection.GetQueryData("select rtrim('[' + CAST(untid AS VARCHAR(200)) + ']-' + untnam ) as [untnam], untid from tbl_unts where CompanyId = '" + Session["CompanyID"] + "' and BranchId= '" + Session["BranchID"] + "'");
                dt_ = DBConnection.GetQueryData("select * from tbl_unts where CompanyId = '" + Session["CompanyID"] + "' and BranchId= '" + Session["BranchID"] + "' order by untid desc");

                for (int i = 0; i < GVPro1.Rows.Count; i++)
                {
                    DropDownList DDL_Unt = (DropDownList)GVPro1.Rows[i].Cells[0].FindControl("DDL_Unt");
                    DDL_Unt.DataSource = dt_;
                    DDL_Unt.DataTextField = "untnam";
                    DDL_Unt.DataValueField = "untid";
                    DDL_Unt.DataBind();
                    //DDL_Unt.Items.Insert(0, new ListItem("--Select Units--", "0"));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //protected void GVPro1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        //{
        //    if (ViewState["dt_adItm"] != null)
        //    {
        //        DataTable dt = (DataTable)ViewState["dt_adItm"];
        //        DataRow drCurrentRow = null;
        //        int rowIndex = Convert.ToInt32(e.RowIndex);
        //        if (dt.Rows.Count > 1)
        //        {
        //            dt.Rows.Remove(dt.Rows[rowIndex]);
        //            drCurrentRow = dt.NewRow();
        //            ViewState["dt_adItm"] = dt;

        //            GVPro1.DataSource = dt;
        //            GVPro1.DataBind();

        //            SetPreRow();
        //            //ptnSno();
        //        }
        //    }
        //}

        protected void DDL_Itm_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                for (int j = 0; j < GVPro1.Rows.Count; j++)
                {
                    DropDownList DDL_Itm = (DropDownList)GVPro1.Rows[j].FindControl("DDL_Itm");

                    TextBox TBQty = (TextBox)GVPro1.Rows[j].FindControl("TBQty");

                    TextBox TbSalRat = (TextBox)GVPro1.Rows[j].FindControl("TbSalRat");

                    
                    Label lbl_Flag = (Label)GVPro1.Rows[j].FindControl("lbl_Flag");


                    string query = "select ProductID,Dstk_ItmQty as [Qty],Dstk_salrat as [Rate] " +
                                   " from tbl_dstk where ProductID = " + DDL_Itm.SelectedValue.Trim() + " and CompanyId = '" + Session["CompanyID"] + "' and BranchId= '" + Session["BranchID"] + "'";

                    SqlCommand cmd = new SqlCommand(query, con);
                    DataTable dt_ = new DataTable();
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);

                    adp.Fill(dt_);

                    if (dt_.Rows.Count > 0)
                    {
                        
                        TbSalRat.Text = dt_.Rows[0]["Rate"].ToString();
                        //lbl_Flag.Text = "1";
                        if (lbl_Flag.Text == "0")
                        {
                            TBQty.Text = dt_.Rows[0]["Qty"].ToString();
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                //lbl_Heading.Text = "Error!";
                lblalert.Text = ex.Message;
                //lbl_err.Text = ex.Message.ToString();
            }
        }

        protected void DDL_area_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            try
            {

                string query = " select cust_acc, CustomerName from Customers_ where areaid='" + DDL_area.SelectedValue.Trim() + "'";
                dt_ = new DataTable();

                dt_ = DBConnection.GetQueryData(query);
                if (dt_.Rows.Count > 0)
                {
                    DDL_Cust.DataSource = dt_;
                    DDL_Cust.DataValueField = "cust_acc";
                    DDL_Cust.DataTextField = "CustomerName";
                    DDL_Cust.DataBind();
                    DDL_Cust.Items.Insert(0, new ListItem("--Select Customer --", "0"));
                }

            }
            catch (Exception ex)
            {
                lbl_err.Text= ex.Message;
            }
        }
        protected void DDL_Cust_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                
                string query = " select CredAmt from tbl_Salcredit where CustomerID='" + DDL_Cust.SelectedValue.Trim() + "'";

                SqlCommand command = new SqlCommand(query, con);
                con.Open();
                DataTable dtven = new DataTable();
                SqlDataAdapter adp = new SqlDataAdapter(command);
                adp.Fill(dtven);
                command.ExecuteNonQuery();

                //for (int i = 0; i < GVPro1.Rows.Count; i++)
                {
                    //TextBox lblOutstan = (TextBox)GVPro1.Rows[i].Cells[7].FindControl("lblOutstan");

                    if (dtven.Rows.Count > 0)
                    {
                        lblOutstan.Text = dtven.Rows[0]["CredAmt"].ToString();
                    }
                    else
                    {
                        lblOutstan.Text = "0.00";
                    }
                }

                query = " select saleper from Customers_ where CustomerID='" + DDL_Cust.SelectedValue.Trim() + "' and CompanyId = '" + Session["CompanyID"] + "' and BranchId= '" + Session["BranchID"] + "'";

                dt_ = DBConnection.GetQueryData(query);

                if (dt_.Rows.Count > 0)
                {
                    lbl_disc.Text = dt_.Rows[0]["saleper"].ToString();
                }
                else
                {
                    lbl_disc.Text = "0"; 
                }
                con.Close();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                //lbl_Heading.Text = "Error!";
                lblalert.Text = ex.Message;
            }
        }

        protected void TBQty_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string saleper="";
                decimal chkqty =0;
                string proid = "";
                LinkButton linkbtnadd = null;

                for (int j = 0; j < GVPro1.Rows.Count; j++)
                {
                    //DropDownList DDL_Itm = (DropDownList)GVPro1.Rows[j].FindControl("DDL_Itm");
                    TextBox TextBox1 = (TextBox)GVPro1.Rows[j].FindControl("TextBox1");
                    TextBox TBQty = (TextBox)GVPro1.Rows[j].FindControl("TBQty");
                    Label lbl_chkqty = (Label)GVPro1.Rows[j].FindControl("lbl_chkqty");
                    TextBox TBAmt = (TextBox)GVPro1.Rows[j].FindControl("TBAmt");
                    TextBox TbSalRat = (TextBox)GVPro1.Rows[j].FindControl("TbSalRat");
                    //Label lblttl = (Label)GVPro1.FooterRow.FindControl("lblttl");
                    Label lbl_Flag = (Label)GVPro1.Rows[j].FindControl("lbl_Flag");
                    linkbtnadd = (LinkButton)GVPro1.Rows[j].FindControl("linkbtnadd");

                    if (TextBox1.Text == "")
                    {
                        lbl_Flag.Text = "0";
                    }

                    // For Product

                    query = " select tbl_Dstk.ProductID, ProductName from tbl_Dstk " +
                        " inner join Products on tbl_Dstk.ProductID = Products.ProductID " +
                        " where ProductName ='" + TextBox1.Text + "'";
                    dt_ = new DataTable();
                    dt_ = DBConnection.GetQueryData(query);
                    
                    if (dt_.Rows.Count > 0)
                    {
                        proid = dt_.Rows[0]["ProductID"].ToString();
                    }

                    // For checking Quantity in Stock

                    query = " select Dstk_ItmQty from tbl_dstk where ProductID = '" + proid + "'";
                    dt_ = new DataTable();
                    dt_ = DBConnection.GetQueryData(query);
                    if (dt_.Rows.Count > 0)
                    {
                        chkqty =Convert.ToDecimal(dt_.Rows[0]["Dstk_ItmQty"]);

                        if (chkqty < Convert.ToDecimal(TBQty.Text.Trim()))
                        {
                            lbl_chkqty.Text = "Items are less in Stock!...";
                            linkbtnadd.Enabled = false;
                            TBAmt.Enabled = false;
                            TbSalRat.Enabled = false;

                        }
                        else
                        {
                            lbl_chkqty.Text = "";
                            linkbtnadd.Enabled = true;
                            TBAmt.Enabled = true;
                            TbSalRat.Enabled = true;
                        }
                    }

                    // For Sales Percent

                    query = " select saleper,* from Customers_ where CustomerID=" + DDL_Cust.SelectedValue.Trim() + " and CompanyId = '" + Session["CompanyID"] + "' and BranchId= '" + Session["BranchID"] + "'";

                    dt_ = new DataTable();
                    dt_ = DBConnection.GetQueryData(query);
                   
                    if (dt_.Rows.Count > 0)
                    {
                        saleper = dt_.Rows[0]["saleper"].ToString();
                    }
                    else
                    {
                        saleper = "0.00";
                    }
                    con.Close();

                    TBAmt.Text = (Convert.ToDouble(TBQty.Text.Trim()) * Convert.ToDouble(TbSalRat.Text.Trim())).ToString();
                }

                float GTotal = 0;
                for (int k = 0; k < GVPro1.Rows.Count; k++)
                {
                    TextBox total = (TextBox)GVPro1.Rows[k].FindControl("TBAmt");

                    double discount = Convert.ToDouble(total.Text) * Convert.ToDouble(saleper) / 100;
                    string ttlamt = (Convert.ToDouble(total.Text) - discount).ToString();

                    GTotal += Convert.ToSingle(ttlamt);
                }

                tbtotal.Text = GTotal.ToString();
                linkbtnadd.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void TBRecy_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string saleper = "";

                if (TBRecy.Text != "0.00")
                {
                    double ttlafrecv = (Convert.ToDouble(lblOutstan.Text.Trim()) - Convert.ToDouble(TBRecy.Text.Trim()));
                    //double ttlafrecvs = (Convert.ToDouble(tbtotal.Text.Trim()) + Convert.ToDouble(TBRecy.Text.Trim()));

                    string query = " select saleper,* from Customers_ where CustomerID=" + DDL_Cust.SelectedValue.Trim() + " and CompanyId = '" + Session["CompanyID"] + "' and BranchId= '" + Session["BranchID"] + "'";

                    SqlCommand command = new SqlCommand(query, con);
                    con.Open();
                    DataTable dt_ = new DataTable();
                    SqlDataAdapter adp = new SqlDataAdapter(command);
                    adp.Fill(dt_);
                    command.ExecuteNonQuery();

                    if (dt_.Rows.Count > 0)
                    {
                        saleper = dt_.Rows[0]["saleper"].ToString();
                    }
                    else
                    {
                        saleper = "0.00";
                    }
                    con.Close();

                    //tbtotal.Text = ttlafrecvs.ToString();
                    lblOutstan.Text = ttlafrecv.ToString();
                }

                for (int i = 0; i < GVPro1.Rows.Count; i++)
                {

                    //TextBox TBRecy = (TextBox)GVPro1.Rows[i].Cells[6].FindControl("TBRecy");
                    //TextBox lblOutstan = (TextBox)GVPro1.Rows[i].Cells[7].FindControl("lblOutstan");
                    TextBox TBAmt = (TextBox)GVPro1.Rows[i].Cells[6].FindControl("TBAmt");
                    TextBox TBQty = (TextBox)GVPro1.Rows[i].Cells[3].FindControl("TBQty");

                }
            }
            catch (Exception ex)
            {
                lbl_err.Text= ex.Message;
            }

        }

        protected void tbfurOutstan_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string saleper = "";

                lblOutstan.Text = (Convert.ToDecimal(lblOutstan.Text) + Convert.ToDecimal(tbfurOutstan.Text)).ToString();

                if (tbfurOutstan.Text != "0.00")
                {
                    //double ttlafoutstand = (Convert.ToDouble(TBAmt.Text.Trim()) - Convert.ToDouble(tbfurOutstan.Text.Trim()));
                    double ttlafoutstands = (Convert.ToDouble(tbtotal.Text.Trim()) - Convert.ToDouble(tbfurOutstan.Text.Trim()));
                    ////TBQty TbSalRat
                    //TBAmt.Text = ttlafoutstand.ToString();
                    tbtotal.Text = ttlafoutstands.ToString();

                    string query = " select saleper,* from Customers_ where CustomerID=" + DDL_Cust.SelectedValue.Trim() + " and CompanyId = '" + Session["CompanyID"] + "' and BranchId= '" + Session["BranchID"] + "'";

                    SqlCommand command = new SqlCommand(query, con);
                    con.Open();
                    DataTable dt_ = new DataTable();
                    SqlDataAdapter adp = new SqlDataAdapter(command);
                    adp.Fill(dt_);
                    command.ExecuteNonQuery();

                    if (dt_.Rows.Count > 0)
                    {
                        saleper = dt_.Rows[0]["saleper"].ToString();
                    }
                    else
                    {
                        saleper = "0.00";
                    }
                    con.Close();

                }
                else
                {
                    float GTotal = 0;
                    for (int k = 0; k < GVPro1.Rows.Count; k++)
                    {
                        TextBox total = (TextBox)GVPro1.Rows[k].FindControl("TBAmt");

                        double discount = Convert.ToDouble(total.Text) * Convert.ToDouble(saleper) / 100;
                        string ttlamt = (Convert.ToDouble(total.Text) - discount).ToString();

                        GTotal += Convert.ToSingle(ttlamt);
                        tbtotal.Text = GTotal.ToString();
                    }
                }
                for (int i = 0; i < GVPro1.Rows.Count; i++)
                {

                    TextBox TBAmt = (TextBox)GVPro1.Rows[i].Cells[6].FindControl("TBAmt");
                    //TextBox lblOutstan = (TextBox)GVPro1.Rows[i].Cells[7].FindControl("lblOutstan");
                    //TextBox tbfurOutstan = (TextBox)GVPro1.Rows[i].Cells[8].FindControl("tbfurOutstan");
                    TextBox TBQty = (TextBox)GVPro1.Rows[i].Cells[3].FindControl("TBQty");

                }
            }
            catch (Exception ex)
            {
                lbl_err.Text= ex.Message;
            }

        }
        
        protected void lblOutstan_TextChanged(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < GVPro1.Rows.Count; i++)
                {
                    string saleper = "";

                    TextBox TBAmt = (TextBox)GVPro1.Rows[i].Cells[6].FindControl("TBAmt");                    
                    TextBox TBQty = (TextBox)GVPro1.Rows[i].Cells[3].FindControl("TBQty");

                    if (lblOutstan.Text != "0.00")
                    {
                        double ttlafoutstand = (Convert.ToDouble(TBAmt.Text.Trim()) - Convert.ToDouble(lblOutstan.Text.Trim()));
                        double ttlafoutstands = (Convert.ToDouble(tbtotal.Text.Trim()) - Convert.ToDouble(lblOutstan.Text.Trim()));
                        //TBQty TbSalRat
                        TBAmt.Text = ttlafoutstand.ToString();
                        tbtotal.Text = ttlafoutstands.ToString();

                        string query = " select saleper,* from Customers_ where CustomerID=" + DDL_Cust.SelectedValue.Trim() + " and CompanyId = '" + Session["CompanyID"] + "' and BranchId= '" + Session["BranchID"] + "'";

                        SqlCommand command = new SqlCommand(query, con);
                        con.Open();
                        DataTable dt_ = new DataTable();
                        SqlDataAdapter adp = new SqlDataAdapter(command);
                        adp.Fill(dt_);
                        command.ExecuteNonQuery();

                        if (dt_.Rows.Count > 0)
                        {
                            saleper = dt_.Rows[0]["saleper"].ToString();
                        }
                        else
                        {
                            saleper = "0.00";
                        }
                        con.Close();

                        float GTotal = 0;
                        for (int k = 0; k < GVPro1.Rows.Count; k++)
                        {
                            TextBox total = (TextBox)GVPro1.Rows[k].FindControl("TBAmt");

                            double discount = Convert.ToDouble(total.Text) * Convert.ToDouble(saleper) / 100;
                            string ttlamt = (Convert.ToDouble(total.Text) - discount).ToString();

                            GTotal += Convert.ToSingle(ttlamt);
                            tbtotal.Text = GTotal.ToString();
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnUpd_Click(object sender, EventArgs e)
        {
            if (DDL_area.SelectedValue == "0")
            {
                lbl_ChkArea.Text = "Please Select Area!!";
                lbl_ChkArea.Visible = true;
                lbl_Chkcust.Visible = false;
                lbl_ChkSalman.Visible = false;
                lbl_Chktotl.Visible = false;
                lbl_ChkProNam.Visible = false;
                lbl_Chksalrat.Visible = false;
                lbl_Chkqty.Visible = false;
                lbl_Chksalrat.Visible = false;
                lbl_ChkAmt.Visible = false;
                lbl_Chkunt.Visible = false;
            }
            else if (DDL_Cust.SelectedValue == "0")
            {
                lbl_Chkcust.Text = "Please Select Customer!!";
                lbl_Chkcust.Visible = true;
                lbl_ChkArea.Visible = false;
                lbl_ChkSalman.Visible = false;
                lbl_Chktotl.Visible = false;
                lbl_ChkProNam.Visible = false;
                lbl_Chksalrat.Visible = false;
                lbl_Chkqty.Visible = false;
                lbl_Chksalrat.Visible = false;
                lbl_ChkAmt.Visible = false;
                lbl_Chkunt.Visible = false;
            }
            else if (DDL_Salman.SelectedValue == "0")
            {
                lbl_ChkSalman.Text = "Please Select Sales Man!!";
                lbl_ChkSalman.Visible = true;
                lbl_ChkArea.Visible = false;
                lbl_Chkcust.Visible = false;
                lbl_Chktotl.Visible = false;
                lbl_ChkProNam.Visible = false;
                lbl_Chksalrat.Visible = false;
                lbl_Chkqty.Visible = false;
                lbl_Chksalrat.Visible = false;
                lbl_ChkAmt.Visible = false;
                lbl_Chkunt.Visible = false;
            }
            else if (tbtotal.Text == "" || tbtotal.Text == "0.00" || tbtotal.Text == "0")
            {
                lbl_Chktotl.Text = "Please Fill Total with Value!!";
                lbl_Chktotl.Visible = true;
                lbl_ChkArea.Visible = false;
                lbl_Chkcust.Visible = false;
                lbl_ChkSalman.Visible = false;
                lbl_ChkProNam.Visible = false;
                lbl_Chksalrat.Visible = false;
                lbl_Chkqty.Visible = false;
                lbl_Chksalrat.Visible = false;
                lbl_ChkAmt.Visible = false;
                lbl_Chkunt.Visible = false;
            }
            else
            {
                lbl_ChkArea.Visible = false;
                lbl_Chkcust.Visible = false;
                lbl_ChkSalman.Visible = false;
                lbl_Chktotl.Visible = false;
                lbl_ChkProNam.Visible = false;
                lbl_Chksalrat.Visible = false;
                lbl_Chkqty.Visible = false;
                lbl_Chksalrat.Visible = false;
                lbl_ChkAmt.Visible = false;
                lbl_Chkunt.Visible = false;

                chkdetails = 0;

                for (int j = 0; j < GVPro1.Rows.Count; j++)
                {
                    TextBox TextBox1 = (TextBox)GVPro1.Rows[j].FindControl("TextBox1");
                    TextBox TBQty = (TextBox)GVPro1.Rows[j].FindControl("TBQty");
                    TextBox TBSalRat = (TextBox)GVPro1.Rows[j].FindControl("TBSalRat");
                    TextBox TBAmt = (TextBox)GVPro1.Rows[j].FindControl("TBAmt");
                    DropDownList DDL_Unt = (DropDownList)GVPro1.Rows[j].FindControl("DDL_Unt");

                    if (DDL_Unt.SelectedValue == "0")
                    {
                        lbl_Chkunt.Text = "Please Select Unit!!";
                        lbl_Chkunt.Visible = true;
                        lbl_ChkArea.Visible = false;
                        lbl_Chkcust.Visible = false;
                        lbl_ChkSalman.Visible = false;
                        lbl_Chktotl.Visible = false;
                        lbl_Chksalrat.Visible = false;
                        lbl_Chkqty.Visible = false;
                        lbl_Chksalrat.Visible = false;
                        lbl_ChkAmt.Visible = false;


                    }
                    else if (TextBox1.Text == "")
                    {
                        lbl_ChkProNam.Text = "Please Select Product!!";
                        lbl_ChkProNam.Visible = true;
                        lbl_ChkArea.Visible = false;
                        lbl_Chkcust.Visible = false;
                        lbl_ChkSalman.Visible = false;
                        lbl_Chktotl.Visible = false;
                        lbl_Chksalrat.Visible = false;
                        lbl_Chkqty.Visible = false;
                        lbl_Chksalrat.Visible = false;
                        lbl_ChkAmt.Visible = false;
                        lbl_Chkunt.Visible = false;
                    }
                    else if (TBQty.Text == "" || TBQty.Text == "0.00" || TBQty.Text == "0")
                    {
                        lbl_Chkqty.Text = "Please Write Quantity!!";
                        lbl_Chkqty.Visible = true;
                        lbl_ChkProNam.Visible = false;
                        lbl_ChkArea.Visible = false;
                        lbl_Chkcust.Visible = false;
                        lbl_ChkSalman.Visible = false;
                        lbl_Chktotl.Visible = false;
                        lbl_Chksalrat.Visible = false;
                        lbl_Chksalrat.Visible = false;
                        lbl_ChkAmt.Visible = false;
                        lbl_Chkunt.Visible = false;
                    }
                    else if (TBSalRat.Text == "" || TBSalRat.Text == "0.00" || TBSalRat.Text == "0")
                    {
                        lbl_Chksalrat.Text = "Please Write Sale Rate!!";
                        lbl_Chksalrat.Visible = true;
                        lbl_ChkProNam.Visible = false;
                        lbl_ChkArea.Visible = false;
                        lbl_Chkcust.Visible = false;
                        lbl_ChkSalman.Visible = false;
                        lbl_Chktotl.Visible = false;
                        lbl_Chkqty.Visible = false;
                        lbl_Chksalrat.Visible = false;
                        lbl_ChkAmt.Visible = false;
                        lbl_Chkunt.Visible = false;
                    }
                    else if (TBAmt.Text == "" || TBAmt.Text == "0.00" || TBAmt.Text == "0")
                    {
                        lbl_ChkAmt.Text = "Please Write Amount!!";
                        lbl_ChkAmt.Visible = true;
                        lbl_ChkProNam.Visible = false;
                        lbl_ChkArea.Visible = false;
                        lbl_Chkcust.Visible = false;
                        lbl_ChkSalman.Visible = false;
                        lbl_Chktotl.Visible = false;
                        lbl_Chkqty.Visible = false;
                        lbl_Chksalrat.Visible = false;
                        lbl_Chksalrat.Visible = false;
                        lbl_Chkunt.Visible = false;
                    }
                    else
                    {
                        chkdetails = 1;
                    }
                }


                //Save DSR
                // {
                if (chkdetails == 1)
                {
                    Update();
                }
                // }
            }

        }

        protected void GVPro1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (ViewState["dt_adItm"] != null)
            {
                DataTable dt = (DataTable)ViewState["dt_adItm"];
                DataRow drCurrentRow = null;
                int rowIndex = Convert.ToInt32(e.RowIndex);
                if (dt.Rows.Count > 1)
                {
                    dt.Rows.Remove(dt.Rows[rowIndex]);
                    drCurrentRow = dt.NewRow();
                    ViewState["dt_adItm"] = dt;

                    GVPro1.DataSource = dt;
                    GVPro1.DataBind();

                    SetPreRow();
                    //ptnSno();
                }
            }
        }

        protected void GVDSR_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lnkDon = e.Row.FindControl("lnkSelect") as LinkButton;
                HiddenField hflock = e.Row.FindControl("hflock") as HiddenField;
                if (hflock.Value == "1")
                {
                    lnkDon.Enabled = false;
                }
             
            }
        }

        protected void TextBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                for (int j = 0; j < GVPro1.Rows.Count; j++)
                {
                    DropDownList DDL_Itm = (DropDownList)GVPro1.Rows[j].FindControl("DDL_Itm");
                    TextBox TextBox1 = (TextBox)GVPro1.Rows[j].FindControl("TextBox1");

                    TextBox TBQty = (TextBox)GVPro1.Rows[j].FindControl("TBQty");

                    TextBox TbSalRat = (TextBox)GVPro1.Rows[j].FindControl("TbSalRat");


                    Label lbl_Flag = (Label)GVPro1.Rows[j].FindControl("lbl_Flag");


                    string query = "select tbl_dstk.ProductID,Dstk_ItmQty as [Qty], isnull(SalePrice,0) as [Rate] " +
                                   " from tbl_dstk inner join Products on tbl_dstk.ProductID = Products.ProductID where ProductName = '" + TextBox1.Text.Trim() + "' and tbl_dstk.CompanyId = '" + Session["CompanyID"] + "' and tbl_dstk.BranchId= '" + Session["BranchID"] + "'";

                    SqlCommand cmd = new SqlCommand(query, con);
                    DataTable dt_ = new DataTable();
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);

                    adp.Fill(dt_);

                    if (dt_.Rows.Count > 0)
                    {
                        
                        TbSalRat.Text = dt_.Rows[0]["Rate"].ToString();

                        if (lbl_Flag.Text == "0")
                        {
                            TBQty.Text = dt_.Rows[0]["Qty"].ToString();
                        }
                    }
                    TBQty.Focus();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                //lbl_Heading.Text = "Error!";
                lblalert.Text = ex.Message;
                //lbl_err.Text = ex.Message.ToString();
            }
        }

        protected void GVDSR_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GVDSR.PageIndex = e.NewPageIndex;
            FillGrid();
        }

        protected void TBSalRtrn_TextChanged(object sender, EventArgs e)
        {

            try
            {
                int rowIndex = 0;
                string saleper = "";

                TextBox TBQty = (TextBox)GVPro1.Rows[rowIndex].Cells[3].FindControl("TBQty");
                TextBox TBSalRat = (TextBox)GVPro1.Rows[rowIndex].Cells[4].FindControl("TBSalRat");
                TextBox TBAmt = (TextBox)GVPro1.Rows[rowIndex].Cells[8].FindControl("TBAmt");
         
                TBQty.Text = (Convert.ToInt32(TBQty.Text) - Convert.ToInt32(TBSalRtrn.Text)).ToString();

                if (lblOutstan.Text == "0" || lblOutstan.Text == "0.00")
                {
                    TBAmt.Text = (Convert.ToInt32(TBQty.Text.Trim()) * Convert.ToInt32(TBSalRat.Text.Trim())).ToString();
                }
                else
                {
                    TBAmt.Text = (Convert.ToInt32(TBQty.Text.Trim()) * Convert.ToInt32(TBSalRat.Text.Trim())).ToString();
                    TBAmt.Text = (Convert.ToInt32(TBAmt.Text.Trim()) - Convert.ToInt32(lblOutstan.Text.Trim())).ToString();
                }

                string query = " select saleper,* from Customers_ where CustomerID=" + DDL_Cust.SelectedValue.Trim() + " and CompanyId = '" + Session["CompanyID"] + "' and BranchId= '" + Session["BranchID"] + "'";

                SqlCommand command = new SqlCommand(query, con);
                con.Open();
                DataTable dt_ = new DataTable();
                SqlDataAdapter adp = new SqlDataAdapter(command);
                adp.Fill(dt_);
                command.ExecuteNonQuery();

                if (dt_.Rows.Count > 0)
                {
                    saleper = dt_.Rows[0]["saleper"].ToString();
                }
                else
                {
                    saleper = "0.00";
                }
                con.Close();


                float GTotal = 0;
                for (int k = 0; k < GVPro1.Rows.Count; k++)
                {
                    TextBox total = (TextBox)GVPro1.Rows[k].FindControl("TBAmt");

                    double discount = Convert.ToDouble(total.Text) * Convert.ToDouble(saleper) / 100;
                    string ttlamt = (Convert.ToDouble(total.Text) - discount).ToString();

                    GTotal += Convert.ToSingle(ttlamt);
                    tbtotal.Text = GTotal.ToString();
                }

            }
            catch (Exception ex)
            {
                lbl_err.Text = ex.Message;
            }


        }

        protected void TB_DSR_TextChanged(object sender, EventArgs e)
        {
            if (TB_DSR.Text != "")
            {
                try
                {
                    //Sales Order
                    using (SqlCommand cmd = new SqlCommand())
                    {

                        string lvl = Session["Level"].ToString();

                        if (lvl == "1")
                        {

                            cmd.CommandText = " select ROW_NUMBER() OVER(ORDER BY (select 1)) AS ID, tbl_Mdsr.dsrid, tbl_Mdsr.CustomerID,Isdon,CustomerName,convert(varchar, dsrdat, 111) as [dsrdat] from tbl_Mdsr inner join Customers_ on tbl_Mdsr.CustomerID = Customers_.CustomerID where tbl_Mdsr.CompanyId = '" + Session["CompanyID"] + "' and tbl_Mdsr.BranchId= '" + Session["BranchID"] + "' and CustomerName='" + TB_DSR.Text.Trim() + "' order by dsrid desc ";
                        }

                        cmd.Connection = con;
                        con.Open();

                        DataTable dtSal_ = new DataTable();

                        SqlDataAdapter adp = new SqlDataAdapter(cmd);
                        adp.Fill(dtSal_);

                        GVDSR.DataSource = dtSal_;
                        GVDSR.DataBind();
                        ViewState["DSR"] = dtSal_;

                        con.Close();
                    }


                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else if (TB_DSR.Text == "")
            {
                FillGrid();
            }
        }
    }
}