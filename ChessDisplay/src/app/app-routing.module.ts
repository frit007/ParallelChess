import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { PlayAiComponent } from "./play-ai/play-ai.component";
import { ThankYouComponent } from "./thank-you/thank-you.component";

const routes: Routes = [
  { path: "", redirectTo: "/play/ai", pathMatch: "full" },
  { path: "play/ai", component: PlayAiComponent },
  { path: "thank-you", component: ThankYouComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
