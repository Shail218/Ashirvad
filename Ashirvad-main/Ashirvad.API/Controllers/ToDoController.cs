using Ashirvad.API.Filter;
using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.ServiceAPI.ServiceAPI.Area.ToDo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
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

        [Route("ToDoMaintenance/{ToDoID}/{ToDo_Date}/{BranchID}/{UserID}/{ToDo_Description}/{CreateId}/{CreateBy}/{TransactionId}/{FileName}/{Extension}/{HasFile}")]
        [HttpPost]
        public OperationResult<ToDoEntity> ToDoMaintenance(long ToDoID, DateTime ToDo_Date, long BranchID,long UserID,
            string ToDo_Description, long CreateId, string CreateBy, long TransactionId, string FileName, string Extension, bool HasFile)
        {
            OperationResult<ToDoEntity> result = new OperationResult<ToDoEntity>();
            var httpRequest = HttpContext.Current.Request;
            ToDoEntity toDoEntity = new ToDoEntity();
            ToDoEntity data = new ToDoEntity();
            toDoEntity.BranchInfo = new BranchEntity();
            toDoEntity.UserInfo = new UserEntity();
            toDoEntity.ToDoID = ToDoID;
            toDoEntity.ToDoDate = ToDo_Date;
            toDoEntity.BranchInfo.BranchID = BranchID;
            toDoEntity.UserInfo.UserID = UserID;
            toDoEntity.ToDoDescription = ToDo_Description;
            toDoEntity.ToDoFileName = FileName;
            toDoEntity.FilePath = "/ToDoDocument/" + FileName + "." + Extension;
            toDoEntity.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            toDoEntity.Transaction = new TransactionEntity()
            {
                TransactionId = TransactionId,
                LastUpdateBy = CreateBy,
                LastUpdateId = CreateId,
                CreatedBy = CreateBy,
                CreatedId = CreateId,
            };
            if (HasFile)
            {
                try
                {
                    if (httpRequest.Files.Count > 0)
                    {
                        foreach (string file in httpRequest.Files)
                        {
                            string fileName;
                            string extension;
                            string currentDir = AppDomain.CurrentDomain.BaseDirectory;
                            // for live server
                            //string UpdatedPath = currentDir.Replace("AshirvadAPI", "ashivadproduct");
                            // for local server
                            string UpdatedPath = currentDir.Replace("Ashirvad.API", "Ashirvad.Web");
                            var postedFile = httpRequest.Files[file];
                            string randomfilename = Common.Common.RandomString(20);
                            extension = Path.GetExtension(postedFile.FileName);
                            fileName = Path.GetFileName(postedFile.FileName);
                            string _Filepath = "/ToDoDocument/" + randomfilename + extension;
                            string _Filepath1 = "ToDoDocument/" + randomfilename + extension;
                            var filePath = HttpContext.Current.Server.MapPath("~/ToDoDocument/" + randomfilename + extension);
                            string _path = UpdatedPath + _Filepath1;
                            postedFile.SaveAs(_path);
                            toDoEntity.ToDoFileName = fileName;
                            toDoEntity.FilePath = _Filepath;
                        }
                    }
                }
                catch (Exception ex)
                {
                    result.Completed = false;
                    result.Data = null;
                    result.Message = ex.ToString();
                }
            }
            data = this._todoService.ToDoMaintenance(toDoEntity).Result;
            result.Completed = false;
            result.Data = null;
            if (data.ToDoID > 0)
            {
                result.Completed = true;
                result.Data = data;
                if (ToDoID > 0)
                {
                    result.Message = "To-Do Updated Successfully";
                }
                else
                {
                    result.Message = "To-Do Created Successfully";
                }
            }
            return result;
        }
    }
}
