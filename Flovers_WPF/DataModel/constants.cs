using SQLite;

namespace Flovers_WPF.DataModel
{
    [Table("constants")]
    public class Constants
    {
        [PrimaryKey, AutoIncrement]
        public int constants_id { get; set; }


    }
}

