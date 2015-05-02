using System.IO;
using System.Threading.Tasks;
using SQLite;
using Flovers_WPF.DataModel;

namespace Flovers_WPF.DataAccess
{
    class DBConnection : IDBConnection
    {
        string dbPath;
        SQLiteAsyncConnection conn;

        public DBConnection()
        {
            dbPath = Path.Combine(Directory.GetCurrentDirectory(),"Flowers.sqlite");
            conn = new SQLiteAsyncConnection(dbPath);
        }

        public async Task InitializeDatabase()
        {
            await conn.CreateTableAsync<Accessories>();
            await conn.CreateTableAsync<Bouquets>();
            await conn.CreateTableAsync<Carts>();
            await conn.CreateTableAsync<Clients>();
            await conn.CreateTableAsync<Constants>();
            await conn.CreateTableAsync<Contents>();
            await conn.CreateTableAsync<Discounts>();
            await conn.CreateTableAsync<Flowers>();
            await conn.CreateTableAsync<Orders>();
            await conn.CreateTableAsync<Payments>();
            await conn.CreateTableAsync<Users>();
        }

        public SQLiteAsyncConnection GetAsyncConnection()
        {
            return conn;
        }
    }
}
