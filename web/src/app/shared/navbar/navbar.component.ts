import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { SecurityService } from 'src/app/core/Segurança/security.service';
import { LoginComponent } from '../login/login.component';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css'],
})
export class NavbarComponent implements OnInit {
  userIsLoggedIn = false;
  userName: string;
  adm: boolean;

  constructor(
    private dialog: MatDialog,
    private securityService: SecurityService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.checkLogIn();
  }

  login() {
    this.dialog.open(LoginComponent, {
      data: {
        naoPermiteCadastro: true,
      },
    });
  }

  onActivate() {
    window.scroll({
      top: 0,
      left: 0,
      behavior: 'smooth',
    });
  }

  checkLogIn() {
    var token = this.securityService.getToken();

    if (token != null) {
      var userInfo = this.securityService.getDecodedAccessToken(token);

      if (userInfo == null) {
        this.userIsLoggedIn = false;
        return;
      }

      this.userName = userInfo.Name;

      this.adm = userInfo.employee != undefined ? true : false;
    } else {
      this.logOut();
      this.router.navigateByUrl('/');
    }

    this.userIsLoggedIn = token != null;
  }

  logOut() {
    localStorage.removeItem('currentUser');

    window.location.reload();
  }

  meusDados() {
    var token = this.securityService.getToken();

    var userInfo = this.securityService.getDecodedAccessToken(token);

    if (userInfo != null)
      this.router.navigateByUrl('cadastro/atleta', userInfo.ID);
  }

  minhasInscricoes() {
    var token = this.securityService.getToken();

    var userInfo = this.securityService.getDecodedAccessToken(token);

    if (userInfo != null)
      this.router.navigateByUrl('cadastro/inscricoes', userInfo.ID);
  }

  voltarInicio() {
    this.router.navigateByUrl('/');
  }
}
