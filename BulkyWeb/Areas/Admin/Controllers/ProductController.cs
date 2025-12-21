using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Tasks;
using Microsoft.Build.Tasks.Deployment.Bootstrapper;
using System.Collections.Generic;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IWebHostEnvironment _WebHostEnviroment;

        public ProductController(IUnitOfWork db, IWebHostEnvironment webHostEnviroment)
        {
            _UnitOfWork = db;
            _WebHostEnviroment = webHostEnviroment;
        }

        public IActionResult Index()
        {
            List<Bulky.Models.Product> products = _UnitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return View(products);
        }

        public IActionResult Upsert(int? Id)
        {
            ProductVM productVM = new()
            {
                CategoryList = _UnitOfWork.Category.GetAll().Select(u =>
                    new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    }
                   ),

                Product = new Bulky.Models.Product()
            };

            if (Id == null || Id == 0)
            {
                //create
               
                return View(productVM);
            }
            else
            {
                //update
                productVM.Product = _UnitOfWork.Product.Get(u => u.Id == Id);
            }
            return View(productVM);


        }

        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file) { 
        
           if (ModelState.IsValid)
            {
                var wwwRoot = _WebHostEnviroment.WebRootPath;
                if (file != null) {

                   string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                   string productPath = Path.Combine(wwwRoot, @"images\product");

                    if (!string.IsNullOrEmpty(productVM.Product.ImgUrl))
                    {
                        //Delete old image
                        var oldImage = Path.Combine(wwwRoot, productVM.Product.ImgUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImage))
                        {
                            System.IO.File.Delete(oldImage);

                        }
                    }
                    using(var filestream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(filestream);

                    };
                    productVM.Product.ImgUrl = @"\images\product\" + fileName;

                }

                if(productVM.Product.Id == 0)
                {
                    _UnitOfWork.Product.Add(productVM.Product);
                }
                else
                {
                    _UnitOfWork.Product.Update(productVM.Product);
                }

                 _UnitOfWork.Save();
                TempData["success"] = "Product Created Successfully";
                return RedirectToAction("Index");
            }
            else
            {
                productVM.CategoryList = _UnitOfWork.Category.GetAll().Select(u =>
                    new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    });
            }
                return View(productVM);
        }

       
        #region API CALL
        [HttpGet]
        public IActionResult GetAll() {

            List<Bulky.Models.Product> products = _UnitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data= products});
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var productToBeDeleted = _UnitOfWork.Product.Get(u => u.Id == id);
            if (productToBeDeleted == null)
            {
                return Json(new {success = false, Message= "Error while deleting."});

            }

            var oldImage = Path.Combine(_WebHostEnviroment.WebRootPath, productToBeDeleted.ImgUrl.TrimStart('\\'));

            if (System.IO.File.Exists(oldImage))
            {
                System.IO.File.Delete(oldImage);
            }

            _UnitOfWork.Product.Remove(productToBeDeleted);
            _UnitOfWork.Save();
            return Json(new {success = true, Message= "Delete Successful." });
        }
        #endregion
    }
}
