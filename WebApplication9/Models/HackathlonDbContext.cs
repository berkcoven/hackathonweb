using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebApplication9.Models
{
    public class HackathlonDbContext:DbContext
    {

        public HackathlonDbContext() { Database.Connection.ConnectionString = "server=DESKTOP-UM9QASS\\SQLEXPRESS1;database=HackathlonDB;uid=sa;password=123"; }
    }

}