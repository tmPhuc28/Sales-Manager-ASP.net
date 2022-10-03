using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Doanltweb.Models;
namespace Doanltweb.Controllers
{
    public class GiohangController : Controller
    {
        // Tao doi tuong data chua du lieu tu model da tao
        QLBanhangDataContext db = new QLBanhangDataContext();
        
        // Lấy giỏ hàng 
        public List<Giohang> Laygiohang()
        {
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if (lstGiohang == null)
            {
                // Neu gio hang chua ton tai thi khoi tao listGiohang
                lstGiohang = new List<Giohang>();
                Session["Giohang"] = lstGiohang;

            }
            return lstGiohang;
        }
        // Theo san pham vao gio hang
        public ActionResult ThemGiohang(int iMaSP, string strURL)
        {
            // Lay ra Session gio hang
            List<Giohang> lstGiohang = Laygiohang();
            // Kiem tra sach nay co ton tai trong session["Giohang"] chưa ?
            Giohang sanpham = lstGiohang.Find(n => n.iMaSP == iMaSP);
            if (sanpham == null)
            {
                sanpham = new Giohang(iMaSP);
                lstGiohang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.iSoLuong++;
                return Redirect(strURL);
            }
        }
        // Tong so luong
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                iTongSoLuong = lstGiohang.Sum(n => n.iSoLuong);
            }
            return iTongSoLuong;
        }
        // Tinh tong tien
        private double TongTien()
        {
            double iTongTien = 0;
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                iTongTien = lstGiohang.Sum(n => n.dThanhTien);
            }
            return iTongTien;
        }
        // Xay dung trang Gio Hang
        public ActionResult GioHang()
        {
            List<Giohang> lstGiohang = Laygiohang();
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "Home");

            }
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstGiohang);
        }
        // Tao Partial view de hien thi thong tin gio hang
        public ActionResult Giohangpartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return PartialView();
        }
        // Xoa Giohang
        public ActionResult XoaGiohang(int iMaSP)
        {
            //Lay gio hang tu Session
            List<Giohang> lstGiohang = Laygiohang();
            // Kiem tra sach da co trong session["Giohang"]
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.iMaSP == iMaSP);
            // Neu ton tai thi sua Soluong
            if (sanpham != null)
            {
                lstGiohang.RemoveAll(n => n.iMaSP == iMaSP);
                return RedirectToAction("GioHang");
            }
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("GioHang");
        }
        // Cap nhat Gio Hang
        public ActionResult CapnhatGiohang(int iMaSp, FormCollection f)
        {
            // Lay gio hang tu Session
            List<Giohang> lstGiohang = Laygiohang();
            // Kiem tra sach da co trong Session["Giohang"]
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.iMaSP == iMaSp);
            //Neu ton tai thi cho sua Soluong
            if (sanpham != null)
            {
                sanpham.iSoLuong = int.Parse(f["txtSoluong"].ToString());
            }
            return RedirectToAction("Giohang");
        }
        public ActionResult XoaTatcaGiohang()
        {
            //Lay gio hang tu session
            List<Giohang> lstGiohang = Laygiohang();
            lstGiohang.Clear();
            return RedirectToAction("Index", "Home");
        }
        // Hien thi View dat hang de cap nhat thong tin cho Don hang
        [HttpGet]
        public ActionResult DatHang()
        {
            //Kiem tra dang nhap
            if(Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("Dangnhap", "Nguoidung");
            }
            if(Session["Giohang"]== null)
            {
                return RedirectToAction("Index", "Home");
            }
            //Lay gio hang tu Session
            List<Giohang> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(lstGiohang);
        }
        [HttpPost]
        public ActionResult Dathang(FormCollection collection)
        {
            ViewBag.Taikhoan = Session["Taikhoan"];
            //Them don hang
            donHang dh = new donHang();
            khachHang tk = (khachHang)Session["maKH"];
            List<Giohang> gh = Laygiohang();
            dh.maKH = tk.maKH;
            dh.ngayDat = DateTime.Now;
            var ngaygiao = string.Format("{0:MN/DD/YYYY}", collection["NgayGH"]);
            dh.ngayGH = DateTime.Parse(ngaygiao);
            dh.daKichHoat = false;
            db.donHangs.InsertOnSubmit(dh);
            db.SubmitChanges();
            //Them chi tiet don hang
            foreach (var i in gh)
            {
                ctDonHang ctdh = new ctDonHang();
                ctdh.soDH = dh.soDH;
                ctdh.maSP = i.iMaSP;
                ctdh.soLuong = i.iSoLuong;
                ctdh.giaBan = (long)i.dGiaBan;
                db.ctDonHangs.InsertOnSubmit(ctdh);
            }
            db.SubmitChanges();
            Session["Giohang"] = null;
            return RedirectToAction("Xacnhandathang", "Giohang");

        }
        public ActionResult Xacnhandathang()
        {
            return View();
        }
    }
}