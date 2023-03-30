namespace api.Models
{
    public class Song
    {
        public ulong songID {get; set;}
        public string Title {get; set;} = string.Empty;
        public string Artist {get; set;} = string.Empty;
        public DateTime dateAdded {get; set;}
        public bool Favorite {get; set;}
        public bool Deleted {get; set;}
    }
}