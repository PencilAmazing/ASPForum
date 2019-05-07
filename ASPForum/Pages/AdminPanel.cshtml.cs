using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ASPForum.AdminClasses;

namespace ASPForum.Pages
{
    public class AdminPanelModel : PageModel
    {

		public bool IsAuthenticated = false;

        public IActionResult OnGet()
        {
			if(HttpContext.Request.Cookies[AdminManager.LoginCookie] == null)
			{
				IsAuthenticated = false;
				return Page();
			} else
			{
				IsAuthenticated = true;
				return Page();
			}
        }
    }
}