import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from 'src/app/_services/auth.service';
import { first } from 'rxjs/operators';
import { Message, MessageService } from 'primeng/api';
import { AppConfig } from 'src/app/_models/app-config';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  styles: [`
		:host ::ng-deep .p-message {
			margin-left: .25em;
		}

        :host ::ng-deep .p-toast{
            z-index:99999;
        }
    `]
})
export class LoginComponent {
  loginForm!: FormGroup;
    loading = false;
    submitted = false;
    error = '';

    
    constructor(
      private config: AppConfig,
      private formBuilder: FormBuilder,
      private route: ActivatedRoute,
      private router: Router,
      private msgService: MessageService,
      private authService: AuthService
  ) { 
      // redirect to home if already logged in
      if (this.authService.getUser()) { 
          this.router.navigate(['/']);
      }
  }

  ngOnInit() {
    this.loginForm = this.formBuilder.group({
        login: ['', Validators.required],
        password: ['', Validators.required]
    });
  }
  
  get f() { return this.loginForm.controls; }

  onSubmit() {
    this.submitted = true;

    // stop here if form is invalid
    if (this.loginForm.invalid) {
        return;
    }

    this.loading = true;
    this.authService.login(this.f.login.value, this.f.password.value)
        .pipe(first())
        .subscribe({
            next: () => {
              const user = this.authService?.getUser();
               if (user?.success)
               {
                  // get return url from route parameters or default to '/'
                  const returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
                  this.router.navigate([returnUrl]);
               }
               else
                this.msgService.add({ key: 'tst', severity: 'error', summary: 'Attention!', detail: user?.message });
            },
            error: error => {
                this.error = error;
                this.loading = false;
                this.msgService.add({ key: 'tst', severity: 'error', summary: 'Attention!', detail: "Login fails, contact the Administrator." });
            }
        });
}

}
