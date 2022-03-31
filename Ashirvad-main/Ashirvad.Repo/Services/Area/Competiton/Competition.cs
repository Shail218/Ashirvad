using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.Repo.DataAcceessAPI.Area.Competition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.Services.Area.Competiton
{
    public class Competition : ModelAccess, ICompetitonAPI
    {
        public async Task<long> CheckCompetitonExist(string CompetitionName, long CompetitionID)
        {
            long result;
            bool isExists = this.context.COMPETITION_MASTER.Where(s => (CompetitionID == 0 || s.competition_id != CompetitionID) && s.competition_name.ToLower() == CompetitionName.ToLower() && s.row_sta_cd == 1).FirstOrDefault() != null;
            result = isExists == true ? -1 : 1;
            return result;
        }

        public async Task<ResponseModel> CompetitionMaintenance(CompetitionEntity CompetitonInfo)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                Model.COMPETITION_MASTER competitonmaster = new Model.COMPETITION_MASTER();
                if (CheckCompetitonExist(CompetitonInfo.CompetitionName,CompetitonInfo.CompetitionID).Result != -1)
                {
                    bool isUpdate = true;
                    var data = (from t in this.context.COMPETITION_MASTER
                                where t.competition_id == CompetitonInfo.CompetitionID
                                select t).FirstOrDefault();
                    if (data == null)
                    {
                        data = new Model.COMPETITION_MASTER();
                        isUpdate = false;
                    }
                    else
                    {
                        competitonmaster = data;
                        CompetitonInfo.Transaction.TransactionId = data.trans_id;
                    }
                    competitonmaster.competition_name = CompetitonInfo.CompetitionName;
                    competitonmaster.competition_id = CompetitonInfo.CompetitionID;
                    competitonmaster.competition_st_time = CompetitonInfo.CompetitionStartTime;
                    competitonmaster.competition_end_time = CompetitonInfo.CompetitionEndTime;
                    //competitonmaster.COMPETITION_MASTER_DTL=CompetitonInfo.
                    competitonmaster.trans_id = CompetitonInfo.Transaction.TransactionId;
                    competitonmaster.remarks = CompetitonInfo.Remarks;
                    competitonmaster.file_name = CompetitonInfo.FileName;                    
                    this.context.COMPETITION_MASTER.Add(competitonmaster);
                    if (isUpdate)
                    {
                        this.context.Entry(competitonmaster).State = System.Data.Entity.EntityState.Modified;
                    }
                    var da = this.context.SaveChanges() > 0 || competitonmaster.competition_id > 0;
                    if (da)
                    {
                        CompetitonInfo.CompetitionID = competitonmaster.competition_id;
                        responseModel.Message = isUpdate == true ? "Competiton Updated." : "Competiton Inserted Successfully";
                        responseModel.Status = true;
                        responseModel.Data = CompetitonInfo;
                    }
                    else
                    {
                        responseModel.Message = isUpdate == true ? "Competiton Not Updated." : "Competiton Not Inserted Successfully";
                        responseModel.Status = false;
                    }
                }
                else
                {
                    responseModel.Status = false;
                    responseModel.Message = "Competiton Already Exist.";
                }
            }
            catch (Exception ex)
            {
                responseModel.Message = ex.Message.ToString();
                responseModel.Status = false;
            }
            return responseModel;
        }

        public Task<ResponseModel> DeleteCompetition(long CompetitionID)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CompetitionEntity>> GetAllCompetiton()
        {
            var data = (from u in this.context.COMPETITION_MASTER
                        orderby u.competition_id descending
                        select new CompetitionEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd
                            },
                            CompetitionName = u.competition_name,
                            CompetitionStartTime = u.competition_st_time,
                            CompetitionEndTime = u.competition_end_time,
                            CompetitionID = u.competition_id,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();
            return data;
        }

        public Task<CompetitionEntity> GetCompetitionByID(int Competiton)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel> SaveCompetition(CompetitionEntity competition, string Username, bool IsUpdate)
        {
            throw new NotImplementedException();
        }
    }
}
