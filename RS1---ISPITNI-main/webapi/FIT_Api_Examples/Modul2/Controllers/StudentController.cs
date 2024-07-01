using System;
using System.Collections.Generic;
using System.Linq;
using FIT_Api_Examples.Data;
using FIT_Api_Examples.Helper.AutentifikacijaAutorizacija;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FIT_Api_Examples.Modul2.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class StudentController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public StudentController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        [HttpGet]
        public ActionResult<List<Student>> GetAll()
        {
            KorisnickiNalog logiraniKorisnik = HttpContext.GetLoginInfo().korisnickiNalog;

            if (logiraniKorisnik != null)
            {
                //primjer kako preuzet informacije o logiranom korisniku
            }

            var data = _dbContext.Student
                .Where(x=>x.Obrisan == false)
                .Include(s => s.opstina_rodjenja.drzava)
                .OrderByDescending(s => s.id)
                .AsQueryable();
            return data.Take(100).ToList();
        }

        [HttpPost]
        public ActionResult Snimi([FromBody] Student x)
        {
            if (!HttpContext.GetLoginInfo().isLogiran)
                return BadRequest("nije logiran");

            Student studentObj;
            var indeks = _dbContext.Student.Count() + 1;

            if(x.id == 0)
            {
                studentObj = new Student();
                _dbContext.Add(studentObj);
            }
            else
            {
                studentObj = _dbContext.Student.Find(x.id);
                if (studentObj == null)
                {
                    return BadRequest("Ne postoji student s tim ID-om");
                }
            }
            studentObj.ime = x.ime;
            studentObj.prezime = x.prezime;
            studentObj.opstina_rodjenja_id = x.opstina_rodjenja_id;
            studentObj.broj_indeksa = $"IB200" + (indeks).ToString();
            studentObj.created_time = DateTime.Now;


            _dbContext.SaveChanges();
            return Ok();
        }



        [HttpGet]
        public ActionResult<Student> Obrisi(int id)
        {

            var student = _dbContext.Student.Find(id);
            if(id == null)
            {
                BadRequest("Ne postoji student s tim ID-om!");
            }
            student.Obrisan = true;

            _dbContext.SaveChanges();
            return Ok(student);
        }



    }
}
