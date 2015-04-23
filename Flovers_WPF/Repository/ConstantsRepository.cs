using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using Flovers_WPF.DataAccess;
using Flovers_WPF.DataModel;

namespace Flovers_WPF.Repository
{
    class ConstantsRepository : IConstantsRepository
    {
        SQLiteAsyncConnection conn;

        public ConstantsRepository(IDBConnection oIDBConnection)
        {
            conn = oIDBConnection.GetAsyncConnection();
        }

        public async Task Insert_Constants_Async(Constants constants)
        {
            await conn.InsertAsync(constants);
        }

        public async Task Update_Constants_Async(Constants constants)
        {
            await conn.UpdateAsync(constants);
        }

        public async Task Delete_Constants_Async(Constants constants)
        {
            await conn.DeleteAsync(constants);
        }

        public async Task<List<Constants>> Select_All_Constants_Async()
        {
            return await conn.Table<Constants>().ToListAsync();
        }

        public async Task<List<Constants>> Select_Constants_Async(string query)
        {
            return await conn.QueryAsync<Constants>(query);
        }
    }
}
