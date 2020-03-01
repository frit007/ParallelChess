import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-play',
  templateUrl: './play.component.html',
  styleUrls: ['./play.component.less']
})
export class PlayComponent implements OnInit {

  constructor(private router: Router) { }

  difficulties = [
    {label:'1',value: 1},
    {label:'2',value: 2},
    {label:'3',value: 3},
    {label:'4',value: 4},
    {label:'5',value: 5},
    {label:'6',value: 6},
    {label:'7',value: 7},
    {label:'8',value: 8},
  ];
  selectedDifficulty = 5;


  ngOnInit(): void {
  }

  startGame() {
    this.router.navigate(["/play/ai"], {
      queryParams: {
        difficulty: this.selectedDifficulty || 5
      },
    })
  }

}
