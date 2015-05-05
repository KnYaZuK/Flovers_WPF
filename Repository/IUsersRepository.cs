using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flovers_WPF.DataModel;

namespace Flovers_WPF.Repository
{
    interface IUsersRepository
    {
        Task Insert_Users_Async(Users users);
        Task Update_Users_Async(Users users);
        Task Delete_Users_Async(Users users);
        Task<List<Users>> Select_All_Users_Async();
        Task<List<Users>> Select_Users_Async(string query);
    }
}
