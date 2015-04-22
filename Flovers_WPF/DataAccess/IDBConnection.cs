using System.Threading.Tasks;
using SQLite;

namespace Flovers_WPF.DataAccess
{
    interface IDBConnection
    {
        Task InitializeDatabase();
        SQLiteAsyncConnection GetAsyncConnection();
    }
}
