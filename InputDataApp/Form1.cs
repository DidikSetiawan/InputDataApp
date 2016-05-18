using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Excel = Microsoft.Office.Interop.Excel;

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

        private void btnExp_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(strconn))
            {
                string sql = null;
                string data = null;

                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlWorkSheet;
                object misValue = System.Reflection.Missing.Value;

                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Add(misValue);
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                conn.Open();
                sql = "getData";
                SqlDataAdapter dscmd = new SqlDataAdapter(sql, conn);
                DataSet ds = new DataSet();
                dscmd.Fill(ds);

                for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                {
                    xlWorkSheet.Cells[1, (i + 1)] = ds.Tables[0].Columns[i].ColumnName;
                }

                for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                {
                    for (int j = 0; j <= ds.Tables[0].Columns.Count - 1; j++)
                    {
                        data = ds.Tables[0].Rows[i].ItemArray[j].ToString();
                        xlWorkSheet.Cells[i + 2, j + 1] = data;
                    }
                }

                xlWorkBook.SaveAs("D:\\InputDataAppOutput.xls", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();

                releaseObject(xlWorkSheet);
                releaseObject(xlWorkBook);
                releaseObject(xlApp);

                MessageBox.Show("File berhasil di-export, Anda bisa menemukannya di D:\\InputDataAppOutput.xls");
            }
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}
