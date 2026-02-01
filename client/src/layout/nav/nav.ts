import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountServices } from '../../core/services/account-services';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { ToastService } from '../../core/services/toast-service';

@Component({
  selector: 'app-nav',
  imports: [FormsModule, RouterLink, RouterLinkActive],
  templateUrl: './nav.html',
  styleUrl: './nav.css',
})
export class Nav {
  protected accountService=inject(AccountServices);
  private toastService=inject(ToastService);
  protected router=inject(Router);
  protected  creds:any={};

  login(){
    this.accountService.login(this.creds).subscribe({
      next:result=> {
        this.router.navigateByUrl("/members");
        this.creds={};
        this.toastService.success('Logged in Successfully')
      },
      error:error=>{
        this.toastService.error(error.error);
      }
    })
  }

  logOut(){
    this.accountService.logout();
    this.router.navigateByUrl("/")
  }
}
