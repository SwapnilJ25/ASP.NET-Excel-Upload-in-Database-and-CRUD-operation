using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

public partial class Excel_Upload : System.Web.UI.Page
{
    public static SqlConnection con = new SqlConnection("Data Source=10.20.50.117;Initial Catalog=TraineeData;Persist Security Info=True;User ID=traineeuser;Password=Tra!nee$0107");

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [WebMethod]
    public static List<int> upload_text(string file_name)
    {
        int total_rows_before = 0;
        int total_rows_after = 0;
        int total_valid_rows = 0;
        List<int> countList = new List<int>();
        string filePath = HttpContext.Current.Server.MapPath("/uploaded_file/" + file_name);
        con.Open();
        string query = "SELECT COUNT(ID) FROM ASP_Insert_data";
        using (SqlCommand command = new SqlCommand(query, con))
        {
            total_rows_before = (int)command.ExecuteScalar();
            //countList.Add(total_rows_before);
        }
        

        FileInfo fileInfo = new FileInfo(filePath);

        using (ExcelPackage package = new ExcelPackage(fileInfo))
        {
            var worksheet = package.Workbook.Worksheets.FirstOrDefault(ws => ws.Name == "Sheet1");

            int rowCount = worksheet.Cells.Count();

            for (int row = 2; row <= rowCount; row++)
            {
                if (worksheet.Cells[row, 1].Text.Length != 0)
                {
                    //string sqlInsert = "INSERT INTO ASP_Insert_data (Name, Age, DOB, Mobile, Email, City, Address, Remark) " +
                    //                   "VALUES (@Name, @Age, @DOB, @Mobile, @Email, @City, @Address, @Remark)";

                    string sqlInsert = "asp_swapnil";
                    

                    using (SqlCommand cmd = new SqlCommand(sqlInsert, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Name", worksheet.Cells[row, 1].Text);
                        cmd.Parameters.AddWithValue("@Age", worksheet.Cells[row, 2].Text);
                        cmd.Parameters.AddWithValue("@DOB", worksheet.Cells[row, 3].Text);
                        cmd.Parameters.AddWithValue("@Mobile", worksheet.Cells[row, 4].Text);
                        cmd.Parameters.AddWithValue("@Email", worksheet.Cells[row, 5].Text);
                        cmd.Parameters.AddWithValue("@City", worksheet.Cells[row, 6].Text);
                        cmd.Parameters.AddWithValue("@Address", worksheet.Cells[row, 7].Text);
                        //cmd.Parameters.AddWithValue("@Remark", worksheet.Cells[row, 8].Text);

                        cmd.ExecuteNonQuery();
                        
                    }
                }
            }
            string query1 = "SELECT COUNT(ID) FROM ASP_Insert_data";
            using (SqlCommand command = new SqlCommand(query1, con))
            {
                total_rows_after = (int)command.ExecuteScalar();
                //countList.Add(total_rows_after);
            }

        }

        
        int total_rows = total_rows_after - total_rows_before;
        countList.Add(total_rows);


        string query2 = "select count(ID) from ASP_Insert_data where Remark is NULL and ID between '"+total_rows_before+"' and '"+total_rows_after+"'";
        using (SqlCommand command = new SqlCommand(query2, con))
        {
            total_valid_rows = (int)command.ExecuteScalar();
            //countList.Add(total_rows_after);
        }
        con.Close();
        countList.Add(total_valid_rows);
        int total_invalid_rows = total_rows - total_valid_rows;
        countList.Add(total_invalid_rows);
        countList.Add(total_rows_before);
        countList.Add(total_rows_after);
        return countList;
    }





    [WebMethod]
    public static string Get_total_row(int total_rows_before, int total_rows_after)
    {
        total_rows_before = total_rows_before + 1;
        string query = "SELECT * FROM ASP_Insert_data WHERE ID BETWEEN '" + total_rows_before + "' AND '" + total_rows_after + "' ";
        return commonfun(query, "All_data");
    }








    public static string commonfun(string query, string filename)
    {


        con.Open();

        DataTable dataTable = new DataTable();

        using (SqlCommand command = new SqlCommand(query, con))
        {
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {

                adapter.Fill(dataTable);
            }
        }

        con.Close();

        // Construct the directory path
        string directoryPath = HttpContext.Current.Server.MapPath("~/uploaded_file/");
        string FilePath = directoryPath + filename + ".xlsx";

        // Ensure the directory exists, or create it
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }


        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Sheet1");

            worksheet.Cells["A1"].LoadFromDataTable(dataTable, true);

            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }

            File.WriteAllBytes(FilePath, package.GetAsByteArray());

        }
        return filename + ".xlsx";
    }

}