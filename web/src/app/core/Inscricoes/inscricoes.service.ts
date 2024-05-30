import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { SecurityService } from '../Segurança/security.service';
import { Observable } from 'rxjs';
import { Subscription } from 'src/app/shared/models/inscricao';

const url = 'http://localhost:11737/v1/subscription/';

@Injectable({
  providedIn: 'root',
})
export class SubscriptionsService {
  constructor(
    private http: HttpClient,
    private securityService: SecurityService
  ) {}

  getSubscriptionById(id: number): Observable<any> {
    const headers = this.securityService.getAuthentication();

    return this.http.get<any>(url + id, {
      headers: headers,
    });
  }

  getSubscriptionsByEvent(idEvent: number): Observable<any> {
    const headers = this.securityService.getAuthentication();

    return this.http.get<any>(url + 'event/' + idEvent, {
      headers: headers,
    });
  }

  getNonEffectiveSubscriptionsByEvent(idEvent: number): Observable<any[]> {
    const headers = this.securityService.getAuthentication();

    return this.http.get<any[]>(url + 'no-effectives/' + idEvent, {
      headers: headers,
    });
  }

  getAthleteSubscriptions(cpfAthlete: string): Observable<any> {
    const headers = this.securityService.getAuthentication();

    return this.http.get<any>(url + 'athlete/' + cpfAthlete, {
      headers: headers,
    });
  }

  checkIfAthleteSubscribedByEvent(cpfAthlete: string, idEvent: number) {
    return this.http.get<any>(
      url + 'athlete-subscribed/' + cpfAthlete + '/' + idEvent
    );
  }

  putSubscription(subscription: Subscription): Observable<any> {
    const idInscricao = subscription.id;

    delete subscription.id;

    const headers = this.securityService.getAuthentication();

    return this.http.put<any>(url + idInscricao, subscription, {
      headers: headers,
    });
  }

  postSubscription(subscription: Subscription): Observable<any> {
    delete subscription.id;
    delete subscription.evento;
    delete subscription.atleta;

    const headers = this.securityService.getAuthentication();

    return this.http.post<any>(url, subscription, {
      headers: headers,
    });
  }

  effectSubscription(
    idSubscription: number,
    effectSubscriptionModel: any
  ): Observable<any> {
    const headers = this.securityService.getAuthentication();

    return this.http.put<any>(
      url + 'effect/' + idSubscription,
      effectSubscriptionModel,
      {
        headers: headers,
      }
    );
  }

  deleteSubscription(idSubscription: number): Observable<any> {
    const headers = this.securityService.getAuthentication();

    return this.http.delete<any>(url + idSubscription, { headers: headers });
  }
}
