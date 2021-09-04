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

        public async Task<JsonResult> Downloadtodo(long todoid)
        {
            string[] array = new string[4];
            try
            {
                var operationResult = await _todoService.GetToDoByToDoID(todoid);
                if (operationResult != null)
                {
                    string contentType = "";
                    string[] extarray = operationResult.ToDoFileName.Split('.');
                    string ext = extarray[1];
                    switch (ext)
                    {
                        case "pdf":
                            contentType = "application/pdf";
                            break;
                        case "xlsx":
                            contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            break;
                        case "docx":
                            contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                            break;
                        case "png":
                            contentType = "image/png";
                            break;
                        case "jpg":
                            contentType = "image/jpeg";
                            break;
                        case "txt":
                            contentType = "application/text/plain";
                            break;
                        case "mp4":
                            contentType = "application/video";
                            break;
                        case "pptx":
                            contentType = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                            break;
                        case "zip":
                            contentType = "application/zip";
                            break;
                        case "rar":
                            contentType = "application/x-rar-compressed";
                            break;
                        case "xls":
                            contentType = "application/vnd.ms-excel";
                            break;
                    }
                    string file = operationResult.ToDoContentText;
                    string filename = extarray[0];
                    array[0] = ext;
                    array[1] = file;
                    array[2] = filename;
                    array[3] = contentType;
                    return Json(array);
                }
            }
            catch (Exception ex)
            {

            }
            return Json(array);
        }

    }
}