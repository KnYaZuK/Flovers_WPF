using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Flovers_WPF.DataAccess;
using Flovers_WPF.DataModel;

namespace Flovers_WPF.Repository
{
    class WorkersRepository : IWorkersRepository
    {
        SQLiteAsyncConnection conn;

        public WorkersRepository(IDBConnection oIDBConnection)
        {
            conn = oIDBConnection.GetAsyncConnection();
        }

        public async Task Insert_Workers_Async(Workers workers)
        {
            await conn.InsertAsync(workers);
        }

        public async Task Update_Workers_Async(Workers workers)
        {
            await conn.UpdateAsync(workers);
        }

        public async Task Delete_Workers_Async(Workers workers)
        {
            await conn.DeleteAsync(workers);
        }

        public async Task<List<Workers>> Select_All_Workers_Async()
        {
            return await conn.Table<Workers>().ToListAsync();
        }

        public async Task<List<Workers>> Select_Workers_Async(string query)
        {
            return await conn.QueryAsync<Workers>(query);
        }
    }
}
