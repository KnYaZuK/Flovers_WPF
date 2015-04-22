using SQLite;

namespace Flovers_WPF.DataModel
{
    [Table("orders")]
    public class Orders
    {
        [PrimaryKey, AutoIncrement]
        public int orders_id { get; set; }


    }
}

