using System.Collections.Generic;
using System.Threading.Tasks;
using Flovers_WPF.DataModel;

namespace Flovers_WPF.Repository
{
    interface IClientsRepository
    {
        Task Insert_Clients_Async(Clients clients);
        Task Update_Clients_Async(Clients clients);
        Task Delete_Clients_Async(Clients clients);
        Task<List<Clients>> Select_All_Clients_Async();
        Task<List<Clients>> Select_Clients_Async(string query);
    }
}
