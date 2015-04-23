using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using Flovers_WPF.DataAccess;
using Flovers_WPF.DataModel;

namespace Flovers_WPF.Repository
{
    class ContentsRepository : IContentsRepository
    {
        SQLiteAsyncConnection conn;

        public ContentsRepository(IDBConnection oIDBConnection)
        {
            conn = oIDBConnection.GetAsyncConnection();
        }

        public async Task Insert_Contents_Async(Contents contents)
        {
            await conn.InsertAsync(contents);
        }

        public async Task Update_Contents_Async(Contents contents)
        {
            await conn.UpdateAsync(contents);
        }

        public async Task Delete_Contents_Async(Contents contents)
        {
            await conn.DeleteAsync(contents);
        }

        public async Task<List<Contents>> Select_All_Contents_Async()
        {
            return await conn.Table<Contents>().ToListAsync();
        }

        public async Task<List<Contents>> Select_Contents_Async(string query)
        {
            return await conn.QueryAsync<Contents>(query);
        }
    }
}
