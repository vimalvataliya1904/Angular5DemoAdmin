import { DateFormatPipe } from './shared/pipes/DateFormatPipe';
import { NgModule } from '@angular/core';
import { TitleCasePipe } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { RoutingModule, RoutedComponents } from './app.routes';
import { ToastyModule } from 'ng2-toasty';
import { AppComponent } from './app.component';
import { ModalModule } from 'ngx-bootstrap';
import { Ng2SmartTableModule } from 'ng2-smart-table';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { HomeComponent } from './home/home.component';
import { SidebarComponent } from './sidebar/sidebar.component';
import { Common } from './shared/common/common';
import { AuthGuard } from './shared/guards/auth.guard';
import { HttpService } from './shared/service/http.service';
import { Toast } from './shared/common/toast';
import { LoaderHttpInterceptor, LoadingIndicatorService } from './shared/common/httpinterceptor';
import { NumberOnlyDirective, DisallowSpaceDirective } from './shared/common/common.directive';

@NgModule({
  declarations: [
    AppComponent,
    SidebarComponent,
    RoutedComponents,
    HomeComponent,
    DateFormatPipe,
    NumberOnlyDirective,
    DisallowSpaceDirective
  ],
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    RoutingModule,
    HttpClientModule,
    Ng2SmartTableModule,
    ModalModule.forRoot(),
    ToastyModule.forRoot(),
  ],
  providers: [Toast, HttpService, AuthGuard, Common, TitleCasePipe, LoadingIndicatorService,
    {
      provide: HTTP_INTERCEPTORS,
      useFactory: httpFactory,
      multi: true,
      deps: [LoadingIndicatorService]
    }],
  bootstrap: [AppComponent]
})

export class AppModule { }


export function httpFactory(LoaderService) {
  return new LoaderHttpInterceptor(LoaderService);
}
