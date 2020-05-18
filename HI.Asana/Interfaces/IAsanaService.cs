using System.Threading.Tasks;
using HI.SharedKernel.Models;
using HI.SharedKernel.Models.Responses;

namespace HI.Asana
{
    public interface IAsanaService
    {
        Task<AsanaTaskModel> GetById(string taskId);
        Task<UpdateSumFieldsResult> UpdateSumFieldTask(string taskId, long sumSeconds);
        Task<UpdateSumFieldsResult> UpdateSumFieldTaskTest(string taskId, long sumSeconds);
    }
}