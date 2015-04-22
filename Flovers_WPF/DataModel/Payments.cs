using SQLite;
using System;

namespace Flovers_WPF.DataModel
{
    [Table("payments")]
    public class Payments
    {
        [PrimaryKey, AutoIncrement]
        public int payments_id { get; set; }

        public DateTime date { get; set; }
        public double value_money { get; set; }
        public decimal value_points { get; set; }

        [Indexed]
        public int orders_id { get; set; }
    }
}

