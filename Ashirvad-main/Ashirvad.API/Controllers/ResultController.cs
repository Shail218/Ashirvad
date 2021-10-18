using Ashirvad.Data;
using Ashirvad.ServiceAPI.ServiceAPI.Area;
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
    public class ResultController : ApiController
    {

        private readonly IMarksService _MarksService;
        public ResultController(IMarksService MarksService)
        {
            _MarksService = MarksService;
        }
        // GET: Marks

        [Route("MarksMaintenance")]
        [HttpPost]
        public OperationResult<MarksEntity> MarksMaintenance(MarksEntity MarksEntity)
        {

            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                try
                {
                    foreach (string file in httpRequest.Files)
                    {
                        string fileName;
                        string extension;
                        var postedFile = httpRequest.Files[file];
                        string randomfilename = Common.Common.RandomString(20);
                        extension = Path.GetExtension(postedFile.FileName);
                        fileName = Path.GetFileName(postedFile.FileName);
                        string _Filepath = "~/MarksImage/" + randomfilename + extension;
                        var filePath = HttpContext.Current.Server.MapPath("~/MarksImage/" + randomfilename + extension);
                        postedFile.SaveAs(filePath);
                        MarksEntity.MarksContentFileName = fileName;
                        MarksEntity.MarksFilepath = _Filepath;
                    }



                }
                catch (Exception ex)
                {

                }
            }



            var data = this._MarksService.MarksMaintenance(MarksEntity);
            OperationResult<MarksEntity> result = new OperationResult<MarksEntity>();
            result.Completed = true;
            result.Data = data.Result;
            return result;


        }

        [Route("GetMarksByID")]
        [HttpPost]
        public OperationResult<MarksEntity> GetMarksByID(long MarksID)
        {
            var data = this._MarksService.GetMarksByMarksID(MarksID);
            OperationResult<MarksEntity> result = new OperationResult<MarksEntity>();
            result.Data = data.Result;
            return result;
        }

        [Route("RemoveMarks")]
        [HttpPost]
        public OperationResult<bool> RemoveMarks(long MarksID, string lastupdatedby)
        {
            var result = _MarksService.RemoveMarks(MarksID, lastupdatedby);
            OperationResult<bool> response = new OperationResult<bool>();
            response.Completed = result;
            response.Data = result;
            return response;
        }
        [Route("GetAllMarks")]
        [HttpPost]
        public OperationResult<List<MarksEntity>> GetAllMarks()
        {
            var MarksData = this._MarksService.GetAllMarks();
            OperationResult<List<MarksEntity>> result = new OperationResult<List<MarksEntity>>();
            result.Data = MarksData.Result;
            return result;
        }
    }
}
