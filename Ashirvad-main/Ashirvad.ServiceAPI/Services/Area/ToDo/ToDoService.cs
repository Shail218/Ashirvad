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
using static Ashirvad.Common.Common;

namespace Ashirvad.ServiceAPI.Services.Area.ToDo
{
    public class ToDoService : IToDoService
    {
        private readonly IToDoAPI _todoContext;
        public ToDoService(IToDoAPI todoContext)
        {
            this._todoContext = todoContext;
        }

        public async Task<ResponseModel> ToDoMaintenance(ToDoEntity todo)
        {
            ResponseModel responseModel = new ResponseModel();
            ToDoEntity td = new ToDoEntity();
            try
            {
                responseModel = await _todoContext.ToDoMaintenance(todo);
                //long ToDoID = await _todoContext.ToDoMaintenance(todo);
                //if (ToDoID > 0)
                //{
                //    td.ToDoID = ToDoID;
                //}
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return responseModel;
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

        public async Task<List<ToDoEntity>> GetAllCustomToDo(DataTableAjaxPostModel model, long branchID)
        {
            try
            {
                return await this._todoContext.GetAllCustomToDo(model, branchID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<ToDoEntity>> GetAllToDoList(long branchID)
        {
            try
            {
                return await this._todoContext.GetAllToDoList(branchID);
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

        public ResponseModel RemoveToDo(long todoID, string lastupdatedby)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                return this._todoContext.RemoveToDo(todoID, lastupdatedby);               
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return responseModel;
        }
    }
}
