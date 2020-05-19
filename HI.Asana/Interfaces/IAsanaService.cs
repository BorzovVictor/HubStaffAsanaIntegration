using System.Threading.Tasks;
using HI.SharedKernel.Models;
using HI.SharedKernel.Models.Responses;

namespace HI.Asana
{
    public interface IAsanaService
    {
        Task<AsanaTaskModel> GetById(string taskId);
        Task<HistoryData> UpdateSumFieldTask(HsTeamMemberTask hubTask);
        Task<HistoryData> UpdateSumFieldTaskTest(HsTeamMemberTask hubTask);
    }
}