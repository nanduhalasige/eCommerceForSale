using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using eCommerceForSale.Data.Repositories.IRepositories;
using eCommerceForSale.Entity.Models;
using eCommerceForSale.Entity.ViewModels;
using eCommerceForSale.Utility.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceForSale.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IWebHostEnvironment webHostEnvironment;

        public CompanyController(IUnitOfWork _unitOfWork, IWebHostEnvironment _webHostEnvironment)
        {
            unitOfWork = _unitOfWork;
            webHostEnvironment = _webHostEnvironment;
        }

        public IActionResult Index()
        {
            var company = unitOfWork.Company.GetAll().Result.FirstOrDefault();
            CompanyView companyView = new CompanyView()
            {
                Company = company ?? new Company(),
                EnableControls = false
            };
            return View(companyView);
        }

        // GET: Company/Create
        public ActionResult AddOrModifyCompany(int? Id)
        {
            var companyView = new CompanyView()
            {
                Company = new Company(),
                EnableControls = true
            };
            var company = new Company();
            if (Id == null)
            {
                return View("Index", companyView);
            }
            companyView.Company = unitOfWork.Company.Get(Id.GetValueOrDefault());
            return View("Index", companyView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddOrModifyCompany(CompanyView companyView)
        {
            if (ModelState.IsValid)
            {
                var message = "";
                var filePath = "";
                var company = companyView.Company;
                string imageUploadPath = @"asset\images\";
                var fileName = "logo.png";
                if (companyView.Logofile != null)
                {
                    string uploadFolder = Path.Combine(webHostEnvironment.WebRootPath, imageUploadPath);
                    filePath = Path.Combine(uploadFolder, fileName);
                    if (System.IO.File.Exists(filePath))
                    {
                        //System.IO.File.Replace(filePath, filePath, uploadFolder + "temp.png.bac");
                        System.IO.File.Delete(filePath);
                    }
                    companyView.Logofile.CopyTo(new FileStream(filePath, FileMode.Create));
                }
                company.LogoPath = @"\asset\images\" + fileName;

                if (company.Id == 0)
                {
                    unitOfWork.Company.Add(company);
                    message = "Company data added";
                }
                else
                {
                    unitOfWork.Company.Update(company);
                    message = "Company updated";
                }
                unitOfWork.Save();
                return RedirectToAction(nameof(Index)).WithSuccess(message);
            }
            else
            {
                return View("Index", companyView);
            }
        }

        // GET: Company/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Company/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Company/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Company/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}