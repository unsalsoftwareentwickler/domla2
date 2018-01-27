import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { DOCUMENT } from '@angular/common';
import 'rxjs/add/operator/retry';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';

interface LogoutUrl {
  url: string;
}

interface BrokerUrl {
  Broker: string;
}

interface EndPointInfo {
  name: string;
  uri: string;
}

interface ServiceInfo {
  name: string;
  baseUrl: string;
  version: number;
  patch: number;
  endPoints: EndPointInfo[];
}

@Injectable()
export class AccountService {

  private static readonly Logout_Url = 'http://localhost:8130/home/logout';
  private static readonly Services_Url = 'http://localhost:8130/home/services';
  private brokerUrl: string;
  private services: ServiceInfo[];

  constructor(
    private http: HttpClient,
    @Inject(DOCUMENT) private document: any
  ) { }

  fetchServices(succeeded: () => void, failed: (message: string) => void) {
    this.http.get<BrokerUrl>(AccountService.Services_Url)
      .catch(error => {
        failed(error.message);
        return Observable.throw(error);
      })
      .subscribe(
        data => {
          console.log('received: ' + data);
          this.brokerUrl = data.Broker;
          this.http.get<ServiceInfo>(`${this.brokerUrl}/apps/Domla2/01/`)
            .catch(error => {
              failed(error.message);
              return Observable.throw(error);
            })
            .subscribe(answer => {
              this.services = answer;
              succeeded();
            });
        }
      );
  }

  logout(id: string, failed: (message: string) => void) {
    this.http.get<LogoutUrl>(AccountService.Logout_Url)
      .catch(error => {
        failed(error.message);
        return Observable.throw(error);
      })
      .subscribe(
        data => {
          this.document.location.href = data.url;
        }
      );
  }
}
