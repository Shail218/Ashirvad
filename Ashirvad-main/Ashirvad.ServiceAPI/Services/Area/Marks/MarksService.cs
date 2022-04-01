using Ashirvad.Data;
using Ashirvad.Logger;
using Ashirvad.Repo.DataAcceessAPI.Area;
using Ashirvad.ServiceAPI.ServiceAPI.Area;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.ServiceAPI.Services.Area
{
    public class MarksService : IMarksService
    {
        private readonly IMarksAPI _MarksContext;
        public MarksService(IMarksAPI MarksContext)
        {
            this._MarksContext = MarksContext;
        }

        public async Task<ResponseModel> MarksMaintenance(MarksEntity MarksInfo)
        {
            ResponseModel responseModel = new ResponseModel();
            MarksEntity Marks = new MarksEntity();
            try
            { 
                responseModel = await _MarksContext.MarksMaintenance(MarksInfo);
                //long MarksID = await _MarksContext.MarksMaintenance(MarksInfo);
                //Marks.MarksID = MarksID;
            }
            catch (Exception ex)
                {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return responseModel;
        }

        public async Task<MarksEntity> GetMarksByMarksID(long MarksID)
        {
            try
            {
                MarksEntity Marks = new MarksEntity();
                Marks = await _MarksContext.GetMarksByMarksID(MarksID);                
                return Marks;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<MarksEntity>> GetAllMarks()
        {
            try
            {
                return await this._MarksContext.GetAllMarks();
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public ResponseModel RemoveMarks(long MarksID, string lastupdatedby)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                responseModel = this._MarksContext.RemoveMarks(MarksID, lastupdatedby);
                //return this._MarksContext.RemoveMarks(MarksID, lastupdatedby);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return responseModel;
        }


        public async Task<List<MarksEntity>> GetAllAchieveMarks(long Std, long Branch, long Batch, long MarksID)
        {
            try
            {
                return await this._MarksContext.GetAllAchieveMarks(Std,Branch,Batch,MarksID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<MarksEntity>> GetAllCustomMarks(DataTableAjaxPostModel model, long Std, long courseid, long Branch, long Batch, long MarksID)
        {
            try
            {
                return await this._MarksContext.GetAllCustomMarks(model, Std,courseid, Branch, Batch, MarksID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<ResponseModel> UpdateMarksDetails(MarksEntity marksEntity)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response = await _MarksContext.UpdateMarksDetails(marksEntity);
                
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return response;
        }

        public async Task<List<MarksEntity>> GetAllStudentMarks(long BranchID, long StudentID)
        {
            try
            {
                return await this._MarksContext.GetAllStudentMarks(BranchID, StudentID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }
    }
}
