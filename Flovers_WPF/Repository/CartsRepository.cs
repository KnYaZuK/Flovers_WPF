using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using Flovers_WPF.DataAccess;
using Flovers_WPF.DataModel;

namespace Flovers_WPF.Repository
{
    class CartsRepository : ICartsRepository
    {
        SQLiteAsyncConnection conn;

        public CartsRepository(IDBConnection oIDBConnection)
        {
            conn = oIDBConnection.GetAsyncConnection();
        }

        public async Task Insert_Carts_Async(Carts carts)
        {
            await conn.InsertAsync(carts);
        }

        public async Task Update_Carts_Async(Carts carts)
        {
            await conn.UpdateAsync(carts);
        }

        public async Task Delete_Carts_Async(Carts carts)
        {
            await conn.DeleteAsync(carts);
        }

        public async Task<List<Carts>> Select_All_Carts_Async()
        {
            return await conn.Table<Carts>().ToListAsync();
        }

        public async Task<List<Carts>> Select_Carts_Async(string query)
        {
            return await conn.QueryAsync<Carts>(query);
        }
    }
}
