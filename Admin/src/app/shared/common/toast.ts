import { Injectable, } from '@angular/core';
import { ToastyService, ToastOptions } from 'ng2-toasty';

@Injectable()
export class Toast {
    constructor(public toastyService: ToastyService) { }

    showToast(title: string, msg: string, type: string) {
        const toastOptions: ToastOptions = {
            title: title,
            msg: msg,
            showClose: true,
            timeout: 5000,
            theme: 'bootstrap',
        };
        switch (type) {
            case 'default': this.toastyService.default(toastOptions); break;
            case 'info': this.toastyService.info(toastOptions); break;
            case 'success': this.toastyService.success(toastOptions); break;
            case 'wait': this.toastyService.wait(toastOptions); break;
            case 'error': this.toastyService.error(toastOptions); break;
            case 'warning': this.toastyService.warning(toastOptions); break;
        }
    }
}
