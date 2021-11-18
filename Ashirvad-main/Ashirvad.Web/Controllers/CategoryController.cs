using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Web.Controllers
{
    public class CategoryController : BaseController
    {
        // GET: Category
        private readonly ICategoryService _CategoryService;
        public ResponseModel res = new ResponseModel();
        public CategoryController(ICategoryService CategoryService)
        {
            _CategoryService = CategoryService;
        }

        // GET: Category
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> CategoryMaintenance(long categoryID)
        {
            long CategoryID = categoryID;
            CategoryMaintenanceModel category = new CategoryMaintenanceModel();
            if (CategoryID > 0)
            {
                var result = await _CategoryService.GetCategorysByID(CategoryID);
                category.CategoryInfo = result;
            }

            var categoryData = await _CategoryService.GetAllCategorys(SessionContext.Instance.LoginUser.UserType == Enums.UserType.SuperAdmin ? 0 : SessionContext.Instance.LoginUser.BranchInfo.BranchID);
            category.CategoryData = categoryData;

            return View("Index", category);
        }

        public async Task<ActionResult> EditCategory(long CategoryID, long categoryID)
        {
            CategoryMaintenanceModel category = new CategoryMaintenanceModel();
            if (CategoryID > 0)
            {
                var result = await _CategoryService.GetCategorysByID(CategoryID);
                category.CategoryInfo = result;
            }

            if (categoryID > 0)
            {
                var result = await _CategoryService.GetAllCategorys(categoryID);
                category.CategoryData = result;
            }

            var categoryData = await _CategoryService.GetAllCategorys();
            category.CategoryData = categoryData;

            return View("Index", category);
        }

        [HttpPost]
        public async Task<JsonResult> SaveCategory(CategoryEntity categoryEntity)
        {

            categoryEntity.Transaction = GetTransactionData(categoryEntity.CategoryID > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
            categoryEntity.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            categoryEntity.BranchInfo = new BranchEntity()
            {
                BranchID = SessionContext.Instance.LoginUser.BranchInfo.BranchID
            };
            var data = await _CategoryService.CategoryMaintenance(categoryEntity);
            res.Status = data.CategoryID > 0 ? true : false;
            res.Message = data.CategoryID == -1 ? "Category Already exists!!" : data.CategoryID == 0 ? "Category failed to insert!!" : "Category Inserted Successfully!!";
            return Json(res);
        }

        [HttpPost]
        public JsonResult RemoveCategory(long categoryID)
        {
            var result = _CategoryService.RemoveCategory(categoryID, SessionContext.Instance.LoginUser.Username);
            return Json(result);
        }

        //public async Task<JsonResult> CategoryData()
        //{
        //    var categoryData = await _CategoryService.GetAllCategorys();
        //    return Json(categoryData);
        //}

        public async Task<JsonResult> CategoryData()
        {
            var categoryData = await _CategoryService.GetAllCategorys(0);
            return Json(categoryData);
        }
    }
}