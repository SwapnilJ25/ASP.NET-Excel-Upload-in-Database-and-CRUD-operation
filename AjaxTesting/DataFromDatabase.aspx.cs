using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Protocols.WSTrust;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Data;
using System.Linq;





public partial class _Default : System.Web.UI.Page
{
    public static SqlConnection con = new SqlConnection("Data Source=10.20.50.117;Initial Catalog=TraineeData;Persist Security Info=True;User ID=traineeuser;Password=Tra!nee$0107");


    public static DataTable getquerydata(string query)
    {
        DataTable dt = new DataTable();
        con.Open();
        SqlCommand cmd = new SqlCommand(query, con);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        con.Close();
        da.Fill(dt);
        return dt;
    }


    //get total of columns at the end


    //[WebMethod]
    //public static string getdate()
    //{
    //    DataTable dt,dt1 = new DataTable();
    //    dt4 = getquerydata("select * from portfolioData");
    //    dt = getquerydata("select fund,SCHEMECODE,SchemeFolio, sum(Price)Price, sum(Units)Units, sum(INV_Amount)INV_Amount, sum(stamp_duty)stamp_duty, sum(total_amount_sd)total_amount_sd, sum(STT)STT, sum(Sch_Folio_Unitbal)Sch_Folio_Unitbal, sum(Present_Value)Present_Value, sum([Notional P/L])[Notional P/L] from portfolioData group by fund,SCHEMECODE,SchemeFolio");
    //    dt1 = getquerydata("select distinct(schemecode) from portfolioData");

    //    string html = "<table id='tbl' class=\"table table-bordered mt-3\" style=\"background-color:#B4A69B\"><thead><tr>";
    //    for (int i = 0; i < dt4.Columns.Count; i++)
    //    {
    //        html = html + "<th scope=\"col\">" + dt4.Columns[i] + "</th >";
    //    }
    //    for (int i = 0; i < dt4.Rows.Count; i++)
    //    {
    //        {
    //            html = html + "<tr>";
    //            for (int j = 0; j < dt4.Columns.Count; j++)
    //            {

    //                html = html + "<td>" + dt4.Rows[i][j] + "</td>";

    //            }
    //        }
    //    }

    //    html = html + "<tr>";
    //    html = html + "<td>" + "<b>Total</b>" + "</td>";

    //for (int i = 1; i < dt.Columns.Count; i++)
    //{

    //    if (dt.Columns[i].ToString() != "SchemeFolio")
    //    {
    //        string checktype = dt4.Columns[i].ToString();

    //        html = html + "<td>" + "<b>" + getsum(checktype).ToString() + "</b>" + "</td>";
    //    }
    //    else
    //    {
    //        html = html + "<td>" + "" + "</td>";
    //    }

    //}



    //    html = html + "</tbody></table>";
    //    con.Close();

    //    return html;
    //}




    //[WebMethod]
    //public static string getdate()
    //{
    //    DataTable dt = new DataTable();
    //    con.Open();
    //    SqlCommand cmd = new SqlCommand("select * from portfolioData ", con);
    //    SqlDataAdapter da = new SqlDataAdapter(cmd);
    //    con.Close();
    //    da.Fill(dt);
    //    string html = "<table id='tbl' class=\"table table-bordered mt-3\" style=\"background-color:#B4A69B\"><thead><tr>";
    //    //table1.text=dt
    //    for (int i = 0; i < dt.Columns.Count; i++)
    //    {
    //        html = html + "<th scope=\"col\">" + dt.Columns[i] + "</th >";
    //    }
    //    //html = html + "<th  scope=\"col\">Operation</th></tr></thead><tbody>";
    //    for (int i = 0; i < dt.Rows.Count; i++)
    //    {
    //        html = html + "<tr>";
    //        for (int j = 0; j < dt.Columns.Count; j++)
    //        {
    //            html = html + "<td>" + dt.Rows[i][j] + "</td>";
    //        }
    //        //html = html + "<td><button type=\"button\" data-id='" + dt.Rows[i][0] + "' class=\"btn btn-secondary m-2 updbtn\" id=\"btn_upd\">update</button>\r\n<button type=\"button\" class=\"btn btn-success m-2 btn_del_del\" data-id='" + dt.Rows[i][0] + "' id=\"btn_del\">delete</button></td></tr>";
    //    }

    //    html = html + "</tbody></table>";
    //    con.Close();

    //    return html;
    //}

    // This function is used in getting sum of each column
    private static double ConvertToDouble(object value)
    {
        if (value == null || value == DBNull.Value)
        {
            return 0.0;
        }
        if (value is double)
        {
            return (double)value;
        }
        if (value is int)
        {
            return (double)(int)value;
        }
        return 0.0;
    }




    [WebMethod]
    public static string getdate()
    {
        DataTable dataTable = new DataTable();
        dataTable = getquerydata("select * from portfolioData");
        List<string> transactionList = new List<string>();

        transactionList.Add("Transaction Type");
        transactionList.Add("Transaction Date");
        transactionList.Add("Transaction Price (INR)");
        transactionList.Add("No. of Units");
        transactionList.Add("Purchase Value (INR)");
        transactionList.Add("Stamp duty (INR)");
        transactionList.Add("Total Amount (inc of SD) (INR)");
        transactionList.Add("Current Amount (INR)");
        transactionList.Add("Notional G/L(INR)");
        //transactionList.Add("Absolute Return(%)");
        //transactionList.Add("XIRR");
        con.Close();


        string[] FUND = dataTable.AsEnumerable()
        .Select(row => row.Field<string>("FUND"))
        .Distinct()
        .ToArray();





        List<List<string>> Fund_with_S_name = new List<List<string>>();

        for (int i = 0; i < FUND.Length; i++)
        {
            List<string> templist = new List<string>();
            templist.Add(FUND[i]);
            string[] S_Name = dataTable.AsEnumerable()
                .Where(row => row.Field<string>("FUND") == FUND[i])
               .Select(row => row.Field<string>("S_Name"))
               .Distinct()
               .ToArray();

            string[] foliono = dataTable.AsEnumerable()
                .Where(row => row.Field<string>("FUND") == FUND[i])
                .Select(row => row.Field<string>("Folio_Number"))
               .Distinct()
               .ToArray();

            templist.AddRange(S_Name);
            //templist.AddRange(foliono);
            Fund_with_S_name.Add(templist);

        }








        string html = "<table id='tbl' class=\"table table-bordered table-hover mt-3\" style=\"background-color:#B4A69B\"><thead><tr>";
        for (int a = 0; a < transactionList.Count; a++)
        {
            html = html + "<th scope=\"col\">" + (transactionList[a]) + "</th >";
        }
        html = html + "</tr></thead><tbody>";
        for (int b = 0; b < Fund_with_S_name.Count; b++)
        {
            if (Fund_with_S_name[b].Count == 2)
            {
                for (int c = 0; c < Fund_with_S_name[b].Count; c++)
                {
                    html = html + "<tr><td colspan=11>" + Fund_with_S_name[b][c] + "</td></tr>";


                }
                string y = Fund_with_S_name[b][1];
                DataTable dt_schemes = dataTable.Select("s_name = '" + y + "'").CopyToDataTable();
                int rowCount = dt_schemes.Rows.Count;
                int colCount = dt_schemes.Columns.Count;
                string[] selectedColumns = { "trxn_type_Name", "Trxn_Date", "Price", "Units", "Price", "stamp_duty", "total_amount_sd", "present_value", "Notional P/L" };

                foreach (DataRow row in dt_schemes.Rows)
                {
                    html = html + "<tr>";
                    foreach (string colName in selectedColumns)
                    {
                        html = html + "<td>" + (row[colName]) + "</td>";
                    }
                    html = html + "</tr>";

                }

                // To get sum of each column
                double pricesum = dataTable.AsEnumerable()
                    .Where(row => row.Field<string>("S_Name") == Fund_with_S_name[b][1])
             .Sum(row => ConvertToDouble(row["Price"]));

                double unitsum = dataTable.AsEnumerable()
                     .Where(row => row.Field<string>("S_Name") == Fund_with_S_name[b][1])
              .Sum(row => ConvertToDouble(row["units"]));

                double stampdutysum = dataTable.AsEnumerable()
                      .Where(row => row.Field<string>("S_Name") == Fund_with_S_name[b][1])
               .Sum(row => ConvertToDouble(row["stamp_duty"]));

                double totalamtsum = dataTable.AsEnumerable()
                       .Where(row => row.Field<string>("S_Name") == Fund_with_S_name[b][1])
                .Sum(row => ConvertToDouble(row["total_amount_sd"]));

                double presentsum = dataTable.AsEnumerable()
                        .Where(row => row.Field<string>("S_Name") == Fund_with_S_name[b][1])
                 .Sum(row => ConvertToDouble(row["Present_Value"]));

                double notionalsum = dataTable.AsEnumerable()
                         .Where(row => row.Field<string>("S_Name") == Fund_with_S_name[b][1])
                  .Sum(row => ConvertToDouble(row["Notional P/L"]));



                html = html + "<tr>";
                html = html + "<td>" + "<b>Total</b>" + "</td>";
                html = html + "<td>" + "" + "</td>";
                html = html + "<td>" + pricesum + "</td>";
                html = html + "<td>" + unitsum + "</td>";
                html = html + "<td>" + pricesum + "</td>";
                html = html + "<td>" + stampdutysum + "</td>";
                html = html + "<td>" + totalamtsum + "</td>";
                html = html + "<td>" + presentsum + "</td>";
                html = html + "<td>" + notionalsum + "</td>";
                html = html + "</tr>";





            }



            else
            {
                html = html + "</tr><td colspan=11>" + Fund_with_S_name[b][0] + "</td></tr>";
                for (int c = 1; c < Fund_with_S_name[b].Count; c++)
                {
                    html = html + "</tr><td colspan=11>" + Fund_with_S_name[b][c] + "</td></tr>";



                    string y = Fund_with_S_name[b][1];
                    DataTable dt_schemes = dataTable.Select("s_name = '" + y + "'").CopyToDataTable();
                    int rowCount = dt_schemes.Rows.Count;
                    int colCount = dt_schemes.Columns.Count;
                    string[] selectedColumns = { "trxn_type_Name", "Trxn_Date", "Price", "Units", "Price", "stamp_duty", "total_amount_sd", "present_value", "Notional P/L" };

                    foreach (DataRow row in dt_schemes.Rows)
                    {
                        html = html + "<tr>";
                        foreach (string colName in selectedColumns)
                        {
                            html = html + "<td>" + (row[colName]) + "</td>";
                        }
                        html = html + "</tr>";

                    }


                    double pricesum = dataTable.AsEnumerable()
                 .Where(row => row.Field<string>("S_Name") == Fund_with_S_name[b][1])
          .Sum(row => ConvertToDouble(row["Price"]));

                    double unitsum = dataTable.AsEnumerable()
                         .Where(row => row.Field<string>("S_Name") == Fund_with_S_name[b][1])
                  .Sum(row => ConvertToDouble(row["units"]));

                    double stampdutysum = dataTable.AsEnumerable()
                          .Where(row => row.Field<string>("S_Name") == Fund_with_S_name[b][1])
                   .Sum(row => ConvertToDouble(row["stamp_duty"]));

                    double totalamtsum = dataTable.AsEnumerable()
                           .Where(row => row.Field<string>("S_Name") == Fund_with_S_name[b][1])
                    .Sum(row => ConvertToDouble(row["total_amount_sd"]));

                    double presentsum = dataTable.AsEnumerable()
                            .Where(row => row.Field<string>("S_Name") == Fund_with_S_name[b][1])
                     .Sum(row => ConvertToDouble(row["Present_Value"]));

                    double notionalsum = dataTable.AsEnumerable()
                             .Where(row => row.Field<string>("S_Name") == Fund_with_S_name[b][1])
                      .Sum(row => ConvertToDouble(row["Notional P/L"]));

                    html = html + "<tr>";
                    html = html + "<td>" + "<b>Total</b>" + "</td>";
                    html = html + "<td>" + "" + "</td>";
                    html = html + "<td>" + pricesum + "</td>";
                    html = html + "<td>" + unitsum + "</td>";
                    html = html + "<td>" + pricesum + "</td>";
                    html = html + "<td>" + stampdutysum + "</td>";
                    html = html + "<td>" + totalamtsum + "</td>";
                    html = html + "<td>" + presentsum + "</td>";
                    html = html + "<td>" + notionalsum + "</td>";
                    html = html + "</tr>";

                }
            }


        }
        html = html + "</tbody>";
        con.Close();

        return html;
    }






}




