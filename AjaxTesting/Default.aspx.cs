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
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


//using System;
//using System.Net.Http;

//using (HttpClient client = new HttpClient())
//        {
//            string apiUrl = "http://localhost:51267/crud/get";
//            HttpResponseMessage response = client.GetAsync(apiUrl).Result;
//            if (response.IsSuccessStatusCode)
//            {
//                string data = response.Content.ReadAsStringAsync().Result;
//                return "data fetched successfully";
//            }
//        }

public partial class _Default : System.Web.UI.Page
{
    public static SqlConnection con = new SqlConnection("Data Source=10.20.50.117;Initial Catalog=TraineeData;Persist Security Info=True;User ID=traineeuser;Password=Tra!nee$0107");


    public static string tokenvalidornot()
    {

        string sessionToken = HttpContext.Current.Session["token"].ToString();
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.ReadToken(sessionToken) as JwtSecurityToken;

        string sessionrefreshToken = HttpContext.Current.Session["refreshToken"].ToString();

       


        // Get the expiration date
        DateTime? tokenExpiration = token.ValidTo;
        if (tokenExpiration.HasValue && tokenExpiration > DateTime.UtcNow)
        {
            return "token not expired";
        }
        else
        {
            //HttpContext.Current.Response.Redirect("/Login.aspx");
            return "token expired"; 
        }
    }







    [WebMethod]
    public static string Add(string name, string pass, int age, int id)
    {
        string tokenvalidity = tokenvalidornot();
        if (tokenvalidity == "token not expired")
        {
            int entryby = 2;
            if (id == -45)//To add Data
            {


                SqlConnection con = new SqlConnection("Data Source=10.20.50.117;Initial Catalog=TraineeData;Persist Security Info=True;User ID=traineeuser;Password=Tra!nee$0107");
                con.Open();
                SqlCommand cmd = new SqlCommand("insert into ASP_DOT_Project values(@name,@pass,@age,@entryby)", con);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@pass", pass);
                cmd.Parameters.AddWithValue("@age", age);
                cmd.Parameters.AddWithValue("@entryby", entryby);

                cmd.ExecuteNonQuery();
                con.Close();
                //getdate();
                //MessageBox.Show("successfully Saved");
                //getdate();
                return "successfully Saved";
            }
            else//To update Datagetdate()
            {
                SqlConnection con = new SqlConnection("Data Source=10.20.50.117;Initial Catalog=TraineeData;Persist Security Info=True;User ID=traineeuser;Password=Tra!nee$0107");
                con.Open();
                SqlCommand cmd = new SqlCommand("update ASP_DOT_Project set Name=@name , password=@pass, Age=@age where ID=@id", con);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@pass", pass);
                cmd.Parameters.AddWithValue("@age", age);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                con.Close();
                return "successfully updated";
            }
        }
            else
            {
                return "token expired";
            }
        
    }

    [WebMethod]
    public static string Del(int id)
    {
        SqlConnection con = new SqlConnection("Data Source=10.20.50.117;Initial Catalog=TraineeData;Persist Security Info=True;User ID=traineeuser;Password=Tra!nee$0107");
        con.Open();
        SqlCommand cmd = new SqlCommand("delete from ASP_DOT_Project where ID=@id", con);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();
        con.Close();
        return "successfully Deleted";
    }


    [WebMethod]
    public static string Update(int id)
    {
        con.Open();
        string name = "";
        string pass = "";
        int age = -1;

        List<object> results = new List<object>();
        SqlCommand cmd = new SqlCommand("select name,password, age from ASP_DOT_Project where ID = @id ", con);
        cmd.Parameters.AddWithValue("@id", id);

        using (SqlDataReader reader = cmd.ExecuteReader())
        {
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    name = reader["name"].ToString();
                    pass = reader["password"].ToString();
                    age = (int)reader["age"];
                    var result = new
                    {
                        Name = name,
                        Pass = pass,
                        Age = age,
                        id=id
                    };

                    results.Add(result);
                }
            }
        }
        con.Close();
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        string json = serializer.Serialize(results);

        return json;
    }





    //function which gives sum of column


    public static float getsum(string data)
    {
        string imp = data;
        float sum = 0;
        DataTable dt = new DataTable();
        con.Open();
        SqlCommand cmd = new SqlCommand("select fund,SCHEMECODE,SchemeFolio, sum(Price)Price, sum(Units)Units, sum(INV_Amount)INV_Amount, sum(stamp_duty)stamp_duty, sum(total_amount_sd)total_amount_sd, sum(STT)STT, sum(Sch_Folio_Unitbal)Sch_Folio_Unitbal, sum(Present_Value)Present_Value, sum([Notional P/L])[Notional P/L] from portfolioData group by fund,SCHEMECODE,SchemeFolio", con);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        con.Close();
        da.Fill(dt);

        try
        {
            sum = dt.AsEnumerable().Sum(row => Convert.ToSingle(row[data]));
        }
        catch (InvalidCastException ex)
        {
            
            sum = 0;
        }
        return sum;
    }




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


   




    [WebMethod]
    public static string getdate()
    {
        DataTable dt = new DataTable();
        con.Open();
        SqlCommand cmd = new SqlCommand("select * from ASP_DOT_Project ", con);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        con.Close();
        da.Fill(dt);
        string html = "<table id='tbl' class=\"table table-bordered mt-3\" style=\"background-color:#B4A69B\"><thead><tr>";
        //table1.text=dt
        for (int i = 0; i < dt.Columns.Count; i++)
        {
            html = html + "<th scope=\"col\">" + dt.Columns[i] + "</th >";
        }
        html = html + "<th  scope=\"col\">Operation</th></tr></thead><tbody>";
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            html = html + "<tr>";
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                html = html + "<td>" + dt.Rows[i][j] + "</td>";
            }
            html = html + "<td><button type=\"button\" data-id='" + dt.Rows[i][0] + "' class=\"btn btn-secondary m-2 updbtn\" id=\"btn_upd\">update</button>\r\n<button type=\"button\" class=\"btn btn-success m-2 btn_del_del\" data-id='" + dt.Rows[i][0] + "' id=\"btn_del\">delete</button></td></tr>";
        }

        html = html + "</tbody></table>";
        con.Close();

        return html;
    }







    
}




