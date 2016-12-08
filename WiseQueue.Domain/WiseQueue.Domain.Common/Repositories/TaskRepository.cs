using Common.Core.Interfaces.DataAccessLayer;
using Common.Core.Logging;
using Common.Domain.DataAccessLayer;
using WiseQueue.Core.Common.Entities.Tasks;
using WiseQueue.Core.Common.Repositories;

namespace WiseQueue.Domain.Common.Repositories
{
    public class TaskRepository: BaseWiseRepository<TaskEntity>, ITaskRepository
    {
        public TaskRepository(IDataContextFactory dataContextFactory, IEntityModelMapper entityModelMapper, ICommonLogger logger) : base(dataContextFactory, entityModelMapper, logger)
        {
        }
    }
}
