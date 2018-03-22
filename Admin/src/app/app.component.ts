import { Router, NavigationEnd } from '@angular/router';
import { LocationStrategy, TitleCasePipe } from '@angular/common';
import { Component, OnInit, isDevMode } from '@angular/core';
import { AdminService } from './shared/service/Admin.service';
import { LoadingIndicatorService } from './shared/common/httpinterceptor';
import { environment } from '../environments/environment.prod';
declare var $: any;

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  providers: [AdminService]
})
export class AppComponent implements OnInit {
  public isLogin = true;
  public name: string;
  public showLoader = false;
  public previousUrl: string;
  constructor(public url: LocationStrategy, public router: Router, public adminService: AdminService,
    public titlecasePipe: TitleCasePipe, public loadingIndicatorService: LoadingIndicatorService) {
    this.adminService.isLogin.subscribe(isLogins => { this.isLogin = isLogins; });
    this.adminService.username.subscribe(username => { this.name = this.titlecasePipe.transform(username); });

    router.events.filter(event => event instanceof NavigationEnd).subscribe(e => {
    //  this.previousUrl = e.url;
    });

    this.loadingIndicatorService.onLoadingChanged.subscribe(isLoading => {
      if (this.previousUrl !== '/home') {
        setTimeout(() => this.showLoader = isLoading, 0);
      }
    }
    );
  }

  ngOnInit() {
    if (!isDevMode() && environment.production) {
      if (location.protocol === 'http:') {
        window.location.href = location.href.replace('http', 'https');
      }
    }
    $('body').removeClass('login-page').addClass('skin-blue');
    $('body').addClass('sidebar-mini');
    this.adminService.CheckUserLoggedIn();
    const currentUrl = this.url.path().substring(1);
    const str = ['login'];
    if ((currentUrl === '' && this.isLogin) || (this.isLogin && str.includes(currentUrl))) {
      this.router.navigate(['home']);
    }
  }

  Logout() {
    this.adminService.Logout();
    this.router.navigate(['/login']);
  }
}
