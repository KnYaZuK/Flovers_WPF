using SQLite;

namespace Flovers_WPF.DataModel
{
    [Table("bouquets")]
    public class Bouquets
    {
        [PrimaryKey, AutoIncrement]
        public int bouquets_id { get; set; }

        public int name { get; set; }
        public double price_extra { get; set; }
    }
}

