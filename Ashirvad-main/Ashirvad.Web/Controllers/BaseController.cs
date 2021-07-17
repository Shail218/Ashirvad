using Ashirvad.Common;
using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Web.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (SessionContext.Instance.LoginUser == null)
            {
                filterContext.Result = new RedirectResult("~/Login/Index");
            }
        }

        public TransactionEntity GetTransactionData(Enums.TransactionType transType)
        {
            if (transType == Enums.TransactionType.Insert)
            {
                return new TransactionEntity()
                {
                    CreatedBy = SessionContext.Instance.LoginUser.Username,
                    CreatedDate = DateTime.Now,
                    CreatedId = SessionContext.Instance.LoginUser.UserID
                };
            }
            else
            {
                return new TransactionEntity()
                {
                    LastUpdateBy = SessionContext.Instance.LoginUser.Username,
                    LastUpdateDate = DateTime.Now,
                    LastUpdateId = SessionContext.Instance.LoginUser.UserID
                };
            }
        }
    }
}