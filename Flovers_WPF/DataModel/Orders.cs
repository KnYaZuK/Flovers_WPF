using SQLite;

namespace Flovers_WPF.DataModel
{
    [Table("orders")]
    public class Orders
    {
        [PrimaryKey, AutoIncrement]
        public int orders_id { get; set; }

        public string address { get; set; }
        public double price { get; set; }
        public string status { get; set; }

        [Indexed]
        public int clients_id { get; set; }
    }
}

