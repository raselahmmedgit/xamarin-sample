using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using App.Web1710.Models;

namespace App.Web1710.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly ILoginClient loginClient;
        private readonly ITokenContainer tokenContainer;

        /// <summary>
        /// Default parameterless constructor. 
        /// Delete this if you are using a DI container.
        /// </summary>
        public AccountController()
        {
            tokenContainer = new TokenContainer();
            var apiClient = new ApiClient(HttpClientInstance.Instance);
            loginClient = new LoginClient(apiClient);
        }

        /// <summary>
        /// Default constructor with dependency.
        /// Delete this if you aren't planning on using a DI container.
        /// </summary>
        /// <param name="loginClient">The login client.</param>
        public AccountController(ILoginClient loginClient, ITokenContainer tokenContainer)
        {
            this.loginClient = loginClient;
            this.tokenContainer = tokenContainer;
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var loginSuccess = await PerformLoginActions(model.Email, model.Password);
                if (loginSuccess)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.Clear();
                ModelState.AddModelError("", "The username or password is incorrect");
                return View(model);
            }
            return View(model);

        }

        // Register methods go here, removed for brevity

        private async Task<bool> PerformLoginActions(string email, string password)
        {
            var response = await loginClient.Login(email, password);
            if (response.StatusIsSuccessful)
            {
                tokenContainer.ApiToken = response.Data;
            }
            else
            {
                AddResponseErrorsToModelState(response);
            }

            return response.StatusIsSuccessful;
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await loginClient.Register(model);
                if (response.StatusIsSuccessful)
                {
                    return RedirectToAction("Index", "Home");
                }

                AddResponseErrorsToModelState(response);
            }

            // If we got this far, something failed, redisplay form
            return View(model);

        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}