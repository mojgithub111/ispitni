import { Component, OnInit } from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {MojConfig} from "../moj-config";
import {Router} from "@angular/router";
import {AutentifikacijaHelper} from "../_helpers/autentifikacija-helper";
declare function porukaSuccess(a: string):any;
declare function porukaError(a: string):any;

@Component({
  selector: 'app-studenti',
  templateUrl: './studenti.component.html',
  styleUrls: ['./studenti.component.css']
})
export class StudentiComponent implements OnInit {

  title:string = 'angularFIT2';
  ime_prezime:string = '';
  opstina: string = '';
  studentPodaci: any;
  filter_ime_prezime: boolean;
  filter_opstina: boolean;
  opstinaPodaci: any;
  opstinaRodjenjaId: any;
  odabraniStudent: any;
  opstinaSaNajviseStudenata: any;


  constructor(private httpKlijent: HttpClient, private router: Router) {
  }

  fetchStudenti() :void
  {
    this.httpKlijent.get(MojConfig.adresa_servera+ "/Student/GetAll", MojConfig.http_opcije()).subscribe(x=>{
      this.studentPodaci = x;
    });
  }
  fetchOpstine() :void
  {
    this.httpKlijent.get(MojConfig.adresa_servera+ "/Opstina/GetByAll", MojConfig.http_opcije()).subscribe(x=>{
      this.opstinaPodaci = x;
    });
  }
  fetchOpstinaNajviseStudenata(){
    this.httpKlijent.get(MojConfig.adresa_servera+ "/Opstina/GetOpstinuSaNajviseStudenata", MojConfig.http_opcije()).subscribe(x=>{
      this.opstinaSaNajviseStudenata = x;
    });
  }

  ngOnInit(): void {
    this.fetchStudenti();
    this.fetchOpstine();
    this.fetchOpstinaNajviseStudenata();
  }

  get_podaci_filtrirano() {
    if(this.studentPodaci == null)
      return [];
    return  this.studentPodaci.filter((s:any)=>
      (!this.filter_ime_prezime || (s.ime.startsWith(this.ime_prezime) || (s.prezime).startsWith(this.ime_prezime)))
      &&
      (!this.filter_opstina || s.opstina_rodjenja != null && (s.opstina_rodjenja_id == this.opstinaRodjenjaId) )
    )
  }

  ObrisiDugme(id:number) {
    this.httpKlijent.get(MojConfig.adresa_servera+ "/Student/Obrisi?id="+id, MojConfig.http_opcije()).subscribe(x=>{
    this.fetchStudenti();
      porukaSuccess("Uspjesno ste uklonili studenta!")
    });
  }

  UrediDugme(s: any) {
    this.odabraniStudent = s;
  }

  MaticnaKniga(s: any) {
  this.router.navigate(["/student-maticnaknjiga",s.id])
  }

  NoviStudent() {
    this.odabraniStudent = {
      id:0,
      ime: this.ime_prezime,
      prezime:'',
      opstina_rodjenja_id:this.opstinaSaNajviseStudenata
    }
  }

  Snimi() {
    this.httpKlijent.post(MojConfig.adresa_servera+ "/Student/Snimi",this.odabraniStudent, MojConfig.http_opcije()).subscribe(x=>{
      this.fetchStudenti();
      porukaSuccess("Uspjesno ste dodali studenta!")
      this.odabraniStudent =null;
    });
  }
}
