using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HI.SharedKernel.Models;

namespace HI.Hubstaff
{
    public interface IHubstaffService
    {
        Task<HubstaffAuthModel> GenAuth();
        Task<List<HubstaffTaskModel>> Tasks(string projects = "", int offset = 0);
    }
}