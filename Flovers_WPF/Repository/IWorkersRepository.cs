using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flovers_WPF.DataModel;

namespace Flovers_WPF.Repository
{
    interface IWorkersRepository
    {
        Task Insert_Workers_Async(Workers workers);
        Task Update_Workers_Async(Workers workers);
        Task Delete_Workers_Async(Workers workers);
        Task<List<Workers>> Select_All_Workers_Async();
        Task<List<Workers>> Select_Workers_Async(string query);
    }
}
