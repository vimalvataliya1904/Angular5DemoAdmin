import { Directive, ElementRef, HostListener } from '@angular/core';

@Directive({
    selector: '[appNumberOnly]'
})
export class NumberOnlyDirective {
    private regex: RegExp = new RegExp(/^-?[0-9]+(\.[0-9]*){0,1}$/g);

    private specialKeys: Array<string> = ['Backspace', 'Tab', 'End', 'Home'];

    constructor(private el: ElementRef) { }

    @HostListener('keydown', ['$event'])
    onKeyDown(event: KeyboardEvent) {
        if (this.specialKeys.indexOf(event.key) !== -1) { return; }
        const current: string = this.el.nativeElement.value;
        const next: string = current.concat(event.key);
        if (next && !String(next).match(this.regex)) {
            event.preventDefault();
        }
    }
}

@Directive({
    selector: '[appDisallowSpaces]'
})
export class DisallowSpaceDirective {
    private regex: RegExp = new RegExp(/\s/g);

    private specialKeys: Array<string> = ['Space'];

    constructor(private el: ElementRef) { }

    @HostListener('keydown', ['$event'])
    onKeyDown(event: KeyboardEvent) {
        const current: string = this.el.nativeElement.value;
        if (event.keyCode === 32) {
            return false;
        }
        const next: string = current.concat(event.key);
    }
}
