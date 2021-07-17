﻿using Ashirvad.Data;
using Ashirvad.Logger;
using Ashirvad.Repo.DataAcceessAPI.Area.School;
using Ashirvad.ServiceAPI.ServiceAPI.Area.School;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.Services.Area.School
{
    public class SchoolService : ISchoolService
    {
        private readonly ISchoolAPI _schoolContext;
        public SchoolService(ISchoolAPI schoolContext)
        {
            this._schoolContext = schoolContext;
        }
        public async Task<SchoolEntity> SchoolMaintenance(SchoolEntity schoolInfo)
        {
            SchoolEntity school = new SchoolEntity();
            try
            {
                long schoolID = await _schoolContext.SchoolMaintenance(schoolInfo);
                if (schoolID > 0)
                {
                    school.SchoolID = schoolID;

                }
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return school;
        }

        public async Task<List<SchoolEntity>> GetAllSchools(long branchID)
        {
            try
            {
                return await this._schoolContext.GetAllSchools(branchID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<SchoolEntity>> GetAllSchools()
        {
            try
            {
                return await this._schoolContext.GetAllSchools();
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public bool RemoveSchool(long SchoolID, string lastupdatedby)
        {
            try
            {
                return this._schoolContext.RemoveSchool(SchoolID, lastupdatedby);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return false;
        }

        public async Task<SchoolEntity> GetSchoolsByID(long schoolInfo)
        {
            SchoolEntity school = new SchoolEntity();
            try
            {
                school = await _schoolContext.GetSchoolsByID(schoolInfo);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }
            return school;
        }
    }
}
