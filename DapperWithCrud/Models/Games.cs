namespace DapperWithCrud.Models
{
    public class Games
    {

        public int GameID { get; set; }
        public string   Title { get; set; } 

        public int ReleaseYear { get; set; }

        public String Platform { get; set; } 
        public decimal Rating { get; set; }

    }

}
