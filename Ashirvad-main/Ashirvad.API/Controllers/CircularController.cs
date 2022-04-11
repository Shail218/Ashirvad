using Ashirvad.API.Filter;
using Ashirvad.Data;
using Ashirvad.Repo.Services.Area.Circular;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Circular;
using Ashirvad.ServiceAPI.Services.Area.Circular;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Ashirvad.API.Controllers
{
    [RoutePrefix("api/circular/v1")]
    [AshirvadAuthorization]
    public class CircularController : ApiController
    {
        private readonly ICircularService _circularService;
        public CircularController(ICircularService circularService)
        {
            _circularService = circularService;
        }

        public CircularController()
        {
            _circularService = new CircularService(new Circular());
        }

        [Route("CircularMaintenance")]
        [HttpPost]
        public OperationResult<CircularEntity> CircularMaintenance(CircularEntity circularEntity)
        {
            var data = _circularService.CircularMaintenance(circularEntity);
            OperationResult<CircularEntity> result = new OperationResult<CircularEntity>();
            result.Completed = data.Result.Status;
            if (data.Result.Status && data.Result.Data != null)
            {
                result.Data = (CircularEntity)data.Result.Data;
            }
            result.Message = data.Result.Message;
            return result;
        }

        [Route("RemoveCircular")]
        [HttpPost]
        public OperationResult<bool> RemoveCircular(long circularID, string lastupdatedby)
        {
            var data = _circularService.RemoveCircular(circularID, lastupdatedby);
            OperationResult<bool> result = new OperationResult<bool>();
            result.Completed = data.Status;
            result.Data = data.Status;
            result.Message = data.Message;
            return result;
        }

        [Route("GetAllCircular")]
        [HttpGet]
        public OperationResult<List<CircularEntity>> GetAllCircular()
        {
            var data = _circularService.GetAllCircular();
            OperationResult<List<CircularEntity>> result = new OperationResult<List<CircularEntity>>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

    }
}
