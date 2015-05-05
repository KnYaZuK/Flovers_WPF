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
    class UsersRepository : IUsersRepository
    {
        SQLiteAsyncConnection conn;

        public UsersRepository(IDBConnection oIDBConnection)
        {
            conn = oIDBConnection.GetAsyncConnection();
        }

        public async Task Insert_Users_Async(Users users)
        {
            await conn.InsertAsync(users);
        }

        public async Task Update_Users_Async(Users users)
        {
            await conn.UpdateAsync(users);
        }

        public async Task Delete_Users_Async(Users users)
        {
            await conn.DeleteAsync(users);
        }

        public async Task<List<Users>> Select_All_Users_Async()
        {
            return await conn.Table<Users>().ToListAsync();
        }

        public async Task<List<Users>> Select_Users_Async(string query)
        {
            return await conn.QueryAsync<Users>(query);
        }
    }
}
