import { Injectable } from '@angular/core';

@Injectable()
export class Common {
    constructor() { }
    IsEmpty(str: string): boolean {
        return str === undefined || str === null || str === '';
    }
}
