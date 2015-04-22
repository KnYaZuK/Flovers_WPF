using SQLite;

namespace Flovers_WPF.DataModel
{
    [Table("contents")]
    public class Contents
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public int contents_id { get; set; }

        [NotNull]
        public decimal count { get; set; }

        [Indexed, NotNull]
        public int bouquets_id { get; set; }

        [Indexed, NotNull]
        public int flowers_id { get; set; }

        [Indexed, NotNull]
        public int accessories_id { get; set; }
    }
}

