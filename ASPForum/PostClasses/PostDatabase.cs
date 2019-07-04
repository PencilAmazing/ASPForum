using System.Data.SQLite;

namespace ASPForum.Post
{
	public static class PostDatabase
	{

		//private static SQLiteConnection PostDatabaseConnection = new SQLiteConnection("Data Source=PostDatabase.sqlite;Version=3;");
		//private static SQLiteCommand SelectAllPosts = new SQLiteCommand("SELECT * FROM Posts", PostDatabaseConnection);

		/* USE THESE IN EXACT ORDER */
		private static SQLiteCommand CreateBoards = new SQLiteCommand(@"CREATE TABLE Boards( " +
			"BoardID INT, " + // Primary key
			"Name VARCHAR(40), " +
			"PRIMARY KEY(BoardID) )");

		private static SQLiteCommand CreatePostCounter = new SQLiteCommand(@"CREATE TABLE PostCounter( " +
			"BoardID INT NOT NULL UNIQUE, " +
			"Counter INT DEFAULT 0, " + // Autoincremented each time post to BoardID is made
			"FOREIGN KEY(BoardID) REFERENCES Boards(BoardID) )");

		private static SQLiteCommand CreateThreads = new SQLiteCommand(@"CREATE TABLE Threads( " +
			"ThreadID INT NOT NULL, " + // NOT Primary key
			"Name VARCHAR(40), " +
			"Content VARCHAR(2400) NOT NULL, " +
			"Other VARCHAR(12), " +
			"BoardID INT NOT NULL, " + // Foreign key references owning board
			//"PRIMARY KEY(ThreadID), " +
			"FOREIGN KEY(BoardID) REFERENCES Boards(BoardID) )");

		private static SQLiteCommand CreatePosts = new SQLiteCommand(@"CREATE TABLE Posts( " +
			"PostID INT NOT NULL, " + // Set by referencing CreatePostCounter
			"Name VARCHAR(40), " +
			"Content VARCHAR(2400) NOT NULL, " +
			"Other VARCHAR(40), " +
			"ThreadID INT NOT NULL, " + // Foreign key references owning thread
			"BoardID INT NOT NULL, " + 
			//"PRIMARY KEY(PostID), " +
			"FOREIGN KEY(ThreadID) REFERENCES Threads(ThreadID)," +
			"FOREIGN KEY(BoardID) REFERENCES Boards(BoardID) )");

		private static SQLiteCommand CreateAdmins = new SQLiteCommand(@"CREATE TABLE Admins( " +
			"Username VARCHAR(40) NOT NULL, " +
			"Password VARCHAR(40) NOT NULL, " +
			"Perms TEXT NOT NULL, " +
			"PRIMARY KEY(Username) )");

		/*private static SQLiteCommand FindThread = new SQLiteCommand("SELECT COUNT(ThreadID) FROM Threads " +
			"WHERE ThreadID=@ThreadID AND BoardID=@BoardID");*/

		private static SQLiteCommand FindBoard = new SQLiteCommand("SELECT COUNT(BoardID) FROM Boards " +
			"WHERE BoardID=@BoardID");

		private static SQLiteCommand GetPostCount = new SQLiteCommand("SELECT Counter FROM PostCounter " +
			"WHERE BoardID=@BoardID");

		private static SQLiteCommand UpdatePostCount = new SQLiteCommand("UPDATE PostCounter " +
			"SET Counter=@Counter WHERE BoardID=@BoardID");

		public const string FileName = "Database.sqlite";
		
		public static void CheckDatabase()
		{
			// Database does not exists so create it
			if (!System.IO.File.Exists(FileName))
			{
				SQLiteConnection.CreateFile(FileName);
				InitDatabase(); // Database is now ready for use
				FillDatabase(); // Fill with basic data
				System.Console.WriteLine("Created file");
			}
			else
			{
				//PostDatabase.Open(); // Database ready for use
				System.Console.WriteLine("File Exists");
			}
		}

		private static void InitDatabase()
		{
			using (SQLiteConnection dbConnection = GetConnection())
			{
				try
				{
					dbConnection.Open();

					CreateBoards.Connection = dbConnection;
					CreateBoards.ExecuteNonQuery();

					CreatePostCounter.Connection = dbConnection;
					CreatePostCounter.ExecuteNonQuery();

					CreateThreads.Connection = dbConnection;
					CreateThreads.ExecuteNonQuery();

					CreatePosts.Connection = dbConnection;
					CreatePosts.ExecuteNonQuery();

					CreateAdmins.Connection = dbConnection;
					CreateAdmins.ExecuteNonQuery();

				} catch (SQLiteException e) // If anything wrong happens with setup
				{
					// nuke db
					// SQLite doesn't allow DROP TABLE *
					System.IO.File.Delete(FileName);
					
					throw e; // Throw back exception
				}
			}

			System.Console.WriteLine("Created Database");

		}

		public static void FillDatabase()
		{
			using (SQLiteConnection dbConnection = GetConnection()) {
				dbConnection.Open();
				SQLiteCommand FillBoard = dbConnection.CreateCommand();
				FillBoard.CommandText = "INSERT INTO Boards(BoardID, Name) VALUES(0, 'test')";
				FillBoard.ExecuteNonQuery();

				SQLiteCommand FillCounter = dbConnection.CreateCommand();
				FillCounter.CommandText = "INSERT INTO PostCounter(BoardID) VALUES(0)";
				FillCounter.ExecuteNonQuery();
			}
		}
		/*public static bool ThreadIDExists(int ThreadID, int BoardID)
		{
			int SelectCount = 0;
			using (SQLiteConnection dbConnection = GetConnection())
			{
				dbConnection.Open();
				FindThread.Connection = dbConnection;
				FindThread.Parameters.Add(new SQLiteParameter("@ThreadID", ThreadID));
				FindThread.Parameters.Add(new SQLiteParameter("@BoardID", BoardID));
				SelectCount = FindThread.ExecuteNonQuery();
			}
			if (SelectCount == 1) {
				return true;
			} else if (SelectCount > 1) {
				System.Console.WriteLine("Found Threads with multiple IDs!");
				return true;
			} else {
				return false;
			}
		}*/

		public static bool BoardIDExists(int BoardID)
		{
			long SelectCount = 0;
			using (SQLiteConnection dbConnection = GetConnection()) {
				dbConnection.Open();
				FindBoard.Connection = dbConnection;
				FindBoard.Parameters.Add(new SQLiteParameter("@BoardID", BoardID));
				SelectCount = (long)FindBoard.ExecuteScalar();
			}
			return SelectCount > 0 ? true : false;
		}

		// Return PostCounter of Board
		public static int GetThreadID(int BoardID)
		{
			int ThreadID = 0;
			using (SQLiteConnection dbConnection = GetConnection()) {
				dbConnection.Open();
				GetPostCount.Connection = dbConnection;
				GetPostCount.Parameters.Add(new SQLiteParameter("@BoardID", BoardID));
				ThreadID = (int)GetPostCount.ExecuteScalar();
			}

			UpdatePostCounter(BoardID, ThreadID);
			return ThreadID;
		}

		// Increments PostCounter by one based on ThreadID given
		public static void UpdatePostCounter(int BoardID, int ThreadID)
		{
			using (SQLiteConnection dbConnection = GetConnection()) {
				dbConnection.Open();
				UpdatePostCount.Connection = dbConnection;
				UpdatePostCount.Parameters.Add(new SQLiteParameter("@Counter", ThreadID+1));
				UpdatePostCount.Parameters.Add(new SQLiteParameter("@BoardID", BoardID));
				UpdatePostCount.ExecuteNonQuery();
			}
		}

		// Replace this with a property anytime soon
		public static SQLiteConnection GetConnection()
		{
			//return new SQLiteConnection("Data Source=PostDatabase.sqlite;Version=3;");
			return new SQLiteConnection("Data Source=" + FileName + ";Version=3;");
		}
	}
}
