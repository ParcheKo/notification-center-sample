import {Component, OnInit} from '@angular/core';
import {environment} from '../../environments/environment';
import * as signalR from '@microsoft/signalr';
import {SignalrService} from '../core/signalr.service';
import {BehaviorSubject} from 'rxjs';
import {AppPublished} from '../contracts/events';
import {not} from 'rxjs/internal-compatibility';
import {ToastrService} from 'ngx-toastr';

@Component({
  selector: 'app-notifications',
  templateUrl: './notifications.component.html',
  styleUrls: ['./notifications.component.scss']
})
export class NotificationsComponent implements OnInit {
  connection: signalR.HubConnection;
  notifications: AppPublished[] = [];
  messageForServer: string = '';

  constructor(private signalrService: SignalrService, private toastr: ToastrService) {
    this.connection = this.signalrService.connection;
  }

  ngOnInit(): void {
    // this.signalrService.connection
    //   .invoke('Hello')
    //   .catch(error => {
    //     console.log(`SignalrDemoHub.Hello() error: ${error}`);
    //     alert('SignalrDemoHub.Hello() error!, see console for details.');
    //   });

    this.signalrService.receiveSimpleMessage.subscribe((simpleMessage: string) => {
      // this.simpleMessage = simpleMessage;
    });

    this.signalrService.receiveAppPublishedMessage.subscribe((appPublishedMessage: AppPublished) => {
      if (!appPublishedMessage) {
        return;
      }
      this.notifications.push(appPublishedMessage);
      console.log(this.propertyNamesOf(appPublishedMessage));
      let notification = new Notification('App Published', {body: appPublishedMessage.toString()});
    });

    // User has already granted the app to show desktop notifications
    if (Notification.permission === 'granted') {
      // If it's okay let's create a notification which means displaying it as well
      let notification = new Notification('You already granted me to show notifications like this!');
    }
    // Otherwise, we need to ask the user for permission
    else if (Notification.permission !== 'denied') {
      Notification.requestPermission().then(function(permission) {
        // If the user accepts, let's create a notification which means displaying it as well
        if (permission === 'granted') {
          let notification = new Notification('Thank you for giving me the permission to show notifications!');
        }
      });
    }

    // this.connection = new signalR.HubConnectionBuilder()
    //   .configureLogging(signalR.LogLevel.Information)
    //   .withUrl(environment.notificationHubBaseUrl)
    //   .withAutomaticReconnect({
    //     nextRetryDelayInMilliseconds: retryContext => {
    //       if (retryContext.elapsedMilliseconds < 60000) {
    //         // If we've been reconnecting for less than 60 seconds so far,
    //         // wait between 0 and 10 seconds before the next reconnect attempt.
    //         return Math.random() * 10000;
    //       } else {
    //         // If we've been reconnecting for more than 60 seconds so far, stop reconnecting.
    //         return null;
    //       }
    //     }
    //   })
    //   .build();
    //
    // this.connection.start().then(() => {
    //   const messageText = 'Connected to "Notification Center Hub" successfully!';
    //   console.log(messageText);
    //   console.log(messageText);
    // }).catch(err => {
    //   console.error(err);
    //   alert(err);
    // });
    //
    // this.connection.on('ReceiveSimpleMessage', (message: string) => {
    //   const messageText = `A new simple message received : ${message}`;
    //   console.log(messageText);
    //   alert(messageText);
    // });
    //
    // this.connection.onreconnecting(error => {
    //   console.assert(this.connection.state === signalR.HubConnectionState.Reconnecting);
    //   // document.getElementById('messageInput').disabled = true;
    //   const li = document.createElement('li');
    //   li.textContent = `Connection lost due to error "${error}". Reconnecting.`;
    //   // document.getElementById('messagesList').appendChild(li);
    //   alert(li.textContent);
    // });
    //
    // this.connection.onreconnected(connectionId => {
    //   console.assert(this.connection.state === signalR.HubConnectionState.Connected);
    //   // document.getElementById('messageInput').disabled = false;
    //   const li = document.createElement('li');
    //   li.textContent = `Connection reestablished. Connected with this.connectionId "${connectionId}".`;
    //   document.getElementById('messagesList').appendChild(li);
    //   alert(li.textContent);
    // });
    //
    // this.connection.onclose(error => {
    //   console.assert(this.connection.state === signalR.HubConnectionState.Disconnected);
    //   // document.getElementById("messageInput").disabled = true;
    //   const li = document.createElement('li');
    //   li.textContent = `Connection closed due to error "${error}". Try refreshing this page to restart the this.connection.`;
    //   document.getElementById('messagesList').appendChild(li);
    //   alert(li.textContent);
    // });
  }

  private propertyNamesOf(appPublishedMessage: AppPublished) {
    return Object.keys(appPublishedMessage).filter(e => typeof Object.getOwnPropertyDescriptors(appPublishedMessage)[e].value !== 'function');
  }


  // async start(connection: signalR.HubConnection) {
  //   try {
  //     await this.connection.start();
  //     console.assert(this.connection.state === signalR.HubConnectionState.Connected);
  //     console.log('Connected to "Notification Center Hub" successfully!');
  //   } catch (err) {
  //     console.assert(this.connection.state === signalR.HubConnectionState.Disconnected);
  //     console.error(err);
  //     setTimeout(() => this.start(this.connection), 5000);
  //   }
  // }
  getNotificationIdentity(index: number, notification: AppPublished): string {
    return notification.Identity;
  }

  notifyOthers(message: string) {
    this.connection.invoke('NotifyOthers', message).then(r => {
    });
  }

  onClick() {
    alert('file uploaded!');
  }

  notify() {
    this.toastr.info('Hello world!', 'Toastr fun!');
    // new DesktopNotifications()
  }
}


// calling hub methods
// try {
//   await this.connection.invoke("SendMessage", user, message);
// } catch (err) {
//   console.error(err);
// }
