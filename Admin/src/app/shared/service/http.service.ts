import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable()
export class HttpService {
    GetAuthHttpCommon() {
        const currentUser = JSON.parse(localStorage.getItem('currentUser'));
        const token = currentUser && currentUser.access_token;
        return {
            headers: new HttpHeaders().set('Authorization', 'Bearer ' + token)
                .set('Content-Type', 'application/json')
        };
    }

    GetJsonHttpCommon() {
        return {
            headers: new HttpHeaders().set('Content-Type', 'application/json')
        };
    }

    GetHttpCommon() {
        return {
            headers: new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded')
        };
    }
}
