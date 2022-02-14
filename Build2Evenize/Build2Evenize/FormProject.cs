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
using ClosedXML;

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
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
            // TODO: This line of code loads data into the 'build2evenizeDataSet.View_1' table. You can move, or remove it, as needed.
            this.view_1TableAdapter.Fill(this.build2evenizeDataSet.View_1);

            string query = "select name from Project where getdate() <= date_start and institution_id in (SELECT C.institution_id FROM Coordinator C where C.coordinator_id ="+ this.id +");";
            SqlCommand cmd = new SqlCommand(query, common.con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                comboBox6.Items.Add(dr.GetString(0));
            }
            dr.Close();
            dr.Dispose();

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

            ArrayList studentValuesReturn = new ArrayList();
            Dictionary<int, double> studentValue;
            Dictionary<int, double> studentValueFinal;

            while (drPT.Read())
            {
                projectTechs[x] = (int)drPT["tech_id"];
                x++;
            }

            ArrayList resultadosReturn = new ArrayList();

            for (int w = 0; w < countTech; w++)
            {
                studentValue = new Dictionary<int, double>();
                studentValueFinal = new Dictionary<int, double>();
                SqlCommand cmdSV = new SqlCommand("select T.* from Student_Tech T, Student_Project P where T.tech_id = " + projectTechs[w] + " and P.project_id = " + idProj + " and T.student_id = P.student_id", common.con);
                SqlDataReader drSV = cmdSV.ExecuteReader();
                while (drSV.Read())
                {
                    studentValue.Add((int)drSV["student_id"], (int)drSV["value"]);
                }

                drSV.Close();
                drSV.Dispose();

                int p = 0;
                foreach (var itemI in studentValue)
                {
                    int q = 0;
                    foreach (var itemJ in studentValue)
                    {                      
                        techMatrix[p, q] = (double)itemI.Value / itemJ.Value;
                        q += 1;
                    }
                    p++;
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
                int k = 0;
                foreach (var itemI in studentValue)
                {
                    int l = 0;
                    foreach (var itemJ in studentValue)
                    {
                        valor = techMatrix[k, l];
                        valorFinal = (double)valorFinal + (valor / resultados[l]);
                        l++;
                    }
                    valorFinal = valorFinal / nrSP;
                    studentValueFinal.Add(itemI.Key, valorFinal);
                    valorFinal = 0;
                    k++;
                }
                studentValuesReturn.Add(studentValueFinal);
            }

            return studentValuesReturn;
        }

        public Array resultadoseachTech(int idProj, int grade, int soft, int hard)
        {
            SqlCommand cmdTech = new SqlCommand("select count(*) as nrTechs from Project_Tech where project_id = " + idProj, common.con);
            SqlDataReader dr = cmdTech.ExecuteReader();
            dr.Read();
            int nrTech = (int)dr["nrTechs"];
            dr.Close();
            dr.Dispose();

            SqlCommand cmdArea = new SqlCommand("select count(*) as nrAreas from Project_Area where project_id = " + idProj, common.con);
            SqlDataReader da = cmdArea.ExecuteReader();
            da.Read();
            int nrArea = (int)da["nrAreas"];
            da.Close();
            da.Dispose();

            SqlCommand cmdSL = new SqlCommand("select nr_students from Project where project_id = " + idProj, common.con);
            SqlDataReader drSL = cmdSL.ExecuteReader();
            drSL.Read();
            int nrSL = (int)drSL["nr_students"];
            drSL.Close();
            drSL.Dispose();

            int slots_per_area = nrSL / nrTech;


            SqlCommand cmdSP = new SqlCommand("select count(*) as nrStudentsProj from Student_Project where project_id = " + idProj, common.con);
            SqlDataReader drSP = cmdSP.ExecuteReader();
            drSP.Read();
            int nrSP = (int)drSP["nrStudentsProj"];
            drSP.Close();
            drSP.Dispose();

            double[,] techMatrix = new double[nrSP, nrSP];
            string[,] info_student = new string[nrSL, 6];
            double[,] sk_Matrix = new double[nrSP, nrSP];
            double[,] gradeMatrix = new double[nrSP, nrSP];
            double[,] criterio_Matrix = new double[3, 3];

            Dictionary<string, double> criterio_value;
            criterio_value = new Dictionary<string, double>();
            Dictionary<string, double> criterio_ValueFinal;
            criterio_ValueFinal = new Dictionary<string, double>();
            criterio_value.Add("hard", hard);
            criterio_value.Add("soft", soft);
            criterio_value.Add("grade", grade);

            int ccol = 0;
            foreach (var itemI in criterio_value)
            {
                int clar = 0;
                foreach (var itemJ in criterio_value)
                {
                    criterio_Matrix[ccol, clar] = (double)itemI.Value / itemJ.Value;
                    clar += 1;
                }
                ccol++;
            }

            double totalFila_cr = 0;
            double[] resultados_cr = new double[3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    double alll = criterio_Matrix[j, i];
                    totalFila_cr = totalFila_cr + alll;
                }
                resultados_cr[i] = totalFila_cr;
                totalFila_cr = 0;
            }

            double valor_cr = 0;
            double valorFinal_cr = 0;
            double[] resultadosFinal_cr = new double[3];
            int ulll = 0;
            foreach (var itemI in criterio_value)
            {
                int loooo = 0;
                foreach (var itemJ in criterio_value)
                {
                    valor_cr = criterio_Matrix[ulll, loooo];
                    valorFinal_cr = (double)valorFinal_cr + (valor_cr / resultados_cr[loooo]);
                    loooo++;
                }
                valorFinal_cr = valorFinal_cr / 3;
                criterio_ValueFinal.Add(itemI.Key, valorFinal_cr);
                valorFinal_cr = 0;
                ulll++;
            }


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

            int[] student_best_on_tech = new int[nrSL];
            ArrayList studentValuesReturn = new ArrayList();
            Dictionary<int, double> studentValue;
            Dictionary<int, double> student_sk_value;
            Dictionary<int, double> student_grade_value;
            Dictionary<int, double> student_sk_ValueFinal;
            Dictionary<int, double> student_grade_ValueFinal;
            Dictionary<int, double> studentValueFinal;
            Dictionary<int, Dictionary<int, double>> student_each_tech;

            ArrayList resultadosReturn = new ArrayList();

            int vagas = 0;
            
            student_sk_value = new Dictionary<int, double>();
            student_sk_ValueFinal = new Dictionary<int, double>();
            student_each_tech = new Dictionary<int, Dictionary<int, double>>();
            SqlCommand cmdAVG = new SqlCommand("Select sk.student_id, AVG(CAST(sk.value as FLOAT)) as media From Student_SK sk, Student_Project sp Where sk.student_id=sp.student_id and sp.project_id=" + idProj + "Group by sk.student_id  ", common.con);
            SqlDataReader drAVG = cmdAVG.ExecuteReader();
            while (drAVG.Read())
            {
                student_sk_value.Add((int)drAVG["student_id"], (double)drAVG["media"]);
            }

            drAVG.Close();
            drAVG.Dispose();

            int co = 0;
            foreach (var itemI in student_sk_value)
            {
                int la = 0;
                foreach (var itemJ in student_sk_value)
                {
                    sk_Matrix[co, la] = (double)itemI.Value / itemJ.Value;
                    la += 1;
                }
                co++;
            }

            double totalFila_sk = 0;
            double[] resultados_sk = new double[nrSP];
            for (int i = 0; i < nrSP; i++)
            {
                for (int j = 0; j < nrSP; j++)
                {
                    double al = sk_Matrix[j, i];
                    totalFila_sk = totalFila_sk + al;
                }
                resultados_sk[i] = totalFila_sk;
                totalFila_sk = 0;
            }

            double valor_sk = 0;
            double valorFinal_sk = 0;
            double[] resultadosFinal_sk = new double[nrSP];
            int ul= 0;
            foreach (var itemI in student_sk_value)
            {
                int student_id = itemI.Key;
                int loo = 0;
                foreach (var itemJ in student_sk_value)
                {
                    valor_sk = sk_Matrix[ul, loo];
                    valorFinal_sk = (double)valorFinal_sk + (valor_sk / resultados_sk[loo]);
                    loo++;
                }
                valorFinal_sk = valorFinal_sk / nrSP;
                    student_sk_ValueFinal.Add(itemI.Key, valorFinal_sk);
                    valorFinal_sk = 0;
                ul++;
            }

            student_grade_value = new Dictionary<int, double>();
            student_grade_ValueFinal = new Dictionary<int, double>();
            SqlCommand cmdGrade = new SqlCommand("select S.grade, S.student_id from Student S, Student_Project SP where S.student_id = SP.student_id and SP.project_id = "+idProj, common.con);
            SqlDataReader drGrade = cmdGrade.ExecuteReader();
            while (drGrade.Read())
            {
                student_grade_value.Add((int)drGrade["student_id"], Convert.ToDouble(drGrade["grade"]));
            }

            drGrade.Close();
            drGrade.Dispose();

            int cogra = 0;
            foreach (var itemI in student_grade_value)
            {
                int lagra = 0;
                foreach (var itemJ in student_grade_value)
                {
                    gradeMatrix[cogra, lagra] = (double)itemI.Value / itemJ.Value;
                    lagra += 1;
                }
                cogra++;
            }

            double totalFila_grade = 0;
            double[] resultados_grade = new double[nrSP];
            for (int i = 0; i < nrSP; i++)
            {
                for (int j = 0; j < nrSP; j++)
                {
                    double ag = gradeMatrix[j, i];
                    totalFila_grade = totalFila_grade + ag;
                }
                resultados_grade[i] = totalFila_grade;
                totalFila_grade = 0;
            }

            double valor_grade = 0;
            double valorFinal_grade = 0;
            double[] resultadosFinal_grade = new double[nrSP];
            int kgra = 0;
            foreach (var itemI in student_grade_value)
            {
                int lgra = 0;
                foreach (var itemJ in student_grade_value)
                {
                    valor_grade = gradeMatrix[kgra, lgra];
                    valorFinal_grade = (double)valorFinal_grade + (valor_grade / resultados_grade[lgra]);
                    lgra++;
                }
                valorFinal_grade = valorFinal_grade / nrSP;
                student_grade_ValueFinal.Add(itemI.Key, valorFinal_grade);
                valorFinal_grade = 0;
                kgra++;
            }

            for (int w = 0; w < countTech; w++)
            {
                studentValue = new Dictionary<int, double>();
                studentValueFinal = new Dictionary<int, double>();
                student_each_tech = new Dictionary<int, Dictionary<int, double>>();
                SqlCommand cmdSV = new SqlCommand("select T.* from Student_Tech T, Student_Project P where T.tech_id = " + projectTechs[w] + " and P.project_id = " + idProj + " and T.student_id = P.student_id", common.con);
                SqlDataReader drSV = cmdSV.ExecuteReader();
                while (drSV.Read())
                {
                    studentValue.Add((int)drSV["student_id"], (int)drSV["value"]);
                }

                drSV.Close();
                drSV.Dispose();

                int p = 0;
                foreach (var itemI in studentValue)
                {
                    int q = 0;
                    foreach (var itemJ in studentValue)
                    {
                        techMatrix[p, q] = (double)itemI.Value / itemJ.Value;
                        q += 1;
                    }
                    p++;
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
                int k = 0;
                foreach (var itemI in studentValue)
                {

                    double student_grade = student_grade_ValueFinal[itemI.Key] * criterio_ValueFinal["grade"];
                    double student_social_skills = student_sk_ValueFinal[itemI.Key] * criterio_ValueFinal["soft"];

                    int student_id = itemI.Key;

                    
                    int l = 0;
                    foreach (var itemJ in studentValue)
                    {
                        valor = techMatrix[k, l];
                        valorFinal = (double)valorFinal + (valor / resultados[l]);
                        l++;
                    }
                    valorFinal = valorFinal / nrSP;
                    double valor_pre_final = valorFinal;

                        if (student_best_on_tech.Contains(itemI.Key)){

                    }
                    else
                    {
                        valorFinal = valorFinal * criterio_ValueFinal["hard"];
                        double ahp = valorFinal + student_grade + student_social_skills;
                        studentValueFinal.Add(itemI.Key, ahp);
                        valorFinal = 0;
                        student_grade = 0;
                    }
                    k++;
                }

                var top5 = studentValueFinal.OrderByDescending(pair => pair.Value).Take(slots_per_area);
                foreach (var itemI in top5)
                {
                    student_best_on_tech[vagas] = itemI.Key;
                    vagas = vagas + 1;
                }
                studentValuesReturn.Add(studentValueFinal);
                
            }

            int juu = 0;

            for (int i = 0; i < student_best_on_tech.Length; i++)
            {
                int id_student = student_best_on_tech[i];
                SqlCommand cmdIS = new SqlCommand("SELECT S.name as student_name, S.email as student_email, S.phone as phone, S.date_birth as date_birth , I.name as instituiton_name, A.name as area_name  FROM Student S, Institution I, Student_Area as SA, Area as A Where s.student_id=" + id_student + " AND s.student_id=sa.student_id AND sa.area_id =a.area_id AND s.institution_id=i.institution_id", common.con);
                SqlDataReader drIS = cmdIS.ExecuteReader();
                while (drIS.Read())
                {
                    info_student[juu,0] = (string)drIS["student_name"];
                    info_student[juu, 1] = (string)drIS["student_email"];
                    info_student[juu, 2] = (string)drIS["phone"];
                    info_student[juu, 3] = drIS["date_birth"].ToString();
                    info_student[juu, 4] = (string)drIS["instituiton_name"];
                    info_student[juu, 5] = (string)drIS["area_name"];
                    juu = juu +1;
                }
                drIS.Close();
                drIS.Dispose();
            }
            

            return info_student;
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
            ArrayList studentValuesReturn = new ArrayList();
            Dictionary<int, double> studentValue;
            Dictionary<int, double> studentValueFinal;

            while (drPSK.Read())
            {
                projectSK[x] = (int)drPSK["social_skill_id"];
                x++;
            }

            ArrayList resultadosReturn = new ArrayList();

            for (int w = 0; w < countSK; w++)
            {
                studentValue = new Dictionary<int, double>();
                studentValueFinal = new Dictionary<int, double>();
                SqlCommand cmdSV = new SqlCommand("select SK.* from Student_SK SK, Student_Project P where SK.social_skill_id = " + projectSK[w] + " and P.project_id = " + idProj + " and SK.student_id = P.student_id", common.con);
                SqlDataReader drSV = cmdSV.ExecuteReader();
                while (drSV.Read())
                {
                    studentValue.Add((int)drSV["student_id"], (int)drSV["value"]);
                }

                drSV.Close();
                drSV.Dispose();

                int p = 0;
                foreach (var itemI in studentValue)
                {
                    int q = 0;
                    foreach (var itemJ in studentValue)
                    {
                        skMatrix[p, q] = (double)itemI.Value / itemJ.Value;
                        q += 1;
                    }
                    p++;
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
                int k = 0;
                foreach (var itemI in studentValue)
                {
                    int l = 0;
                    foreach (var itemJ in studentValue)
                    {
                        valor = skMatrix[k, l];
                        valorFinal = (double)valorFinal + (valor / resultados[l]);
                        l++;
                    }
                    valorFinal = valorFinal / nrSP;
                    studentValueFinal.Add(itemI.Key, valorFinal);
                    valorFinal = 0;
                    k++;
                }
                studentValuesReturn.Add(studentValueFinal);
            }

            return studentValuesReturn;

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(this.dataGridView2.Rows.Count == 0 && this.dataGridView3.Rows.Count == 0)
            {
                MessageBox.Show("You need to simulate first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Microsoft.Office.Interop.Excel.Application xcelApp = new Microsoft.Office.Interop.Excel.Application();
                xcelApp.Application.Workbooks.Add(Type.Missing);

                xcelApp.Cells[1, 1] = "Balanced Team";

                for (int i = 1; i < dataGridView2.Columns.Count + 1; i++)
                {
                    xcelApp.Cells[2, i] = dataGridView2.Columns[i - 1].HeaderText;
                }

                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    for (int j = 0; j < dataGridView2.Columns.Count; j++)
                    {
                        xcelApp.Cells[i+2, j+1] = dataGridView2.Rows[i].Cells[j].Value.ToString();
                    }
                }
                xcelApp.Columns.AutoFit();
                xcelApp.Visible = true;

                Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
                excelApp.Application.Workbooks.Add(Type.Missing);

                excelApp.Cells[1, 1] = "Best Team";

                for (int i = 1; i < dataGridView3.Columns.Count + 1; i++)
                {
                    excelApp.Cells[2, i] = dataGridView3.Columns[i - 1].HeaderText;
                }

                for (int i = 0; i < dataGridView3.Rows.Count; i++)
                {
                    for (int j = 0; j < dataGridView3.Columns.Count; j++)
                    {
                        excelApp.Cells[i + 2, j + 1] = dataGridView3.Rows[i].Cells[j].Value.ToString();
                    }
                }
                excelApp.Columns.AutoFit();
                excelApp.Visible = true;
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void comboBox6_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        public Dictionary<int, double> resultsGrade(int idProj)
        {
            SqlCommand cmdSP = new SqlCommand("select count(*) as nrStudentsProj from Student_Project where project_id = " + idProj, common.con);
            SqlDataReader drSP = cmdSP.ExecuteReader();
            drSP.Read();
            int nrSP = (int)drSP["nrStudentsProj"];
            drSP.Close();
            drSP.Dispose();

            double[,] gradeMatrix = new double[nrSP, nrSP];

            ArrayList studentValuesReturn = new ArrayList();
            Dictionary<int, double> studentValue;
            Dictionary<int, double> studentValueFinal;

            ArrayList resultadosReturn = new ArrayList();

                studentValue = new Dictionary<int, double>();
                studentValueFinal = new Dictionary<int, double>();
                SqlCommand cmdSV = new SqlCommand("select S.grade, S.student_id from Student S, Student_Project SP where S.student_id = SP.student_id and SP.project_id = "+idProj, common.con);
                SqlDataReader drSV = cmdSV.ExecuteReader();
                while (drSV.Read())
                {
                    studentValue.Add((int)drSV["student_id"], Convert.ToDouble(drSV["grade"]));
                }

                drSV.Close();
                drSV.Dispose();

                int p = 0;
                foreach (var itemI in studentValue)
                {
                    int q = 0;
                    foreach (var itemJ in studentValue)
                    {
                        gradeMatrix[p, q] = (double)itemI.Value / itemJ.Value;
                        q += 1;
                    }
                    p++;
                }

                double totalFila = 0;
                double[] resultados = new double[nrSP];
                for (int i = 0; i < nrSP; i++)
                {
                    for (int j = 0; j < nrSP; j++)
                    {
                        double a = gradeMatrix[j, i];
                        totalFila = totalFila + a;
                    }
                    resultados[i] = totalFila;
                    totalFila = 0;
                }

                double valor = 0;
                double valorFinal = 0;
                double[] resultadosFinal = new double[nrSP];
                int k = 0;
                foreach (var itemI in studentValue)
                {
                    int l = 0;
                    foreach (var itemJ in studentValue)
                    {
                        valor = gradeMatrix[k, l];
                        valorFinal = (double)valorFinal + (valor / resultados[l]);
                        l++;
                    }
                    valorFinal = valorFinal / nrSP;
                    studentValueFinal.Add(itemI.Key, valorFinal);
                    valorFinal = 0;
                    k++;
                }
            return studentValueFinal;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();
            dataGridView3.Rows.Clear();
            if (comboBox4.Text != "" && comboBox5.Text != "" && comboBox7.Text != "" && comboBox6.Text != "")
            {
                int hard = Int32.Parse(comboBox4.Text);
                int soft = Int32.Parse(comboBox5.Text);
                int grade = Int32.Parse(comboBox7.Text);
                string project = comboBox6.Text;
                SqlCommand cmdIP = new SqlCommand("select project_id from Project where name = '" + project + "'", common.con);
                SqlDataReader drIP = cmdIP.ExecuteReader();
                drIP.Read();
                int nrIP = (int)drIP["project_id"];
                drIP.Close();
                drIP.Dispose();

                SqlCommand cmdSP = new SqlCommand("select nr_students from Project where project_id = " + nrIP, common.con);
                SqlDataReader drSP = cmdSP.ExecuteReader();
                drSP.Read();
                int nrSP = (int)drSP["nr_students"];
                drSP.Close();
                drSP.Dispose();

                SqlCommand cmdSSP = new SqlCommand("select count(*) as nrStudentsProj from Student_Project where project_id = " + nrIP, common.con);
                SqlDataReader drSSP = cmdSSP.ExecuteReader();
                drSSP.Read();
                int nrSSP = (int)drSSP["nrStudentsProj"];
                drSP.Close();
                drSP.Dispose();

                int possible = nrSSP - nrSP;

                if (possible >= 0)
                {

                    ArrayList valuesTech = resultadosTech(nrIP);
                    ArrayList valuesSK = resultadosSK(nrIP);
                    string[,] course = (string[,])resultadoseachTech(nrIP, grade, soft, hard);
                    Dictionary<int, double> valuesGrade = resultsGrade(nrIP);

                    Dictionary<int, double> valuesTechFinal = new Dictionary<int, double>();
                    Dictionary<int, double> valuesSKFinal = new Dictionary<int, double>();
                    Dictionary<int, Dictionary<int, double>> valuesTechEach = new Dictionary<int, Dictionary<int, double>>();

                    int j = 0;

                    for (int i = 0; i < course.GetLength(0); i++)
                    {
                        this.dataGridView2.Rows.Add(course[i, j], course[i, j+1], course[i, j+2], course[i, j+3], course[i, j+4], course[i, j+5]);
                    }

                    for (int i = 0; i < valuesTech.Count; i++)
                    {
                        Dictionary<int, double> a = (Dictionary<int, double>)valuesTech[i];
                        foreach (var itemI in a)
                        {
                            if (valuesTechFinal.ContainsKey(itemI.Key))
                            {
                                valuesTechFinal[itemI.Key] = valuesTechFinal[itemI.Key] + itemI.Value;
                            }
                            else
                            {
                                valuesTechFinal.Add(itemI.Key, itemI.Value);
                            }
                        }
                    }
                    for (int i = 0; i < valuesSK.Count; i++)
                    {
                        Dictionary<int, double> a = (Dictionary<int, double>)valuesSK[i];
                        foreach (var itemI in a)
                        {
                            if (valuesSKFinal.ContainsKey(itemI.Key))
                            {
                                valuesSKFinal[itemI.Key] = valuesSKFinal[itemI.Key] + itemI.Value;
                            }
                            else
                            {
                                valuesSKFinal.Add(itemI.Key, itemI.Value);
                            }
                        }
                    }

                    Dictionary<int, double> ranking = new Dictionary<int, double>();
                    foreach (var itemI in valuesTechFinal)
                    {
                        int id = itemI.Key;
                        double valueT = itemI.Value;
                        double valueSK = valuesSKFinal[id];
                        double valueGrade = valuesGrade[id];
                        double valueFinal = valueT + valueSK + valueGrade;
                        ranking.Add(id, valueFinal);
                    }

                    Dictionary<int, double> finalTeam = new Dictionary<int, double>();
                    Dictionary<int, double> secondTeam = new Dictionary<int, double>();
                    string[,] info_student = new string[nrSP, 6];
                    var top = ranking.OrderByDescending(pair => pair.Value).Take(nrSP);
                    int ln = 0;
                    int ll = 0;
                    foreach (var itemI in top)
                    {
                        int id_student = itemI.Key;
                        SqlCommand cmdIS = new SqlCommand("SELECT S.name as student_name, S.email as student_email, S.phone as phone, S.date_birth as date_birth , I.name as instituiton_name, A.name as area_name  FROM Student S, Institution I, Student_Area as SA, Area as A Where s.student_id=" + id_student + "AND s.student_id=sa.student_id AND sa.area_id =a.area_id AND s.institution_id=i.institution_id ", common.con);
                        SqlDataReader drIS = cmdIS.ExecuteReader();
                        while (drIS.Read())
                        {
                            info_student[ln, 0] = (string)drIS["student_name"];
                            info_student[ln, 1] = (string)drIS["student_email"];
                            info_student[ln, 2] = (string)drIS["phone"];
                            info_student[ln, 3] = drIS["date_birth"].ToString();
                            info_student[ln, 4] = (string)drIS["instituiton_name"];
                            info_student[ln, 5] = (string)drIS["area_name"];
                        }
                        drIS.Close();
                        drIS.Dispose();
                        this.dataGridView3.Rows.Add(info_student[ln, ll], info_student[ln, ll+1], info_student[ln, ll+2], info_student[ln, ll+3], info_student[ln, ll+4], info_student[ln, ll+5]);
                        ln = ln +1;

                    }
                } 
                else
                {
                    MessageBox.Show("Not enough students enrolled for the project", "Error",
    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Required fill in all fields", "Error",
    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        public void Refresh()
        {
            this.view_1TableAdapter.Fill(this.build2evenizeDataSet.View_1);
        }
    }


}
