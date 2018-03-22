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
  selector: 'app-currency',
  templateUrl: './currency.component.html',
  providers: [AdminService, Common]
})
export class CurrencyComponent implements OnInit {

  public settings = {
    columns: {
      CurrencyName: {
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
  public formCurrency: FormGroup;
  IsCurrencySubmitted: boolean = false;
  CurrencyName: string = '';
  CurrencyId: number = 0;
  @ViewChild(ModalDirective) public modal: ModalDirective;

  constructor(public adminService: AdminService, public common: Common, public fb: FormBuilder,public toast: Toast) {
    this.source = new LocalDataSource();
    this.BindForm();
  }

  ngOnInit() {
    this.GetCurrency();
  }

  GetCurrency() {
    this.adminService.GetAllCurrency().subscribe(data => {
      this.source.load(data.Data);
    }, error => this.errorMessage = <any>error);
  }
  BindForm() {
    this.formCurrency = this.fb.group({
      'CurrencyName': new FormControl('', [Validators.required])
    });
  }
  closePopup() {
    this.modal.hide();
  }
  openPopup() {
    this.CurrencyId=0;
    this.CurrencyName="";
    this.IsCurrencySubmitted = false;
    this.modal.show();
  }
  onsubmit() {
    debugger
    this.IsCurrencySubmitted = true;
    if (this.formCurrency.valid) {
      if (this.CurrencyId == 0) {
        var obj={"CurrencyId":0,"CurrencyName":this.CurrencyName};
        this.adminService.InsertCurrency(obj).subscribe((data: any) => {
          if (data.ResponseStatus === 0) {
            this.toast.showToast('Success', data.Message, 'success');
          } else {
            this.toast.showToast('Error', data.Message, 'error');
          }
          this.GetCurrency();
          this.modal.hide();
          this.formCurrency.reset();
        });
      }
      else{
        var obj={"CurrencyId":this.CurrencyId,"CurrencyName":this.CurrencyName};
        this.adminService.UpdateCurrency(this.CurrencyId, obj).subscribe((data: any) => {
          if (data.ResponseStatus === 0) {
            this.toast.showToast('Success', data.Message, 'success');
          } else {
            this.toast.showToast('Error', data.Message, 'error');
          }
          this.GetCurrency();
          this.modal.hide();
          this.formCurrency.reset();
        });
      }
    }
    else{
      return false;
    }
  }
  onCustom(event) {
    if (event.action === 'Delete') {
      this.CurrencyId = event.data.CurrencyId;
      this.adminService.DeleteCurrency(this.CurrencyId).subscribe(data => {
        if (data.ResponseStatus === 0) {
          this.toast.showToast('Success', data.Message, 'success');
        } else {
          this.toast.showToast('Error', data.Message, 'error');
        }
        this.GetCurrency();
        this.formCurrency.reset();
        this.modal.hide();
      });
    }
    else if (event.action === 'Edit') {
      debugger
      this.openPopup();
      this.CurrencyId = event.data.CurrencyId;
      this.CurrencyName=event.data.CurrencyName;
    }
  }
}
