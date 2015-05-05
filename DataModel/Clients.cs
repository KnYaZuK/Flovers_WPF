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

        public string referer_number { get; set; }

        [Indexed, NotNull]
        public int cards_id { get; set; }

        public Clients() { }

        public Clients(string full_name, string phone_number, string email, string referer_number, int cards_id )
        {
            this.full_name = full_name;
            this.phone_number = phone_number;
            this.email = email;
            this.referal_number = Guid.NewGuid().ToString();
            this.referer_number = referer_number;
            this.cards_id = cards_id;
        }
    }
}

