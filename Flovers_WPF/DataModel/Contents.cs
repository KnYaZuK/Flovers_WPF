using SQLite;

namespace Flovers_WPF.DataModel
{
    [Table("contents")]
    public class Contents
    {
        [PrimaryKey, AutoIncrement]
        public int contents_id { get; set; }


    }
}

