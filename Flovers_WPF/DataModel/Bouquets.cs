using SQLite;

namespace Flovers_WPF.DataModel
{
    [Table("bouquets")]
    public class Bouquets
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public int bouquets_id { get; set; }

        [NotNull]
        public int name { get; set; }

        [NotNull]
        public double price_extra { get; set; }
    }
}

