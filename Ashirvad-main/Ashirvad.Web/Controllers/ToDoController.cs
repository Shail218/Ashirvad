using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area.ToDo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Web.Controllers
{
    public class ToDoController : BaseController
    {
        private readonly IToDoService _todoService = null;
        public ToDoController(IToDoService todoService)
        {
            _todoService = todoService;
        }

        // GET: ToDo
        public ActionResult Index()
        {
            return View();
        }
        
        public async Task<ActionResult> ToDoMaintenance(long todoID)
        {
            ToDoMaintenanceModel branch = new ToDoMaintenanceModel();
            if (todoID > 0)
            {
                var todo = await _todoService.GetToDoByToDoID(todoID);
                branch.ToDoInfo = todo;
            }

            var todoData = await _todoService.GetAllToDoByBranch(SessionContext.Instance.LoginUser.BranchInfo.BranchID);
            branch.ToDoData = todoData;

            return View("Index", branch);
        }

        [HttpPost]
        public async Task<JsonResult> SaveToDo(ToDoEntity toDoEntity)
        {
            toDoEntity.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            toDoEntity.Transaction = GetTransactionData(toDoEntity.ToDoID > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
            var data = await _todoService.ToDoMaintenance(toDoEntity);
            if (data != null)
            {
                return Json(true);
            }

            return Json(false);
        }

        [HttpPost]
        public JsonResult RemoveToDo(long todoID)
        {
            var result = _todoService.RemoveToDo(todoID, SessionContext.Instance.LoginUser.Username);
            return Json(result);
        }

    }
}