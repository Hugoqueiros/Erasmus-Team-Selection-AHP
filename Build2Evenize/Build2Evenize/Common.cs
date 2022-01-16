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
    public void Fill(string query, ComboBox cb1, ComboBox cb2, ComboBox cb3)
    {
        cmd = new SqlCommand(query, con);
        dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            //add every row found on database to combobox
            string name = (string)dr[0];
            cb1.Items.Add(name);
            cb2.Items.Add(name);
            cb3.Items.Add(name);
        }
        dr.Close();
        dr.Dispose();
    }
    public void Switcher(int check, string query, ComboBox cb1, ComboBox cb2, ComboBox cb3, Button Delete1, Button Delete2, Button Delete3, Button Add1, Button Add2)
    {
        cmd = new SqlCommand(query, con);
        dr = cmd.ExecuteReader();
        int i = 0;
        ArrayList name_list = new ArrayList();
        ArrayList id_list = new ArrayList();

        while (dr.Read())
        {
            int id = (int)dr[0];
            string name = (string)dr[1];
            name_list.Add(name);
            id_list.Add(id);
            i++;
        }
        dr.Close();
        dr.Dispose();
        if (id_list.Count != 0)
        {
            //check - 0-institution data 1-Hard Skills data 2-Soft Skills data
            if (check == 0)
            {
                Index("select name from Institution where institution_id = " + (int)id_list[0], cb1);
            }
            else if (check == 1)
            {
                Index("select name from Tech where tech_id = " + (int)id_list[0], cb1);
            }
            else if (check == 2)
            {
                Index("select name from Social_Skill where social_skill_id = " + (int)id_list[0], cb1);
            }
            //i - check if there is 0,1,2 or 3 chosen options
            switch (i)
            {
                case 0:
                    break;
                case 1:
                    cb2.Visible = false;
                    cb3.Visible = false;
                    Add1.Visible = true;
                    Delete1.Visible = true;
                    break;
                case 2:
                    if (check == 0)
                    {
                        Index("select name from Institution where institution_id = " + (int)id_list[1], cb2);
                    }
                    else if (check == 1)
                    {
                        Index("select name from Tech where tech_id = " + (int)id_list[1], cb2);
                    }
                    else if (check == 2)
                    {
                        Index("select name from Social_Skill where social_skill_id = " + (int)id_list[1], cb2);
                    }
                    cb1.Visible = true;
                    cb2.Visible = true;
                    cb3.Visible = false;
                    Delete2.Visible = true;
                    Add2.Visible = true;
                    break;
                case 3:
                    if (check == 0)
                    {
                        Index("select name from Institution where institution_id = " + (int)id_list[1], cb2);
                        Index("select name from Institution where institution_id = " + (int)id_list[2], cb3);
                    }
                    else if (check == 1)
                    {
                        Index("select name from Tech where tech_id = " + (int)id_list[1], cb2);
                        Index("select name from Tech where tech_id = " + (int)id_list[2], cb3);
                    }
                    else if (check == 2)
                    {
                        Index("select name from Social_Skill where social_skill_id = " + (int)id_list[1], cb2);
                        Index("select name from Social_Skill where social_skill_id = " + (int)id_list[2], cb3);
                    }

                    cb1.Visible = true;
                    cb2.Visible = true;
                    cb3.Visible = true;
                    Delete3.Visible = true;
                    break;
            }

        }
    }
    public void Index(string query, ComboBox cb)
    {
        cmd = new SqlCommand(query, con);
        dr = cmd.ExecuteReader();

        if (dr.Read())
        {
            cb.SelectedItem = dr.GetString(0);
        }
        dr.Close();
        dr.Dispose();
    }

}
