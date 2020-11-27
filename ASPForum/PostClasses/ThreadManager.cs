using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using ASPForum.Post;

namespace ASPForum.Post
{
	public struct ThreadInfo
	{
		public int ThreadID;
		public string Name, Content, Other;

		public ThreadInfo(int ThreadID, string Name, string Content, string Other)
		{
			// Return someone if Name given is empty
			this.Name = (Name != string.Empty ? Name : "Someone");
			this.Content = Content;
			this.Other = Other;
			this.ThreadID = ThreadID;
		}
	}

	public static class ThreadManager
	{
		private static SQLiteCommand SelectAllPosts = new SQLiteCommand("SELECT * FROM Posts " +
			"WHERE ThreadID=@ThreadID AND BoardID=@BoardID");
		private static SQLiteCommand SelectThread = new SQLiteCommand("SELECT * FROM Threads " +
			"Where BoardID=@BoardID AND ThreadID=@ThreadID");
		private static SQLiteCommand SelectAllThreads = new SQLiteCommand("SELECT * FROM Threads " +
			"WHERE BoardID=@BoardID ORDER BY ThreadID DESC");

		public static List<ThreadInfo> GetAllThreads(int BoardID)
		{
			var ThreadList = new List<ThreadInfo>();
			SelectAllThreads.Parameters.Add(new SQLiteParameter("@BoardID", BoardID));

			using (SQLiteConnection connection = PostDatabase.GetConnection()) {
				SelectAllThreads.Connection = connection;
				connection.Open();
				// Collect from Posts
				using (SQLiteDataReader Reader = SelectAllThreads.ExecuteReader()) {
					while (Reader.Read()) {
						// TODO: change db type from varchar to text
						ThreadList.Add(new ThreadInfo(
								//System.Convert.ToInt32(Reader.GetInt64(0)), // ThreadID
								(int)Reader[0],
								Reader[1].ToString(), // Name
								Reader[2].ToString(), // Content
								Reader[3].ToString() // Other
							));
					}
				}
			}

			return ThreadList;
		}

		public static List<ThreadInfo> GetThreadReplies(int BoardID, int ThreadID)
		{
			var ReplyList = new List<ThreadInfo>();
			SelectAllPosts.Parameters.Add(new SQLiteParameter("@ThreadID", ThreadID));
			SelectAllPosts.Parameters.Add(new SQLiteParameter("@BoardID", BoardID));

			SelectThread.Parameters.Add(new SQLiteParameter("@ThreadID", ThreadID));
			SelectThread.Parameters.Add(new SQLiteParameter("@BoardID", BoardID));

			using (SQLiteConnection dbConnection = PostDatabase.GetConnection()) {
				SelectAllPosts.Connection = dbConnection;
				SelectThread.Connection = dbConnection;
				dbConnection.Open();

				// First pass to get thread data
				using (SQLiteDataReader Reader = SelectThread.ExecuteReader()) {
					while(Reader.Read()) {
						ReplyList.Add(new ThreadInfo(
							(int)Reader[0], // PostID
							Reader[1].ToString(), // Name
							Reader[2].ToString(), // Content
							Reader[3].ToString() // Other
							));
					}
				}

				// Second pass to get child replies
				using (SQLiteDataReader Reader = SelectAllPosts.ExecuteReader()) {
					while (Reader.Read()) {
						ReplyList.Add(new ThreadInfo(
							//System.Convert.ToInt32(Reader.GetInt64(0)), // ThreadID
							(int)Reader[0],
							Reader[1].ToString(), // Name
							Reader[2].ToString(), // Content
							Reader[3].ToString() // Other
						));
					}
				}
			}

			return ReplyList;
		}
	}
}
