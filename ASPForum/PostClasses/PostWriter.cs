using System.Data.SQLite;
using System.Text.RegularExpressions;

namespace ASPForum.Post
{
	public static class PostWriter
	{

		private static SQLiteCommand WritePost = new SQLiteCommand("INSERT INTO " +
			"Posts (PostID, Name, Content, Other, ThreadID, BoardID) " +
			"VALUES (@PostID, @Name, @Content, @Other, @ThreadID, @BoardID)");

		private static SQLiteCommand WriteThread = new SQLiteCommand("INSERT INTO " +
			"Threads (ThreadID, Name, Content, Other, BoardID) " +
			"VALUES (@ThreadID, @Name, @Content, @Other, @BoardID)");

		/*
		* Request ID from PostCounter based on BoardID
		* check if it already exists
		* if not, write to Posts
		* Return ThreadID which was written for redirects and such
		*/
		public static int? WriteThreadToDatabase(int BoardID, string Name,
			string Content, string Other)
		{
			if (string.IsNullOrWhiteSpace(Content))
				return null;
			if (!PostDatabase.BoardIDExists(BoardID)) // Check if board exists
				return null;

			int ThreadID;
			using (SQLiteConnection dbConnection = PostDatabase.GetConnection()) {
				dbConnection.Open();
				ThreadID = PostDatabase.GetThreadID(BoardID); // Get ID for this thread

				WriteThread.Connection = dbConnection;
				WriteThread.Parameters.Add(new SQLiteParameter("@ThreadID", ThreadID));
				WriteThread.Parameters.Add(new SQLiteParameter("@Name", Name));
				WriteThread.Parameters.Add(new SQLiteParameter("@Content", Content.Trim()));
				//WriteThread.Parameters.Add(new SQLiteParameter("@Content", Regex.Replace(Content, @"\r\n?|\n",@"<br>")));
				WriteThread.Parameters.Add(new SQLiteParameter("@Other", Other));
				WriteThread.Parameters.Add(new SQLiteParameter("@BoardID", BoardID));

				WriteThread.ExecuteNonQuery(); // non-unique threadID
			}

			//WritePostToDatabase(BoardID, ThreadID, Name, Content, Other)

			return ThreadID;
		}

		public static int WritePostToDatabase(int OwningBoard, int OwningThread,
			string Name, string Content, string Other)
		{
			//PostDatabase.GetConnection().Open();

			if (string.IsNullOrWhiteSpace(Content)) return -1;
			if (!PostDatabase.BoardIDExists(OwningBoard) || !PostDatabase.ThreadIDExists(OwningBoard, OwningThread))
				return -1; // Check if board exists

			int PostID = -1;
			using (SQLiteConnection dbConnection = PostDatabase.GetConnection()) {
				dbConnection.Open();
				PostID = PostDatabase.GetThreadID(OwningBoard);

				WritePost.Connection = dbConnection;
				//WritePost.Parameters.Add("@Name", System.Data.DbType.String).Value = Name;
				WritePost.Parameters.Add(new SQLiteParameter("@PostID", PostID));
				WritePost.Parameters.Add(new SQLiteParameter("@Name", Name));
				WritePost.Parameters.Add(new SQLiteParameter("@Content", Content));
				WritePost.Parameters.Add(new SQLiteParameter("@Other", Other));
				WritePost.Parameters.Add(new SQLiteParameter("@ThreadID", OwningThread));
				WritePost.Parameters.Add(new SQLiteParameter("@BoardID", OwningBoard));

				WritePost.ExecuteNonQuery();
			}

			return PostID;
		}
	}
}
