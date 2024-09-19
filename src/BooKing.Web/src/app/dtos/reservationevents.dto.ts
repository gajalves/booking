export class ReservationEventsDto {
    id: string;
    eventType: string;
    createdAt: Date;
    eventTypeDescription: string;
    icon: string;
    additionalInformation: string;

    constructor(
      id: string,
      eventType: string,
      createdAt: Date,
      eventTypeDescription: string,
      icon: string,
      additionalInformation: string
    ) {
      this.id = id;
      this.eventType = eventType;
      this.createdAt = createdAt;
      this.eventTypeDescription = eventTypeDescription;
      this.icon = icon;
      this.additionalInformation = additionalInformation;
    }
}
