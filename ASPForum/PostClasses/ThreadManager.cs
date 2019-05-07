using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using ASPForum.Post;

namespace ASPForum.Post
{
	public struct ThreadInfo {
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
			"WHERE ThreadID=@ThreadID");
		private static SQLiteCommand SelectAllThreads = new SQLiteCommand("SELECT * FROM Threads " +
			"WHERE BoardID=@BoardID");

		public static List<ThreadInfo> GetAllThreads(int BoardID)
		{
			var ThreadList = new List<ThreadInfo>();
			SelectAllThreads.Parameters.Add(new SQLiteParameter("@BoardID", BoardID));

			using (SQLiteConnection connection = PostDatabase.GetConnection())
			{
				SelectAllThreads.Connection = connection;
				connection.Open();
				using (SQLiteDataReader Reader = SelectAllThreads.ExecuteReader())
				{
					while (Reader.Read())
					{
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
	}
}
