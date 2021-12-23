﻿using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Paper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Web.Controllers
{
    public class PapersController : BaseController
    {
        private readonly IPaperService _paperService;
        public PapersController(IPaperService paperService)
        {
            _paperService = paperService;
        }
        // GET: Papers
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> PaperMaintenance(long paperID)
        {
            PaperMaintenanceModel branch = new PaperMaintenanceModel();
            if (paperID > 0)
            {
                var result = await _paperService.GetPaperByPaperID(paperID);
                branch.PaperInfo = result.Data;
            }
            var paperData = await _paperService.GetAllPaperWithoutContent(SessionContext.Instance.LoginUser.BranchInfo.BranchID);
                branch.PaperData = paperData.Data;
            return View("Index", branch);
        }


        [HttpPost]
        public async Task<JsonResult> SavePaper(PaperEntity paperEntity)
        {
            if (paperEntity.PaperData.PaperFile != null)
            {
                //fileModel= fileUploadCommon.SaveFileUploadweb(Fees.ImageFile, "FeesImage").Result;
                //Fees.Fees_Content = Common.Common.ReadFully(Fees.ImageFile.InputStream);
                string _FileName = Path.GetFileName(paperEntity.PaperData.PaperFile.FileName);
                string extension = System.IO.Path.GetExtension(paperEntity.PaperData.PaperFile.FileName);
                string randomfilename = Common.Common.RandomString(20);
                string _Filepath = "/PaperDocument/" + randomfilename + extension;
                string _path = Path.Combine(Server.MapPath("~/PaperDocument"), randomfilename + extension);
                paperEntity.PaperData.PaperFile.SaveAs(_path);
                paperEntity.PaperData.PaperPath = _FileName;
                paperEntity.PaperData.FilePath = _Filepath;
            }
            paperEntity.Transaction = GetTransactionData(paperEntity.PaperID > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
            paperEntity.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            var data = await _paperService.PaperMaintenance(paperEntity);
            if (data != null)
            {
                return Json(true);
            }

            return Json(false);
        }

        [HttpPost]
        public JsonResult RemovePaper(long paperID)
        {
            var result = _paperService.RemovePaper(paperID, SessionContext.Instance.LoginUser.Username);
            return Json(result);
        }

        public async Task<JsonResult> Downloadpaper(long paperid)
        {
            string[] array = new string[4];
            try
            {
                OperationResult<PaperEntity> operationResult = new OperationResult<PaperEntity>();
                operationResult = await _paperService.GetPaperByPaperID(paperid);
                if (operationResult != null)
                {
                    string contentType = "";
                    string[] extarray = operationResult.Data.PaperData.PaperPath.Split('.');
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
                    string file = operationResult.Data.PaperData.PaperContentText;
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