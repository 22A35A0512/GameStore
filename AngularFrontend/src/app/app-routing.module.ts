import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './home';
import { GameDetailsComponent } from './game-details/game-details.component';
import { AuthGuard } from './_helpers/auth.guard';
const accountModule = () => import('./account/account.module').then(x => x.AccountModule);
const routes: Routes = [
    { path: '', component: HomeComponent, canActivate: [AuthGuard] },
    { path: 'game/:id', component: GameDetailsComponent },
    { path: 'account', loadChildren: accountModule },

    // otherwi, canActivate: [AuthGuard] se redirect to home
    { path: '**', redirectTo: '' }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule { }