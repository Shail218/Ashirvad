using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data
{
    public class ResponseModel
    {
        public string Message { get; set; }
        public bool Status { get; set; }
        public bool IsEdit { get; set; }
        public string Permission { get; set; }
        public string URL { get; set; }
        public object Data { get; set; }
        public string Overall { get; set; }
      
    }
    public class CommonResponseModel<T>:ResponseModel
    {
        public string Message { get; set; }
        public bool Status { get; set; }
        public T Data { get; set; }
        public override string ToString()
        {
            return Environment.NewLine + typeof(ResponseModel).Name + "<" + typeof(T).Name + ">:" + Environment.NewLine + Data + Environment.NewLine + base.ToString();
        }
    }
}
