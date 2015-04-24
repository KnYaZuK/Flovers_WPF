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

        [Indexed]
        public int flowers_id { get; set; }

        [Indexed]
        public int accessories_id { get; set; }

        public Contents() { }

        public Contents( decimal count, int bouquets_id, int flowers_id, int accessories_id )
        {
            this.count = count;
            this.bouquets_id = bouquets_id;
            this.flowers_id = flowers_id;
            this.accessories_id = accessories_id;
        }
    }
}

