using ASPForum.Post;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASPForum.Pages
{
	public class AdminPanelModel : PageModel
    {
		public string UserID { get; set; }
		//public bool IsAuthenticated = false;

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

		[HttpPost]
		public IActionResult OnPostNuke()
		{
			if (IsAuthenticated()) PostDatabase.WipeDatabase();
			return Page();
		}

		private bool IsAuthenticated()
		{
			return Request.HttpContext.Connection.RemoteIpAddress.ToString() == "127.0.0.1";
		}
	}
}