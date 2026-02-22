using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Tasks;
using Microsoft.Build.Tasks.Deployment.Bootstrapper;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
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
                productVM.Product = _UnitOfWork.Product.Get(u => u.Id == Id, includeProperties:"ProductImages");
            }
            return View(productVM);


        }

        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, List<IFormFile> files) { 
        
           if (ModelState.IsValid)
            {

                if (productVM.Product.Id == 0)
                {
                    _UnitOfWork.Product.Add(productVM.Product);
                }
                else
                {
                    _UnitOfWork.Product.Update(productVM.Product);
                }

                _UnitOfWork.Save();

                var wwwRoot = _WebHostEnviroment.WebRootPath;
                if (files != null) {

                    foreach (IFormFile file in files)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string productPath = @"images\products\product-" + productVM.Product.Id;
                        string finalpath = Path.Combine(wwwRoot, productPath);


                        if (!Directory.Exists(finalpath)) { 

                            Directory.CreateDirectory(finalpath);
                            
                        }

                        using (var filestream = new FileStream(Path.Combine(finalpath, fileName), FileMode.Create))
                        {
                            file.CopyTo(filestream);

                        };

                        ProductImage productImage = new()
                        {
                            ImageUrl = @"\" + productPath + @"\" + fileName,
                            ProductId = productVM.Product.Id,
                        };

                        if(productVM.Product.ProductImages == null)
                        {
                            productVM.Product.ProductImages = new List<ProductImage>();
                        }

                        productVM.Product.ProductImages.Add(productImage);

                    }

                    _UnitOfWork.Product.Update(productVM.Product);
                    _UnitOfWork.Save();


                    //if (!string.IsNullOrEmpty(productVM.Product.ImgUrl))
                    //{
                    //    //Delete old image
                    //    var oldImage = Path.Combine(wwwRoot, productVM.Product.ImgUrl.TrimStart('\\'));

                    //    if (System.IO.File.Exists(oldImage))
                    //    {
                    //        System.IO.File.Delete(oldImage);

                    //    }
                    //}
                    //using (var filestream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    //{
                    //    file.CopyTo(filestream);

                    //};
                    //productVM.Product.ImgUrl = @"\images\product\" + fileName;

                }

              
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

        public IActionResult DeleteImage(int imageId)
        {
            var imageToBeDeleted = _UnitOfWork.ProductImage.Get(u => u.Id == imageId);
            int productId = imageToBeDeleted.ProductId;
            if (imageToBeDeleted != null)
            {
                if (!string.IsNullOrEmpty(imageToBeDeleted.ImageUrl))
                {
                    var oldImagePath = Path.Combine(_WebHostEnviroment.WebRootPath,
                        imageToBeDeleted.ImageUrl.TrimStart('\\')
                        );

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                _UnitOfWork.ProductImage.Remove(imageToBeDeleted);
                _UnitOfWork.Save();

                TempData["success"] = "Deleted Successfully";
            }

            return RedirectToAction(nameof(Upsert), new { id = productId });
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

            string productPath = @"images\products\product-" + id;
            string finalPath = Path.Combine(_WebHostEnviroment.WebRootPath, productPath);

            if (Directory.Exists(finalPath))
            {
                string[] filePaths = Directory.GetFiles(finalPath);
                foreach (string filePath in filePaths) { 
                
                  System.IO.File.Delete(filePath);
                }

                Directory.Delete(finalPath);
            }

            //var oldImage = Path.Combine(_WebHostEnviroment.WebRootPath, productToBeDeleted.ImgUrl.TrimStart('\\'));

            //if (System.IO.File.Exists(oldImage))
            //{
            //    System.IO.File.Delete(oldImage);
            //}

            _UnitOfWork.Product.Remove(productToBeDeleted);
            _UnitOfWork.Save();
            return Json(new {success = true, Message= "Product Deleted Successful." });
        }
        #endregion
    }
}
