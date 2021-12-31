﻿using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.Repo.DataAcceessAPI.Area
{
    public interface ICategoryAPI
    {
        Task<long> CategoryMaintenance(CategoryEntity CategoryInfo);
        Task<List<CategoryEntity>> GetAllCategorys(long branchID);
        Task<List<CategoryEntity>> GetAllCategorys();
        Task<CategoryEntity> GetCategorysByID(long CategoryInfo);
        bool RemoveCategory(long CategoryID, string lastupdatedby);
        Task<List<CategoryEntity>> GetAllCustomCategory(DataTableAjaxPostModel model, long branchID);
    }
}
