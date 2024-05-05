import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  title = 'DatingAppClient';
  users: any;

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
      this.http.get('http://localhost:5032/api/Users').subscribe({
        next: response => this.users = response,
        error: error => console.log(error),
        complete: () => console.log('request has completed!')
      });
  }

}
