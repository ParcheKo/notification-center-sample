import {BrowserModule} from '@angular/platform-browser';
import {APP_INITIALIZER, ModuleWithProviders, NgModule} from '@angular/core';

import {AppRoutingModule} from './app-routing.module';
import {AppComponent} from './app.component';
import {NotificationsComponent} from './notifications/notifications.component';
import {CommonModule} from '@angular/common';
import {SignalrService} from './core/signalr.service';
import {FormsModule} from '@angular/forms';

@NgModule({
  declarations: [
    AppComponent,
    NotificationsComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    // CommonModule
  ],
  providers: [
    SignalrService,
    {
      provide: APP_INITIALIZER,
      useFactory: (signalrService: SignalrService) => () => signalrService.initiateSignalrConnection(),
      deps: [SignalrService],
      multi: true,
    }],
  bootstrap: [AppComponent]
})
export class AppModule {
  // static forRoot(config: EnvironmentConfig): ModuleWithProviders<AppModule> {
  //   return {
  //     ngModule: AppModule,
  //     providers: [
  //       {
  //         provide: ENV_CONFIG,
  //         useValue: config
  //       }
  //     ]
  //   };
  // }
}
