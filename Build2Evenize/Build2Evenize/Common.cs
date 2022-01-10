using System;
using System.Collections;
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
    public void InstitutionCountry(string institution, Label label)
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
    public void FillInstitution(ComboBox Partner1, ComboBox Partner2, ComboBox Partner3)
    {
        string query = "select distinct name from Institution";
        cmd = new SqlCommand(query, con);
        dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            //add every row found on database to combobox
            string name = (string)dr[0];
            Partner1.Items.Add(name);
            Partner2.Items.Add(name);
            Partner3.Items.Add(name);
        }
        dr.Close();
        dr.Dispose();
    }
    public void Partner(int id, ComboBox Partner1, ComboBox Partner2, ComboBox Partner3, Button Delete1, Button Delete2, Button Delete3, Button Add1, Button Add2)
    {
        string query = "select I.institution_id,I.name from Project_Partner PP JOIN Institution I on PP.institution_id = I.institution_id where project_id = "+ id;
        cmd = new SqlCommand(query, con);
        dr = cmd.ExecuteReader();
        int partnersNumber = 0;
        ArrayList name_list = new ArrayList();
        ArrayList id_list = new ArrayList();

        while (dr.Read())
        {
            //add every row found on database to combobox
            int institution_id = (int)dr[0];
            string institution_name = (string)dr[1];
            name_list.Add(institution_name);
            id_list.Add(institution_id);
            partnersNumber++;
        }
        dr.Close();
        dr.Dispose();
        PartnerNumber((int)id_list[0], Partner1);
        
        switch (partnersNumber)
        {
            case 0:
                break;
            case 1:
                Partner2.Visible = false;
                Partner3.Visible = false;
                Add1.Visible = true;
                Delete1.Visible = true;
                break;
            case 2:
                PartnerNumber((int)id_list[1], Partner2);
                Partner1.Visible = true;
                Partner2.Visible = true;
                Partner3.Visible = false;
                Delete2.Visible = true;
                Add2.Visible = true;
                break;
            case 3:
                PartnerNumber((int)id_list[1], Partner2);
                PartnerNumber((int)id_list[2], Partner3);
                Partner1.Visible = true;
                Partner2.Visible = true;
                Partner3.Visible = true;
                Delete3.Visible = true;
                break;
        }

    }
    public void PartnerNumber(int id, ComboBox Partner)
    {
        string query = "select name from Institution where institution_id = " + id;
        cmd = new SqlCommand(query, con);
        dr = cmd.ExecuteReader();

        if (dr.Read())
        {
            Partner.SelectedItem = dr.GetString(0);
        }
        dr.Close();
        dr.Dispose();
    }

}
