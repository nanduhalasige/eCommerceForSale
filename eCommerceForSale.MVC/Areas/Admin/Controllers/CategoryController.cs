using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eCommerceForSale.Data.Repositories.IRepositories;
using eCommerceForSale.Entity.Models;
using eCommerceForSale.Utility;
using eCommerceForSale.Utility.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceForSale.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Constants.AdminRole)]
    [Authorize(Roles = Constants.EmployeeRole)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public CategoryController(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddOrModifyCategory(Guid? id)
        {
            var category = new Category();
            if (id == null)
            {
                return View(category);
            }
            category = unitOfWork.Category.Get(id.GetValueOrDefault());
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        #region Methods

        [HttpGet]
        public async Task<IEnumerable<Category>> GetAllCategory()
        {
            var categories = await unitOfWork.Category.GetAll();
            return categories;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddOrModifyCategory(Category category)
        {
            string message = "";
            if (ModelState.IsValid)
            {
                if (category.Id == null || category.Id == new Guid())
                {
                    category.IsActive = true;
                    unitOfWork.Category.Add(category);
                    message = "New category added";
                }
                else
                {
                    unitOfWork.Category.Update(category);
                    message = "Category updated";
                }
                unitOfWork.Save();
                return RedirectToAction(nameof(Index)).WithSuccess(message);
            }
            return View(category);
        }

        [HttpDelete]
        public IActionResult SoftDelete(Guid id)
        {
            var category = unitOfWork.Category.Get(id);
            if (category == null)
            {
                return Json(new { success = false, message = "Error occured while deleting" });
            }
            unitOfWork.Category.softDelete(id);
            unitOfWork.Save();
            return Json(new { success = true, message = "Category deactivated" });
        }

        [HttpDelete]
        public IActionResult HardDelete(Guid id)
        {
            var category = unitOfWork.Category.Get(id);
            if (category == null)
            {
                return Json(new { success = false, message = "Error occured while deleting" });
            }
            unitOfWork.Category.Remove(id);
            unitOfWork.Save();
            return Json(new { success = true, message = "Category deleted" });
        }

        #endregion Methods
    }
}