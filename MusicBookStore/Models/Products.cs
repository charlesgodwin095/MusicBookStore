namespace MusicBookStore.Models
{
    public class Products
    {
        public  int id  { get; set; }
        public  string Title { get; set; }
        public string Composer { get; set; }
        public string Instrument { get; set; } // e.g., Piano, Guitar
        public string Genre { get; set; } // e.g., Classical, Jazz
        public string Description { get; set; }
        public decimal Price{ get; set; }
        public string ImageUrl { get; set; }
        public int StockQuantity { get; set; }
        public DateTime PeleaseDate { get; set; }

    }
}
