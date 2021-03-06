using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ashirvad.Data;
using static Ashirvad.Common.Common;

namespace Ashirvad.Repo.DataAcceessAPI.Area.Circular
{
    public interface ICircularAPI
    {
        Task<ResponseModel> CircularMaintenance(CircularEntity circularInfo);
        Task<List<CircularEntity>> GetAllCircular();
        ResponseModel RemoveCircular(long circularId, string lastupdatedby);
        Task<List<CircularEntity>> GetAllCustomCircular(DataTableAjaxPostModel model);
        Task<CircularEntity> GetCircularById(long circularID);
    }
}
