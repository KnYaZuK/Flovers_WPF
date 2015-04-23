using SQLite;

namespace Flovers_WPF.DataModel
{
    [Table("bouquets")]
    public class Bouquets
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public int bouquets_id { get; set; }

        [NotNull]
        public string name { get; set; }

        [NotNull]
        public double price_extra { get; set; }

        public Bouquets() { }

        public Bouquets( string name, double price_extra )
        {
            this.name = name;
            this.price_extra = price_extra;
        }
    }
}

