﻿using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area
{
    public interface IMarksService
    {
        Task<MarksEntity> MarksMaintenance(MarksEntity MarksInfo);
        Task<List<MarksEntity>> GetAllMarks();
        Task<List<MarksEntity>> GetAllAchieveMarks(long Std, long Branch, long Batch, long MarksID,DateTime TestDate);
        Task<MarksEntity> GetMarksByMarksID(long MarksID);
        bool RemoveMarks(long MarksID, string lastupdatedby);
        Task<MarksEntity> UpdateMarksDetails(MarksEntity marksEntity);
        Task<List<MarksEntity>> GetAllStudentMarks(long BranchID, long StudentID);
    }
}
