export class MonitoringEventBase {
  id: string;
  when: Date;
  who: string;

  get Identity(): string {
    return this.id;
  }
}

export class AppPublished extends MonitoringEventBase {
  appName: string;
  version: string;

  public toString = (): string => {
    return `Application ${this.appName} (${this.version}) published on ${this.when.toLocaleString()} by ${this.who}`;
  };

  constructor(init?: Partial<AppPublished>) {
    super();
    Object.assign(this, init);
  }

  static from(id: string, when: Date, who: string, appName: string, version: string): AppPublished {
    return new AppPublished({id, when: new Date(when), who, appName, version});
  }
}
