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

namespace medicineManagement
{
    public partial class Form7 : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-IR8P1CI\TANVIR1;Initial Catalog=Medicine_management;Integrated Security=True");

        public int userId;
        DataTable dt;
        public Form7()
        {
            InitializeComponent();
            

        }
        private void Form7_Load_1(object sender, EventArgs e)
        {
            getUserRecordForGrid();
        }

        private void getUserRecordForGrid()
        {
            SqlCommand cmd = new SqlCommand("Select * from [User]", con);
            dt = new DataTable();

            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            dt.Columns.Remove("Password");
            con.Close();
            dataGridView1.DataSource = dt;

        }

        private bool IsValid()
        {
            if (textBox1.Text == String.Empty)
            {
                MessageBox.Show("User name is required", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void Insert_Click(object sender, EventArgs e)
        {

            SqlCommand cmd2 = new SqlCommand("SELECT * FROM [User] WHERE userName = " + "'" + textBox1.Text + "'", con);

            con.Open();

            int count = 0;
            SqlDataReader DR2 = cmd2.ExecuteReader();
            while (DR2.Read())
            {
                if (DR2[1].ToString().Equals(textBox1.Text.ToString()))
                {
                    MessageBox.Show("Name is not valid!\nTake another name, plz.");
                    count++;
                    break;
                }
            }
            DR2.Close();

            if (count == 0)
            {

                if (MessageBox.Show("Are you Confirm?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (IsValid())
                    {
                        SqlCommand cmd = new SqlCommand("INSERT INTO [User] values (@Name,@phoneNo,@Address,@Password,@userType)", con);
                        cmd.CommandType = CommandType.Text;

                        String cmbItemValue = userType3.SelectedItem.ToString();

                        cmd.Parameters.AddWithValue("@Name", textBox1.Text);
                        cmd.Parameters.AddWithValue("@phoneNo", textBox2.Text);
                        cmd.Parameters.AddWithValue("@Address", textBox3.Text);
                        cmd.Parameters.AddWithValue("@Password", textBox4.Text);
                        cmd.Parameters.AddWithValue("@userType", cmbItemValue);

                        
                        cmd.ExecuteNonQuery();
                        con.Close();
                        MessageBox.Show("User added successfully", "saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        getUserRecordForGrid();
                        ResetFormControls();
                    }
                }

            }



            cmd2.CommandType = CommandType.Text;
            con.Close();


        }

        private void ResetFormControls()
        {
            userId = 0;
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();

            textBox1.Focus();

        }
        

        private void Update_Click(object sender, EventArgs e)
        {

            SqlCommand cmd2 = new SqlCommand("SELECT * FROM [User] WHERE userName = " + "'" + textBox1.Text + "'", con);

            con.Open();

            int count = 0;
            SqlDataReader DR2 = cmd2.ExecuteReader();
            while (DR2.Read())
            {
                if (DR2[1].ToString().Equals(textBox1.Text.ToString()))
                {
                    

                    if (MessageBox.Show("Are you Confirm?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (userId > 0)
                        {
                            SqlCommand cmd = new SqlCommand("UPDATE [User] SET userName = @Name,phoneNo = @phoneNo,Address = @Address,userType = @userType WHERE userId = @userId", con);
                            cmd.CommandType = CommandType.Text;
                            String cmbItemValue = userType3.SelectedItem.ToString();

                            cmd.Parameters.AddWithValue("@Name", textBox1.Text);
                            cmd.Parameters.AddWithValue("@phoneNo", textBox2.Text);
                            cmd.Parameters.AddWithValue("@Address", textBox3.Text);
                            // cmd.Parameters.AddWithValue("@Password", textBox4.Text);
                            cmd.Parameters.AddWithValue("@userType", cmbItemValue);
                            cmd.Parameters.AddWithValue("@userId", this.userId);

                            DR2.Close();
                            cmd.ExecuteNonQuery();
                            con.Close();
                            MessageBox.Show("Login Info Updated successfully", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            getUserRecordForGrid();
                            ResetFormControls();
                        }
                        else
                        {
                            MessageBox.Show("please select a row from grid", "Select?", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    count++;
                    break;
                }
            }
            

            if (count == 0)
            {

                MessageBox.Show("Name is not valid!\nTake another name, plz.");

            }


            cmd2.CommandType = CommandType.Text;
            con.Close();


        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            userId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            textBox1.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            textBox3.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            //textBox4.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            userType3.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
        }

        private void Reset_Click(object sender, EventArgs e)
        {
            ResetFormControls();
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you Confirm?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {

                if (userId > 0)
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM [User] WHERE userId = @userId", con);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.AddWithValue("@userId", this.userId);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("User Deleted successfully", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    getUserRecordForGrid();
                    ResetFormControls();
                }
                else
                {
                    MessageBox.Show("please select an user", "Select?", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Full_Click(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            TopMost = true;
        }

        private void Form7_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                FormBorderStyle = FormBorderStyle.Sizable;
                WindowState = FormWindowState.Normal;
                TopMost = false;
            }
        }

        private void Logout_Click(object sender, EventArgs e)
        {
            Admin f1 = new Admin();
            f1.Show();
            this.Hide();
        }
    }
}
