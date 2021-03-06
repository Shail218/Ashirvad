using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ashirvad.Web
{
    public sealed class SessionContext
    {
        private static volatile SessionContext instance;
        private static object syncRoot = new Object();

        const string CURRENTSESSION = "__CurrentSession__";
        private SessionContext()
        {
            //Set defaults
            SessionID = Guid.NewGuid().ToString();
            PageSize = 20;
        }

        public static SessionContext Instance
        {
            get
            {
                instance = (SessionContext)HttpContext.Current.Session[CURRENTSESSION];
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new SessionContext();
                            HttpContext.Current.Session[CURRENTSESSION] = instance;
                        }
                    }
                }
                return instance;
            }
        }

        public string SessionID { get; set; }

        public int PageSize { get; set; }
        public string userRightsList { get; set; }
        public string userRightList { get; set; }
        public UserEntity LoginUser { get; set; }
        public string[] Permission { get; set; }
    }
}