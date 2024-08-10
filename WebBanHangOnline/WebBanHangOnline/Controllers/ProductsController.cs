using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Products
        public ActionResult Index(string searchtext, int? page)
        {
            int pageSize = 10; // Số lượng sản phẩm hiển thị mỗi trang
            int pageNumber = (page ?? 1); // Trang hiện tại

            // Lấy danh sách sản phẩm từ database
            var products = db.Products.AsQueryable();

            // Nếu có từ khóa tìm kiếm, lọc danh sách sản phẩm theo từ khóa
            if (!string.IsNullOrEmpty(searchtext))
            {
                products = products.Where(p => p.Title.Contains(searchtext));
            }

            // Sắp xếp và phân trang
            var pagedProducts = products.OrderBy(p => p.Title).ToPagedList(pageNumber, pageSize);

            return View(pagedProducts); // Trả về view với danh sách sản phẩm phân trang
        }

        public ActionResult Detail(string alias,int id)
        {
            var item = db.Products.Find(id);
            if (item != null)
            {
                db.Products.Attach(item);
                item.ViewCount = item.ViewCount + 1;
                db.Entry(item).Property(x => x.ViewCount).IsModified = true;
                db.SaveChanges();
            }
            
            return View(item);
        }
        public ActionResult ProductCategory(string alias,int id)
        {
            var items = db.Products.ToList();
            if (id > 0)
            {
                items = items.Where(x => x.ProductCategoryId == id).ToList();
            }
            var cate = db.ProductCategories.Find(id);
            if (cate != null)
            {
                ViewBag.CateName = cate.Title;
            }

            ViewBag.CateId = id;
            return View(items);
        }

        public ActionResult Partial_ItemsByCateId()
        {
            var items = db.Products.Where(x => x.IsHome && x.IsActive).Take(12).ToList();
            return PartialView(items);
        }

        public ActionResult Partial_ProductSales()
        {
            var items = db.Products.Where(x => x.IsSale && x.IsActive).Take(12).ToList();
            return PartialView(items);
        }
        //alldelete de nham ben trang chu controller
        //[HttpPost]
        //public ActionResult DeleteAll(string ids)
        //{
        //    if (string.IsNullOrEmpty(ids))
        //    {
        //        return Json(new { success = false, message = "ID không được để trống." });
        //    }

        //    var itemIds = ids.Split(',').Select(id =>
        //    {
        //        int idValue;
        //        return int.TryParse(id, out idValue) ? (int?)idValue : null;
        //    }).Where(id => id.HasValue).Select(id => id.Value).ToList();

        //    if (!itemIds.Any())
        //    {
        //        return Json(new { success = false, message = "Không có ID hợp lệ để xóa." });
        //    }

        //    // Tìm tất cả các sản phẩm có id trong danh sách itemIds
        //    var itemsToDelete = db.News.Where(n => itemIds.Contains(n.Id)).ToList();

        //    if (!itemsToDelete.Any())
        //    {
        //        return Json(new { success = false, message = "Không tìm thấy sản phẩm nào để xóa." });
        //    }

        //    db.News.RemoveRange(itemsToDelete);
        //    db.SaveChanges();

        //    return Json(new { success = true });
        //}



    }
}