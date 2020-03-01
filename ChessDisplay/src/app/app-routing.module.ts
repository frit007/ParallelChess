import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { PlayAiComponent } from "./play-ai/play-ai.component";
import { ThankYouComponent } from "./thank-you/thank-you.component";
import { PlayComponent } from './play/play.component';

const routes: Routes = [
  { path: "", redirectTo: "/play", pathMatch: "full" },
  { path: 'play', component: PlayComponent},
  { path: "play/ai", component: PlayAiComponent },
  { path: "thank-you", component: ThankYouComponent },
  { path: '**', redirectTo: '/play', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
