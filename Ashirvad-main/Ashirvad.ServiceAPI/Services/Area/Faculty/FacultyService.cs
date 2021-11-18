using Ashirvad.Data;
using Ashirvad.Logger;
using Ashirvad.Repo.DataAcceessAPI.Area.Faculty;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Faculty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.Services.Area.Faculty
{
   public class FacultyService : IFacultyService
    {
        private readonly IFacultyAPI _facultyContext;
        public FacultyService(IFacultyAPI faculContext)
        {
            this._facultyContext = faculContext;
        }

        public async Task<OperationResult<List<FacultyEntity>>> GetAllFaculty(long branchID, int typeID)
        {
            OperationResult<List<FacultyEntity>> facul = new OperationResult<List<FacultyEntity>>();
            facul.Data = await _facultyContext.GetAllFaculty(branchID, typeID);
            facul.Completed = true;
            return facul;
        }
        public async Task<FacultyEntity> FacultyMaintenance(FacultyEntity faculInfo)
        {
            FacultyEntity notif = new FacultyEntity();
            try
            {
                long faculID = await _facultyContext.FacultyMaintenance(faculInfo);
                notif.FacultyID = faculID;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return notif;
        }

        public async Task<List<FacultyEntity>> GetAllFaculty(long branchID = 0)
        {
            try
            {
                List<FacultyEntity> banner = new List<FacultyEntity>();
                banner = await _facultyContext.GetAllFaculty(branchID);
                return banner;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<OperationResult<FacultyEntity>> GetFacultyByFacultyID(long facultyID)
        {
            try
            {
                OperationResult<FacultyEntity> banner = new OperationResult<FacultyEntity>();
                banner.Data = await _facultyContext.GetFacultyByFacultyID(facultyID);
                banner.Completed = true;
                return banner;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public bool RemoveFaculty(long faculID, string lastupdatedby)
        {
            try
            {
                return this._facultyContext.RemoveFaculty(faculID, lastupdatedby);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return false;
        }
    }
}
