using SQLite;

namespace Flovers_WPF.DataModel
{
    [Table("orders")]
    public class Orders
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public int orders_id { get; set; }

        [NotNull]
        public string address { get; set; }

        [NotNull]
        public double price { get; set; }

        [NotNull]
        public string status { get; set; }

        [Indexed, NotNull]
        public int clients_id { get; set; }
    }
}

