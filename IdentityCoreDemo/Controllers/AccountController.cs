using IdentityCoreDemo.Models;
using IdentityCoreDemo.Serviecs;
using IdentityCoreDemo.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Encodings.Web;

namespace IdentityCoreDemo.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<Users> signInManager;
        private readonly UserManager<Users> userManager;
        private readonly IEmailServices emailServices;
        public AccountController(SignInManager<Users> signInManager, UserManager<Users> userManager, IEmailServices emailServices)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.emailServices = emailServices;
        }
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            
            if(ModelState.IsValid)
            {
                Users user = new Users 
                { 
                    Name=model.Name,
                    DOB=model.DOB,
                    UserName=model.Email,
                    Email=model.Email,
                    PhoneNumber=model.Phone,
                };
                var res=await userManager.CreateAsync(user, model.Password);
                if(res.Succeeded)
                {
                    TempData["Message"] = "User Sucessfully Registered, Please Login..";
                    return RedirectToAction("Login");
                }
                else
                {
                    ModelState.AddModelError("", "Unable to create user...");
                    foreach(var x in res.Errors)
                    {
                        ModelState.AddModelError("", x.Description);
                    }
                }
            }
            return View(model);
        }
        [AllowAnonymous]
        public IActionResult Login(string? ReturnUrl)
        {
            if(ReturnUrl != null)
            {
                TempData["MessageError"] = "Please log in before using authorized resources..";
            }
            TempData["ReturnUrl"] = ReturnUrl;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
       
            if(ModelState.IsValid)
            {
                var user =await userManager.FindByEmailAsync(model.Email);
             
               
                if(user!=null)
                {   
                    await userManager.UpdateSecurityStampAsync(user);
                }
                var res = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if(res.Succeeded)
                {
                    if (TempData["ReturnUrl"]!=null)
                    {
                        return Redirect(TempData["ReturnUrl"].ToString());
                    }
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid User Id or Password");
                }
               
            }
            return View(model);
        }
        
        public async Task<IActionResult> Logout()
        {
            if(signInManager.IsSignedIn(User))
            {
                await signInManager.SignOutAsync();
                TempData["Message"] = "You have successfully logged out..";
            }
            else
            {
                TempData["MessageError"] = "You have not signed in..";
            }
            return RedirectToAction("Login");
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if(user!=null)
                {
                    var res=SendForgotPasswordEmail(user);
                    if(res)
                    {
                        return RedirectToAction("ForgotPasswordConfirmation");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Unable to send password changing token to your email id..");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "User does registered with this email address");
                }
            }
            return View(model);
        }
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
        public IActionResult ResetPassword(string? Email,string? Token)
        {
            if(Email==null || Token==null)
            {
                return BadRequest();
            }
            ResetPasswordViewModel model = new ResetPasswordViewModel { Email=Email,Token=Token};
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await userManager.ResetPasswordAsync(user, model.Token, model.Password);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ResetPasswordConfirmation");
                    }
                    else
                    {
                        foreach (var x in result.Errors)
                        {
                            ModelState.AddModelError("", x.Description);
                        }
                    }
                }
            }
            return View(model);
        
        }
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
        public bool SendForgotPasswordEmail(Users? user)
        {
            var token = userManager.GeneratePasswordResetTokenAsync(user);
            var passwordResetLink = Url.Action("ResetPassword", "Account", new { Email = user.Email, Token = token.Result },protocol:HttpContext.Request.Scheme);
            var safeLink = HtmlEncoder.Default.Encode(passwordResetLink);
            var subject = "Reset Your Password";
            var messageBody = @$"
                <h2 style='background-color:blue;color:white; margin-top:10px;padding:10px;text-align:center;'>Password Reset Request</h2>
                <p style='margin-bottom:20px;margin-top:20px;'>
                    We received a request to reset your password for your <strong>Indian School of Coding</strong> account. If you made this request, please click the link below to reset your password:
                </p>
                <a href='{safeLink}'>Click here to reset your password...</a> 
               
                <div style='margin-top:20px'>
                   <p>If your are unable to use above link, then copy and paste the below link on address bar of your browser<br/>
                        {safeLink}
                </div>
                <div style='margin-top:20px;'>
                    <strong>Regards,</strong><br/>
                    <i>Indian School of Coding Team</i>
                </div>
                ";

            var res=emailServices.SendMail(user.Email, subject, messageBody);
            return res;
        }
    }
}
