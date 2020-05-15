using System.Threading.Tasks;
using HI.SharedKernel.Models;

namespace HI.Asana
{
    public interface IAsanaService
    {
        Task<AsanaTaskModel> GetById(string taskId);
    }
}