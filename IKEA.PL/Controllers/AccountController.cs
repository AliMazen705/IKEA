using Humanizer;
using IKEA.BLL.Common.Services.EmailSetting;
using IKEA.DAL.Models.Identity;
using IKEA.PL.Models.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Security.Cryptography.X509Certificates;

namespace IKEA.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSettings _emailSettings;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,IEmailSettings emailSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSettings = emailSettings;
        }
        #region Register
        #region  Get 
        [HttpGet]
        public async Task<IActionResult> SignUp()
        {
            return View();
        }
        #endregion
        #region Post
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel signUpViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();


            }
            //Check If The User Name  already Exists
            var existingUser = await _userManager.FindByNameAsync(signUpViewModel.UserName);
            if (existingUser != null)
            {
                ModelState.AddModelError(nameof(SignUpViewModel.UserName), "This User Name Is Already Taken");
                return View(signUpViewModel);
            }
            var User = new ApplicationUser()
            {
                FName = signUpViewModel.FirstName,
                LName = signUpViewModel.LastName,
                UserName = signUpViewModel.UserName,
                Email = signUpViewModel.Email,
                IsAgree = signUpViewModel.IsAgree,

            };
            var result = await _userManager.CreateAsync(User, signUpViewModel.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(SignIn));
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(signUpViewModel);

        }
        #endregion
        #endregion
        #region login
        #region get
        [HttpGet]
        //P@ssw0rd
        public async Task<IActionResult> SignIn()
        {
            return View();
        }
        #endregion
        #region post
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel signInViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();


            }
            var User = await _userManager.FindByEmailAsync(signInViewModel.Email);
            if (User is { })
            {
                var flag = await _userManager.CheckPasswordAsync(User, signInViewModel.Password);
                if (flag)
                {
                    var result = await _signInManager.PasswordSignInAsync(User, signInViewModel.Password, signInViewModel.RememberMe, true);
                    if (result.IsNotAllowed)
                    {
                        ModelState.AddModelError(string.Empty, "Your Account Is Not Confirmed Yet");

                    }
                    if (result.IsLockedOut)
                    {
                        ModelState.AddModelError(string.Empty, "Your Account Is Locked");

                    }
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(HomeController.Index), "Home");
                    }


                }

            }


            ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            return View(signInViewModel);
        }


        #endregion
        #endregion
        #region Logout
        public async Task<IActionResult> SignOut()
        {
           await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
        }
        #endregion
        #region forget password
        #region get
        [HttpGet]
        public async Task<IActionResult> ForgetPassword()
        {
            return View();
        }
        #endregion
        #region post
        [HttpPost]
        public async Task<IActionResult> SendRessetPasswordUrl(ForgetPasswordViewModel forgetPasswordViewModel)
        {
            if (ModelState.IsValid)  //!!
            {
                var User=await _userManager.FindByEmailAsync(forgetPasswordViewModel.Email);
                if (User is not null) //لو عندي يوزر بالمواصفات دي
                {var token= await _userManager.GeneratePasswordResetTokenAsync(User);
                   // var token = await _userManager.FindByEmailAsync(forgetPasswordViewModel.Email);

                    //https://localhost:7178/Account/ResetPassword/alimazen09811@gmail.com
                    var url = Url.Action("ResetPassword", "Account", new {email=forgetPasswordViewModel.Email,token=token},Request.Scheme);
                    //to , subject , body
                    var emailSend = new EmailSend()
                    {
                        To = forgetPasswordViewModel.Email,
                        Subject = "Reset Your Password",
                        Body = url
                    };
                    //send email
                    _emailSettings.SendEmail(emailSend);
                    return RedirectToAction("CheckYourInbox");
                      
                         
                }
                ModelState.AddModelError(string.Empty, "Invalid Operation, Please Try");
            }
            return View(forgetPasswordViewModel);
        
        

        }
        #endregion

        #endregion
        #region Check Inbox
        [HttpGet]
        public async Task<IActionResult> CheckYourInbox()
        {
            return View();
        }
        #endregion
        #region reset password
        [HttpGet]
        public IActionResult ResetPassword(string email,string token)
        {
            TempData["email"]=email;
            TempData["token"]=token;
            //pass email, token
            return View();

        }
        //------------------------------------------------

        [HttpPost]
        public async Task< IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                var email =TempData["email"] as string;
                var token =TempData["token"] as string;
                var user= await _userManager.FindByEmailAsync(email);
                if (user is not null)
                {
              var result=await _userManager.ResetPasswordAsync(user,token,resetPasswordViewModel.Password);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(SignIn));
                    }
                    
                }

            }
            ModelState.AddModelError(string.Empty, "Invalid Operation  Please Try Again");
            return View(resetPasswordViewModel);
        }

        #endregion

    }
}

