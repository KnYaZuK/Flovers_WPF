using System.Collections.Generic;
using System.Threading.Tasks;
using Flovers_WPF.DataModel;

namespace Flovers_WPF.Repository
{
    interface ICartsRepository
    {
        Task Insert_Carts_Async(Carts carts);
        Task Update_Carts_Async(Carts carts);
        Task Delete_Carts_Async(Carts carts);
        Task<List<Carts>> Select_All_Carts_Async();
        Task<List<Carts>> Select_Carts_Async(string query);
    }
}
