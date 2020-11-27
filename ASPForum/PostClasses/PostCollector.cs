using System.Collections.Generic;
using System.Data.SQLite;

namespace ASPForum.Post
{

	// Make Name have a default value
	public struct PostInformation
	{
		public string Name, PostContent, Other;

		public PostInformation(string Name, string PostContent, string Other)
		{
			// Ternary operator gives a syntax error
			this.Name = (Name != string.Empty ? Name : "Someone");

			this.PostContent = PostContent;
			this.Other = Other;
		}

	}

	/*DEPRECATED*/
	public static class PostCollector
	{

		// private static SQLiteConnection PostDatabase = new SQLiteConnection("Data Source=PostDatabase.sqlite;Version=3;");
		private static SQLiteCommand SelectAllPosts = new SQLiteCommand("SELECT * FROM Posts");

		public static List<PostInformation> GetPostInformation()
		{
			List<PostInformation> PostList = new List<PostInformation>();

			using (SQLiteConnection dbConnection = PostDatabase.GetConnection())
			{
				dbConnection.Open();
				SelectAllPosts.Connection = dbConnection;

				using (SQLiteDataReader Reader = SelectAllPosts.ExecuteReader()) // SELECT commands need no transaction
				{

					// Read() returns false when there are no more rows
					while (Reader.Read())
					{
						PostList.Add(new PostInformation(Reader["Name"].ToString(),
							Reader["PostContent"].ToString(),
							Reader["Other"].ToString()
							));
					}
				};
			}

			return PostList;
		}
	}

}
