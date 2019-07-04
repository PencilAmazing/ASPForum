using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ASPForum.Post;

namespace ASPForum.Pages
{
    public class ThreadModel : PageModel
    {
        public IActionResult OnGet()
        {
			if (RouteData.Values["ID"] == null) {
				return NotFound();
			}

			return Page();
		}
    }
}