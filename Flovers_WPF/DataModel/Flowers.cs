using SQLite;

namespace Flovers_WPF.DataModel
{
    [Table("flowers")]
    public class Flowers
    {
        [PrimaryKey, AutoIncrement]
        public int flowers_id { get; set; }

        [NotNull]
        public string name { get; set; }

        [NotNull]
        public double price { get; set; }

        [NotNull]
        public decimal in_stock { get; set; }

        [NotNull]
        public string measure { get; set; }

        public Flowers() { }

        public Flowers( string name, double price, decimal in_stock, string measure )
        {
            this.name = name;
            this.price = price;
            this.in_stock = in_stock;
            this.measure = measure;
        }
    }
}

