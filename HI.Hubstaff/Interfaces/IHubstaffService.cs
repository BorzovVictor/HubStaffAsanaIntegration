using System.Threading.Tasks;
using HI.SharedKernel.Models;

namespace HI.Hubstaff
{
    public interface IHubstaffService
    {
        Task<HubstaffAuthModel> GenAuth();
    }
}