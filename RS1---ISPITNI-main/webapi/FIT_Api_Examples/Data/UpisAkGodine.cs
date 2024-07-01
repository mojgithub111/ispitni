using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIT_Api_Examples.Data
{
    public class UpisAkGodine
    {
        [Key]
        public int id { get; set; }

        [ForeignKey(nameof(student))]
        public int student_id { get; set; }
        public Student student { get; set; }

        public DateTime datumUpisaZimski { get; set; }
        public int godinaStudija { get; set; }

        [ForeignKey(nameof(akademskaGodina))]
        public int akademskaGodina_id { get; set; }
        public AkademskaGodina akademskaGodina { get; set; }

        public float cijenaSkolarine { get; set; }
        public bool obnova { get; set; }
        public DateTime? datumOvjereZimski { get; set; }
        public string napomena { get; set; }

        [ForeignKey(nameof(evidentiraoKorisnik))]
        public int evidentiraoKorisnik_id { get; set; }
        public KorisnickiNalog evidentiraoKorisnik { get; set; }
    }
}
