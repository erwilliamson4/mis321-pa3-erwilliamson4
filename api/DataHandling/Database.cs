using System.Data.SQLite;
using api.Models;
//using MySql.Data.MySqlClient;
using MySqlConnector;

namespace api.DataHandling
{
    public class Database
    {
        private string connectionString = "server=r4wkv4apxn9btls2.cbetxkdyhwsb.us-east-1.rds.amazonaws.com; uid=h4lsxw2ehbf98ora;pwd=j11yv16a1nro3wqe;database=yqefsf9kjeyylyjd";

        public Database(){}

        public List<Song> GetSongs()
        {
            var songs = new List<Song>();
            var connection = new MySqlConnection();
            connection.ConnectionString = connectionString;
            connection.Open();

            //make a statement to hold SQL commands
            var statement = "select song_id, title, artist, date_added, favorited from song where deleted = 0 order by date_added desc;";
            //initilaize a new SQL command instance class
            using var command = new MySqlCommand();
            command.Connection = connection;
            command.CommandText = statement;
            var reader = command.ExecuteReader();

            while(reader.Read())
            {
                var song = new Song{
                    songID = reader.GetUInt64(0),
                    Title = reader.GetString(1),
                    Artist = reader.GetString(2),
                    dateAdded = reader.GetDateTime(3),
                    Favorite = reader.GetBoolean(4)
                };
                songs.Add(song);
            }
            return songs;

        }

        public Song GetSong(int ID)
        {
            var song = new Song();
            var connection = new MySqlConnection();
            connection.ConnectionString = connectionString;
            connection.Open();

            var statement = $"select song_id, title, artist, date_added, favorited from song where song_id = {ID};";
            using var command = new MySqlCommand();
            command.Connection = connection;
            command.CommandText = statement;
            var reader = command.ExecuteReader();

            while(reader.Read())
            {
                song = new Song{
                    songID = reader.GetUInt64(0),
                    Title = reader.GetString(1),
                    Artist = reader.GetString(2),
                    dateAdded = reader.GetDateTime(3),
                    Favorite = reader.GetBoolean(4)
                };
            }
            return song;
        }

        public Song AddSong(Song song)
        {
            var connection = new MySqlConnection();
            connection.ConnectionString = connectionString;
            connection.Open();

            var statement = $"insert into song(title, artist, date_added) values('{song.Title}', '{song.Artist}, '{song.dateAdded.ToString("yyyy-MM-dd")}');";

            using var command = new MySqlCommand();
            command.Connection = connection;
            command.CommandText = statement;
            command.ExecuteNonQuery();

            var lastIDStatement = "SELECT LAST_INSERT_ID();";
            using var lastIDCommand = new MySqlCommand();

            lastIDCommand.Connection = connection;
            lastIDCommand.CommandText = lastIDStatement;
            var lastIDObject = lastIDCommand.ExecuteScalar();
            var lastID = lastIDObject == null ? 0 : (UInt64)lastIDObject;

            song.songID = lastID;
            return song;
        }

        public void DeleteSong(int ID)
        {
            var connection = new MySqlConnection();
            connection.ConnectionString = connectionString;
            connection.Open();

            //get the song to delete based on ID
            var statement = $"Delete from song where song_id = {ID};";

            using var command = new MySqlCommand();
            command.Connection = connection;
            command.CommandText = statement;
            command.ExecuteNonQuery();
        }

        public void SoftDelete(int ID)
        {
            var connection = new MySqlConnection();
            connection.ConnectionString = connectionString;
            connection.Open();

            var statement = $"Update song set deleted = 1 where song_id = {ID};";

            using var command = new MySqlCommand();
            command.Connection = connection;
            command.CommandText = statement;
            command.ExecuteNonQuery();
        }

        public void UpdateSong(int ID, Song song)
        {
            var connection = new MySqlConnection();
            connection.ConnectionString = connectionString;
            connection.Open();

            var favoriteAsInt = song.Favorite ? 1 : 0;
            var deletedAsInt = song.Deleted ? 1 : 0;
            var statement = $"Update song set title = '{song.Title}', artist = '{song.Artist}', date_added = '{song.dateAdded.ToString("yyyy-MM-dd")}, favorited = {favoriteAsInt}, deleted = {deletedAsInt} where song_id = {ID}";

            using var command = new MySqlCommand();
            command.Connection = connection;
            command.CommandText = statement;
            command.ExecuteNonQuery();
        }
    }
}