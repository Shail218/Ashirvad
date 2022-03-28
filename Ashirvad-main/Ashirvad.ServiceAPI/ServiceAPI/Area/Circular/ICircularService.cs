using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area.Circular
{
    public interface ICircularService
    {
        Task<ResponseModel> CircularMaintenance(CircularEntity branchInfo);        
        Task<List<CircularEntity>> GetAllCircular();
        Task<List<CircularEntity>> GetAllCustomCircular(DataTableAjaxPostModel model);
        ResponseModel RemoveCircular(long BranchID, string lastupdatedby);
        Task<CircularEntity> GetCircularById(long circularID);
    }
}
