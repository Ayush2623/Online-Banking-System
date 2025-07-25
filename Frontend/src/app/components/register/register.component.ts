import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {
  user = {
    username: '',
    password: '',
    role: '',
    mobileNumber: ''
  };

  message = '';
  isLoading = false;

  constructor(private authService: AuthService) {}

  register() {
    this.isLoading = true;
    this.authService.register(this.user).subscribe({
      next: (res) => {
        this.isLoading = false;
        // Check if the response contains an error-like message
        if (res && res.message && res.message.toLowerCase().includes('already exists')) {
          this.message = res.message;
        } else if (res && res.message) {
          this.message = res.message;
        } else {
          this.message = 'Registration successful!';
        }
        this.resetForm();
      },
      error: (err) => {
        this.isLoading = false;
        // If error callback is triggered, show error message
        this.message = err?.error?.text || 'Registration failed.';
      }
    });
  }


  private resetForm() {
    this.user = {
      username: '',
      password: '',
      role: '',
      mobileNumber: ''
    };
  }
}
