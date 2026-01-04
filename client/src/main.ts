import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { App } from './app/app';
import { environment } from './environments/environment';

declare const FB: any;
declare global {
  interface Window {
    fbAsyncInit: () => void;
  }
}

// Initialize Facebook SDK with environment configuration
if (typeof FB !== 'undefined') {
  window.fbAsyncInit = function() {
    FB.init({
      appId      : environment.facebookAppId,
      cookie     : true,
      xfbml      : true,
      version    : 'v18.0'
    });
    FB.AppEvents.logPageView();
  };
}

bootstrapApplication(App, appConfig)
  .catch((err) => console.error(err));


