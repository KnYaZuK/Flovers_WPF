using SQLite;

namespace Flovers_WPF.DataModel
{
    [Table("accessories")]
    public class Accessories
    {
        [PrimaryKey, AutoIncrement,NotNull]
        public int accessories_id { get; set; }

        [NotNull]
        public string name { get; set; }

        [NotNull]
        public double price { get; set; }

        [NotNull]
        public decimal in_stock { get; set; }
    }
}

