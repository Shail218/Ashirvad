using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area
{
    public interface IFeesService
    {
        Task<FeesEntity> FeesMaintenance(FeesEntity FeesInfo);
        Task<List<FeesEntity>> GetAllFees();

        Task<List<FeesEntity>> GetAllFeesWithoutImage();
        Task<FeesEntity> GetFeesByFeesID(long FeesID);
        bool RemoveFees(long FeesID, string lastupdatedby);
        
    }
}
