using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using Flovers_WPF.DataAccess;
using Flovers_WPF.DataModel;

namespace Flovers_WPF.Repository
{
    class OrdersRepository : IOrdersRepository
    {
        SQLiteAsyncConnection conn;

        public OrdersRepository(IDBConnection oIDBConnection)
        {
            conn = oIDBConnection.GetAsyncConnection();
        }

        public async Task Insert_Orders_Async(Orders orders)
        {
            await conn.InsertAsync(orders);
        }

        public async Task Update_Orders_Async(Orders orders)
        {
            await conn.UpdateAsync(orders);
        }

        public async Task Delete_Orders_Async(Orders orders)
        {
            await conn.DeleteAsync(orders);
        }

        public async Task<List<Orders>> Select_All_Orders_Async()
        {
            return await conn.Table<Orders>().ToListAsync();
        }

        public async Task<List<Orders>> Select_Orders_Async(string query)
        {
            return await conn.QueryAsync<Orders>(query);
        }
    }
}
