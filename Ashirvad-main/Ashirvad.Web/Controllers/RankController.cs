using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Web.Controllers
{
    public class RankController : BaseController
    {
        // GET: Rank
        public ActionResult Index()
        {
            List<TestEntity> testEntity = new List<TestEntity>();
            return View(testEntity);
        }
    }
}