using System.Data.SQLite;
using System.Text.RegularExpressions;

namespace ASPForum.Post
{
	public static class PostWriter
	{

		private static SQLiteCommand WritePost = new SQLiteCommand("INSERT INTO " +
			"Posts (Name, PostContent, Other) " +
			"VALUES (@Name, @PostContent, @Other)");

		private static SQLiteCommand WriteThread = new SQLiteCommand("INSERT INTO " +
			"Threads (ThreadID, Name, Content, Other, BoardID) " +
			"VALUES (@ThreadID, @Name, @Content, @Other, @BoardID)");

		/*
		* Request ID from PostCounter based on BoardID
		* check if it already exists
		* if not, write to Posts
		*/
		public static bool WriteThreadToDatabase(int BoardID, string Name, 
			string Content, string Other)
		{
			if (Content == string.Empty)
				return false;
			if (!PostDatabase.BoardIDExists(BoardID)) // Check if board exists
				return false;

			using (SQLiteConnection dbConnection = PostDatabase.GetConnection())
			{
				dbConnection.Open();
				int ThreadID = PostDatabase.GetThreadID(BoardID); // Get ID for this thread

				WriteThread.Connection = dbConnection;
				WriteThread.Parameters.Add(new SQLiteParameter("@ThreadID", ThreadID));
				WriteThread.Parameters.Add(new SQLiteParameter("@Name", Name));
				//WriteThread.Parameters.Add(new SQLiteParameter("@Content", Content));
				WriteThread.Parameters.Add(new SQLiteParameter("@Content", 
					Regex.Replace(Content, @"\r\n?|\n","<br/")));
				WriteThread.Parameters.Add(new SQLiteParameter("@Other", Other));
				WriteThread.Parameters.Add(new SQLiteParameter("@BoardID", BoardID));

				WriteThread.ExecuteNonQuery(); // non-unique threadID
			}

			return true;
		}

		public static bool WritePostToDatabase(string Name, string Content, string Other)
		{
			//PostDatabase.GetConnection().Open();

			if (Content == string.Empty) return false;

			using (SQLiteConnection dbConnection = PostDatabase.GetConnection())
			{
				dbConnection.Open();
				WritePost.Connection = dbConnection;
				
				//WritePost.Parameters.Add("@Name", System.Data.DbType.String).Value = Name;
				WritePost.Parameters.Add(new SQLiteParameter("@Name", Name));
				WritePost.Parameters.Add(new SQLiteParameter("@PostContent", Content));
				WritePost.Parameters.Add(new SQLiteParameter("@Other", Other));

				WritePost.ExecuteNonQuery();
			}

			return true;
		}
	}
}
