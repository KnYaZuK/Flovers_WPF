using SQLite;

namespace Flovers_WPF.DataModel
{
    [Table("accessories")]
    public class Accessories
    {
        [PrimaryKey, AutoIncrement]
        public int accessories_id { get; set; }

        public string name { get; set; }
        public double price { get; set; }
        public decimal in_stock { get; set; }
    }
}

