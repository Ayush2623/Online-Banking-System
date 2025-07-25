// user-dashboard-routing.module.ts
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardHomeComponent } from './components/dashboard-home/dashboard-home.component';
import { OpenAccountComponent } from './components/open-account/open-account.component';
// import { AccountDetailsComponent } from './components/account-details/account-details.component';
// import { AccountSummaryComponent } from './components/account-summary/account-summary.component';
// import { AccountStatementComponent } from './components/account-statement/account-statement.component';
// import { ChangePasswordComponent } from './components/change-password/change-password.component';
// import { SessionExpiredComponent } from './components/session-expired/session-expired.component';

const routes: Routes = [
  { path: '', component: DashboardHomeComponent },
  { path: 'open-account', component: OpenAccountComponent},
  //  { path: 'accountSummary', component: AccountSummaryComponent },
  // { path: 'accountDetails', component: AccountDetailsComponent },
  // { path: 'accountStatement', component: AccountStatementComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UserDashboardRoutingModule {}
