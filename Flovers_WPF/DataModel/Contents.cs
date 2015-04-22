using SQLite;

namespace Flovers_WPF.DataModel
{
    [Table("contents")]
    public class Contents
    {
        [PrimaryKey, AutoIncrement]
        public int contents_id { get; set; }

        public decimal count { get; set; }

        [Indexed]
        public int bouquets_id { get; set; }

        [Indexed]
        public int flowers_id { get; set; }

        [Indexed]
        public int accessories_id { get; set; }
    }
}

