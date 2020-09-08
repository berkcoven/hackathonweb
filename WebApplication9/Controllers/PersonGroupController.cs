using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication9.Models;

namespace WebApplication9.Controllers
{
    public class PersonGroupController : ApiController
    {
        HackathlonDBEntities db = new HackathlonDBEntities();
        [HttpPost]
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public IHttpActionResult AddPersonToGroup(int personid,int groupid)
        {

            try
            {
                Person person = db.People.Where(x => x.PersonID == personid).FirstOrDefault();
                Group group = db.Groups.Where(x => x.GroupID == groupid).FirstOrDefault();
                GroupPerson gp = new GroupPerson();
                gp.PersonId = person.PersonID;
                gp.GroupId = group.GroupID;
                db.GroupPersons.Add(gp);
                db.SaveChanges();
                return Ok(1);
            }
            catch
            {
                return Ok(0);
            }

            
        }
    }
}
