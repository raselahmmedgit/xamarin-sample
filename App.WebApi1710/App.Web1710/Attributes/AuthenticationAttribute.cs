using App.Web1710.Helpers;
using App.Web1710.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Web1710.Attributes
{
    public class AuthenticationAttribute : ActionFilterAttribute
    {
        private readonly ITokenContainer tokenContainer;

        public AuthenticationAttribute()
        {
            tokenContainer = new TokenContainer();
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            if (tokenContainer.ApiToken == null)
            {
                filterContext.HttpContext.Response.RedirectToRoute(AppConstant.LoginUrl);
            }
        }
    }
}