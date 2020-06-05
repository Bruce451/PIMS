using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Pims.Dal.Services
{
    /// <summary>
    /// TaskService class, provides a service layer to interact with tasks within the datasource.
    /// </summary>
    public class TaskService : BaseService<Task>, ITaskService
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of a TaskService, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public TaskService(PimsContext dbContext, ClaimsPrincipal user, ILogger<TaskService> logger) : base(dbContext, user, logger) { }
        #endregion

        #region Methods
        /// <summary>
        /// Get an array of tasks for the specified 'statusId'.
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>
        public IEnumerable<Task> GetForStatus(int statusId)
        {
            var tasks = from s in this.Context.ProjectStatus.AsNoTracking()
                        join st in this.Context.ProjectStatusTasks on s.Id equals st.StatusId
                        where s.Id == statusId
                        orderby st.Task.SortOrder, st.Task.Name
                        select st.Task;

            return tasks.ToArray();
        }

        /// <summary>
        /// Get an array of tasks for the specified 'statusCode'.
        /// </summary>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public IEnumerable<Task> GetForStatus(string statusCode)
        {
            var tasks = from s in this.Context.ProjectStatus.AsNoTracking()
                        join st in this.Context.ProjectStatusTasks on s.Id equals st.StatusId
                        where s.Code == statusCode
                        orderby st.Task.SortOrder, st.Task.Name
                        select st.Task;

            return tasks.ToArray();
        }

        /// <summary>
        /// Get an array of tasks for the specified 'workflowCode'.
        /// </summary>
        /// <param name="workflowCode"></param>
        /// <returns></returns>
        public IEnumerable<Task> GetForWorkflow(string workflowCode)
        {
            var tasks = from w in this.Context.Workflows.AsNoTracking()
                        join ws in this.Context.WorkflowProjectStatus on w.Id equals ws.WorkflowId
                        join s in this.Context.ProjectStatus on ws.StatusId equals s.Id
                        join st in this.Context.ProjectStatusTasks on s.Id equals st.StatusId
                        where w.Code == workflowCode
                        orderby st.StatusId, st.Task.SortOrder, st.Task.Name
                        select st.Task;

            return tasks.ToArray();
        }
        #endregion
    }
}
