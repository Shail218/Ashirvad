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
    public class ToDoRegisterController : BaseController
    {
        private readonly IToDoService _todoService = null;
        public ToDoRegisterController(IToDoService todoService)
        {
            _todoService = todoService;
        }

        // GET: ToDoRegister
        public ActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> ToDoRegisterMaintenance(long todoID)
        {
            ToDoMaintenanceModel branch = new ToDoMaintenanceModel();
            if (todoID > 0)
            {
                var todo = await _todoService.GetToDoByToDoID(todoID);
                branch.ToDoInfo = todo;
            }

            var todoData = await _todoService.GetAllToDoByBranch(SessionContext.Instance.LoginUser.BranchInfo.BranchID, (SessionContext.Instance.LoginUser.UserID));
            branch.ToDoData = todoData;

            return View("Index", branch);
        }

        public async Task<ActionResult> ToDoEdit(long todoID)
        {
            ToDoMaintenanceModel branch = new ToDoMaintenanceModel();
            var todo = await _todoService.GetToDoByToDoID(todoID);
            branch.ToDoInfo = todo;
            return View("~/Views/ToDoRegister/Create.cshtml", branch.ToDoInfo);
        }

        [HttpPost]
        public async Task<JsonResult> SaveToDo(ToDoEntity toDoEntity)
        {
            toDoEntity.Transaction = GetTransactionData(toDoEntity.ToDoID > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
            var data = await _todoService.ToDoMaintenance(toDoEntity);
           
            return Json(data);
        }

    }
}