using System.Collections.Generic;
using System.Threading.Tasks;
using HI.SharedKernel.Models;

namespace HI.Hubstaff
{
    public interface IHubstaffService
    {
        Task<HsAuthModel> GenAuth();
        Task<List<HsTaskModel>> Tasks(string projects = "", int offset = 0);
        Task<HsTeamMemberModel> TeamByMember(HsTeamMemberRequest request);
        Task<List<HsTeamMemberTask>> GetTasksDurations(HsTeamMemberRequest request);
    }
}