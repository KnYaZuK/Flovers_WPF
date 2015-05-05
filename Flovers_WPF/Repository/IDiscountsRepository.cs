using System.Collections.Generic;
using System.Threading.Tasks;
using Flovers_WPF.DataModel;

namespace Flovers_WPF.Repository
{
    interface IDiscountsRepository
    {
        Task Insert_Discounts_Async(Discounts discounts);
        Task Update_Discounts_Async(Discounts discounts);
        Task Delete_Discounts_Async(Discounts discounts);
        Task<List<Discounts>> Select_All_Discounts_Async();
        Task<List<Discounts>> Select_Discounts_Async(string query);
    }
}
