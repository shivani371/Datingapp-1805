import { inject, Injectable } from '@angular/core';
import { AccountServices } from './account-services';
import { of } from 'rxjs/internal/observable/of';

@Injectable({
  providedIn: 'root',
})
export class InitService {
  private  accountService=inject(AccountServices);

  init(){
     const userString=localStorage.getItem('user');
    if(!userString) return of(null);

    const user=JSON.parse(userString);
    this.accountService.currentUser.set(user);

    return of(null)
  }


}
