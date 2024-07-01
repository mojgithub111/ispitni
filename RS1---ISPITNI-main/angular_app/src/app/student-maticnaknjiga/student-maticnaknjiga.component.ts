import { Component, OnInit } from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {MojConfig} from "../moj-config";
import {HttpClient} from "@angular/common/http";
import {AutentifikacijaHelper} from "../_helpers/autentifikacija-helper";

declare function porukaSuccess(a: string):any;
declare function porukaError(a: string):any;

@Component({
  selector: 'app-student-maticnaknjiga',
  templateUrl: './student-maticnaknjiga.component.html',
  styleUrls: ['./student-maticnaknjiga.component.css']
})
export class StudentMaticnaknjigaComponent implements OnInit {
  studentId: any;
  maticnaKnjigaPodaci: any;
  akademskeGodine: any;
  noviSemestar: any;
  ovjeraZimskog: any;

  constructor(private httpKlijent: HttpClient, private route: ActivatedRoute) {}

  fetchMaticnaKnjigaDetalji()
  {
    this.httpKlijent.get(MojConfig.adresa_servera+ "/MaticnaKnjigaDetalji/GetByID?studentId="+this.studentId, MojConfig.http_opcije()).subscribe(x=>{
      this.maticnaKnjigaPodaci = x;
    });
  }
  ucitajAkademskeGodine() {
    this.httpKlijent.get(MojConfig.adresa_servera + "/AkademskeGodine/GetAll_ForCmb", MojConfig.http_opcije()).subscribe(x => {
      this.akademskeGodine = x;
    });
  }
  ovjeriLjetni(s:any) {

  }

  upisLjetni(s:any) {

  }

  ovjeriZimski(s:any) {

  }

  ngOnInit(): void {
    this.route.params.subscribe((s:any)=>{
      this.studentId = +s["studentparametarid"];
      this.fetchMaticnaKnjigaDetalji();
      this.ucitajAkademskeGodine();
    })
  }
  ovjeriZimskiSemestar() {
    this.httpKlijent.post(MojConfig.adresa_servera+ "/MaticnaKnjigaDetalji/Ovjera",this.ovjeraZimskog, MojConfig.http_opcije()).subscribe(x=>{
      this.fetchMaticnaKnjigaDetalji();
      this.ovjeraZimskog=null;
      porukaSuccess("Uspješno ste ovjerili zimski semestar");
    });
  }
  btnOvjeraZimskog(upisAkGodID: any) {
  this.ovjeraZimskog ={
    datum:new Date(),
    napomena:'',
    semestarId:upisAkGodID,
    }
  }
  upisUZimskiSemestar() {
    this.noviSemestar ={
      datumUpisaZimski:'',
      godinaStudija:1,
      akademskaGodina_id:1,
      cijenaSkolarine:1800,
      obnova:false,
      student_id:this.studentId,
      evidentiraoKorisnik_id:AutentifikacijaHelper.getLoginInfo().autentifikacijaToken.korisnickiNalogId
    }
  }

  dodajZimskiSemestar() {
    this.httpKlijent.post (MojConfig.adresa_servera+"/MaticnaKnjigaDetalji/Add", this.noviSemestar,MojConfig.http_opcije()).subscribe(x=> {
      this.fetchMaticnaKnjigaDetalji();
      this.noviSemestar = null;
      porukaSuccess("Uspješno ste dodali zimski semestar");
    },error => {
      porukaError("Ne možete dodati upisati istu godinu ukoliko nije obnova!")
    });
  }


}
