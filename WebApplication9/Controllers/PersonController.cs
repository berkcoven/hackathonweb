using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication9.Models;


namespace WebApplication9.Controllers
{
    public class PersonController : ApiController
    {
        HackathlonDBEntities db = new HackathlonDBEntities();

        public IHttpActionResult GetPersons()
        {
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            GlobalConfiguration.Configuration.Formatters.Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);
            var persons = db.People.Select(x=> new { x.Name,x.PersonID});
            if (persons == null) { return NotFound(); }
            return Ok(persons);
        }
        [HttpGet]
        public IHttpActionResult PersonById(int id)
        {
            var person = db.People.Select(x=>new { x.PersonID,x.Name,x.GunlukHarcama}).Where(x => x.PersonID == id).FirstOrDefault();

            return Ok(person);
            
        }



        [System.Web.Http.HttpPost]
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public IHttpActionResult AddPerson(string name)
        {

            try
            {
                Person person = new Person();
                person.Name = name;
                person.GunlukHarcama = 0;
                db.People.Add(person);
                db.SaveChanges();
                

                return Ok("Kayıt Başarılıdır.");

            }
            catch(Exception ex)
            {
                return Ok(ex.Message);
            }
        }

    }// expenses update - post
}
