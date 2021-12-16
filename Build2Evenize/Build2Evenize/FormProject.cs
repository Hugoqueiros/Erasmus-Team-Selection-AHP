using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Build2Evenize
{
    public partial class FormProject : Form
    {
        SqlConnection con;
        public FormProject(string name, int id, SqlConnection con)
        {
            InitializeComponent();
            label6.Text = name;
            this.con =con;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ProjectManagementPanel.Visible = false;
            SimulationPanel.Visible = true;
            button1.BackColor = Color.FromArgb(37, 55, 127);
            button1.ForeColor = Color.White;
            button2.BackColor = Color.FromArgb(250, 218, 24);
            button2.ForeColor = Color.FromArgb(37, 55, 127);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ProjectManagementPanel.Visible = true;
            SimulationPanel.Visible = false;
            button2.BackColor = Color.FromArgb(37, 55, 127);
            button2.ForeColor = Color.White;
            button1.BackColor = Color.FromArgb(250, 218, 24);
            button1.ForeColor = Color.FromArgb(37, 55, 127);
        }
        private void Filters(string table, string columnName, ComboBox comboBox) //function for searching filters
        {

            // query to get columnname from table and fill in the combobox items
            string query = "select distinct " + columnName+" from " + table;
            SqlCommand cmd = new SqlCommand(query, this.con);

            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                //add every row found on database to combobox
                string name = (string)dr[columnName];
                comboBox.Items.Add(name);
            }
            dr.Close();
        }
        private void FormProject_Load(object sender, EventArgs e)
        {
            Filters("area", "name", comboBox1); // filter of area 
            Filters("institution", "name", comboBox2); //filter of institution name
            Filters("institution", "country", comboBox3); //filter of institution country

            // TODO: This line of code loads data into the 'build2evenizeDataSet.View_1' table. You can move, or remove it, as needed.
            this.view_1TableAdapter.Fill(this.build2evenizeDataSet.View_1);

        }
        
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        public void label3_Click(object sender, EventArgs e)
        {

        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataView DV = new DataView(this.build2evenizeDataSet.View_1);
            DV.RowFilter = string.Format("Expr1 LIKE '%{0}%'", comboBox1.Text);
            dataGridView1.DataSource = DV;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            DataView DV = new DataView(this.build2evenizeDataSet.View_1);
            DV.RowFilter = string.Format("Name LIKE '%{0}%'", textBox1.Text);
            dataGridView1.DataSource = DV;
        }


        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataView DV = new DataView(this.build2evenizeDataSet.View_1);
            DV.RowFilter = string.Format("Country LIKE '%{0}%'", comboBox3.Text);
            dataGridView1.DataSource = DV;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataView DV = new DataView(this.build2evenizeDataSet.View_1);
            DV.RowFilter = string.Format("Expr2 LIKE '%{0}%'", comboBox2.Text);
            dataGridView1.DataSource = DV;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            FormProjectInfo fpi = new FormProjectInfo();
            fpi.ShowDialog();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            Login_Register lr = new Login_Register();
            lr.ShowDialog();
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }
    }


}
