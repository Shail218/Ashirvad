﻿using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area.School
{
   public interface ISchoolService
    {
        Task<SchoolEntity> SchoolMaintenance(SchoolEntity schoolInfo);
        Task<List<SchoolEntity>> GetAllSchools(long branchID);
        Task<List<SchoolEntity>> GetAllSchools();
        Task<SchoolEntity> GetSchoolsByID(long schoolInfo);
        bool RemoveSchool(long SchoolID, string lastupdatedby);
    }
}
