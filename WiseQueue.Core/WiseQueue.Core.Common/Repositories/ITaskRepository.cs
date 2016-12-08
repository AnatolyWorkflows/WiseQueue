using Common.Core.Interfaces.DataAccessLayer;
using WiseQueue.Core.Common.Entities.Tasks;

namespace WiseQueue.Core.Common.Repositories
{
    public interface ITaskRepository: IRepository<TaskEntity>
    {
    }
}
