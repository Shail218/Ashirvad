using Ashirvad.Data;
using Ashirvad.Logger;
using Ashirvad.Repo.DataAcceessAPI.Area;
using Ashirvad.ServiceAPI.ServiceAPI.Area;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.Services.Area
{
    public class MarksService : IMarksService
    {
        private readonly IMarksAPI _MarksContext;
        public MarksService(IMarksAPI MarksContext)
        {
            this._MarksContext = MarksContext;
        }

        public async Task<MarksEntity> MarksMaintenance(MarksEntity MarksInfo)
        {
            MarksEntity Marks = new MarksEntity();
            try
            {
                long MarksID = await _MarksContext.MarksMaintenance(MarksInfo);
                Marks.MarksID = MarksID;
            }
            catch (Exception ex)
                {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return Marks;
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

        public bool RemoveMarks(long MarksID, string lastupdatedby)
        {
            try
            {
                return this._MarksContext.RemoveMarks(MarksID, lastupdatedby);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return false;
        }


        public async Task<List<MarksEntity>> GetAllAchieveMarks(long Std, long Branch, long Batch, long MarksID,DateTime TestDate)
        {
            try
            {
                return await this._MarksContext.GetAllAchieveMarks(Std,Branch,Batch,MarksID,TestDate);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<MarksEntity> UpdateMarksDetails(MarksEntity marksEntity)
        {
            MarksEntity marks = new MarksEntity();
            try
            {
                var data = await _MarksContext.UpdateMarksDetails(marksEntity);
                marks.MarksID = data;
                return marks;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return marks;
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
