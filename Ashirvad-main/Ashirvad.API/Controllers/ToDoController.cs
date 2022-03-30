using Ashirvad.API.Filter;
using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.ServiceAPI.ServiceAPI.Area.ToDo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
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
            result.Completed = data.Result.Status;
            result.Message = data.Result.Message;
            if (data.Result.Status)
            {
                result.Data =(ToDoEntity)data.Result.Data;

            }
        
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
            var data = this._todoService.GetAllToDoByBranch(branchID);
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
            result.Completed = data.Status;
            result.Data = data.Status;
            result.Message = data.Message;
            return result;
        }

        [Route("ToDoMaintenance")]
        [HttpPost]
        public OperationResult<ToDoEntity> ToDoMaintenance(string model,bool HasFile)
        {
            OperationResult<ToDoEntity> result = new OperationResult<ToDoEntity>();
            var httpRequest = HttpContext.Current.Request;            
            ToDoEntity toDoEntity = new ToDoEntity();
            toDoEntity.BranchInfo = new BranchEntity();
            toDoEntity.UserInfo = new UserEntity();
            var entity = JsonConvert.DeserializeObject<ToDoEntity>(model);
            toDoEntity.ToDoID = entity.ToDoID;
            toDoEntity.ToDoDate = entity.ToDoDate;
            toDoEntity.BranchInfo.BranchID = entity.BranchInfo.BranchID;
            toDoEntity.UserInfo.UserID = entity.UserInfo.UserID;
            toDoEntity.ToDoDescription = entity.ToDoDescription;     
            toDoEntity.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            toDoEntity.Transaction = new TransactionEntity()
            {
                TransactionId = entity.Transaction.TransactionId,
                LastUpdateBy = entity.Transaction.LastUpdateBy,
                LastUpdateId = entity.Transaction.LastUpdateId,
                CreatedBy = entity.Transaction.CreatedBy,
                CreatedId = entity.Transaction.CreatedId
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
                            string UpdatedPath = currentDir.Replace("WebAPI", "wwwroot");
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
            else
            {
                toDoEntity.ToDoFileName = entity.ToDoFileName;
                toDoEntity.FilePath = entity.FilePath;
            }
            var data = this._todoService.ToDoMaintenance(toDoEntity).Result;
            result.Completed = data.Status;
            result.Message = data.Message;
            if (data.Status)
            {
                result.Data = (ToDoEntity)data.Data;

            }
            return result;
        }

        public static string Decode(string Path)
        {
            byte[] mybyte = Convert.FromBase64String(Path);
            string returntext = Encoding.UTF8.GetString(mybyte);
            return returntext;
        }
    }
}
