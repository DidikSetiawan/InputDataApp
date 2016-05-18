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
                using (SqlCommand cmd = new SqlCommand("getDataK", conn))
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
                    cmd = new SqlCommand("getDataCari @namauser", conn);
                    cmd.Parameters.Add("@namauser", SqlDbType.VarChar, 30).Value = tboxUser.Text;
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
                    cmd = new SqlCommand("upDataK @namauser, @ipaddr, @ket", conn);
                    cmd.Parameters.Add("@namauser", SqlDbType.VarChar, 30).Value = tboxUser.Text;
                    cmd.Parameters.Add("@ipaddr", SqlDbType.VarChar, 30).Value = tboxIP.Text;
                    cmd.Parameters.Add("@ket", SqlDbType.VarChar, 30).Value = tboxKet.Text;
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
                    cmd = new SqlCommand("upDataEdit @namauser, @ipaddr, @ket", conn);
                    cmd.Parameters.Add("@namauser", SqlDbType.VarChar, 30).Value = tboxUser.Text;
                    cmd.Parameters.Add("@ipaddr", SqlDbType.VarChar, 30).Value = tboxIP.Text;
                    cmd.Parameters.Add("@ket", SqlDbType.VarChar, 30).Value = tboxKet.Text;
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
                cmd = new SqlCommand("upDataDel @namauser", conn);
                cmd.Parameters.Add("@namauser", SqlDbType.VarChar, 30).Value = tboxUser.Text;
                conn.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Data berhasil dihapus", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                awal();
                BindGrid();
            }
        }
    }
}
