using System;
using System.Collections;
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
        int id;
        String name, area, institution, country;
        private Common common;

        public FormProject(string name, int id, Common common)
        {
            InitializeComponent();
            label6.Text = name;
            this.id = id;
            this.common = common;
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
        private void CheckFilters()
        {
            String query = null;
            DataView DV = new DataView(this.build2evenizeDataSet.View_1);
            if (name != null)
            {
                if (query == null)
                {
                    query = "name LIKE '%" + name + "%'";
                }
                else
                {
                    query += " AND name LIKE '%" + name + "%'";
                }
            };
            if (area != "All")
            {
                if (query == null)
                {
                    query = "area LIKE '%" + area + "%'";
                }
                else
                {
                    query += " AND area LIKE '%" + area + "%'";
                }
            };
            if (institution != "All")
            {
                if (query == null)
                {
                    query = "institution LIKE '%" + institution + "%'";
                }
                else
                {
                    query += " AND institution LIKE '%" + institution + "%'";
                }
            };
            if (country != "All")
            {
                if (query == null)
                {
                    query = "country LIKE '%" + country + "%'";
                }
                else
                {
                    query += " AND country LIKE '%" + country + "%'";
                }
            };
            DV.RowFilter = query;
            dataGridView1.DataSource = DV;
        }
        private void FormProject_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("All");
            common.Filters("area", "name", comboBox1); // filter of area 
            comboBox2.Items.Add("All");
            common.Filters("institution", "name", comboBox2); //filter of institution name
            comboBox3.Items.Add("All");
            common.Filters("institution", "country", comboBox3); //filter of institution country
            common.Filters("project", "name", comboBox6); // filter of area
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
            // TODO: This line of code loads data into the 'build2evenizeDataSet.View_1' table. You can move, or remove it, as needed.
            this.view_1TableAdapter.Fill(this.build2evenizeDataSet.View_1);

        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            area = comboBox1.Text;
            CheckFilters();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            name = textBox1.Text;
            CheckFilters();
        }


        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            country = comboBox3.Text;
            CheckFilters();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int projectId = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());

            FormProjectInfo fpi = new FormProjectInfo(this, projectId, id, common);
            fpi.ShowDialog();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            institution = comboBox2.Text;
            CheckFilters();
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void SimulationPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            FormProjectInfo fpi = new FormProjectInfo(this, 0, id, common);
            fpi.Show();
            fpi.setPanelVisible(true);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            Login_Register lr = new Login_Register();
            lr.ShowDialog();
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public ArrayList resultadosTech(int idProj)
        {
            SqlCommand cmdTech = new SqlCommand("select count(*) as nrTechs from Project_Tech where project_id = " + idProj, common.con);
            SqlDataReader dr = cmdTech.ExecuteReader();
            dr.Read();
            int nrTech = (int)dr["nrTechs"];
            dr.Close();
            dr.Dispose();


            SqlCommand cmdSP = new SqlCommand("select count(*) as nrStudentsProj from Student_Project where project_id = " + idProj, common.con);
            SqlDataReader drSP = cmdSP.ExecuteReader();
            drSP.Read();
            int nrSP = (int)drSP["nrStudentsProj"];
            drSP.Close();
            drSP.Dispose();

            double[,] techMatrix = new double[nrSP, nrSP];

            SqlCommand cmdCount = new SqlCommand("select count(tech_id) as count from Project_Tech where project_id = " + idProj, common.con);
            SqlDataReader drCount = cmdCount.ExecuteReader();
            drCount.Read();
            int countTech = (int)drCount["count"];

            SqlCommand cmdPT = new SqlCommand("select tech_id from Project_Tech where project_id = " + idProj, common.con);
            SqlDataReader drPT = cmdPT.ExecuteReader();
            int[] projectTechs = new int[countTech];
            int x = 0;
            while (drPT.Read())
            {
                projectTechs[x] = (int)drPT["tech_id"];
                x++;
            }

            ArrayList resultadosReturn = new ArrayList();

            for (int w = 0; w < countTech; w++)
            {
                SqlCommand cmdSV = new SqlCommand("select T.* from Student_Tech T, Student_Project P where T.tech_id = " + projectTechs[w] + " and P.project_id = " + idProj + " and T.student_id = P.student_id", common.con);
                SqlDataReader drSV = cmdSV.ExecuteReader();
                int[,] StudentTechLists = new int[nrSP, 4];
                int count = 0;
                while (drSV.Read())
                {
                    StudentTechLists[count, 0] = (int)drSV["student_tech_id"];
                    StudentTechLists[count, 1] = (int)drSV["tech_id"];
                    StudentTechLists[count, 2] = (int)drSV["student_id"];
                    StudentTechLists[count, 3] = (int)drSV["value"];
                    count++;
                }

                drSV.Close();
                drSV.Dispose();

                for (int i = 0; i < nrSP; i++)
                {
                    for (int j = 0; j < nrSP; j++)
                    {
                        techMatrix[i, j] = (double)StudentTechLists[i, 3] / StudentTechLists[j, 3];
                    }
                }

                double totalFila = 0;
                double[] resultados = new double[nrSP];
                for (int i = 0; i < nrSP; i++)
                {
                    for (int j = 0; j < nrSP; j++)
                    {
                        double a = techMatrix[j, i];
                        totalFila = totalFila + a;
                    }
                    resultados[i] = totalFila;
                    totalFila = 0;
                }

                double valor = 0;
                double valorFinal = 0;
                double[] resultadosFinal = new double[nrSP];
                for (int i = 0; i < nrSP; i++)
                {
                    for (int j = 0; j < nrSP; j++)
                    {
                        valor = techMatrix[i, j];
                        valorFinal = (double)valorFinal + (valor / resultados[j]);
                    }
                    valorFinal = valorFinal / nrSP;
                    resultadosFinal[i] = valorFinal;
                    valorFinal = 0;
                }

                resultadosReturn.Add(resultadosFinal);
            }

            return resultadosReturn;
        }

        public ArrayList resultadosSK(int idProj)
        {
            SqlCommand cmdSP = new SqlCommand("select count(*) as nrStudentsProj from Student_Project where project_id = " + idProj, common.con);
            SqlDataReader drSP = cmdSP.ExecuteReader();
            drSP.Read();
            int nrSP = (int)drSP["nrStudentsProj"];
            drSP.Close();
            drSP.Dispose();

            double[,] skMatrix = new double[nrSP, nrSP];

            SqlCommand cmdCount = new SqlCommand("select count(social_skill_id) as count from Project_SK where project_id = " + idProj, common.con);
            SqlDataReader drCount = cmdCount.ExecuteReader();
            drCount.Read();
            int countSK = (int)drCount["count"];

            SqlCommand cmdPSK = new SqlCommand("select social_skill_id from Project_SK where project_id = " + idProj, common.con);
            SqlDataReader drPSK = cmdPSK.ExecuteReader();
            int[] projectSK = new int[countSK];
            int x = 0;
            while (drPSK.Read())
            {
                projectSK[x] = (int)drPSK["social_skill_id"];
                x++;
            }

            ArrayList resultadosReturn = new ArrayList();

            for (int w = 0; w < countSK; w++)
            {
                SqlCommand cmdSV = new SqlCommand("select SK.* from Student_SK SK, Student_Project P where SK.social_skill_id = " + projectSK[w] + " and P.project_id = " + idProj + " and SK.student_id = P.student_id", common.con);
                SqlDataReader drSV = cmdSV.ExecuteReader();
                int[,] StudentSKLists = new int[nrSP, 4];
                int count = 0;
                while (drSV.Read())
                {
                    StudentSKLists[count, 0] = (int)drSV["student_sk_id"];
                    StudentSKLists[count, 1] = (int)drSV["social_skill_id"];
                    StudentSKLists[count, 2] = (int)drSV["student_id"];
                    StudentSKLists[count, 3] = (int)drSV["value"];
                    count++;
                }

                drSV.Close();
                drSV.Dispose();

                for (int i = 0; i < nrSP; i++)
                {
                    for (int j = 0; j < nrSP; j++)
                    {
                        skMatrix[i, j] = (double)StudentSKLists[i, 3] / StudentSKLists[j, 3];
                    }
                }

                double totalFila = 0;
                double[] resultados = new double[nrSP];
                for (int i = 0; i < nrSP; i++)
                {
                    for (int j = 0; j < nrSP; j++)
                    {
                        double a = skMatrix[j, i];
                        totalFila = totalFila + a;
                    }
                    resultados[i] = totalFila;
                    totalFila = 0;
                }

                double valor = 0;
                double valorFinal = 0;
                double[] resultadosFinal = new double[nrSP];
                for (int i = 0; i < nrSP; i++)
                {
                    for (int j = 0; j < nrSP; j++)
                    {
                        valor = skMatrix[i, j];
                        valorFinal = (double)valorFinal + (valor / resultados[j]);
                    }
                    valorFinal = valorFinal / nrSP;
                    resultadosFinal[i] = valorFinal;
                    valorFinal = 0;
                }

                resultadosReturn.Add(resultadosFinal);
            }

            return resultadosReturn;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            ArrayList valuesTech = resultadosTech(1);
            ArrayList valuesSK = resultadosSK(1);
            double[] sk;
            for (int i = 0; i < 2; i++)
            {
                sk = (double[])valuesSK[i];
                for (int j = 0; j < 2; j++)
                {
                    MessageBox.Show(sk[j].ToString());
                }

            }

        }

        public void Refresh()
        {
            this.view_1TableAdapter.Fill(this.build2evenizeDataSet.View_1);
            MessageBox.Show("SUCCESS");
        }
    }


}
