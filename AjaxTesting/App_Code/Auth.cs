

using System.Data.SqlClient;
using System.Drawing;

public class _Auth_user
{
     
    public bool Logindata(string loginid, string pass)
    {
        SqlConnection con = new SqlConnection("Data Source=10.20.50.117;Initial Catalog=TraineeData;Persist Security Info=True;User ID=traineeuser;Password=Tra!nee$0107");
        con.Open();
        SqlCommand cmd = new SqlCommand("select name,password from ASP_DOT_Project where name = @loginid ", con);
        cmd.Parameters.AddWithValue("@loginid", loginid);
        
        object result = cmd.ExecuteScalar();
        con.Close();

        if (result != null)
        {
            return true;
        }
        return false;
    }
}