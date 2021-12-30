using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area.ToDo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static Ashirvad.Common.Common;

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

            //var todoData = await _todoService.GetAllToDoByBranch(SessionContext.Instance.LoginUser.BranchInfo.BranchID);
            branch.ToDoData = new List<ToDoEntity>();

            return View("Index", branch);
        }

        [HttpPost]
        public async Task<JsonResult> SaveToDo(ToDoEntity toDoEntity)
        {
            if (toDoEntity.FileInfo != null)
            {
                string _FileName = Path.GetFileName(toDoEntity.FileInfo.FileName);
                string extension = System.IO.Path.GetExtension(toDoEntity.FileInfo.FileName);
                string randomfilename = Common.Common.RandomString(20);
                string _Filepath = "/ToDoDocument/" + randomfilename + extension;
                string _path = Path.Combine(Server.MapPath("~/ToDoDocument"), randomfilename + extension);
                toDoEntity.FileInfo.SaveAs(_path);
                toDoEntity.ToDoFileName = _FileName;
                toDoEntity.FilePath = _Filepath;
            }
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

        public async Task<JsonResult> CustomServerSideSearchAction(DataTableAjaxPostModel model)
        {
            List<string> columns = new List<string>();
            columns.Add("ToDoDate");
            columns.Add("ToDoDescription");
            foreach (var item in model.order)
            {
                item.name = columns[item.column];
            }
            var branchData = await _todoService.GetAllCustomToDo(model, SessionContext.Instance.LoginUser.BranchInfo.BranchID);
            long total = 0;
            if (branchData.Count > 0)
            {
                total = branchData[0].Count;
            }
            return Json(new
            {
                draw = model.draw,
                iTotalRecords = total,
                iTotalDisplayRecords = total,
                data = branchData
            });

        }

    }
}