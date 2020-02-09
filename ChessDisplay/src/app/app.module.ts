import { BrowserModule } from "@angular/platform-browser";
import { NgModule } from "@angular/core";
import { HttpClientModule, HTTP_INTERCEPTORS } from "@angular/common/http";

import { AppRoutingModule } from "./app-routing.module";
import { AppComponent } from "./app.component";
import { ChessBoardComponent } from "./chess-board/chess-board.component";
import { PlayAiComponent } from "./play-ai/play-ai.component";
import { ThankYouComponent } from "./thank-you/thank-you.component";
import { BaseUrlInterceptor } from "./helpers/base-url.interceptor";
import { environment } from "src/environments/environment";

@NgModule({
  declarations: [
    AppComponent,
    ChessBoardComponent,
    PlayAiComponent,
    ThankYouComponent
  ],
  imports: [BrowserModule, AppRoutingModule, HttpClientModule],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      // useClass: Base
      useClass: BaseUrlInterceptor,
      multi: true
    },
    {
      provide: "BASE_API_URL",
      useValue: environment.url
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule {}
