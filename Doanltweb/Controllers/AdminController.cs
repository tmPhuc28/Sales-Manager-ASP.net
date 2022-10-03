using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Doanltweb.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace Doanltweb.Controllers
{
    public class AdminController : Controller
    {
        QLBanhangDataContext db = new QLBanhangDataContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            //Gán các giá trị người dùng nhập liệu cho các biến
            var tendn = collection["username"];
            var matkhau = collection["password"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Vui lòng nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Vui lòng nhập mật khẩu";
            }
            else
            {
                //Gán giá trị cho đói tượng được ạo mới
                Admin ad = db.Admins.SingleOrDefault(n => n.UserAdmin == tendn && n.PassAdmin == matkhau);
                if (ad != null)
                {
                    //ViewBag.Thongbao = "Đăng nhập thành công"
                    Session["Taikhoanadmin"] = ad;
                    return RedirectToAction("Index","Admin");
                }
                ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
        [HttpGet]
        public ActionResult Themmoisanpham()
        {
            ViewBag.maloai = new SelectList(db.loaiSPs.ToList().OrderBy(n => n.maLoai), "maLoai", "loaiHang");
            return View();
        }
        public ActionResult sanpham(int ? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            //return View(db.sanPhams.ToList());
            return View(db.sanPhams.ToList().OrderBy(n => n.maSP).ToPagedList(pageNumber, pageSize));
        }
        [HttpPost]
        public ActionResult Themmoisanpham(sanPham sanpham, HttpPostedFileBase fileupload)
        {
            //Dua du lieu vao dropdownload
            ViewBag.Maloai = new SelectList(db.loaiSPs.ToList().OrderBy(n => n.loaiHang), "maLoai", "loaiHang");
            // Kiem tra duong dan file
            if (fileupload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh";
                return View();
            }
            // Them vao CSDL
            else
            {
                if (ModelState.IsValid)
                {
                    // Luu ten file, bo sung using System.IO;
                    var fileName = Path.GetFileName(fileupload.FileName);
                    //Luu duong dan cua file
                    var path = Path.Combine(Server.MapPath("~/Asset/Images/sanPham"), fileName);
                    //Kiem tra hinh anh ton tai chua ?
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                    {
                        // Luu hinh anh vao duong dan
                        fileupload.SaveAs(path);
                    }
                    sanpham.hinhDD = fileName;
                    //Lưu vào csdl
                    db.sanPhams.InsertOnSubmit(sanpham);
                    db.SubmitChanges();
                }
                return RedirectToAction("sanpham");
               
            }
        }
        public ActionResult Chitietsanpham(int id)
        {
            sanPham sanpham = db.sanPhams.SingleOrDefault(n => n.maSP == id);
            ViewBag.maSP = sanpham.maSP;
            if(sanpham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sanpham);
        }
        [HttpGet]
        public ActionResult Xoasanpham(int id)
        {
            //Lấy ra đối tượng cần xóa theo mã
            sanPham sanpham = db.sanPhams.SingleOrDefault(n => n.maSP == id);
            ViewBag.masp = sanpham.maSP;
            if (sanpham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sanpham);
        }
        [HttpPost, ActionName("Xoasanpham")]
        public ActionResult xacnhanxoasanpham(int id)
        {
            //Lấy ra đối tượng cần xóa theo mã
            sanPham sp = db.sanPhams.SingleOrDefault(n => n.maSP == id);
            ViewBag.masp = sp.maSP;
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.sanPhams.DeleteOnSubmit(sp);
            db.SubmitChanges();
            return RedirectToAction("sanpham");
        }
        [HttpGet]
        public ActionResult Suasanpham(int id)
        {
            //Lấy ra đối tượng cần sửa theo mã
            sanPham sp = db.sanPhams.SingleOrDefault(n => n.maSP == id);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Đưa dữ liệu vào
            //Lấy ds từ table loaiSP, sắp xếp tăng dần theo mã loại, hiển thị tên chủ đề
            ViewBag.maloai = new SelectList(db.loaiSPs.ToList().OrderBy(n => n.maLoai), "maloai", "loaiHang", sp.maSP);
            return View(sp);
        }
    }
}