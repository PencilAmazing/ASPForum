using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASPForum.Pages
{
	public class AdminPanelModel : PageModel
    {

		public bool IsAuthenticated = false;

        public IActionResult OnGet()
        {
			/*if(HttpContext.Request.Cookies[AdminManager.LoginCookie] == null)
			{
				IsAuthenticated = false;
				return Page();
			} else
			{
				IsAuthenticated = true;
				return Page();
			}*/
			return Page();
        }
		/*
		[Route("[action]")]
		[HttpPost]
		public IActionResult Nuke()
		{
			
			return Page();
		}*/
    }
}