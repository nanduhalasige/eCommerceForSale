using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using eCommerceForSale.Data.Repositories.IRepositories;
using eCommerceForSale.Entity.Models;
using eCommerceForSale.Entity.ViewModels;
using eCommerceForSale.Utility;
using eCommerceForSale.Utility.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eCommerceForSale.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Constants.AdminRole)]
    public class ProductsController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ProductsController(IUnitOfWork _unitOfWork, IWebHostEnvironment _webHostEnvironment)
        {
            unitOfWork = _unitOfWork;
            webHostEnvironment = _webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddOrModifyProduct(int? id)
        {
            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategoryList = unitOfWork.Category.GetAll(x => x.IsActive.Equals(true)).Result.Select(x => new SelectListItem
                {
                    Text = x.CategoryName,
                    Value = x.Id.ToString()
                })
            };
            if (id == null)
            {
                return View(productVM);
            }
            productVM.Product = unitOfWork.Product.Get(id.GetValueOrDefault());

            //string UploadedFolder = Path.Combine(webHostEnvironment.WebRootPath, "asset/images/Products");
            //productVM.

            if (productVM.Product == null)
            {
                return NotFound();
            }
            return View(productVM);
        }

        #region Methods

        [HttpGet]
        public IEnumerable<Product> GetAllProduct()
        {
            var products = unitOfWork.Product.GetAll(isIncludeProperties: "Category").Result.OrderByDescending(x => x.CreatedOn);
            return products;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(int.MaxValue)]
        [RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue)]
        public IActionResult AddOrModifyProduct(ProductVM productVM)
        {
            var product = productVM.Product;
            string message = "";

            if (ModelState.IsValid)
            {
                string imageUploadPath = @"asset\images\Products\";
                string imagePathString = null;
                if (productVM.Photos != null && productVM.Photos.Count > 0)
                {
                    var newPhotosName = new List<string>();
                    if (product.ImagePath != null)
                    {
                        //Filter edited photos and delete unused pics from folder
                        var OldphotosModified = product.ImagePath.Split('~').ToList();
                        OldphotosModified = OldphotosModified.Where(x => !string.IsNullOrEmpty(x)).ToList();

                        var ImapePathDb = unitOfWork.Product.Get(product.Id).ImagePath.Split('~').ToList();
                        ImapePathDb = ImapePathDb.Where(x => !string.IsNullOrEmpty(x)).ToList();

                        var picsToRemove = ImapePathDb.Except(OldphotosModified).ToList();
                        DeleteUnusedPhotos(picsToRemove);
                    }
                    //Append New images to ImagePath string
                    foreach (IFormFile photo in productVM.Photos)
                    {
                        imagePathString = CopyPhotoToFolder(imageUploadPath, imagePathString, photo);
                    }
                }
                product.ImagePath = imagePathString + product.ImagePath;
                product.CreatedOn = DateTime.Now;
                if (product.Id == 0)
                {
                    product.IsActive = true;
                    unitOfWork.Product.Add(product);
                    message = "New Product added";
                }
                else
                {
                    unitOfWork.Product.Update(product);
                    message = "Product updated";
                }
                unitOfWork.Save();
                return RedirectToAction(nameof(Index)).WithSuccess(message);
            }
            return View(productVM);
        }

        private void DeleteUnusedPhotos(List<string> picsToRemove)
        {
            foreach (var removePic in picsToRemove)
            {
                var path = Path.Combine(webHostEnvironment.WebRootPath, removePic);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
        }

        private string CopyPhotoToFolder(string imageUploadPath, string imagePathString, IFormFile photo)
        {
            string UploadFolder = Path.Combine(webHostEnvironment.WebRootPath, imageUploadPath);
            if (!Directory.Exists(UploadFolder))
            {
                Directory.CreateDirectory(UploadFolder);
            }
            var uniqueFileName = Guid.NewGuid().ToString() + photo.FileName;
            string filePath = Path.Combine(UploadFolder, uniqueFileName);
            photo.CopyTo(new FileStream(filePath, FileMode.Create));
            imagePathString += @"/asset/images/Products/" + uniqueFileName + "~";
            return imagePathString;
        }

        [HttpDelete]
        public IActionResult SoftDelete(int id)
        {
            var Product = unitOfWork.Product.Get(id);
            if (Product == null)
            {
                return Json(new { success = false, message = "Error occured while deleting" });
            }
            unitOfWork.Product.softDelete(id);
            unitOfWork.Save();
            return Json(new { success = true, message = "Product deactivated" });
        }

        [HttpDelete]
        public IActionResult HardDelete(int id)
        {
            var product = unitOfWork.Product.Get(id);
            if (product == null)
            {
                return Json(new { success = false, message = "Error occured while deleting" });
            }
            //Delete images uploaded for the deleting product
            var imagesToRemove = product.ImagePath.Split('~').Where(x => !string.IsNullOrEmpty(x)).ToList();
            DeleteUnusedPhotos(imagesToRemove);

            unitOfWork.Product.Remove(id);
            unitOfWork.Save();
            return Json(new { success = true, message = "Product deleted" });
        }

        #endregion Methods
    }
}