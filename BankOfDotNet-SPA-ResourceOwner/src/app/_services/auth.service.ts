import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { JwtHelperService } from '@auth0/angular-jwt';

// OpenID Connect (OIDC) and OAuth2 protocol support for browser-based JavaScript applications
// URL: https://github.com/IdentityModel/oidc-client-js
// Sample implementation
// URL: https://github.com/jmurphzyo/Angular2OidcClient/blob/master/src/app/shared/services/auth.service.ts
// For now, the Resource Owner Password flow IS NOT SUPPORTED
// URL: https://github.com/IdentityModel/oidc-client-js/issues/234
const settings: any = {
  authority: environment.identityServerUrl,
  client_id: 'ResOwnerPwd-Client'
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  jwtHelper = new JwtHelperService();
  decodedToken: any;

  constructor(private http: HttpClient) { }

  login(model: any) {

  }
}
