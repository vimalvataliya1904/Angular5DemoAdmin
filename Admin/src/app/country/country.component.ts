import { AdminService } from './../shared/service/Admin.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { Ng2SmartTableModule, LocalDataSource } from 'ng2-smart-table';
import { Common } from './../shared/common/common';
import { ActiveStatus } from './../shared/common/enum';
import { ModalDirective } from 'ngx-bootstrap';
import { FormControl, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { CountryMaster } from '../shared/model/Admin';
import { Toast } from '../shared/common/toast';

@Component({
  selector: 'app-country',
  templateUrl: './country.component.html',
  providers: [AdminService, Common]
})
export class CountryComponent implements OnInit {
  public settings = {
    columns: {
      CountryName: {
        type: 'html',
        title: 'Name',
        valuePrepareFunction: (val) => {
          if (this.common.IsEmpty(val)) { return; }
          return val;
        }
      },
      IsActive: {
        type: 'html',
        title: 'Status',
        valuePrepareFunction: (val) => {
          if (this.common.IsEmpty(val)) { return; }
          return val === true ? '<span class="label label-success">' + ActiveStatus[1] + '</span>' :
            '<span class="label label-danger">' + ActiveStatus[2] + '</span>';
        }
      },
    },
    pager: {
      display: true,
      perPage: 10
    },
    actions: {
      columnTitle: 'Actions',
      add: false,
      edit: false,
      delete: false,
      custom: [
        {
          name: 'Edit',
          title:
            '<a class="btn btn-sm btn-danger"  title="Edit"><i  class="fa fa-edit"></i></a>'
        },
        {
          name: 'Delete',
          title:
            '<a class="btn btn-sm btn-danger"  title="Delete"><i  class="fa fa-trash"></i></a>'
        }
      ],
      position: 'left' // left|right
    },
    attr: {
      class: 'table table-striped table-bordered table-hover'
    },
    defaultStyle: false
  };
  source: LocalDataSource;
  errorMessage: string;
  public formCountry: FormGroup;
  IsCountrySubmitted: boolean = false;
  CountryName: string = '';
  CountryId: number = 0;
  @ViewChild(ModalDirective) public modal: ModalDirective;
  constructor(public adminService: AdminService, public common: Common, public fb: FormBuilder,public toast: Toast) {
    this.BindForm();
    this.source = new LocalDataSource();
  }

  ngOnInit() {
    this.GetCountry();
  }

  GetCountry() {
    this.adminService.GetAllCountry().subscribe(data => {
      this.source.load(data.Data);
    }, error => this.errorMessage = <any>error);
  }
  BindForm() {
    this.formCountry = this.fb.group({
      'CountryName': new FormControl('', [Validators.required])
    });
  }
  closePopup() {
    this.modal.hide();
  }
  openPopup() {
    this.CountryId=0;
    this.CountryName="";
    this.IsCountrySubmitted = false;
    this.modal.show();
  }
  onsubmit() {
    debugger
    this.IsCountrySubmitted = true;
    if (this.formCountry.valid) {
      if (this.CountryId == 0) {
        var obj={"CountryId":0,"CountryName":this.CountryName};
        this.adminService.InsertCountry(obj).subscribe((data: any) => {
          if (data.ResponseStatus === 0) {
            this.toast.showToast('Success', data.Message, 'success');
          } else {
            this.toast.showToast('Error', data.Message, 'error');
          }
          this.GetCountry();
          this.modal.hide();
          this.formCountry.reset();
        });
      }
      else{
        var obj={"CountryId":this.CountryId,"CountryName":this.CountryName};
        this.adminService.UpdateCountry(this.CountryId, obj).subscribe((data: any) => {
          if (data.ResponseStatus === 0) {
            this.toast.showToast('Success', data.Message, 'success');
          } else {
            this.toast.showToast('Error', data.Message, 'error');
          }
          this.GetCountry();
          this.modal.hide();
          this.formCountry.reset();
        });
      }
    }
    else{
      return false;
    }
  }
  onCustom(event) {
    if (event.action === 'Delete') {
      this.CountryId = event.data.CountryId;
      this.adminService.DeleteCountry(this.CountryId).subscribe(data => {
        if (data.ResponseStatus === 0) {
          this.toast.showToast('Success', data.Message, 'success');
        } else {
          this.toast.showToast('Error', data.Message, 'error');
        }
        this.GetCountry();
        this.formCountry.reset();
        this.modal.hide();
      });
    }
    else if (event.action === 'Edit') {
      debugger
      this.openPopup();
      this.CountryId = event.data.CountryId;
      this.CountryName=event.data.CountryName;
    }
  }
}
