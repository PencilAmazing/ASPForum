using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASPForum.Pages
{

	public class ReplyModel : PageModel
    {

		public enum PostStatus : byte
		{
			OK,
			ContentEmpty,
			NameEmpty
		};
		public PostStatus Status;

        public void OnGet()
        {
			RedirectToPage("Index");	
        }

		[HttpPost]
		public IActionResult OnPost(string NameInput, string OtherInput, string ContentInput, int BoardID)
		{
			if (string.IsNullOrWhiteSpace(ContentInput)) { // Content is empty
				Status = PostStatus.ContentEmpty;
			}
			else // All OK
			{
				Status = PostStatus.OK;
				//Post.PostWriter.WritePostToDatabase(NameInput, ContentInput, OtherInput);
				Post.PostWriter.WriteThreadToDatabase(BoardID, NameInput, ContentInput, OtherInput);
				System.Console.WriteLine("Wrote to database");
			}

			return Page();
		}
    }
}