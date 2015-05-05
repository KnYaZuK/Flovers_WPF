using System.Collections.Generic;
using System.Threading.Tasks;
using Flovers_WPF.DataModel;

namespace Flovers_WPF.Repository
{
    interface IAccessoriesRepository
    {
        Task Insert_Accessories_Async(Accessories accessories);
        Task Update_Accessories_Async(Accessories accessories);
        Task Delete_Accessories_Async(Accessories accessories);
        Task<List<Accessories>> Select_All_Accessories_Async();
        Task<List<Accessories>> Select_Accessories_Async(string query);
    }
}
