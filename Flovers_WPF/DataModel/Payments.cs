using SQLite;
using System;

namespace Flovers_WPF.DataModel
{
    [Table("payments")]
    public class Payments
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public int payments_id { get; set; }

        [NotNull]
        public DateTime time { get; set; }

        [NotNull]
        public double value_money { get; set; }

        [NotNull]
        public decimal value_points { get; set; }

        [Indexed, NotNull]
        public int orders_id { get; set; }
    }
}

