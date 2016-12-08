using System;
using System.Collections.Generic;
using System.Data;
using Common.Core.BaseClasses;
using Common.Core.Logging;
using WiseQueue.Core.Common.DataContexts;
using WiseQueue.Core.Common.Models;
using WiseQueue.Domain.MsSql.Utils;
using WiseQueue.Domain.MsSql.Utils.Implementation;

namespace WiseQueue.Domain.MsSql.MsSqlDataContext
{
    class QueueDataContext : BaseLoggerObject, IQueueDataContext
    {
        #region Consts...
       
        /// <summary>
        /// Table's name where all queues are stored.
        /// </summary>
        private const string queueTableName = "Queues"; //TODO: move to settings.

        #endregion

        #region Fields...

        /// <summary>
        /// Sql settings.
        /// </summary>
        private readonly MsSqlSettings sqlSettings;

        /// <summary>
        /// The <see cref="ISqlConnectionFactory"/> instance.
        /// </summary>
        private readonly ISqlConnectionFactory connectionFactory;

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="sqlSettings">Sql settings.</param>
        /// <param name="connectionFactory">The <see cref="ISqlConnectionFactory"/> instance.</param>
        /// <param name="loggerFactory">The <see cref="ICommonLoggerFactory"/> instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="sqlSettings"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="connectionFactory"/> is <see langword="null" />.</exception>
        public QueueDataContext(MsSqlSettings sqlSettings, ISqlConnectionFactory connectionFactory, ICommonLoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            if (sqlSettings == null) 
                throw new ArgumentNullException("sqlSettings");
            if (connectionFactory == null) 
                throw new ArgumentNullException("connectionFactory");

            this.sqlSettings = sqlSettings;
            this.connectionFactory = connectionFactory;
        }

        #region Implementation of IQueueDataContext

        /// <summary>
        /// Get queue by its name.
        /// </summary>
        /// <param name="queueName">The queue name.</param>
        /// <returns>The <see cref="QueueModel"/> instance.</returns>
        public QueueModel GetQueueByName(string queueName)
        {
            string sqlCommand = string.Format("SELECT * FROM {0}.{1} WHERE [Name] = '{2}'", sqlSettings.WiseQueueDefaultSchema, queueTableName, queueName);

            logger.WriteTrace("Executing sql command...");
            using (IDbConnection connection = connectionFactory.CreateConnection())
            {
                using (IDbCommand command = connectionFactory.CreateCommand(connection))
                {
                    command.CommandText = sqlCommand;
                    using (IDataReader rdr = command.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Int64 id = (Int64) rdr["Id"];
                            string name = (string) rdr["Name"];
                            string description = (string) rdr["Description"];

                            QueueModel queueModel = new QueueModel(name, description)
                            {
                                Id = id
                            };

                            logger.WriteTrace("The command has been executed. Result = {0}", queueModel);
                            return queueModel;
                        }
                    }                    
                }
            }
            return null; //TODO: It is not a good idea. We should return something like NullTaskEntity.
        }

        /// <summary>
        /// Insert queue.
        /// </summary>
        /// <param name="queue">The <see cref="QueueModel"/> instance.</param>
        /// <returns>The queue identifier.</returns>
        public long InsertQueue(QueueModel queue)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get available queues.
        /// </summary>
        /// <returns><c>List</c> of <see cref="QueueModel"/> instances.</returns>
        public IReadOnlyCollection<QueueModel> GetAvailableQueues()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
