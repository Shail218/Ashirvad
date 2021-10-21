using Ashirvad.Data;
using Ashirvad.Logger;
using Ashirvad.Repo.DataAcceessAPI.Area.AboutUs;
using Ashirvad.ServiceAPI.ServiceAPI.Area.AboutUs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.Services.Area.AboutUs
{
    public class AboutUsService : IAboutUsService
    {
        private readonly IAboutUs _aboutusContext;
        public AboutUsService(IAboutUs aboutusContext)
        {
            this._aboutusContext = aboutusContext;
        }

        public async Task<AboutUsEntity> AboutUsMaintenance(AboutUsEntity aboutUsInfo)
        {
            AboutUsEntity aboutus = new AboutUsEntity();
            try
            {
                //if (aboutUsInfo.FileInfo != null)
                //{
                //    aboutUsInfo.HeaderImage = Common.Common.ReadFully(aboutUsInfo.FileInfo.InputStream);
                //    aboutUsInfo.HeaderImageName = Path.GetFileName(aboutUsInfo.FileInfo.FileName);
                //}
                //else
                //{
                //    aboutUsInfo.HeaderImage = Convert.FromBase64String(aboutUsInfo.HeaderImageText);
                //}

                long uniqueID = await _aboutusContext.AboutUsMaintenance(aboutUsInfo);
                aboutus.AboutUsID = uniqueID;
                //if (uniqueID > 0)
                //{
                //    if (!string.IsNullOrEmpty(Common.Common.GetStringConfigKey("DocDirectory")))
                //    {
                //        Common.Common.SaveFile(aboutUsInfo.HeaderImage, aboutUsInfo.HeaderImageName, "AboutUs\\");
                //    }

                //    aboutus.AboutUsID = uniqueID;
                //}
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }
            
            return aboutus;
        }

        public async Task<OperationResult<List<AboutUsEntity>>> GetAllAboutUsWithoutContent(long branchID = 0)
        {
            try
            {
                OperationResult<List<AboutUsEntity>> banner = new OperationResult<List<AboutUsEntity>>();
                banner.Data = await _aboutusContext.GetAllAboutUsWithoutContent(branchID);
                banner.Completed = true;
                return banner;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<OperationResult<AboutUsEntity>> GetAboutUsByUniqueID(long uniqueID,long BranchID=0)
        {
            try
            {
                OperationResult<AboutUsEntity> banner = new OperationResult<AboutUsEntity>();
                banner.Data = await _aboutusContext.GetAboutUsByUniqueID(uniqueID, BranchID);
                banner.Completed = true;
                return banner;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<AboutUsDetailEntity>> GetAllAboutUs(long branchID = 0)
        {
            try
            {
                return await this._aboutusContext.GetAllAboutUs(branchID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public bool RemoveAboutUs(long aboutUsID, string lastupdatedby, bool remomveAboutUsDetail = false)
        {
            try
            {
                return this._aboutusContext.RemoveAboutUs(aboutUsID, lastupdatedby, remomveAboutUsDetail);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return false;
        }


        public async Task<AboutUsDetailEntity> AboutUsDetailMaintenance(AboutUsDetailEntity aboutUsInfo)
        {
            AboutUsDetailEntity aboutus = new AboutUsDetailEntity();
            try
            {
                //if (aboutUsInfo.FileInfo != null)
                //{
                //    aboutUsInfo.HeaderImage = Common.Common.ReadFully(aboutUsInfo.FileInfo.InputStream);
                //}
                //else
                //{
                //    aboutUsInfo.HeaderImage = Convert.FromBase64String(aboutUsInfo.HeaderImageText);
                //}

                long uniqueID = await _aboutusContext.AboutUsDetailMaintenance(aboutUsInfo);
                aboutus.DetailID = uniqueID;
                //if (uniqueID > 0)
                //{
                //    if (!string.IsNullOrEmpty(Common.Common.GetStringConfigKey("DocDirectory")))
                //    {
                //        Common.Common.SaveFile(aboutUsInfo.HeaderImage, uniqueID.ToString(), "AboutUsDetail\\");
                //    }

                //    aboutus.DetailID = uniqueID;
                //}
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }
           
            return aboutus;
        }

        public async Task<OperationResult<List<AboutUsDetailEntity>>> GetAllAboutUsDetailWithoutContent(long aboutusID = 0, long branchID = 0)
        {
            try
            {
                OperationResult<List<AboutUsDetailEntity>> banner = new OperationResult<List<AboutUsDetailEntity>>();
                banner.Data = await _aboutusContext.GetAllAboutUsDetailWithoutContent(aboutusID, branchID);
                banner.Completed = true;
                return banner;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<OperationResult<AboutUsDetailEntity>> GetAboutUsDetailByUniqueID(long uniqueID)
        {
            try
            {
                OperationResult<AboutUsDetailEntity> banner = new OperationResult<AboutUsDetailEntity>();
                banner.Data = await _aboutusContext.GetAboutUsDetailByUniqueID(uniqueID);
                banner.Completed = true;
                return banner;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<AboutUsDetailEntity>> GetAllAboutUsDetail(long aboutusID = 0, long branchID = 0)
        {
            try
            {
                return await this._aboutusContext.GetAllAboutUsDetails(aboutusID, branchID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public bool RemoveAboutUsDetail(long aboutUsID, string lastupdatedby)
        {
            try
            {
                return this._aboutusContext.RemoveAboutUsDetail(aboutUsID, lastupdatedby);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return false;
        }

    }
}
