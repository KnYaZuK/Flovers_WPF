using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using Flovers_WPF.DataAccess;
using Flovers_WPF.DataModel;

namespace Flovers_WPF.Repository
{
    class BouquetsRepository : IBouquetsRepository
    {
        SQLiteAsyncConnection conn;

        public BouquetsRepository(IDBConnection oIDBConnection)
        {
            conn = oIDBConnection.GetAsyncConnection();
        }

        public async Task Insert_Bouquets_Async(Bouquets bouquets)
        {
            await conn.InsertAsync(bouquets);
        }

        public async Task Update_Bouquets_Async(Bouquets bouquets)
        {
            await conn.UpdateAsync(bouquets);
        }

        public async Task Delete_Bouquets_Async(Bouquets bouquets)
        {
            await conn.DeleteAsync(bouquets);
        }

        public async Task<List<Bouquets>> Select_All_Bouquets_Async()
        {
            return await conn.Table<Bouquets>().ToListAsync();
        }

        public async Task<List<Bouquets>> Select_Bouquets_Async(string query)
        {
            return await conn.QueryAsync<Bouquets>(query);
        }
    }
}
