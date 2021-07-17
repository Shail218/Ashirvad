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
                if (todo.FileInfo != null)
                {
                    todo.ToDoContent = Common.Common.ReadFully(todo.FileInfo.InputStream);
                    todo.ToDoFileName = Path.GetFileName(todo.FileInfo.FileName);
                }
                else
                {
                    todo.ToDoContent = Convert.FromBase64String(todo.ToDoContentText);
                }


                var data = await _todoContext.ToDoMaintenance(todo);
                if (data > 0)
                {
                    if (!string.IsNullOrEmpty(Common.Common.GetStringConfigKey("DocDirectory")))
                    {
                        Common.Common.SaveFile(todo.ToDoContent, todo.ToDoFileName, "ToDo\\");
                    }
                }

                td = todo;
                td.ToDoID = data;
                return td;
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
