using System;
using System.Collections.Generic;
using System.Linq;
using FIT_Api_Examples.Data;
using FIT_Api_Examples.Helper;
using FIT_Api_Examples.Helper.AutentifikacijaAutorizacija;
using FIT_Api_Examples.Modul2.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FIT_Api_Examples.Modul2.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class MaticnaKnjigaDetaljiController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public MaticnaKnjigaDetaljiController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public class MaticnaKnjigaDetaljiVM
        {
            public int student_id { get; set; }
            public string ime { get; set; }
            public string prezime { get; set; }
            public List<MaticaKnjigaDetaljiUpisiVM> AkGodine { get; set; }
        }

        public class MaticaKnjigaDetaljiUpisiVM
        {
            public int upisAkGodineID { get; set; }
            public string upisAkGodine_opis { get; set; }
            public int godinaStudija { get; set; }
            public bool obnova { get; set; }
            public DateTime zimskiUpis { get; set; }
            public DateTime? zimskiOvjera { get; set; }
            public string evidentiraoKorisnik { get; set; }
        }

        [HttpGet]
        public ActionResult<MaticnaKnjigaDetaljiVM> GetByID(int studentId)
        {
            var student = _dbContext.Student.Find(studentId);
            var resultVM = new MaticnaKnjigaDetaljiVM
            {
                student_id = studentId,
                ime = student.ime,
                prezime = student.prezime,
                AkGodine = _dbContext.UpisAkGodine.Where(x => x.student_id == studentId)
                .Select(u => new MaticaKnjigaDetaljiUpisiVM
                {
                    upisAkGodineID = u.id,
                    upisAkGodine_opis = u.akademskaGodina.opis,
                    godinaStudija = u.godinaStudija,
                    obnova = u.obnova,
                    zimskiUpis = u.datumUpisaZimski,
                    zimskiOvjera = u.datumOvjereZimski,
                    evidentiraoKorisnik = u.evidentiraoKorisnik.korisnickoIme
                }).ToList()
            };
            return resultVM;
        }


        [HttpPost]
        public ActionResult Add([FromBody] UpisAkGodine semestar)
        {
            if (semestar.obnova == false)
            {
                var jeLiPostojiIstaGodina = _dbContext.UpisAkGodine.Count(x => x.student_id == semestar.student_id && x.godinaStudija == semestar.godinaStudija) > 0;
                if (jeLiPostojiIstaGodina)
                {
                    return BadRequest("Student ima već upisan semestar sa tom godinom studija");
                }
            }
            _dbContext.UpisAkGodine.Add(semestar);
            _dbContext.SaveChanges();
            return Ok();
        }


        [HttpPost]
        public ActionResult Ovjera(OvjeraVM ovjera)
        {
            var semestar = _dbContext.UpisAkGodine.Find(ovjera.SemestarId);

            semestar.datumOvjereZimski = ovjera.Datum;
            semestar.napomena = ovjera.Napomena;
            _dbContext.SaveChanges();
            return Ok();
        }


        public class OvjeraVM
        {
            public int SemestarId { get; set; }
            public DateTime Datum { get; set; }
            public string Napomena { get; set; }
        }

       
    }
}
