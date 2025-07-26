// user-dashboard-routing.module.ts
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardHomeComponent } from './components/dashboard-home/dashboard-home.component';
import { OpenAccountComponent } from './components/open-account/open-account.component';
import { FundTransferComponent } from './components/fund-transfer/fund-transfer.component';
import { PayeeManagementComponent } from './components/payee-management/payee-management.component';
import { AccountStatementComponent } from './components/account-statement/account-statement.component';
import { NetBankingComponent } from './components/net-banking/net-banking.component';
import { AccountGuard } from '../../guards/account.guard';
// import { AccountDetailsComponent } from './components/account-details/account-details.component';
// import { AccountSummaryComponent } from './components/account-summary/account-summary.component';
// import { AccountStatementComponent } from './components/account-statement/account-statement.component';
// import { ChangePasswordComponent } from './components/change-password/change-password.component';
// import { SessionExpiredComponent } from './components/session-expired/session-expired.component';

const routes: Routes = [
  { path: '', component: DashboardHomeComponent, canActivate: [AccountGuard] },
  { path: 'open-account', component: OpenAccountComponent},
  { path: 'fund-transfer', component: FundTransferComponent, canActivate: [AccountGuard] },
  { path: 'payee-management', component: PayeeManagementComponent, canActivate: [AccountGuard] },
  { path: 'account-statement', component: AccountStatementComponent, canActivate: [AccountGuard] },
  { path: 'net-banking', component: NetBankingComponent, canActivate: [AccountGuard] },
  //  { path: 'accountSummary', component: AccountSummaryComponent },
  // { path: 'accountDetails', component: AccountDetailsComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UserDashboardRoutingModule {}
