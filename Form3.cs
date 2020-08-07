using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace medicineManagement
{
    public partial class Seller : Form
    {
        public Seller()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-IR8P1CI\TANVIR1;Initial Catalog=Medicine_management;Integrated Security=True");
        public int medicineId;
        DataTable dt;
        private void Insert_Click(object sender, EventArgs e)
        {
            
            String MName = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();

            con.Open();
            SqlCommand cmd2 = new SqlCommand("SELECT * FROM Medicine WHERE medicineName = " + "'" + MName + "'", con);

            

            int count = 0;
            SqlDataReader DR2 = cmd2.ExecuteReader();
            while (DR2.Read())
            {
                if (Convert.ToInt32(DR2[3].ToString()) < Convert.ToInt32(textBox1.Text))
                {
                    MessageBox.Show("Unsuffecient Quantity!");
                    count++;
                    break;
                }
            }
            DR2.Close();

            if (count == 0)
            {

                if (MessageBox.Show("Are you Confirm?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (medicineId > 0)
                    {
                        int MQuantity =Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[3].Value.ToString()) - 
                            Convert.ToInt32(textBox1.Text); 
                        SqlCommand cmd = new SqlCommand("UPDATE Medicine SET medicineQuantity = @medicineQuantity WHERE medicineId = @medicineId", con);
                        cmd.CommandType = CommandType.Text;
                        

                        cmd.Parameters.AddWithValue("@medicineQuantity", MQuantity);
                        
                        cmd.Parameters.AddWithValue("@medicineId", this.medicineId);

                        
                        cmd.ExecuteNonQuery();
                        con.Close();
                        MessageBox.Show("Medicine Updated successfully", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        getMedicineRecord();
                        ResetFormControls();
                    }
                    else
                    {
                        MessageBox.Show("please select a Medicine", "Select?", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private bool IsValid()
        {
            if (textBox1.Text == String.Empty)
            {
                MessageBox.Show("Medicine name is required", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
        private void Seller_Load(object sender, EventArgs e)
        {
            getMedicineRecord();
        }

        private void getMedicineRecord()
        {
            SqlCommand cmd = new SqlCommand("Select * from Medicine", con);
            dt = new DataTable();

            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();
            dataGridView1.DataSource = dt;
        }
        private void ResetFormControls()
        {
            medicineId = 0;
            textBox1.Clear();
            
            find.Clear();

            textBox1.Focus();

        }

        private void Logout_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            f1.Show();
            this.Hide();
        }

        private void Update_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you Confirm?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (medicineId > 0)
                {
                    SqlCommand cmd = new SqlCommand("UPDATE Medicine SET medicineName = @medicineName,medicineCatagory = @medicineCatagory,medicineQuantity = @medicineQuantity,purchaseDate = @purchaseDate,cost = @cost WHERE medicineId = @medicineId", con);
                    cmd.CommandType = CommandType.Text;
                    

                    cmd.Parameters.AddWithValue("@medicineName", textBox1.Text);
              
                    cmd.Parameters.AddWithValue("@medicineId", this.medicineId);

                    
                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Medicine Updated successfully", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    getMedicineRecord();
                    ResetFormControls();
                }
                else
                {
                    MessageBox.Show("please select a Medicine", "Select?", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Reset_Click(object sender, EventArgs e)
        {
            ResetFormControls();
        }

        private void Full_Click(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            TopMost = true;
        }

        private void Seller_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                FormBorderStyle = FormBorderStyle.Sizable;
                WindowState = FormWindowState.Normal;
                TopMost = false;
            }
        }

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            medicineId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            textBox1.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            
        }

        private void Search_Click(object sender, EventArgs e)
        {

        }

        private void Find_TextChanged(object sender, EventArgs e)
        {
            DataView dv = new DataView(dt);
            dv.RowFilter = String.Format("medicineName Like '%{0}%'", find.Text);
            dataGridView1.DataSource = dv;
        }

        private void Label2_Click(object sender, EventArgs e)
        {

        }

        private void Label3_Click(object sender, EventArgs e)
        {

        }
    }
}
