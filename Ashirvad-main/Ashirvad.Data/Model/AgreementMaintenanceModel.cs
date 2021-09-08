using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data.Model
{
    public class AgreementMaintenanceModel
    {
        public BranchAgreementEntity AgreementInfo { get; set; }
        public List<BranchAgreementEntity> AgreementData { get; set; }
    }
}
