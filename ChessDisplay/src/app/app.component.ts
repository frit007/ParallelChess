import { Component } from '@angular/core';

import {MenuItem} from 'primeng/api';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.less']
})
export class AppComponent {
  title = 'chess-display';
  items: MenuItem[];

  ngOnInit() {
    this.items = [
      {
          label: 'Play',
          routerLink: "/play"
          // items: [{
          //         label: 'New', 
          //         icon: 'pi pi-fw pi-plus',
          //         items: [
          //             {
          //               label: 'Project',
          //               routerLink: "/play/ai"
          //             },
          //             {label: 'Other'},
          //         ]
          //     },
          //     {label: 'Open'},
          //     {label: 'Quit'}
          // ]
      },
      {
        label: 'Thank you',
        routerLink: "/thank-you"
      },
      {
        label: 'Source code',
        // routerLink: 'https://github.com/frit007/ParallelChess',
        command: () => {
          window.location.href = "https://github.com/frit007/ParallelChess";
        }
      }
  ];
  }
}
