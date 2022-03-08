using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.Logger;
using Ashirvad.Repo.DataAcceessAPI.Area.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area.Charts
{
    public class ChartService : IChartService
    {
        private readonly IChartAPI _studentContext;
        public ChartService(IChartAPI studentContext)
        {
            this._studentContext = studentContext;
        }

        public async Task<List<StudentEntity>> GetAllStudentsName(long branchID, string financialyear)
        {
            try
            {
                return await this._studentContext.GetAllStudentsName(branchID,financialyear);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<StudentEntity>> GetAllStudentsNameByStandard(long StdID, long courseid, string financialyear)
        {
            try
            {
                return await this._studentContext.GetAllStudentsNameByStandard(StdID,courseid,financialyear);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<StandardEntity>> GetAllClassDDL(long branchID, string financialyear)
        {
            try
            {
                return await this._studentContext.GetAllClassDDL(branchID,financialyear);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<StudentEntity> GetStudentContentByID(long studentID)
        {
            try
            {
                return await this._studentContext.GetStudentContentByID(studentID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<StudentEntity>> GetStudentContent(long stdID, long branchID, long batchID, string financialyear)
        {
            try
            {
                return await this._studentContext.GetStudentContent(stdID, branchID, batchID,financialyear);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<BranchStandardEntity>> AllBranchStandardWithCountByBranch(long branchid, string financialyear)
        {
            try
            {
                return await this._studentContext.AllBranchStandardWithCountByBranch(branchid,financialyear);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<AttendanceEntity>> GetStudentAttendanceDetails(long studentID, long type)
        {
            try
            {
                return await this._studentContext.GetStudentAttendanceDetails(studentID, type);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

    }
}
