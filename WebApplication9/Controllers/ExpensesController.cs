using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication9.Models;

namespace WebApplication9.Controllers
{
    public class ExpensesController : ApiController
    {
        private HackathlonDBEntities db;
        
        public ExpensesController()
        {
            db = DbTool.DbInstance;
        }

        [HttpPost]
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public IHttpActionResult AddExpenses(int odeyenId,int personId,float value,string expensesName,int quantity,int groupId)
        {

            try
            {
                Expens expens = new Expens();
                expens.ExpenesesName = expensesName;
                expens.ExpensesQuantity = quantity;
                expens.GroupId = groupId;
                expens.OdeyenId = odeyenId;
                expens.PersonId = personId;
                expens.ExpensesValue = value;
                db.Expenses.Add(expens);

                if (odeyenId == personId)
                {
                    db.SaveChanges();
                    return Ok(1);
                }

                Person person = db.People.Where(x => x.PersonID == personId).FirstOrDefault();
                person.GunlukHarcama += quantity * value;
                Person odeyen = db.People.Where(x => x.PersonID == odeyenId).FirstOrDefault();
                odeyen.GunlukHarcama += (-1) * quantity * value;


                db.SaveChanges();
                return Ok(1);

            }
            catch (Exception ex) { return Ok(ex.Message); }

        }
        [HttpGet]
        public IHttpActionResult ExpensesById(int expId)
        {
            var expens = db.Expenses.Select(x=>new { x.ExpenesesName,x.ExpensesQuantity,x.ExpensesValue,x.OdeyenId,x.PersonId,x.GroupId,x.ExpensesId}).Where(x => x.ExpensesId == expId).FirstOrDefault();
            
            return Ok(expens);
        }

        [HttpGet]
        public IHttpActionResult CalculateByOne(int groupId)
        {



            List<Expens> expenss = db.Expenses.Where(x => x.GroupId == groupId).ToList();
            List<int?> gp = db.GroupPersons.Where(x => x.GroupId == groupId).Select(x => x.PersonId).ToList();
            List<double?> amountMoney = new List<double?>();
            List<Person> people = new List<Person>();
            
            foreach (var item in gp)
            {
                people.Add(db.People.Where(x => x.PersonID == item).FirstOrDefault());
            }
            var listofPeople = people.Select(x => new { x.Name, x.GunlukHarcama }).OrderByDescending(x => x.GunlukHarcama).ToList();
           
            double? totalAmount=0;
            List<string> los = new List<string>();
            foreach (var item in listofPeople)
            {

                if (item.GunlukHarcama > 0)
                {
                    if (item != listofPeople[0]) {
                        totalAmount += item.GunlukHarcama;
                        string borcuolanlar = item.Name + " Adlı kişi " + item.GunlukHarcama + "tl" + listofPeople[0].Name + " Adlı kişiye ödeyecektir";
                        los.Add(borcuolanlar);
                        
                    }
                    
                    
                }
                if (item.GunlukHarcama < 0)
                {
                    string paraAlacalar = item.Name + " Adlı kişi "+ listofPeople[0].Name+ " adlı kişiden  " + item.GunlukHarcama * (-1) + " tl para alacak";
                    los.Add(paraAlacalar);
                }

            }
            
            return Ok(los);

        }


        [HttpGet]
        public IHttpActionResult CalculateExpenses(int groupId)
        {



            
            List<int?> gp = db.GroupPersons.Where(x => x.GroupId == groupId).Select(x => x.PersonId).ToList();
            
            //foreach (var item in gp)
            //{
            //    foreach (var item in expenss.Where(x=>x.PersonId==item.PersonId).ToList())
            //    {

            //    }
            //}
            List<Person> people = new List<Person>();
            foreach (var item in gp)
            {
                people.Add(db.People.Where(x => x.PersonID == item).FirstOrDefault());
            }
            string sonuc;
            string finalsonuc;
            int negativeCount=0;
            int positiveCount=0;
            List<string> odemelist = new List<string>();
            var a = people.Select(x => new { x.Name, x.GunlukHarcama }).OrderByDescending(x => x.GunlukHarcama).Where(x=>x.GunlukHarcama!=0).ToList();
            var b = people.Select(x => new { x.Name, x.GunlukHarcama }).OrderByDescending(x => x.GunlukHarcama).Where(x => x.GunlukHarcama != 0).ToList();
            int esitSayi = 0;
          

           
            for (int i = 0; i <= a.Count-1; i++)
            {
                for (int y = i+1; y <= a.Count-1; y++)
                {
                    if (a[i].GunlukHarcama == a[y].GunlukHarcama*(-1))
                    {
                        sonuc = a[i].Name + " Adlı kişi " + a[y].Name + " Adlı kişiye " + a[y].GunlukHarcama + " tutarı ödeyecek";
                        odemelist.Add(sonuc);
                        b.Remove(a[i]);
                        b.Remove(a[y]);
                        
                        esitSayi += 2;
                        if (esitSayi == a.Count)
                        {
                            //para eşit bölünürse.
                            return Ok(odemelist);
                        }
                        
                        
                        
                    }
                    
                    
                }

            }

            foreach (var item in a)
            {
                if (item.GunlukHarcama < 0)
                {
                    negativeCount++;
                    sonuc = item.Name + " adlı kişi " + item.GunlukHarcama + "tutarında ödeme alacaktır.";
                    odemelist.Add(sonuc);
                }
                else if (item.GunlukHarcama > 0)
                {
                    positiveCount++;
                    sonuc = item.Name + " adlı kişi " + item.GunlukHarcama + "tutarında ödeme yapacaktır.";
                    odemelist.Add(sonuc);
                }

            }





            if (negativeCount == 1)
            {
                var tekEskiKisi = b.Select(x => new { x.Name, x.GunlukHarcama }).Where(x => x.GunlukHarcama < 0).FirstOrDefault();
                finalsonuc = "Kalan Herkes borcunu " + tekEskiKisi.Name + " Adlı kişiye verecek";
                odemelist.Add(finalsonuc);
            }
            else if (positiveCount == 1)
            {
                var tekArtiKisi = b.Select(x => new { x.Name, x.GunlukHarcama }).Where(x => x.GunlukHarcama > 0).FirstOrDefault();
                finalsonuc = "Kalan Herkes borcunu " + tekArtiKisi.Name + " Adlı kişiden alacak";
                odemelist.Add(finalsonuc);
            }
            else
            {

                if (b[0].GunlukHarcama > (b[b.Count - 1].GunlukHarcama * (-1)))
                {
                    finalsonuc = b[0].Name + " isimli kullanıcı" + b[b.Count - 1].Name + " isimli kullanıcıya " + (b[b.Count - 1].GunlukHarcama * (-1)) + " tl ödeyecek";
                    
                    odemelist.Add(finalsonuc);
                }
                else if (b[0].GunlukHarcama < (b[b.Count - 1].GunlukHarcama * (-1)))
                {
                    finalsonuc = b[0].Name + " isimli kullanıcı" + b[b.Count - 1].Name + " isimli kullanıcıya " + (b[0].GunlukHarcama) + " tl ödeyecek";
                    ;
                    odemelist.Add(finalsonuc);
                }
                else if ((b[0].GunlukHarcama == (b[b.Count - 1].GunlukHarcama * (-1))))
                {
                    finalsonuc = b[0].Name + " isimli kullanıcı" + b[b.Count - 1].Name + " isimli kullanıcıya " + (b[0].GunlukHarcama) + " tl ödeyecek";
                    odemelist.Add(finalsonuc);
                }

            }

            return Ok(odemelist);
                
        }

        void OdemeYap()
        {


        }

        
        [HttpPut]
        public IHttpActionResult UpdateEmployee(Expens expens)
        {
            try
            {
                Expens updated = db.Expenses.Find(expens.ExpensesId);
                db.Entry(updated).CurrentValues.SetValues(expens);
                db.SaveChanges();
                return Ok(1);

            }
            catch
            {
                return Ok(0);
            }

            
        }


        //TODO:expense update olunca kişilerin gunlukharcaması değişecek

    }
}
