using System;
using SQLite;

namespace Flovers_WPF.DataModel
{
    [Table("cards")]
    public class Cards
    {
        [PrimaryKey, AutoIncrement]
        public int cards_id { get; set; }

        [NotNull]
        public string bonus_card_number { get; set; }

        [NotNull]
        public int bonus_card_points { get; set; }

        [Indexed, NotNull]
        public int clients_id { get; set; }

        public Cards() { }

        public Cards( int clients_id )
        {
            this.bonus_card_number = Guid.NewGuid().ToString();
            this.bonus_card_points = 0;
            this.clients_id = clients_id;
        }
    }
}

