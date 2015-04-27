using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Flovers_WPF.DataModel
{
    [Table("users")]
    public class Users
    {
        [PrimaryKey, AutoIncrement]
        public int users_id { get; set; }

        [NotNull]
        public string login { get; set; }

        [NotNull]
        public string password { get; set; }

        public Users() { }

        public Users(string login, string password)
        {
            this.login = login;
            this.password = password;
        }
    }
}
