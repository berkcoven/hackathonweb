using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication9.Models;
using System.Data.Entity;

namespace WebApplication9.Controllers
{
    public class GroupController : ApiController
    {
        HackathlonDBEntities db = new HackathlonDBEntities();
        // GET: api/Group
        public IEnumerable<Group> Get()
        {
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            GlobalConfiguration.Configuration.Formatters.Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);
            return db.Groups.ToList();
        }

        public IHttpActionResult GetGroupById(int id)
        {
            var group = db.Groups.Select(x=>new { x.GroupName,x.GroupID}).Where(x => x.GroupID == id).FirstOrDefault();
            return Ok(group) ;
        }


        [HttpPost]
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public IHttpActionResult AddGroup(string name)
        {
            try {
                Group group = new Group();
                group.GroupName = name;
                db.Groups.Add(group);
                db.SaveChanges();
                return Ok("Başarılı");

            }
            catch(Exception ex) { return Ok(ex.Message); }
        }

        // PUT: api/Group/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Group/5
        public void Delete(int id)
        {
        }
    }
}
