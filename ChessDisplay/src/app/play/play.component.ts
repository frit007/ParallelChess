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
    {label:'Easy',value: 3},
    {label:'Normal',value: 5},
    {label:'Hard',value: 6},
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
