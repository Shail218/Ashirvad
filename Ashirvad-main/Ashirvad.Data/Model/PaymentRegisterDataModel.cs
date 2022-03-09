using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data.Model
{
  public class PaymentRegisterDataModel
    {
        public List<PaymentRegisterEntity> registerEntities { get; set; } = new List<PaymentRegisterEntity>();
        public PaymentRegisterEntity RegisterEntity { get; set; } = new PaymentRegisterEntity();
        public List<PaymentEntity> PaymentEntities { get; set; } = new List<PaymentEntity>();
        public PaymentEntity paymentEntity { get; set; } = new PaymentEntity();

    }
}
