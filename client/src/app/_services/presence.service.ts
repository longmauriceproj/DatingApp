import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { ToastrService } from 'ngx-toastr';
import { BehaviorSubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root',
})
export class PresenceService {
  hubUrl = environment.hubUrl;
  private hubConnection: HubConnection;
  private onlineUsersSource = new BehaviorSubject<string[]>([]);
  onlineUsers$ = this.onlineUsersSource.asObservable();

  constructor(private toastr: ToastrService) {}

  createHubConnection(user: User) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(`${this.hubUrl}presence`, {
        accessTokenFactory: () => user.token,
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start().catch(error => console.error(error));

    this.hubConnection.on('UserIsOnline', username => {
      this.toastr.info(`${username} has connected.`);
    });

    this.hubConnection.on('UserIsOffline', username => {
      this.toastr.warning(`${username} has diconnected.`);
    });

    this.hubConnection.on('GetOnlineUsers', (usernames: string[]) => {
      this.onlineUsersSource.next(usernames);
      console.log(`Should be array of usernames: ${usernames}`);
    });
  }

  stopHubConnection() {
    this.hubConnection.stop().catch(error => console.error(error));
  }
}
