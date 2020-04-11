using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerceForSale.Data.Data;
using eCommerceForSale.Entity.Models;
using eCommerceForSale.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceForSale.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Constants.AdminRole)]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _db;

        public UsersController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region API Calls

        [HttpGet]
        public List<ApplicationUser> GetAll()
        {
            var appUsers = _db.ApplicationUsers.ToList();
            var userRoles = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();
            foreach (var user in appUsers)
            {
                var roleId = userRoles.FirstOrDefault(x => x.UserId.Equals(user.Id)).RoleId;
                user.Role = roles.FirstOrDefault(r => r.Id.Equals(roleId)).Name;
            }

            return appUsers;
        }

        [HttpPost]
        public IActionResult LockUnlockUser([FromBody]string id)
        {
            var userToLockUnlock = _db.ApplicationUsers.FirstOrDefault(x => x.Id.Equals(id));
            if (userToLockUnlock == null)
            {
                return Json(new { success = false, message = "Failed to Lock/Unlock user" });
            }
            if (userToLockUnlock.LockoutEnd == null && userToLockUnlock.LockoutEnd > DateTime.Now)
            {
                userToLockUnlock.LockoutEnd = DateTime.Now;
            }
            else
            {
                userToLockUnlock.LockoutEnd = DateTime.Now.AddYears(1);
            }
            _db.SaveChanges();
            return Json(new { success = true, message = "Operation successfull" });
        }

        #endregion API Calls
    }
}