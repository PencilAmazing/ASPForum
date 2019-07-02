using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPForum.ViewComponents
{
	[ViewComponent]
	public class ThreadView : ViewComponent
	{

		public ThreadView()
		{

		}

		public async Task<IViewComponentResult> InvokeAsync(List<Post.ThreadInfo> ThreadInfo)
		{
			return View("Default");
		}
	}
}
