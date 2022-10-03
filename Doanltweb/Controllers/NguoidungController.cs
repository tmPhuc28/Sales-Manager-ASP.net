using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Doanltweb.Models;

namespace Doanltweb.Controllers
{
    public class NguoidungController : Controller
    {
        QLBanhangDataContext db = new QLBanhangDataContext();
        // GET: Nguoidung
        
        [HttpGet]
        public ActionResult Dangky()
        {
            return View();
        }

        //POST: Hàm Dangky(post) Nhận dữ liệu từ trang DangKy và thực hiện việc tạo mới dữ liệu
        [HttpPost]
        public ActionResult Dangky(FormCollection collection, taiKhoanTV kh)
        {
            // Gán các giá trị người dùng nhập liệu cho các biến
            var hodemkh = collection["HodemKH"];
            var tenkh = collection["TenKH"];
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            var nhaplaimatkhau = collection["Nhaplaimatkhau"];
            var email = collection["Diachi"];
            var diachi = collection["Email"];
            var sodienthoai = collection["Dienthoai"];
            
            if (string.IsNullOrEmpty(hodemkh))
            {
                ViewData["Loi1"] = " Họ đệm không được để trống";
            }
            else if (string.IsNullOrEmpty(tendn))
            {
                ViewData["Loi2"] = " Tên không được để trống";
            }
            else if (string.IsNullOrEmpty(tendn))
            {
                ViewData["Loi3"] = " Tên đăng nhập không được để trống";
            }
            else if (string.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi4"] = " Mật khẩu không được để trống";
            }
            else if (string.IsNullOrEmpty(nhaplaimatkhau))
            {
                ViewData["Loi5"] = "Nhập lại mật khẩu";
            }
            if (string.IsNullOrEmpty(diachi))
            {
                ViewData["Loi6"] = "Địa chỉ không được để trống";
            }
            if (string.IsNullOrEmpty(sodienthoai))
            {
                ViewData["Loi7"] = "Số điện thoại không được để trống";
            }
            else
            {
                //Gán giá trị cho đối tượng được tạo mới (kh)
                kh.hoDem = hodemkh;
                kh.tenTV = tenkh;
                kh.taiKhoan = tendn;
                kh.matKhau = matkhau;
                kh.email = email;
                kh.diaChi = diachi;
                kh.soDT = sodienthoai;
               
                db.taiKhoanTVs.InsertOnSubmit(kh);
                db.SubmitChanges();
                return RedirectToAction("Dangnhap");
            }
            return this.Dangky();

        }
        [HttpGet]
        public ActionResult Dangnhap()
        {
            return View();
        }
        //POST: Hàm Dangnhap(post) Nhận dữ liệu từ trang Dangnhap và thực hiện việc tạo mới dữ liệu
        [HttpPost]
        public ActionResult Dangnhap(FormCollection collection)
        {
            //Gán các giá trị người dùng nhập liệu cho các biến
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            if (string.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Tên đăng nhập không được bỏ trống";
            }
            else if (string.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Mật khẩu không được bỏ trống";
            }
            else
            {
                // Gán giá trị đối tượng được tạo mới(kh)
                taiKhoanTV kh = db.taiKhoanTVs.SingleOrDefault(n => n.taiKhoan == tendn && n.matKhau == matkhau);
                if (kh != null)
                {
                    ViewBag.Thongbao = "Đăng nhập thành công";
                    Session["taiKhoan"] = kh;
                    return RedirectToAction("Index", "Home");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
    }
}