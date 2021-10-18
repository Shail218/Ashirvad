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
                if (MarksID > 0)
                {
                    //Add User
                    //Get Marks
                    Marks.MarksID = MarksID;
                }
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return Marks;
        }

        public async Task<OperationResult<List<MarksEntity>>> GetAllMarksWithoutImage()
        {
            try
            {
                OperationResult<List<MarksEntity>> Marks = new OperationResult<List<MarksEntity>>();
                Marks.Data = await _MarksContext.GetAllMarksWithoutImage();
                Marks.Completed = true;
                return Marks;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
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

        Task<List<MarksEntity>> IMarksService.GetAllMarksWithoutImage()
        {
            throw new NotImplementedException();
        }
    }
}
