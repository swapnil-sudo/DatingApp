import { ToastrService } from 'ngx-toastr';
import { AccountService } from './../_services/account.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
model:any={}
loggedIn:boolean;
  constructor(public accountService:AccountService,private toastrService:ToastrService) { }

  ngOnInit(): void {
    this.getCurrentUser();
  }

  login(){
    this.accountService.login(this.model).subscribe(response=>{
      console.log(response);
      this.loggedIn=true
    },error=>{
      console.log(error.error);
      this.toastrService.error(error.error);
      
    })
  }
  
  logOut(){
    this.accountService.logOut();
    this.loggedIn=false;
  }

  getCurrentUser(){
    this.accountService.currentUser$.subscribe(user=>{
      this.loggedIn=!!user;
    })
  }
}
