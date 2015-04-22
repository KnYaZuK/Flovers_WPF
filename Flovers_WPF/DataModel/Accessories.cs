using SQLite;

namespace Flovers_WPF.DataModel
{
    [Table("accessories")]
    public class Accessories
    {
        [PrimaryKey, AutoIncrement]
        public int accessories_id { get; set; }


    }
}

