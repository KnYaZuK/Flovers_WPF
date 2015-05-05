using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using Flovers_WPF.DataAccess;
using Flovers_WPF.DataModel;

namespace Flovers_WPF.Repository
{
    class PaymentsRepository : IPaymentsRepository
    {
        SQLiteAsyncConnection conn;

        public PaymentsRepository(IDBConnection oIDBConnection)
        {
            conn = oIDBConnection.GetAsyncConnection();
        }

        public async Task Insert_Payments_Async(Payments payments)
        {
            await conn.InsertAsync(payments);
        }

        public async Task Update_Payments_Async(Payments payments)
        {
            await conn.UpdateAsync(payments);
        }

        public async Task Delete_Payments_Async(Payments payments)
        {
            await conn.DeleteAsync(payments);
        }

        public async Task<List<Payments>> Select_All_Payments_Async()
        {
            return await conn.Table<Payments>().ToListAsync();
        }

        public async Task<List<Payments>> Select_Payments_Async(string query)
        {
            return await conn.QueryAsync<Payments>(query);
        }
    }
}
