using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using Flovers_WPF.DataAccess;
using Flovers_WPF.DataModel;

namespace Flovers_WPF.Repository
{
    class DiscountsRepository : IDiscountsRepository
    {
        SQLiteAsyncConnection conn;

        public DiscountsRepository(IDBConnection oIDBConnection)
        {
            conn = oIDBConnection.GetAsyncConnection();
        }

        public async Task Insert_Discounts_Async(Discounts discounts)
        {
            await conn.InsertAsync(discounts);
        }

        public async Task Update_Discounts_Async(Discounts discounts)
        {
            await conn.UpdateAsync(discounts);
        }

        public async Task Delete_Discounts_Async(Discounts discounts)
        {
            await conn.DeleteAsync(discounts);
        }

        public async Task<List<Discounts>> Select_All_Discounts_Async()
        {
            return await conn.Table<Discounts>().ToListAsync();
        }

        public async Task<List<Discounts>> Select_Discounts_Async(string query)
        {
            return await conn.QueryAsync<Discounts>(query);
        }
    }
}
