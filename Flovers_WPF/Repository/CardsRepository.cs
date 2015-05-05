using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using Flovers_WPF.DataAccess;
using Flovers_WPF.DataModel;

namespace Flovers_WPF.Repository
{
    class CardsRepository :ICardsRepository
    {
        SQLiteAsyncConnection conn;

        public CardsRepository(IDBConnection oIDBConnection)
        {
            conn = oIDBConnection.GetAsyncConnection();
        }

        public async Task Insert_Cards_Async(Cards cards)
        {
            await conn.InsertAsync(cards);
        }

        public async Task Update_Cards_Async(Cards cards)
        {
            await conn.UpdateAsync(cards);
        }

        public async Task Delete_Cards_Async(Cards cards)
        {
            await conn.DeleteAsync(cards);
        }

        public async Task<List<Cards>> Select_All_Cards_Async()
        {
            return await conn.Table<Cards>().ToListAsync();
        }

        public async Task<List<Cards>> Select_Cards_Async(string query)
        {
            return await conn.QueryAsync<Cards>(query);
        }
    }
}
