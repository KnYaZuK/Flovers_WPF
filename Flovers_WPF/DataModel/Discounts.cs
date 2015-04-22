using SQLite;

namespace Flovers_WPF.DataModel
{
    [Table("discounts")]
    public class Discounts
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public int discounts_id { get; set; }

        [NotNull]
        public double percents { get; set; }

        [NotNull]
        public double value { get; set; }
    }
}

