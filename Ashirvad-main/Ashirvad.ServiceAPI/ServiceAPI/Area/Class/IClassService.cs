﻿using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area.Class
{
    public interface IClassService
    {
        Task<ClassEntity> ClassMaintenance(ClassEntity classEntity);
        Task<OperationResult<ClassEntity>> GetClassByClassID(long classID);
        Task<OperationResult<List<ClassEntity>>> GetAllClass();
        bool RemoveClass(long classID, string lastupdatedby);
    }
}
