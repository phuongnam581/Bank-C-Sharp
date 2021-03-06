﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using NGANHANG.Process;

namespace NGANHANG.Forms
{
    public partial class MoTaiKhoanKH : DevExpress.XtraEditors.XtraForm
    {
        private string chiNhanhTaoTK;
        public MoTaiKhoanKH(string chinhanh)
        {
            InitializeComponent();
            this.chiNhanhTaoTK = chinhanh;
            this.taiKhoanTableAdapter.Connection.ConnectionString = Program.connectionstring;
        }

        private void MoTaiKhoanKH_Load(object sender, EventArgs e)
        {
            this.taiKhoanTableAdapter.Connection.ConnectionString = Program.connectionstring;
            this.taiKhoanTableAdapter.Fill(this.cN_NGANHANG.TaiKhoan);
            this.Owner.Enabled = false;
            groupBox1.Enabled = false;
            tbSoDu.Text = "100000";
        }

        private void MoTaiKhoanKH_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Owner.Enabled = true;
        }

        bool kiemTraSo(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }

        private void HienThiThongTin(string cmnd)
        {
            List<string> list = new List<string>();
            list = KT_TaiKhoanKH.ThongTinTaiKhoanKhachHang(cmnd);
            if (list == null)
            {
                MessageBox.Show("Danh sách null");
            }
            else
            {
                groupBox1.Enabled = true;
                tbCMND.Enabled = tbHo.Enabled = tbSoDu.Enabled = tbTen.Enabled = rtbDiaChi.Enabled = tbSDT.Enabled = tbChiNhanh.Enabled = false;
                // tbCMND.Text = list[0];
                tbCMND.Text = list[0];
                tbHo.Text = list[1];
                tbTen.Text = list[2];
                rtbDiaChi.Text = list[3];
                tbSDT.Text = list[4];
                tbChiNhanh.Text = list[5];
            }
        }

        private int KiemTraViTri_CMND(string SoCMND)
        {
            int i = 0;
            string cmnd = "";
            for (i = 0; i < taiKhoanBindingSource.Count; i++)
            {
                cmnd = ((DataRowView)taiKhoanBindingSource[i])["CMND"].ToString().Trim();
                if (SoCMND.Trim().Equals(cmnd)) return i;
            }

            return -1;
        }

        private void btnKiemTraThongTin_Click(object sender, EventArgs e)
        {
            if (tbNhapSoCMND.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập số chứng minh nhân dân khách hàng cần tạo tài khoản", "", MessageBoxButtons.OK);
                tbNhapSoCMND.Focus();
                return;
            }
            else if (!kiemTraSo(tbNhapSoCMND.Text))
            {
                MessageBox.Show("Giá trị nhập vào không hợp lệ", "", MessageBoxButtons.OK);
                tbNhapSoCMND.Focus();
                return;
            }
            else if (tbNhapSoCMND.Text.Length > 10)
            {
                
            }
            if (KT_KhachHang.KiemTraSoCMND(tbNhapSoCMND.Text) == 0)
            {
                MessageBox.Show("Số CMND không tồn tại trong hệ thống! Vui lòng kiểm tra lại");
                tbNhapSoCMND.Focus();
                return;
            }
            else if (KiemTraViTri_CMND(tbNhapSoCMND.Text.Trim()) != -1)
            {
                MessageBox.Show("Khách hàng đã tồn tại tài khoản! Vui lòng nhập lại");
                tbNhapSoCMND.Focus();
                return;

            }
            else
            {
                HienThiThongTin(tbNhapSoCMND.Text);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void btnTaoTK_Click(object sender, EventArgs e)
        {
            
            if (tbSoTK.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập số tài khoản");
                tbSoTK.Focus();
                return;
            }
            if (tbSoTK.Text.Length > 9)
            {
                MessageBox.Show("Số tài khoản tối đa là 9 số");
                tbSoTK.Focus();
                return;
            }
            else if (!kiemTraSo(tbSoTK.Text))
            {
                MessageBox.Show("Giá trị nhập vào không hợp lệ", "", MessageBoxButtons.OK);
                tbSoTK.Focus();
                return;
            }
            else if (KT_TaiKhoanKH.KiemTraSoTaiKhoan(tbSoTK.Text) == 1)
            {
                MessageBox.Show("Số tài khoản bị trùng! Vui lòng nhập lại");
                tbSoTK.Focus();
                return;
            }
            else
            {
              
                taiKhoanBindingSource.AddNew();
                ((DataRowView)taiKhoanBindingSource[taiKhoanBindingSource.Count - 1])["SOTK"] = tbSoTK.Text;
                ((DataRowView)taiKhoanBindingSource[taiKhoanBindingSource.Count - 1])["CMND"] = tbCMND.Text;
                ((DataRowView)taiKhoanBindingSource[taiKhoanBindingSource.Count - 1])["SODU"] = tbSoDu.Text;
                ((DataRowView)taiKhoanBindingSource[taiKhoanBindingSource.Count - 1])["MACN"] = chiNhanhTaoTK;

                taiKhoanBindingSource.EndEdit();
                taiKhoanBindingSource.ResetCurrentItem();

                this.taiKhoanTableAdapter.Connection.ConnectionString = Program.connectionstring;

                if (this.taiKhoanTableAdapter.Update(this.cN_NGANHANG) == 1)
                {
                    MessageBox.Show("Tạo tài khoản khách hàng thành công");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Tạo tài khoản khách hàng không thành công");
                    return;
                }
                
            }
        }

        private void taiKhoanBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.taiKhoanBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.cN_NGANHANG);

        }

        private void taiKhoanBindingNavigatorSaveItem_Click_1(object sender, EventArgs e)
        {
            this.Validate();
            this.taiKhoanBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.cN_NGANHANG);

        }
    }
    
}