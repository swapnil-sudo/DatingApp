import { AccountService } from './../_services/account.service';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  model:any={}
 @Output() cancelRegister=new EventEmitter();
  constructor(private accountService:AccountService,private toastrService:ToastrService) { }

  ngOnInit(): void {
  
  }

  register(){
   this.accountService.register(this.model).subscribe(response=>{
     console.log(response);
   },error=>{
     console.log(error);
     this.toastrService.error(error.error)
   })
   //console.log(this.model);
  }
  cancel(){
    this.cancelRegister.emit(false);
  }

}
