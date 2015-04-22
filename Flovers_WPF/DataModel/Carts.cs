using SQLite;

namespace Flovers_WPF.DataModel
{
    [Table("carts")]
    public class Carts
    {
        [PrimaryKey, AutoIncrement]
        public int carts_id { get; set; }


    }
}

