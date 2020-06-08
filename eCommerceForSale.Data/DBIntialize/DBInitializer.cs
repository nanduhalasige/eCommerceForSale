using eCommerceForSale.Data.Data;
using eCommerceForSale.Entity.Models;
using eCommerceForSale.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eCommerceForSale.Data.DBIntialize
{
    public class DBInitializer : IDBInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DBInitializer(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            try
            {
                if (_context.Database.GetPendingMigrations().Count() > 0)
                {
                    _context.Database.Migrate();
                }
            }
            catch
            {
            }

            if (_context.Roles.Any(r => r.Name.Equals(Constants.AdminRole))) return;

            _roleManager.CreateAsync(new IdentityRole(Constants.AdminRole)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Constants.EmployeeRole)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Constants.CustomerRole)).GetAwaiter().GetResult();

            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                FullName = "Nandan Hegde"
            }, "Admin@123").GetAwaiter().GetResult();

            ApplicationUser adminUser = _context.ApplicationUsers.Where(u => u.Email.Equals("admin@gmail.com")).FirstOrDefault();

            _userManager.AddToRoleAsync(adminUser, Constants.AdminRole).GetAwaiter().GetResult();
        }
    }
}