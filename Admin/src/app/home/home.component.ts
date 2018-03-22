import { Toast } from './../shared/common/toast';
import { Component, OnInit } from '@angular/core';
import { AdminService } from './../shared/service/Admin.service';
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  providers: [AdminService, Toast]
})
export class HomeComponent implements OnInit {

  TotalUser: number;
  PendingPurchase: number;
  CompletePurchase: number;
  CurrentRate: number;
  SoldOutHash: number;
  constructor(public adminService: AdminService, public toast: Toast) { }
  ngOnInit() {
    this.TotalUser = 221;
    this.PendingPurchase = 231;
    this.CompletePurchase = 256;
    this.CurrentRate = 52;
    this.SoldOutHash = 365;
  }

}
