using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Doanltweb.Models
{
    public class Giohang
    {
        // Tao doi tuong data chua du lieu tu model QLBanhang
        QLBanhangDataContext db = new QLBanhangDataContext();
        public int iMaSP { get; set; }
        public string sTenSP { get; set; }
        public string sHinhDD { get; set; }
        public Double dGiaBan { get; set; }
        public int iSoLuong { get; set; }

        public Double dThanhTien
        {
            get
            {
                return iSoLuong * dGiaBan;
            }
        }
        // Khoi tao gio hang theo Ma SP duoc tuyen vao voi so luong mac dinh la 1
        public Giohang(int MaSP)
        {
            iMaSP = MaSP;
            sanPham sp = db.sanPhams.Single(n => n.maSP == iMaSP);
            sTenSP = sp.tenSP;
            sHinhDD = sp.hinhDD;
            dGiaBan = double.Parse(sp.giaBan.ToString());
            iSoLuong = 1;
        }
    }

}