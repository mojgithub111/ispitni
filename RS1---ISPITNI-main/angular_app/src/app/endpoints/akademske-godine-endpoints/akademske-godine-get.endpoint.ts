import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {Injectable} from "@angular/core";
import {MyBaseEndpoint} from "../MyBaseEndpoint";
import {MojConfig} from "../../moj-config";
@Injectable({providedIn: 'root'})
export class AkademskeGodineGetEndpoint implements  MyBaseEndpoint<void, AkademskeGodineGetResponse>{
  constructor(public httpClient:HttpClient) { }

  obradi(): Observable<AkademskeGodineGetResponse> {
      let url=MojConfig.adresa_servera+`/akademske-godine`;
      return this.httpClient.get<AkademskeGodineGetResponse>(url, MojConfig.http_opcije());
    }
}

export interface AkademskeGodineGetResponse {
  akademskeGodine:AkademskeGodineGetResponseAkGodine[]
}

export interface AkademskeGodineGetResponseAkGodine{
  id: number,
  opis: string,
}

