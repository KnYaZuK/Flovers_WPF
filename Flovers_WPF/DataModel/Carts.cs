using SQLite;

namespace Flovers_WPF.DataModel
{
    [Table("carts")]
    public class Carts
    {
        [PrimaryKey, AutoIncrement]
        public int carts_id { get; set; }

        public decimal count { get; set; }

        [Indexed]
        public int orders_id { get; set; }

        [Indexed]
        public int bouquets_id { get; set; }
    }
}

