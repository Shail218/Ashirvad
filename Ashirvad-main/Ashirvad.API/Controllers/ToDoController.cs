using Ashirvad.API.Filter;
using Ashirvad.Data;
using Ashirvad.ServiceAPI.ServiceAPI.Area.ToDo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Ashirvad.API.Controllers
{
    [RoutePrefix("api/todo/v1")]
    [AshirvadAuthorization]
    public class ToDoController : ApiController
    {
        private readonly IToDoService _todoService = null;
        public ToDoController(IToDoService todoService)
        {
            _todoService = todoService;
        }


        [Route("ToDoMaintenance")]
        [HttpPost]
        public OperationResult<ToDoEntity> ToDoMaintenance(ToDoEntity todo)
        {
            OperationResult<ToDoEntity> result = new OperationResult<ToDoEntity>();

            var data = this._todoService.ToDoMaintenance(todo);
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        [Route("GetAllToDoByBranch")]
        [HttpGet]
        public OperationResult<List<ToDoEntity>> GetAllToDoByBranch(long branchID)
        {
            var data = this._todoService.GetAllToDoByBranch(branchID);
            OperationResult<List<ToDoEntity>> result = new OperationResult<List<ToDoEntity>>();
            result.Data = data.Result;
            result.Completed = true;
            return result;
        }

        [Route("GetAllToDoByBranchAndUser")]
        [HttpGet]
        public OperationResult<List<ToDoEntity>> GetAllToDoByBranch(long branchID, long userID)
        {
            var data = this._todoService.GetAllToDoByBranch(branchID, userID);
            OperationResult<List<ToDoEntity>> result = new OperationResult<List<ToDoEntity>>();
            result.Data = data.Result;
            result.Completed = true;
            return result;
        }

        [Route("GetAllToDoWithoutContentByBranch")]
        [HttpGet]
        public OperationResult<List<ToDoEntity>> GetAllToDoWithoutContentByBranch(long branchID)
        {
            var data = this._todoService.GetAllToDoWithoutContentByBranch(branchID);
            OperationResult<List<ToDoEntity>> result = new OperationResult<List<ToDoEntity>>();
            result.Data = data.Result;
            result.Completed = true;
            return result;
        }

        [Route("GetAllToDoWithoutContentByBranchAndUser")]
        [HttpGet]
        public OperationResult<List<ToDoEntity>> GetAllToDoWithoutContentByBranch(long branchID, long userID)
        {
            var data = this._todoService.GetAllToDoWithoutContentByBranch(branchID, userID);
            OperationResult<List<ToDoEntity>> result = new OperationResult<List<ToDoEntity>>();
            result.Data = data.Result;
            result.Completed = true;
            return result;
        }

        [Route("GetToDoByHWID")]
        [HttpGet]
        public OperationResult<ToDoEntity> GetToDoByHWID(long todoID)
        {
            var data = this._todoService.GetToDoByToDoID(todoID);
            OperationResult<ToDoEntity> result = new OperationResult<ToDoEntity>();
            result.Data = data.Result;
            result.Completed = true;
            return result;
        }


        [Route("RemoveToDo")]
        [HttpPost]
        public OperationResult<bool> RemoveToDo(long todoID, string lastupdatedby)
        {
            var data = this._todoService.RemoveToDo(todoID, lastupdatedby);
            OperationResult<bool> result = new OperationResult<bool>();
            result.Completed = true;
            result.Data = data;
            return result;
        }
    }
}
