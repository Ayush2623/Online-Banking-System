import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { UserDashboardRoutingModule } from './user-dashboard-route.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

// Components
import { DashboardHomeComponent } from './components/dashboard-home/dashboard-home.component';
import { OpenAccountComponent } from './components/open-account/open-account.component';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { FundTransferComponent } from './components/fund-transfer/fund-transfer.component';
import { PayeeManagementComponent } from './components/payee-management/payee-management.component';
import { AccountStatementComponent } from './components/account-statement/account-statement.component';
import { NetBankingComponent } from './components/net-banking/net-banking.component';

@NgModule({
  declarations: [
    DashboardHomeComponent,
    OpenAccountComponent,
    SidebarComponent,
    FundTransferComponent,
    PayeeManagementComponent,
    AccountStatementComponent,
    NetBankingComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    UserDashboardRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule
  ]
})
export class UserDashboardModule {}