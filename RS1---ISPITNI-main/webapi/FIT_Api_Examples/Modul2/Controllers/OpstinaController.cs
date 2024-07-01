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
    public class OpstinaController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public OpstinaController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        [HttpPost]
        public ActionResult<Opstina> Add([FromBody] OpstinaAddVM x)
        {
            if (!HttpContext.GetLoginInfo().isLogiran)
                return BadRequest("nije logiran");

            var opstina = new Opstina
            {
                description = x.opis,
                drzava_id = x.drzava_id,
            };

            _dbContext.Add(opstina);
            _dbContext.SaveChanges();
            return opstina;
        }

        [HttpGet]
        public IActionResult GetOpstinuSaNajviseStudenata()
        {
            /*
                SELECT TOP 1
                [opstina_rodjenja_id] as [Opstina rodjenja],
                COUNT([id]) as [Broj studenata]
                FROM febr_RS1_2024.dbo.Student
                GROUP BY[opstina_rodjenja_id]
                ORDER BY[Broj studenata] DESC
            */
            var data = _dbContext.Student
                .GroupBy(x => x.opstina_rodjenja_id)
                .Select(x => new
                {
                    BrojStudenata = x.Count(),
                    OpstinaRodjenja = x.Key
                })
                .OrderByDescending(x => x.BrojStudenata)
                .ToList()
                .Take(1);
            return Ok(data.FirstOrDefault().OpstinaRodjenja);
        }


        [HttpGet]
        public List<CmbStavke> GetByDrzava(int drzava_id)
        {
            var data = _dbContext.Opstina.Where(x => x.drzava_id == drzava_id)
                .OrderBy(s => s.description)
                .Select(s => new CmbStavke()
                {
                    id = s.id,
                    opis = s.drzava.naziv + " - " + s.description,
                })
                .AsQueryable();
            return data.Take(100).ToList();
        }

        [HttpGet]
        public List<CmbStavke> GetByAll()
        {
            var data = _dbContext.Opstina
                .OrderBy(s => s.description)
                .Select(s => new CmbStavke()
                {
                    id = s.id,
                    opis = s.drzava.naziv + " - " + s.description,
                })
                .AsQueryable();
            return data.Take(100).ToList();
        }
    }
}
