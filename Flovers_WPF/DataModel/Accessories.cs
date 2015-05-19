using SQLite;

namespace Flovers_WPF.DataModel
{
    [Table("accessories")]
    public class Accessories
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public int accessories_id { get; set; }

        [NotNull]
        public string name { get; set; }

        [NotNull]
        public double price { get; set; }

        [NotNull]
        public decimal in_stock { get; set; }

        [NotNull]
        public string measure { get; set; }

        public Accessories() { }

        public Accessories(string name, double price, decimal in_stock, string measure)
        {
            this.name = name;
            this.price = price;
            this.in_stock = in_stock;
            this.measure = measure;
        }
    }
}

