using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.TagHelpers;

namespace ASPForum.Pages
{
	public class IndexModel : PageModel
	{

		public IActionResult OnGet()
		{
			return Page();
		}

	}
}
