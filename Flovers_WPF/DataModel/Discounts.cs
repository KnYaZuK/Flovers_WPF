using SQLite;

namespace Flovers_WPF.DataModel
{
    [Table("discounts")]
    public class Discounts
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public int discounts_id { get; set; }

        [NotNull]
        public double percent { get; set; }

        [NotNull]
        public double value { get; set; }

        public Discounts() { }

        public Discounts( double percents, double value )
        {
            this.percent = percent;
            this.value = value;
        }
    }
}

