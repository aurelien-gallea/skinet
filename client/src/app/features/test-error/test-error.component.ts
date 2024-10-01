import { HttpClient } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { MatButton } from '@angular/material/button';

@Component({
  selector: 'app-test-error',
  standalone: true,
  imports: [MatButton],
  templateUrl: './test-error.component.html',
  styleUrl: './test-error.component.scss',
})
export class TestErrorComponent {
  private readonly baseUrl = 'https://localhost:5001/api/';
  private http = inject(HttpClient);
  validationErrors?: string[];

  get404Error() {
    this.http.get(this.baseUrl + 'buggy/notfound').subscribe({
      next: (resp) => console.log(resp),
      error: (err) => console.log(err),
    });
  }

  get401Error() {
    this.http.get(this.baseUrl + 'buggy/unauthorized').subscribe({
      next: (resp) => console.log(resp),
      error: (err) => console.log(err),
    });
  }
  get400Error() {
    this.http.get(this.baseUrl + 'buggy/badrequest').subscribe({
      next: (resp) => console.log(resp),
      error: (err) => console.log(err),
    });
  }

  get500Error() {
    this.http.get(this.baseUrl + 'buggy/internalerror').subscribe({
      next: (resp) => console.log(resp),
      error: (err) => console.log(err),
    });
  }

  get400ValidationError() {
    this.http.post(this.baseUrl + 'buggy/validationerror', {}).subscribe({
      next: (resp) => console.log(resp),
      error: (err) => this.validationErrors = err,
    });
  }
}
