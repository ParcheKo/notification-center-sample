import {Component, OnInit} from '@angular/core';
import {environment} from '../../environments/environment';
import * as signalR from '@microsoft/signalr';

@Component({
  selector: 'app-notifications',
  templateUrl: './notifications.component.html',
  styleUrls: ['./notifications.component.scss']
})
export class NotificationsComponent implements OnInit {

  items: string[];

  constructor() {
  }

  ngOnInit(): void {
    const connection = new signalR.HubConnectionBuilder()
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

    connection.start().then(() => {
      console.log('Connected to "Notification Center Hub" successfully!');
    }).catch(err => console.error(err));

    connection.on('ReceiveSimpleMessage', (message: string) => {
      console.log(`A new simple message received : ${message}`);
    });

    connection.onreconnecting(error => {
      console.assert(connection.state === signalR.HubConnectionState.Reconnecting);
      // document.getElementById('messageInput').disabled = true;
      // const li = document.createElement('li');
      // li.textContent = `Connection lost due to error "${error}". Reconnecting.`;
      // document.getElementById('messagesList').appendChild(li);
    });

    connection.onreconnected(connectionId => {
      console.assert(connection.state === signalR.HubConnectionState.Connected);
      // document.getElementById('messageInput').disabled = false;
      const li = document.createElement('li');
      li.textContent = `Connection reestablished. Connected with connectionId "${connectionId}".`;
      document.getElementById('messagesList').appendChild(li);
    });

    connection.onclose(error => {
      console.assert(connection.state === signalR.HubConnectionState.Disconnected);
      // document.getElementById("messageInput").disabled = true;
      const li = document.createElement('li');
      li.textContent = `Connection closed due to error "${error}". Try refreshing this page to restart the connection.`;
      document.getElementById('messagesList').appendChild(li);
    });
  }

  async start(connection: signalR.HubConnection) {
    try {
      await connection.start();
      console.assert(connection.state === signalR.HubConnectionState.Connected);
      console.log('Connected to "Notification Center Hub" successfully!');
    } catch (err) {
      console.assert(connection.state === signalR.HubConnectionState.Disconnected);
      console.error(err);
      setTimeout(() => this.start(connection), 5000);
    }
  }
}


// calling hub methods
// try {
//   await connection.invoke("SendMessage", user, message);
// } catch (err) {
//   console.error(err);
// }
