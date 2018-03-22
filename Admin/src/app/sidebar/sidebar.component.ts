import { Component, OnInit } from '@angular/core';
import { AdminService } from './../shared/service/Admin.service';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  providers: [AdminService]
})
export class SidebarComponent implements OnInit {
  public name: string;

  constructor(private adminService: AdminService) {
    this.adminService.username.subscribe(username => {
      this.name = username;
    });
  }

  ngOnInit() {  }
}
