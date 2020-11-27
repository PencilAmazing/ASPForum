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
			NameEmpty,
			RequestWasGET,
			IncorrectID
		};

		public PostStatus Status = PostStatus.IncorrectID;
		public int ReturnToThread;
		public int ReturnToBoard;

		public void OnGet()
		{
			Status = PostStatus.RequestWasGET;
			//RedirectToPage("/Index");
			//return Redirect(Url.Content("~/"))
		}

		[HttpPost]
		// Making new threads handler
		public IActionResult OnPostThread(int? BoardID, string NameInput, string OtherInput, string ContentInput)
		{
			if (string.IsNullOrWhiteSpace(ContentInput)) { // Content is empty
				Status = PostStatus.ContentEmpty;
			} else if (!BoardID.HasValue) {
				Status = PostStatus.IncorrectID;
			} else // All OK
			  {
				Status = PostStatus.OK;
				ReturnToBoard = BoardID.Value;
				ReturnToThread = Post.PostWriter.WriteThreadToDatabase(BoardID.Value, NameInput, ContentInput, OtherInput).Value;
				System.Console.WriteLine("Wrote to database");
			}

			return Page();
		}

		[HttpPost]
		// Reply to threads handler
		public IActionResult OnPostReply(int? BoardID, int? ThreadID, string NameInput, string OtherInput, string ContentInput)
		{
			if (string.IsNullOrWhiteSpace(ContentInput)) {
				Status = PostStatus.ContentEmpty;
			} else if (!BoardID.HasValue || !ThreadID.HasValue) {
				Status = PostStatus.IncorrectID;
			} else {
				Status = PostStatus.OK;
				ReturnToBoard = BoardID.Value;
				ReturnToThread = ThreadID.Value;
				Post.PostWriter.WritePostToDatabase(BoardID.Value, ThreadID.Value, NameInput, ContentInput, OtherInput);
			}

			return Page();
		}
	}
}