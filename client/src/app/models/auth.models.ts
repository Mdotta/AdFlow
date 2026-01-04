export interface FacebookUser {
  id: string;
  facebookId: string;
  email: string;
  name: string;
  profilePictureUrl?: string;
  createdAt: string;
  lastLoginAt?: string;
}

export interface FacebookLoginResponse {
  user: FacebookUser;
  token: string;
}

export interface FacebookLoginRequest {
  accessToken: string;
}
