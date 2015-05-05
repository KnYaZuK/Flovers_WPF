using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using Flovers_WPF.DataAccess;
using Flovers_WPF.DataModel;

namespace Flovers_WPF.Repository
{
    class FlowersRepository : IFlowersRepository
    {
        SQLiteAsyncConnection conn;

        public FlowersRepository(IDBConnection oIDBConnection)
        {
            conn = oIDBConnection.GetAsyncConnection();
        }

        public async Task Insert_Flowers_Async(Flowers flowers)
        {
            await conn.InsertAsync(flowers);
        }

        public async Task Update_Flowers_Async(Flowers flowers)
        {
            await conn.UpdateAsync(flowers);
        }

        public async Task Delete_Flowers_Async(Flowers flowers)
        {
            await conn.DeleteAsync(flowers);
        }

        public async Task<List<Flowers>> Select_All_Flowers_Async()
        {
            return await conn.Table<Flowers>().ToListAsync();
        }

        public async Task<List<Flowers>> Select_Flowers_Async(string query)
        {
            return await conn.QueryAsync<Flowers>(query);
        }
    }
}
