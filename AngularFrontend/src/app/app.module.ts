import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AlertComponent } from './_components/alert.component';
import { CommonModule } from '@angular/common';
// import { JwtInterceptor, ErrorInterceptor } from './_helpers';
import { MatButtonToggleModule } from '@angular/material/button-toggle';

// Angular Material Modules
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';

// Third-party Modules

// Components
import { AppComponent } from './app.component';
// import { AlertComponent } from './_components';
import { HomeComponent } from './home';

// Fake backend provider
// import { fakeBackendProvider } from './_helpers';
import { GameDetailsComponent } from './game-details/game-details.component';

@NgModule({
    imports: [
        BrowserModule,
        ReactiveFormsModule,
        FormsModule,
        HttpClientModule,
        AppRoutingModule,
        MatCardModule,
        MatButtonModule,
        MatToolbarModule,
        MatIconModule,
        MatButtonToggleModule,
        CommonModule
    ],
    declarations: [
    AlertComponent,
        AppComponent,
        HomeComponent,
        GameDetailsComponent
    ],
    providers: [
        // { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
        // { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
        // fakeBackendProvider
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
