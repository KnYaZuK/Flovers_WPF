using System.Collections.Generic;
using System.Threading.Tasks;
using Flovers_WPF.DataModel;

namespace Flovers_WPF.Repository
{
    interface IFlowersRepository
    {
        Task Insert_Flowers_Async(Flowers flowers);
        Task Update_Flowers_Async(Flowers flowers);
        Task Delete_Flowers_Async(Flowers flowers);
        Task<List<Flowers>> Select_All_Flowers_Async();
        Task<List<Flowers>> Select_Flowers_Async(string query);
    }
}
