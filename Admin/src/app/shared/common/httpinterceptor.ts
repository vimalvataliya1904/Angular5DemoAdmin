import {
    HTTP_INTERCEPTORS, HttpClientModule, HttpClient, HttpEvent, HttpInterceptor, HttpHandler, HttpRequest
} from '@angular/common/http';
import 'rxjs/add/operator/finally';
import { EventEmitter, Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class LoaderHttpInterceptor implements HttpInterceptor {
    constructor(private loadingIndicatorService: LoadingIndicatorService) { }
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        this.loadingIndicatorService.onStarted(req);
        return next.handle(req).finally(() => this.loadingIndicatorService.onFinished(req));
    }
}

@Injectable()
export class LoadingIndicatorService {
    onLoadingChanged: EventEmitter<boolean> = new EventEmitter<boolean>();

    private requests: HttpRequest<any>[] = [];

    onStarted(req: HttpRequest<any>): void {
        this.requests.push(req);
        this.notify();
    }

    onFinished(req: HttpRequest<any>): void {
        const index = this.requests.indexOf(req);
        if (index !== -1) {
            this.requests.splice(index, 1);
        }
        this.notify();
    }

    private notify(): void {
        this.onLoadingChanged.emit(this.requests.length !== 0);
    }
}
