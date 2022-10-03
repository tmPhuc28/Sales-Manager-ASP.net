using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Doanltweb.Models;

using PagedList.Mvc;
using PagedList;
    
namespace Doanltweb.Controllers
{
    public class HomeController : Controller
    {
        // Tao doi tuong chua toan bo CSDL tu QLBanhang
        QLBanhangDataContext db = new QLBanhangDataContext();
        // GET: Home
        // lay sản phẩm mới
        private List<sanPham> Laysanpham(int count)
        {
            // Sap xep giam dan theo ngayDang, lay count dong dau
            return db.sanPhams.OrderByDescending(a => a.ngayDang).Take(count).ToList();
        }
        // lấy sản phẩm giảm giá
        private List<sanPham> Sanphamgiam(int count)
        {
            // Sap xep giam dan theo giamgia, lay count dong dau
            return db.sanPhams.OrderByDescending(a => a.giamGia).Take(count).ToList();
        }
        // partial view loại sản phẩm
        public ActionResult Type()
        {
            var type = from cd in db.loaiSPs select cd;
            return PartialView(type);
        }
        //
        public ActionResult Producttype(int id)
        {
            var sanpham = from s in db.sanPhams where s.maLoai == id select s;
            return View(sanpham);
        }
        public ActionResult Products(int ? page)
        {
            //Tham số int? dùng để thể hiện null và kiểu int
            // page có thể có giá trị là null và kiểu int.

            // Nếu page = null thì đặt lại là 1.
            if (page == null) page = 1;

            // Tạo truy vấn
            var sanpham = (from l in db.sanPhams select l).OrderBy(x => x.ngayDang);

            // Tạo kích thước trang (pageSize) hay là số Link hiển thị trên 1 trang
            int pageSize = 6;
            //mô tả nếu page khác null thì lấy giá trị page, còn nếu page = null thì lấy giá trị 1 cho biến pageNumber.
            int pageNumber = (page ?? 1);

            //Trả về các Link được phân trang theo kích thước và số trang.
            return View(sanpham.ToPagedList(pageNumber, pageSize));    
        }
        public ActionResult Details(int id)
        {
            var sanpham = from s in db.sanPhams
                       where s.maSP == id
                       select s;
            ViewBag.sanpham = sanpham;
            return View(sanpham);
        }
        public ActionResult Index()
        {
            //lay 10 san pham giam gia nhieu nhat
            var spgiamgia = Sanphamgiam(6);
            ViewBag.spgiamgia = spgiamgia;

            // Lay 6 san pham moi 
            var sanphammoi = Laysanpham(6);
            ViewBag.sanphammoi = sanphammoi;

            // Lay san pham dua vao ma loai la 5 (Thiet bi thong minh)
            var sanphami = (from i in db.sanPhams
                            where i.maLoai == 5
                            select i).ToList();
            ViewBag.sanphami = sanphami;

            // Lay san pham dua vao ma loai la 2 (Dien gia dung)
            var sanphamii = (from i in db.sanPhams
                            where i.maLoai == 2
                            select i).ToList();
            ViewBag.sanphamii = sanphamii;

            return View();
        }
    }
}