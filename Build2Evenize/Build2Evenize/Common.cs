using System;
using System.Data.SqlClient;
using System.Windows.Forms;

public class Common
{
    public SqlConnection con;
    public string server = @"build2evenize.database.windows.net";
    public string database = "build2evenize";
    public string user = @"ispg4259";
    public string pass = "BUILD2evenize";
    public SqlCommand cmd;
    public SqlDataReader dr;
    public void Connection()
    {
        String str = $"server={server};database={database};UID={user};password={pass};Trusted_Connection=False;Encrypt=True;MultipleActiveResultSets=true;";
        con = new SqlConnection(str);
        con.Open();
    }
	public void Filters(string table, string columnName, ComboBox comboBox)
    {
        // query to get columnname from table and fill in the combobox items
        string query = "select distinct " + columnName + " from " + table;
        cmd = new SqlCommand(query, con);
        dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            //add every row found on database to combobox
            string name = (string)dr[columnName];
            comboBox.Items.Add(name);
        }
        dr.Close();
        dr.Dispose();
    }
    public void Country(string institution, Label label)
    {
        string query = "select country from Institution where name LIKE '" + institution + "'";
        cmd = new SqlCommand(query, con);
        dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            label.Text = dr.GetString(0);
        }
        dr.Close();
        dr.Dispose();
    }
    public int Partner(int id, ComboBox comboBox)
    {
        string query = "select I.name from Project_Partner PP JOIN Institution I on PP.institution_id = I.institution_id where project_id = "+ id;
        cmd = new SqlCommand(query, con);
        dr = cmd.ExecuteReader();
        int partnersNumber = 0;
        while (dr.Read())
        {
            //add every row found on database to combobox
            string name = (string)dr[0];
            comboBox.Items.Add(name);
            partnersNumber++;
        }
        dr.Close();
        dr.Dispose();

        return partnersNumber;
    }
}
