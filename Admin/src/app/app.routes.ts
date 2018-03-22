import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from './shared/guards/auth.guard';
import { CurrencyComponent } from './currency/currency.component';
import { CountryComponent } from './country/country.component';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';

export const appRoutes: Routes = [
    { path: '', redirectTo: 'login', pathMatch: 'full' },
    { path: 'login', component: LoginComponent },
    { path: 'home', component: HomeComponent, canActivate: [AuthGuard] },
    { path: 'currency', component: CurrencyComponent, canActivate: [AuthGuard] },
    { path: 'country', component: CountryComponent, canActivate: [AuthGuard] },
];

@NgModule({
    imports: [RouterModule.forRoot(appRoutes)],
    exports: [RouterModule]
})

export class RoutingModule { }

export const RoutedComponents = [ LoginComponent, HomeComponent, CountryComponent, CurrencyComponent];
