import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-open-account',
  templateUrl: './open-account.component.html',
  styleUrls: ['./open-account.component.scss']
})
export class OpenAccountComponent implements OnInit {
  accountForm!: FormGroup;

  constructor(private fb: FormBuilder, private router: Router, private http: HttpClient) {}

  ngOnInit(): void {
    this.accountForm = this.fb.group({
      personalDetails: this.fb.group({
        title: ['', Validators.required],
        firstName: ['', Validators.required],
        middleName: [''],
        lastName: ['', Validators.required],
        fatherName: ['', Validators.required],
        mobileNumber: ['', Validators.required],
        email: ['', [Validators.required, Validators.email]],
        aadhar: ['', Validators.required],
        dob: ['', Validators.required]
      }),
      residentialAddress: this.fb.group({
        address1: ['', Validators.required],
        address2: ['', Validators.required],
        landmark: [''],
        state: ['', Validators.required],
        city: ['', Validators.required],
        pincode: ['', Validators.required]
      }),
      isSameAddress: [false],
      permanentAddress: this.fb.group({
        address1: ['', Validators.required],
        address2: ['', Validators.required],
        landmark: [''],
        state: ['', Validators.required],
        city: ['', Validators.required],
        pincode: ['', Validators.required]
      }),
      occupationDetails: this.fb.group({
        occupationType: ['', Validators.required],
        sourceOfIncome: ['', Validators.required],
        grossAnnualIncome: ['', Validators.required]
      }),
      wantsDebitCard: ['', Validators.required],
      optNetBanking: [false],
      agree: [false, Validators.requiredTrue]
    });

    this.syncAddresses();
  }

  syncAddresses(): void {
    this.accountForm.get('isSameAddress')?.valueChanges.subscribe(checked => {
      const permAddress = this.accountForm.get('permanentAddress') as FormGroup;
      const resAddress = this.accountForm.get('residentialAddress') as FormGroup;

      if (checked) {
        permAddress.patchValue(resAddress.value);
        permAddress.disable();
      } else {
        permAddress.enable();
      }
    });
  }

  onSubmit(): void {
    if (this.accountForm.invalid) {
      this.accountForm.markAllAsTouched();
      return;
    }

    const payload = this.accountForm.value;
    console.log('Submitted data:', payload);

    // TODO: Replace with your actual API call
    /*
    this.http.post('/api/open-account', payload).subscribe(
      response => {
        console.log('Account created successfully', response);
        this.router.navigate(['/success']);
      },
      error => {
        console.error('Error creating account', error);
      }
    );
    */
  }
}
