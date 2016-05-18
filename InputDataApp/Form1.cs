using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace InputDataApp
{
    public partial class InputDataForm : Form
    {
        private static string strconn = "server=.; user id = user; password = sebuahpassword; database=InputDataDB;";
        private SqlCommand cmd;
        private SqlDataReader rd;

        public InputDataForm()
        {
            InitializeComponent();
            BindGrid();
        }

        private void BindGrid()
        {
            using (SqlConnection conn = new SqlConnection(strconn))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM datakomputer", conn))
                {
                    cmd.CommandType = CommandType.Text;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            dataGridView1.DataSource = dt;
                        }
                    }
                }
            }
        }

        private void awal()
        {
            tboxUser.Text = "";
            tboxIP.Text = "";
            tboxKet.Text = "";
        }

        private void InputDataForm_Load(object sender, EventArgs e)
        {
            awal();
            BindGrid();
        }

        private void btnCari_Click(object sender, EventArgs e)
        {
            if (tboxUser.Text.Trim() != "")
            {
                using (SqlConnection conn = new SqlConnection(strconn))
                {
                    cmd = new SqlCommand("select * from datakomputer where NamaUser='" + tboxUser.Text + "'", conn);
                    conn.Open();
                    rd = cmd.ExecuteReader();
                    rd.Read();

                    if (rd.HasRows)
                    {
                        tboxUser.Text = rd["NamaUser"].ToString();
                        tboxIP.Text = rd["IP_Address"].ToString();
                        tboxKet.Text = rd["Deskripsi"].ToString();

                    }
                    else
                    {
                        MessageBox.Show("Data komputer tidak ditemukan", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            if (tboxUser.Text.Trim() == "" || tboxIP.Text.Trim() == "" || tboxKet.Text.Trim() == "")
            {
                MessageBox.Show("Data inputan belum lengkap", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                using (SqlConnection conn = new SqlConnection(strconn))
                {
                    cmd = new SqlCommand("insert into datakomputer (NamaUser,IP_Address,Deskripsi) values ('" + tboxUser.Text + "','" + tboxIP.Text + "','" + tboxKet.Text + "')", conn);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Data berhasil disimpan", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    awal();
                    BindGrid();
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (tboxUser.Text.Trim() == "" || tboxIP.Text.Trim() == "" || tboxKet.Text.Trim() == "")
            {
                MessageBox.Show("Data siswa belum lengkap", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                using (SqlConnection conn = new SqlConnection(strconn))
                {
                    cmd = new SqlCommand("update datakomputer set NamaUser='" + tboxUser.Text + "', IP_Address='" + tboxIP.Text + "', Deskripsi='" + tboxKet.Text + "' where NamaUser ='" + tboxUser.Text + "'", conn);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Data berhasil diedit", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    awal();
                    BindGrid();
                }
            }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(strconn))
            {
                cmd = new SqlCommand("delete from datakomputer where NamaUser='" + tboxUser.Text + "'", conn);
                conn.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Data berhasil dihapus", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                awal();
                BindGrid();
            }
        }
    }
}
