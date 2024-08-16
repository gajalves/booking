import { ApartmentDto } from "./apartment.dto";
import { ReservationEventsDto } from "./reservationevents.dto";

export class ReservationDto {
  id: string;
  userId: string;
  apartmentId: string;
  apartment: ApartmentDto;
  duration: DateRange;
  priceForPeriod: number;
  cleaningFee: number;
  totalPrice: number;
  statusDescription: string;
  statusValue: number;
  createdOnUtc: Date;
  confirmedOnUtc?: Date;
  rejectedOnUtc?: Date;
  completedOnUtc?: Date;
  cancelledOnUtc?: Date;
  events: ReservationEventsDto[];

  constructor(
    id: string,
    userId: string,
    apartmentId: string,
    apartment: ApartmentDto,
    duration: DateRange,
    priceForPeriod: number,
    cleaningFee: number,
    totalPrice: number,
    statusDescription: string,
    statusValue: number,
    createdOnUtc: Date,
    events: ReservationEventsDto[],
    confirmedOnUtc?: Date,
    rejectedOnUtc?: Date,
    completedOnUtc?: Date,
    cancelledOnUtc?: Date,
  ) {
    this.id = id;
    this.userId = userId;
    this.apartmentId = apartmentId;
    this.apartment = apartment;
    this.duration = duration;
    this.priceForPeriod = priceForPeriod;
    this.cleaningFee = cleaningFee;
    this.totalPrice = totalPrice;
    this.statusDescription = statusDescription;
    this.statusValue = statusValue;
    this.createdOnUtc = createdOnUtc;
    this.confirmedOnUtc = confirmedOnUtc;
    this.rejectedOnUtc = rejectedOnUtc;
    this.completedOnUtc = completedOnUtc;
    this.cancelledOnUtc = cancelledOnUtc;
    this.events = events;
  }
}

export class DateRange {
  start: Date;
  end: Date;
  lengthInDays: number;

  constructor(
    start: Date,
    end: Date,
    lengthInDays: number
  ){
    this.start = start;
    this.end = end;
    this.lengthInDays = lengthInDays;
  }
}
