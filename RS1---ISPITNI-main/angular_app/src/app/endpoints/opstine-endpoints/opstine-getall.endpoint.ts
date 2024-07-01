import {MyBaseEndpoint} from "../MyBaseEndpoint";
import {MojConfig} from "../../moj-config";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {Injectable} from "@angular/core";
@Injectable({providedIn: 'root'})
export class OpstineGetAllEndpoint implements  MyBaseEndpoint<void, OpstineGetAllResponse>{
  constructor(public httpClient:HttpClient) {

  }
  obradi(request: void): Observable<OpstineGetAllResponse> {
    let url=MojConfig.adresa_servera+`/Opstina/get-all`;
      return this.httpClient.get<OpstineGetAllResponse>(url, MojConfig.http_opcije());
    }
}

export interface OpstineGetAllResponseOpstina {
  id: number
  opis: string
}
export interface OpstineGetAllResponse {
  opstine: OpstineGetAllResponseOpstina[]
}

