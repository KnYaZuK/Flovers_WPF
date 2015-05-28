using System;

using SQLite;

namespace Flovers_WPF.DataModel
{
    [Table("specialdeals")]
    class SpecialDeals
    {
        [PrimaryKey,AutoIncrement,Indexed]
        public int specialdeals_id { get; set; }

        [NotNull,Indexed]
        public int bouquets_id { get; set; }

        [NotNull]
        public double discount { get; set; }

        [NotNull]
        public DateTime datetime { get; set; }

        public SpecialDeals() { }

        public SpecialDeals( int bouquets_id, double discount, DateTime datetime )
        {
            this.bouquets_id = bouquets_id;
            this.discount = discount;
            this.datetime = datetime;
        }
    }
}
