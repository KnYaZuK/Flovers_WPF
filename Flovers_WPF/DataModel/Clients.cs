using System;
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
        public string referal_number { get; set; }

        [Indexed]
        public int referer_id { get; set; }

        [Indexed, NotNull]
        public int cards_id { get; set; }

        public Clients() { }

        public Clients(string full_name, string phone_number, string email, int cards_id, int referer_id = -1)
        {
            this.full_name = full_name;
            this.phone_number = phone_number;
            this.email = email;
            this.referal_number = Guid.NewGuid().ToString();
            this.referer_id = referer_id;
            this.cards_id = cards_id;
        }
    }
}

