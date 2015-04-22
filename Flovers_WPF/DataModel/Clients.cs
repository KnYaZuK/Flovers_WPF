using SQLite;

namespace Flovers_WPF.DataModel
{
    [Table("clients")]
    public class Clients
    {
        [PrimaryKey, AutoIncrement]
        public int clients_id { get; set; }

        [NotNull]
        public string full_name { get; set; }

        [NotNull]
        public string phone_number { get; set; }

        public string email { get; set; }

        [NotNull]
        public int bonus_card_number { get; set; }

        [NotNull]
        public int bonus_card_points { get; set; }

        [NotNull]
        public int referal_number { get; set; }

        public int referer_number { get; set; }
    }
}

