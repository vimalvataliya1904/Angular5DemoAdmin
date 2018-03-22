import { Injectable, Output, EventEmitter } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/observable';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';
import { environment } from '../../../environments/environment';
import { Response } from '../model/Response';
import { Login, CountryMaster } from '../model/Admin';
import { HttpService } from './http.service';

@Injectable()
export class AdminService {
    @Output() username: EventEmitter<any> = new EventEmitter();
    @Output() isLogin: EventEmitter<any> = new EventEmitter();

    constructor(private http: HttpClient, public httpService: HttpService) {

    }

    Login(obj: Login) {
        return this.http.post(environment.apiUrl + 'token', 'username=' + obj.Username +
            '&password=' + obj.Password + '&type=admin', this.httpService.GetHttpCommon())
            .map((response) => {
                localStorage.setItem('currentUser', JSON.stringify(response));
                this.CheckUserLoggedIn();
                return response;
            }).catch(this.handleError);
    }

    GetAllCountry() {
        return this.http.get(environment.apiUrl + 'Country')
            .map((response) => response).catch(this.handleError);
    }

    InsertCountry(obj) {
        return this.http.post(environment.apiUrl + 'Country/',JSON.stringify(obj), this.httpService.GetAuthHttpCommon())
            .map((response) => response).catch(this.handleError);
    }

    UpdateCountry(Id: number,obj) {
        return this.http.put(environment.apiUrl + 'Country/' + Id, JSON.stringify(obj), this.httpService.GetAuthHttpCommon())
            .map((response) => response).catch(this.handleError);
    }

    DeleteCountry(id: number) {
        return this.http.delete(environment.apiUrl + 'Country/' + id, this.httpService.GetAuthHttpCommon())
            .map((response) => response).catch(this.handleError);
    }

    GetAllCurrency() {
        return this.http.get(environment.apiUrl + 'Currency')
            .map((response) => response).catch(this.handleError);
    }
    InsertCurrency(obj) {
        return this.http.post(environment.apiUrl + 'Currency/',JSON.stringify(obj), this.httpService.GetAuthHttpCommon())
            .map((response) => response).catch(this.handleError);
    }

    UpdateCurrency(Id: number,obj) {
        return this.http.put(environment.apiUrl + 'Currency/' + Id, JSON.stringify(obj), this.httpService.GetAuthHttpCommon())
            .map((response) => response).catch(this.handleError);
    }

    DeleteCurrency(id: number) {
        return this.http.delete(environment.apiUrl + 'Currency/' + id, this.httpService.GetAuthHttpCommon())
            .map((response) => response).catch(this.handleError);
    }
    CheckUserLoggedIn(): boolean {
        if (localStorage.getItem('currentUser')) {
            const currentUser = JSON.parse(localStorage.getItem('currentUser'));
            this.username.emit(currentUser.username);
            this.isLogin.emit(true);
            return true;
        }
        this.username.emit('');
        this.isLogin.emit(false);
        return false;
    }
    Logout() {
        localStorage.removeItem('currentUser');
        this.CheckUserLoggedIn();
    }
    private handleError(error: Response) {
        return Observable.throw(error || 'Server error');
    }
}