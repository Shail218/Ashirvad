using Ashirvad.Data;
using Ashirvad.Logger;
using Ashirvad.Repo.DataAcceessAPI.Area.ToDo;
using Ashirvad.ServiceAPI.ServiceAPI.Area.ToDo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.Services.Area.ToDo
{
    public class ToDoService : IToDoService
    {
        private readonly IToDoAPI _todoContext;
        public ToDoService(IToDoAPI todoContext)
        {
            this._todoContext = todoContext;
        }

        public async Task<ToDoEntity> ToDoMaintenance(ToDoEntity todo)
        {
            ToDoEntity td = new ToDoEntity();
            try
            {
                long ToDoID = await _todoContext.ToDoMaintenance(todo);
                if (ToDoID > 0)
                {
                    td.ToDoID = ToDoID;
                }
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return td;
        }

        public async Task<List<ToDoEntity>> GetAllToDoByBranch(long branchID, long userID = 0)
        {
            try
            {
                var data = await _todoContext.GetAllToDoByBranch(branchID, userID);
                return data;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<ToDoEntity>> GetAllToDoWithoutContentByBranch(long branchID, long userID = 0)
        {
            try
            {
                var data = await _todoContext.GetAllToDoWithoutContentByBranch(branchID, userID);
                return data;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<ToDoEntity> GetToDoByToDoID(long todoID)
        {
            try
            {
                var data = await _todoContext.GetToDoByToDoID(todoID);
                return data;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public bool RemoveToDo(long todoID, string lastupdatedby)
        {
            try
            {
                var data = _todoContext.RemoveToDo(todoID, lastupdatedby);
                return data;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return false;
        }
    }
}
