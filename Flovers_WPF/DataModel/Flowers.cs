using SQLite;

namespace Flovers_WPF.DataModel
{
    [Table("flowers")]
    public class Flowers
    {
        [PrimaryKey, AutoIncrement]
        public int flowers_id { get; set; }

        public string name { get; set; }
        public double price { get; set; }
        public decimal in_stock { get; set; }
    }
}

