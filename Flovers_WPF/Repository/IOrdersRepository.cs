using System.Collections.Generic;
using System.Threading.Tasks;
using Flovers_WPF.DataModel;

namespace Flovers_WPF.Repository
{
    interface IOrdersRepository
    {
        Task Insert_Orders_Async(Orders orders);
        Task Update_Orders_Async(Orders orders);
        Task Delete_Orders_Async(Orders orders);
        Task<List<Orders>> Select_All_Orders_Async();
        Task<List<Orders>> Select_Orders_Async(string query);
    }
}
