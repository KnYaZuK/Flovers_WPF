using SQLite;

namespace Flovers_WPF.DataModel
{
    [Table("payments")]
    public class Payments
    {
        [PrimaryKey, AutoIncrement]
        public int payments_id { get; set; }


    }
}

