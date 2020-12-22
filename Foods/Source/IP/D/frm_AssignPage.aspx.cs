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
using Foods;
using DataAccess;

namespace Foods
{
    public partial class frm_AssignPage : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["D"].ConnectionString);
        string query;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDll();
            }
        }

        public void BindDll()
        {
            try
            {
                /// For User
                /// ...
                /// 

                using (SqlCommand cmd = new SqlCommand())
                {

                    DataTable dtUsr = new DataTable();

                    //string usrqy = "select  Username,CompanyId,BranchId  from Users where CompanyId = '" + Session["CompanyID"] + "' and BranchId= '" + Session["BranchID"] + "'";

                    cmd.CommandText = " select  Username,CompanyId,BranchId  from Users  ";

                    cmd.Connection = con;
                    con.Open();


                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dtUsr);

                    if (dtUsr.Rows.Count > 0)
                    {

                        DDL_User.DataSource = dtUsr;
                        DDL_User.DataTextField = "Username";
                        DDL_User.DataValueField = "Username";
                        DDL_User.DataBind();
                        DDL_User.Items.Add(new ListItem("--Select Users--", "0"));
                    }

                    con.Close();
                }

                /// For Menu
                /// ...
                /// 

                using (SqlCommand cmd = new SqlCommand())
                {

                    DataTable dtMen = new DataTable();

                    //string usrqy = "select  Username,CompanyId,BranchId  from Users where CompanyId = '" + Session["CompanyID"] + "' and BranchId= '" + Session["BranchID"] + "'";

                    cmd.CommandText = " select MenuId,Title from tbl_Menu  ";

                    cmd.Connection = con;
                    con.Open();


                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dtMen);

                    if (dtMen.Rows.Count > 0)
                    {
                        DDL_Mod.DataSource = dtMen;
                        DDL_Mod.DataTextField = "Title";
                        DDL_Mod.DataValueField = "MenuId";
                        DDL_Mod.DataBind();
                        DDL_Mod.Items.Add(new ListItem("--Select Menu--", "0"));

                        DDL_Modules.DataSource = dtMen;
                        DDL_Modules.DataTextField = "Title";
                        DDL_Modules.DataValueField = "MenuId";
                        DDL_Modules.DataBind();
                        DDL_Modules.Items.Add(new ListItem("--Select Menu--", "0"));
                    }

                    con.Close();
                }

                /// For Sub Menu
                /// ...
                /// 

                using (SqlCommand cmd = new SqlCommand())
                {

                    DataTable dtsubMen = new DataTable();

                    //string usrqy = "select  Username,CompanyId,BranchId  from Users where CompanyId = '" + Session["CompanyID"] + "' and BranchId= '" + Session["BranchID"] + "'";

                    cmd.CommandText = " select submenuid,SubMenuNam from tbl_SubMenu  ";

                    cmd.Connection = con;
                    con.Open();


                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dtsubMen);

                    if (dtsubMen.Rows.Count > 0)
                    {
                        DDL_Pages.DataSource = dtsubMen;
                        DDL_Pages.DataTextField = "SubMenuNam";
                        DDL_Pages.DataValueField = "submenuid";
                        DDL_Pages.DataBind();
                        DDL_Pages.Items.Add(new ListItem("--Select Pages--", "0"));
                    }

                    con.Close();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void DDL_User_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                DataTable dtusrmen = new DataTable();

                using (SqlCommand cmd = new SqlCommand())
                {

                    cmd.CommandText = " select * from tbl_menusr inner join tbl_Menu on tbl_menusr.MenuId = tbl_Menu.MenuId where Username= '" +  DDL_User.SelectedValue.Trim() + "' and IsAssign='1'";

                    cmd.Connection = con;
                    con.Open();

                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dtusrmen);

                    if (dtusrmen.Rows.Count > 0)
                    {
                        GVMod.DataSource = dtusrmen;
                        GVMod.DataBind();

                        //for (int j = 0; j < dtusrmen.Rows.Count; j++)
                        //{
                        //    string HFIsAssign = dtusrmen.Rows[j]["IsAssign"].ToString();
                        //}

                        //for (int j = 0; j < dtusrmen.Rows.Count; j++)
                        //{
                        //    for (int i = 0; i < GVMod.Rows.Count; i++)
                        //    {

                        //        CheckBox CB_Menu = (CheckBox)GVMod.Rows[i].FindControl("CB_Menu");

                        //        HiddenField HFSlctModul = (HiddenField)GVMod.Rows[i].FindControl("HFSlctModul");
                        //        HiddenField HFIsAssign = (HiddenField)GVMod.Rows[i].FindControl("HFIsAssign");

                        //        HFIsAssign.Value = dtusrmen.Rows[j]["IsAssign"].ToString();

                        //        if (HFIsAssign.Value == "1")
                        //        {
                        //            CB_Menu.Checked = true;
                        //        }


                        //        //GVSubMenu.DataSource = dtusrsubmen;
                        //        //GVSubMenu.DataBind();
                        //    }
                        //}

                    }
                    else
                    {
                        GVMod.DataSource = null;
                        GVMod.DataBind();
                    }

                    con.Close();
                }


                /// Sub Menu
                /// ...
                /// 

                DataTable dtusrsubmen = new DataTable();

                using (SqlCommand cmd = new SqlCommand())
                {

                    cmd.CommandText = " select SubMenuNam,SubMenuDesc,SubMenuUrl,* from tbl_usrmen inner join tbl_SubMenu  " +
                        " on tbl_usrmen.SubMenuId = tbl_SubMenu.SubMenuId " +
                        " where username='" +  DDL_User.SelectedValue.Trim() + "' and IsAssign= '1' ";

                    cmd.Connection = con;
                    con.Open();

                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dtusrsubmen);

                    if (dtusrsubmen.Rows.Count > 0)
                    {
                        GVSubMenu.DataSource = dtusrsubmen;
                        GVSubMenu.DataBind();
                    }
                    else
                    {
                        GVSubMenu.DataSource = null;
                        GVSubMenu.DataBind();
                    }

                    con.Close();
                }




                //if (dtmen.Rows.Count > 0)
                //{
                //    //GVModul.DataSource = dtmen;
                //    //GVModul.DataBind();

                //    for (int i = 0; i < GVModul.Rows.Count; i++)
                //    {
                //        for (int j = 0; j < dtmen.Rows.Count; j++)
                //        {
                //            CheckBox chk_slct = (CheckBox)GVModul.Rows[i].FindControl("chk_slct");
                //            //HiddenField HFSlctModul = (HiddenField)GVModul.Rows[i].FindControl("HFSlctModul");

                //            //HFSlctModul.Value = dtmen.Rows[j]["MenuId"].ToString();

                //            if (HFSlctModul.Value != "")
                //            {
                //                chk_slct.Checked = true;
                //            }
                //        }
                //    }
                //}
                //else
                //{

                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        protected void btn_Savmod_Click(object sender, EventArgs e)
        {
            /// For Saving Modules
            /// ...
            /// 

            SaveMod();
        }


        private void SaveMod()
        {
            try
            {
                /// For Checking Modules
                /// ...
                /// 

                using (SqlCommand cmd = new SqlCommand())
                {

                    DataTable dtchkmod = new DataTable();

                    //string usrqy = "select  Username,CompanyId,BranchId  from Users where CompanyId = '" + Session["CompanyID"] + "' and BranchId= '" + Session["BranchID"] + "'";

                    cmd.CommandText = "  select * from tbl_menusr  inner join tbl_Menu on tbl_menusr.MenuId = tbl_Menu.MenuId " +
                        " where username='" +  DDL_User.SelectedValue.Trim() + "' and tbl_Menu.MenuId = '" + DDL_Mod.SelectedValue.Trim() + "'";

                    cmd.Connection = con;
                    con.Open();

                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dtchkmod);

                    if (dtchkmod.Rows.Count > 0)
                    {
                        query = " update tbl_menusr set IsAssign='" + DDL_Assign.SelectedValue.Trim() + "' where username='" +  DDL_User.SelectedValue.Trim() + "' and MenuId = '" +
                            DDL_Mod.SelectedValue.Trim() + "'";

                        cmd.CommandText = query;
                        cmd.ExecuteNonQuery();

                    }
                    else
                    {
                        query = " insert into  tbl_menusr values('" +  DDL_User.SelectedValue.Trim() + "','" + DDL_Mod.SelectedValue.Trim() + "','" + DDL_Assign.SelectedValue.Trim() + "')";

                        cmd.CommandText = query;
                        cmd.ExecuteNonQuery();
                    }

                    //con.Close();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DataTable dtusrmen1 = new DataTable();

                using (SqlCommand cmd = new SqlCommand())
                {

                    cmd.CommandText = " select * from tbl_menusr inner join tbl_Menu on tbl_menusr.MenuId = tbl_Menu.MenuId where Username= '" +  DDL_User.SelectedValue.Trim() + "' and IsAssign='1'";

                    cmd.Connection = con;
                    //con.Open();

                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dtusrmen1);

                    if (dtusrmen1.Rows.Count > 0)
                    {
                        GVMod.DataSource = dtusrmen1;
                        GVMod.DataBind();
                    }
                    else
                    {
                        GVMod.DataSource = null;
                        GVMod.DataBind();
                    }

                    con.Close();
                }


            }

        }

        private void SavePages()
        {
            try
            {
                /// For Checking Pages
                /// ...
                /// 

                using (SqlCommand cmd = new SqlCommand())
                {

                    DataTable dtchkpag = new DataTable();

                    //string usrqy = "select  Username,CompanyId,BranchId  from Users where CompanyId = '" + Session["CompanyID"] + "' and BranchId= '" + Session["BranchID"] + "'";

                    cmd.CommandText = "   select SubMenuNam,SubMenuDesc,SubMenuUrl from tbl_usrmen inner join tbl_SubMenu on tbl_usrmen.SubMenuId = tbl_SubMenu.SubMenuId " +
                        " where username='" + DDL_User.SelectedValue.Trim() + "' and tbl_SubMenu.SubMenuId = '" + DDL_Pages.SelectedValue.Trim() + "'";

                    cmd.Connection = con;
                    con.Open();

                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dtchkpag);

                    if (dtchkpag.Rows.Count > 0)
                    {
                        query = " update tbl_usrmen set MenuId='" + DDL_Modules.SelectedValue.Trim() + "', SubMenuId='" + DDL_Pages.SelectedValue.Trim() + "', IsAssign='" + DDL_Assign2.SelectedValue.Trim() + "' where username='" + DDL_User.SelectedValue.Trim() + "' and SubMenuId = '" +
                            DDL_Pages.SelectedValue.Trim() + "'";

                        cmd.CommandText = query;
                        cmd.ExecuteNonQuery();

                    }
                    else
                    {
                        query = " insert into  tbl_usrmen values('" + DDL_User.SelectedValue.Trim() + "','" + DDL_Modules.SelectedValue.Trim() + "','" + DDL_Pages.SelectedValue.Trim() + "','" + DDL_Assign2.SelectedValue.Trim() + "')";

                        cmd.CommandText = query;
                        cmd.ExecuteNonQuery();
                    }

                    //con.Close();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DataTable dtusrmen1 = new DataTable();

                using (SqlCommand cmd = new SqlCommand())
                {

                    cmd.CommandText = " select * from tbl_usrmen inner join tbl_SubMenu on tbl_usrmen.SubMenuId = tbl_SubMenu.SubMenuId where Username= '" + DDL_User.SelectedValue.Trim() + "' and IsAssign='1'";

                    cmd.Connection = con;
                    //con.Open();

                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dtusrmen1);

                    if (dtusrmen1.Rows.Count > 0)
                    {
                        GVSubMenu.DataSource = dtusrmen1;
                        GVSubMenu.DataBind();
                    }
                    else
                    {
                        GVSubMenu.DataSource = null;
                        GVSubMenu.DataBind();
                    }

                    con.Close();
                }


            }

        }

        protected void btn_SavPag_Click(object sender, EventArgs e)
        {
            SavePages();
        }
    }
}