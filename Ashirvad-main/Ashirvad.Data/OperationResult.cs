using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Enums;

namespace Ashirvad.Data
{
    public class OperationResult
    {
        public OperationResult()
        {
            this.Message = new ResultMessageCollection();
            this.Completed = false;
        }
        public bool Completed { get; set; }
        public ResultMessageCollection Message { get; set; }
        public void AddMessage(OperationResult op)
        {
            if ((null != op) && (null != op.Message))
            {
                foreach (var msg in op.Message)
                {
                    if (null != op)
                    {
                        Message.Add(msg);
                    }
                }
            }
        }
    }

    public class ResultMessageCollection : Collection<ResultMessage>
    {

    }

    public class ResultMessage
    {
        public ResultMessage(ResultMessageLevel level, string message, string parameterName, string messageType, Exception ex)
        {
            if (ex != null)
            {
                this.Exception = ex;
            }

            if (string.IsNullOrWhiteSpace(messageType))
            {
                if (ex != null)
                {
                    this.Message = ex.Message;
                }
                else
                {
                    throw new ArgumentNullException("message");
                }
            }
            else
            {
                this.Message = message;
            }
            this.Level = level;
            this.ParameterName = parameterName;
            if (!string.IsNullOrWhiteSpace(messageType))
            {
                this.MessageType = messageType;
            }
            else
            {
                this.MessageType = ex != null ? ex.GetType().FullName : string.Empty;
            }
        }
        public string Message { get; set; }
        public Exception Exception { get; set; }
        public string ParameterName { get; set; }
        public ResultMessageLevel Level { get; set; }
        public string MessageType { get; set; }
    }

    public class OperationResult<T> : OperationResult
    {
        public T Data { get; set; }
        public string Message { get; set; }
        public override string ToString()
        {
            return Environment.NewLine + typeof(OperationResult).Name + "<" + typeof(T).Name + ">:" + Environment.NewLine + Data + Environment.NewLine + base.ToString();
        }
    }
}
