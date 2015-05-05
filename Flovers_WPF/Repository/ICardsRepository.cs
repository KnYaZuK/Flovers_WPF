using System.Collections.Generic;
using System.Threading.Tasks;
using Flovers_WPF.DataModel;

namespace Flovers_WPF.Repository
{
    interface ICardsRepository
    {
        Task Insert_Cards_Async(Cards cards);
        Task Update_Cards_Async(Cards cards);
        Task Delete_Cards_Async(Cards cards);
        Task<List<Cards>> Select_All_Cards_Async();
        Task<List<Cards>> Select_Cards_Async(string query);
    }
}
