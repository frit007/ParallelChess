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
import { MenubarModule } from 'primeng/menubar';
import { PlayComponent } from './play/play.component';
import { CardModule } from 'primeng/card';
import { CheckboxModule } from 'primeng/checkbox';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { DropdownModule } from 'primeng/dropdown';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { DialogModule } from 'primeng/dialog'

@NgModule({
  declarations: [
    AppComponent,
    ChessBoardComponent,
    PlayAiComponent,
    ThankYouComponent,
    PlayComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule, 
    HttpClientModule,
    MenubarModule,
    FormsModule,
    CardModule,
    CheckboxModule,
    ButtonModule,
    DropdownModule,
    BrowserAnimationsModule,
    DialogModule
  ],
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
