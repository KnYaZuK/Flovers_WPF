using SQLite;

namespace Flovers_WPF.DataModel
{
    [Table("constants")]
    public class Constants
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public int constants_id { get; set; }

        [NotNull]
        public string name { get; set; }

        [NotNull]
        public string value { get; set; }

        public Constants() { }

        public Constants( string name, string value )
        {
            this.name = name;
            this.value = value;
        }
    }
}

