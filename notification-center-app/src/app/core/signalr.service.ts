import {Injectable} from '@angular/core';
import {environment} from 'src/environments/environment';
import {BehaviorSubject} from 'rxjs';
import * as signalR from '@microsoft/signalr';
import {AppPublished, MonitoringEventBase} from '../contracts/events';

@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  connection: signalR.HubConnection;
  receiveSimpleMessage: BehaviorSubject<string>;
  receiveAppPublishedMessage: BehaviorSubject<AppPublished>;

  constructor() {
    this.receiveSimpleMessage = new BehaviorSubject<string>(null);
    this.receiveAppPublishedMessage = new BehaviorSubject<AppPublished>(null);
  }

  // Establish a connection to the SignalR server hub
  public initiateSignalrConnection(): Promise<any> {
    return new Promise((resolve, reject) => {
      this.connection = new signalR.HubConnectionBuilder()
        .configureLogging(signalR.LogLevel.Information)
        .withUrl(environment.notificationHubBaseUrl)
        .withAutomaticReconnect({
          nextRetryDelayInMilliseconds: retryContext => {
            if (retryContext.elapsedMilliseconds < 60000) {
              // If we've been reconnecting for less than 60 seconds so far,
              // wait between 0 and 10 seconds before the next reconnect attempt.
              return Math.random() * 10000;
            } else {
              // If we've been reconnecting for more than 60 seconds so far, stop reconnecting.
              return null;
            }
          }
        })
        .build();

      this.setSignalrClientMethods();

      this.connection
        .start()
        .then(() => {
          console.log(`SignalR connection success! connectionId: ${this.connection.connectionId} `);
          resolve();
        })
        .catch((error) => {
          console.log(`SignalR connection error: ${error}`);
          reject();
        });
    });
  }

  // This method will implement the methods defined in the ISignalrDemoHub interface in the SignalrDemo.Server .NET solution
  private setSignalrClientMethods(): void {
    this.connection.on('ReceiveSimpleMessage', (message: string) => {
      this.receiveSimpleMessage.next(message);
    });

    this.connection.on('ReceiveAppPublishedMessage', (message: AppPublished) => {
      const typedMessage = AppPublished.from(message.id, message.when, message.who, message.appName, message.version);
      // const typedMessage = new AppPublished(message);
      this.receiveAppPublishedMessage.next(typedMessage);
    });
  }
}
