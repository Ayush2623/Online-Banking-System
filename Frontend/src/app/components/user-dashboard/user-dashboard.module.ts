// user-dashboard.module.ts
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserDashboardRoutingModule } from './user-dashboard-route.module';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';
import { DashboardHomeComponent } from './components/dashboard-home/dashboard-home.component';
import { OpenAccountComponent } from './components/open-account/open-account.component';
// import { AccountDetailsComponent } from './components/account-details/account-details.component';
// import { AccountSummaryComponent } from './components/account-summary/account-summary.component';
// import { AccountStatementComponent } from './components/account-statement/account-statement.component';
// import { ChangePasswordComponent } from './components/change-password/change-password.component';
// import { SessionExpiredComponent } from './components/session-expired/session-expired.component';
@NgModule({
  declarations: [
    DashboardHomeComponent,
    OpenAccountComponent
    // AccountDetailsComponent,
    // AccountSummaryComponent,
    // AccountStatementComponent,
    // ChangePasswordComponent,
    // SessionExpiredComponent
  ],
  imports: [
    CommonModule,
    UserDashboardRoutingModule,
    FormsModule,
    HttpClientModule,
    ReactiveFormsModule
  ]
})
export class UserDashboardModule {}
