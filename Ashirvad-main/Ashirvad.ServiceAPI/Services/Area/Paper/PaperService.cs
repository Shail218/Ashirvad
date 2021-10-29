﻿using Ashirvad.Data;
using Ashirvad.Logger;
using Ashirvad.Repo.DataAcceessAPI.Area.Paper;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Paper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.Services.Area.Paper
{
    public class PaperService : IPaperService
    {
        private readonly IPaperAPI _paperContext;
        public PaperService(IPaperAPI paperContext)
        {
            _paperContext = paperContext;
        }

        public async Task<PaperEntity> PaperMaintenance(PaperEntity paperInfo)
        {
            PaperEntity paper = new PaperEntity();
            try
            {               
                long paperID = await _paperContext.PaperMaintenance(paperInfo);
                if (paperID > 0)
                {
                    paper.PaperID = paperID;                   
                }
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return paper;
        }

        public async Task<OperationResult<List<SubjectEntity>>> GetPracticePaperSubject(long branchID, long stdID)
        {
            try
            {
                OperationResult<List<SubjectEntity>> paper = new OperationResult<List<SubjectEntity>>();
                paper.Data = await _paperContext.GetPracticePaperSubject(branchID, stdID);
                paper.Completed = true;
                return paper;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<OperationResult<List<PaperEntity>>> GetPracticePapersByStandardSubjectAndBranch(long branchID, long stdID, long subID, int batchTypeID)
        {
            try
            {
                OperationResult<List<PaperEntity>> paper = new OperationResult<List<PaperEntity>>();
                paper.Data = await _paperContext.GetPracticePapersByStandardSubjectAndBranch(branchID, stdID, subID, batchTypeID);
                paper.Completed = true;
                return paper;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<OperationResult<List<SubjectEntity>>> GetPracticePapersSubjectByStandardBatchAndBranch(long branchID, long stdID, int batchTypeID)
        {
            try
            {
                OperationResult<List<SubjectEntity>> paper = new OperationResult<List<SubjectEntity>>();
                var data = await _paperContext.GetPracticePapersByStandardSubjectAndBranch(branchID, stdID, 0, batchTypeID);
                List<SubjectEntity> sub = new List<SubjectEntity>();
                if (data?.Count > 0)
                {
                    sub = data.Select(x =>
                        x.Subject).ToList();
                }

                paper.Data = sub;
                paper.Completed = true;
                return paper;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<OperationResult<List<PaperEntity>>> GetAllPaperWithoutContent(long branchID = 0)
        {
            try
            {
                OperationResult<List<PaperEntity>> paper = new OperationResult<List<PaperEntity>>();
                paper.Data = await _paperContext.GetAllPaperWithoutContent(branchID);
                paper.Completed = true;
                return paper;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<OperationResult<PaperEntity>> GetPaperByPaperID(long paperID)
        {
            try
            {
                OperationResult<PaperEntity> lib = new OperationResult<PaperEntity>();
                lib.Data = await _paperContext.GetPaperByPaperID(paperID);
                lib.Completed = true;
                return lib;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<PaperEntity>> GetAllPaper(long branchID = 0)
        {
            try
            {
                return await this._paperContext.GetAllPapers(branchID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public bool RemovePaper(long paperID, string lastupdatedby)
        {
            try
            {
                return this._paperContext.RemovePaper(paperID, lastupdatedby);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return false;
        }
    }
}
