using SQLite;

namespace Flovers_WPF.DataModel
{
    [Table("flowers")]
    public class Flowers
    {
        [PrimaryKey, AutoIncrement]
        public int flowers_id { get; set; }


    }
}

