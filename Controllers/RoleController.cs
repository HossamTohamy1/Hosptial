using Hospitel_Project.Context;
using Hospitel_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hospitel_Project.Controllers
{
    
    public class RoleController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        public RoleController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager , ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public IActionResult AddRole()
        {
            return View("AddRole");
        }
        [HttpPost]
        public async Task <IActionResult>  SaveRole( RoleViewModel roleViewModel)
        {
            if(ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole();
                role.Name = roleViewModel.RoleName;
                IdentityResult result= await _roleManager.CreateAsync(role);

                if(result.Succeeded)
                {
                    ViewBag.Succeeded=true;
                    return View("AddRole");
                }
                foreach(var erorr in result.Errors)
                {
                    ModelState.AddModelError("", erorr.Description);
                }
            }
            return View("AddRole",roleViewModel);
        }
        public async Task<IActionResult> ManageUsers()
        {
            var users = await _userManager.Users.ToListAsync();

            var usersWithoutRoles = new List<ApplicationUser>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Count == 0)
                {
                    usersWithoutRoles.Add(user);
                }
            }

            return View(usersWithoutRoles);
        }


        public async Task<IActionResult> PromoteToAdmin(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                await _userManager.AddToRoleAsync(user, "Admin");
            }
            return RedirectToAction("ManageUsers");
        }


        //================================================================

        public async Task<IActionResult> ManageAdmins() // view for DemoteAdmin
        {
            var users = await _userManager.Users.ToListAsync();
            var usersWithRoles = new List<(ApplicationUser User, List<string> Roles)>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                usersWithRoles.Add((user, roles.ToList()));
            }

            return View("ManageRoles", usersWithRoles);
        }

        //public async Task<IActionResult> ManageRoles()
        //{
        //    var users = await _userManager.Users.ToListAsync();
        //    var usersWithRoles = new List<(ApplicationUser User, List<string> Roles)>();

        //    foreach (var user in users)
        //    {
        //        var roles = await _userManager.GetRolesAsync(user);
        //        usersWithRoles.Add((user, roles.ToList()));
        //    }

        //    return View("ManageRoles", usersWithRoles); 
        //}

        //================================================================
        [HttpPost]
        public async Task<IActionResult> DemoteAdmin(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                if (role == "Admin")
                {
                    
                    var admins = await _userManager.GetUsersInRoleAsync("Admin");
                    if (admins.Count > 1)
                    {
                        await _userManager.RemoveFromRoleAsync(user, "Admin");
                    }
                    else
                    {
                        TempData["Error"] = "Cannot remove the last Admin.";
                        return RedirectToAction("ManageAdmins");
                    }
                }
                else if (role == "Doctor" || role == "Patient")
                {
                    if (await _userManager.IsInRoleAsync(user, role))
                    {
                        await _userManager.RemoveFromRoleAsync(user, role);
                    }
                }
            }

            return RedirectToAction("ManageAdmins");
        }
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> AssignRole(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            if (role == "Doctor")
            {
                // البحث عن الطبيب في جدول Doctors باستخدام البريد الإلكتروني
                var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Email == user.Email);
                if (doctor != null)
                {
                    user.DoctorId = doctor.DoctorId; // تحديث DoctorId في AspNetUsers
                    user.Name = doctor.Name; // تحديث الاسم
                }
            }
            else if (role == "Patient")
            {
                // البحث عن المريض في جدول Patients باستخدام البريد الإلكتروني
                var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Email == user.Email);
                if (patient != null)
                {
                    user.PatientId = patient.PatientId; // تحديث PatientId في AspNetUsers
                    user.Name = patient.FirstName; // تحديث الاسم
                }
            }

            // إضافة المستخدم إلى الدور وتحديث البيانات
            await _userManager.AddToRoleAsync(user, role);
            await _userManager.UpdateAsync(user); // حفظ التحديثات

            return RedirectToAction("ManageUsers");
        }





    }
}
