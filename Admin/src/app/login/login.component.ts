import { Toast } from './../shared/common/toast';
import { AdminService } from './../shared/service/Admin.service';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { Login } from '../shared/model/Admin';
import { LoadingIndicatorService } from './../shared/common/httpinterceptor';
declare var $: any;

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  providers: [AdminService]
})

export class LoginComponent implements OnInit {
  public LoginForm: FormGroup;
  IsLoginSubmitted = false;
  constructor(public router: Router, public adminService: AdminService, public toast: Toast,
    public loadingIndicatorService: LoadingIndicatorService) {
    this.BindForm();
  }

  BindForm() {
    this.LoginForm = new FormGroup({
      'Username': new FormControl('', [Validators.required]),
      'Password': new FormControl('', [Validators.required])
    });
  }

  ngOnInit() {
    $('body').removeClass('sidebar-mini').removeClass('skin-blue').addClass('login-page');
  }

  Login(obj: Login, isValid: boolean) {
    this.IsLoginSubmitted = true;
    if (isValid) {
      this.adminService.Login(obj).subscribe((data: any) => {
        if (data.status === 'success') {
          window.location.href = '/home';
        } else {
          this.toast.showToast('Error', data.Message, 'error');
        }
      });
    }
  }
}
