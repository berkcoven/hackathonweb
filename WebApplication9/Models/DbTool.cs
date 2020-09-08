using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication9.Models
{
    public class DbTool
    {

        private static HackathlonDBEntities _dbInstance;

        public static HackathlonDBEntities DbInstance
        {
            get
            {
                if (_dbInstance == null)
                {
                    _dbInstance = new HackathlonDBEntities();
                }
                return _dbInstance;
            }
        }
    }
}