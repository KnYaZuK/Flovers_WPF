using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Flovers_WPF.DataModel
{
    [Table("workers")]
    public class Workers
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }

        [NotNull]
        public string login { get; set; }

        [NotNull]
        public string password { get; set; }

        public Workers() { }

        public Workers(string login, string password)
        {
            this.login = login;
            this.password = password;
        }
    }
}
