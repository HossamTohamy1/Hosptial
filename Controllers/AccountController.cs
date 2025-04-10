using Hospitel_Project.Models;
using Hospitel_Project.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using Hospitel_Project.Context;


namespace Hospitel_Project.Controllers
{
    public class AccountController : Controller
    {
        

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly EmailService _emailService;
        private readonly ApplicationDbContext _context;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, EmailService emailService, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _context = context;
            
        }

        // ======== Registration =========
        

        public IActionResult Register()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]


        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            Console.WriteLine("Register method called!");

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {

                   

                    Console.WriteLine("User registered successfully!");

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"Error: {error.Description}");
                    ModelState.AddModelError("", error.Description);
                }
            }
            else
            {
                Console.WriteLine("ModelState is invalid!");
            }

            return View(model);
        }

        

        // ======== Login =========
      
        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "User not found.");
                    return View(model);
                }

                var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    Console.WriteLine("Login successful!");
                    return RedirectToAction("Index", "Home");
                }

                Console.WriteLine("Invalid login attempt.");
                ModelState.AddModelError("", "Invalid login attempt.");
            }

            return View(model);
        }
        
        // ======== Forgot Password =========
        [HttpGet]
        public IActionResult ForgotPassword() => View();

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                TempData["Message"] = "If your email exists, you will receive a reset link.";
                return RedirectToAction("ForgotPasswordConfirmation");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = Url.Action("ResetPassword", "Account", new { email = model.Email, token }, Request.Scheme);

            Console.WriteLine($"Reset Link: {resetLink}");

            await _emailService.SendEmailAsync(
                model.Email,
                "Reset Your Password",
                $@"
 <!DOCTYPE html>
 <html>
 <head>
     <meta charset='UTF-8'>
     <meta name='viewport' content='width=device-width, initial-scale=1.0'>
     <title>Password Reset</title>
     <style>
         body {{
             font-family: Arial, sans-serif;
             line-height: 1.6;
             color: #333333;
             margin: 0;
             padding: 0;
             background-color: #f4f4f4;
         }}
         .container {{
             max-width: 600px;
             margin: 0 auto;
             padding: 20px;
             background-color: #ffffff;
             border-radius: 5px;
             box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
         }}
         .header {{
             text-align: center;
             padding: 20px 0;
             border-bottom: 1px solid #eeeeee;
         }}
         .header h1 {{
             color: #3498db;
             margin: 0;
             font-size: 24px;
         }}
         .content {{
             padding: 20px 0;
         }}
         .button {{
             display: inline-block;
             padding: 12px 24px;
             background-color: #3498db;
             color: #ffffff !important;
             text-decoration: none;
             border-radius: 4px;
             font-weight: bold;
             margin: 20px 0;
         }}
         .footer {{
             text-align: center;
             font-size: 12px;
             color: #888888;
             padding-top: 20px;
             border-top: 1px solid #eeeeee;
         }}
     </style>
 </head>
 <body>
     <div class='container'>
         <div class='header'>
             <h1>Password Reset Request</h1>
         </div>
         <div class='content'>
             <p>Hello,</p>
             <p>We received a request to reset your password. If you did not make this request, please ignore this email.</p>
             <p>To reset your password, please click the button below:</p>
             <div style='text-align: center;'>
                 <a href='{resetLink}' class='button'>Reset Password</a>
             </div>
             
             <p>This password reset link will expire in 24 hours.</p>
             <p>Best regards,<br>The Hospital Team</p>
         </div>
         <div class='footer'>
             <p>This is an automated message, please do not reply to this email.</p>
             <p>&copy; {DateTime.Now.Year} Hospital Management System. All rights reserved.</p>
         </div>
     </div>
 </body>
 </html>",
                isHtml: true);

            TempData["Message"] = "If your email exists, you will receive a reset link.";
            return RedirectToAction("ForgotPasswordConfirmation");
        }

        

       

        public IActionResult ForgotPasswordConfirmation() => View();

        // ======== Logout =========
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        // ======== ResetPassword =========
        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            return View(new ResetPasswordViewModel { Email = email, Token = token });
        }

        

        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
              
                return RedirectToAction("ResetPassword");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (result.Succeeded)
            {
                TempData["Message"] = "Reset password is done successfully.";
                return RedirectToAction("ResetPasswordConfirmation");
            }

           
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }


        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
      
       

        // ======== Default Index =========
        public IActionResult Index3() => View();
    }
}
