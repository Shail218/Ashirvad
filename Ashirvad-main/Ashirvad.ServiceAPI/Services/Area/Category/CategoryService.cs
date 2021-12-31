﻿using Ashirvad.Data;
using Ashirvad.Logger;
using Ashirvad.Repo.DataAcceessAPI.Area;
using Ashirvad.ServiceAPI.ServiceAPI.Area;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.ServiceAPI.Services.Area
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryAPI _CategoryContext;
        public CategoryService(ICategoryAPI CategoryContext)
        {
            this._CategoryContext = CategoryContext;
        }
        public async Task<CategoryEntity> CategoryMaintenance(CategoryEntity CategoryInfo)
        {
            CategoryEntity Category = new CategoryEntity();
            try
            {
                long CategoryID = await _CategoryContext.CategoryMaintenance(CategoryInfo);
                Category.CategoryID = CategoryID;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return Category;
        }

        public async Task<List<CategoryEntity>> GetAllCategorys(long branchID)
        {
            try
            {
                return await this._CategoryContext.GetAllCategorys(branchID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<CategoryEntity>> GetAllCustomCategory(DataTableAjaxPostModel model, long branchID)
        {
            try
            {
                return await this._CategoryContext.GetAllCustomCategory(model, branchID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<CategoryEntity>> GetAllCategorys()
        {
            try
            {
                return await this._CategoryContext.GetAllCategorys();
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public bool RemoveCategory(long CategoryID, string lastupdatedby)
        {
            try
            {
                return this._CategoryContext.RemoveCategory(CategoryID, lastupdatedby);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return false;
        }

        public async Task<CategoryEntity> GetCategorysByID(long CategoryInfo)
        {
            CategoryEntity Category = new CategoryEntity();
            try
            {
                Category = await _CategoryContext.GetCategorysByID(CategoryInfo);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }
            return Category;
        }
    }
}
