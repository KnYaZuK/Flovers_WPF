using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using Flovers_WPF.DataAccess;
using Flovers_WPF.DataModel;

namespace Flovers_WPF.Repository
{
    class AccessoriesRepository
    {
        SQLiteAsyncConnection conn;

        public AccessoriesRepository(IDBConnection oIDBConnection)
        {
            conn = oIDBConnection.GetAsyncConnection();
        }

        public async Task Insert_Accessories_Async(Accessories accessories)
        {
            await conn.InsertAsync(accessories);
        }

        public async Task Update_Accessories_Async(Accessories accessories)
        {
            await conn.UpdateAsync(accessories);
        }

        public async Task Delete_Accessories_Async(Accessories accessories)
        {
            await conn.DeleteAsync(accessories);
        }

        public async Task<List<Accessories>> Select_All_Accessories_Async()
        {
            return await conn.Table<Accessories>().ToListAsync();
        }

        public async Task<List<Accessories>> Select_Accessories_Async(string query)
        {
            return await conn.QueryAsync<Accessories>(query);
        }
    }
}
