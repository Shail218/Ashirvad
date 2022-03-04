﻿using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.DataAcceessAPI.Area.PaymentRegister
{
    public interface IPaymentRegisterAPI
    {
        Task<long> PaymentRegisterMaintenance(PaymentRegisterEntity entity);
        Task<List<PaymentRegisterEntity>> GetPaymentListForStudent(long studentID, long branchID);
    }
}