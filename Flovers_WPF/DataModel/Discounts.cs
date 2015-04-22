using SQLite;

namespace Flovers_WPF.DataModel
{
    [Table("discounts")]
    public class Discounts
    {
        [PrimaryKey, AutoIncrement]
        public int discounts_id { get; set; }

        public double percents { get; set; }
        public double value { get; set; }
    }
}

