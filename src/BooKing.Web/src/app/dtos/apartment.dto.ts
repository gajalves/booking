import { AmenityDto } from "./amenity.dto";

export class ApartmentDto {
  id: string;
  name: string;
  description: string;
  address: AddressDto;
  price: number;
  cleaningFee: number;
  amenities: AmenityDto[];
  amenitiesDescription: string;
  imagePath: string;
  ownerId: string;
  isActive: boolean;
  isDeleted: boolean;
  searchField: string;

  constructor(
    id: string,
    name: string,
    description: string,
    address: AddressDto,
    price: number,
    cleaningFee: number,
    amenities: AmenityDto[],
    amenitiesDescription: string,
    imagePath: string,
    ownerId: string,
    isActive: boolean,
    isDeleted: boolean,
    searchField: string
  ) {
    this.id = id;
    this.name = name;
    this.description = description;
    this.address = address;
    this.price = price;
    this.cleaningFee = cleaningFee;
    this.amenities = amenities;
    this.imagePath = imagePath;
    this.ownerId = ownerId;
    this.amenitiesDescription = amenitiesDescription;
    this.isActive = isActive;
    this.isDeleted = isDeleted;
    this.searchField = searchField;
  }
}

export class AddressDto {
  street: string;
  city: string;
  country: string;
  state: string;
  number: string;
  zipCode: string;

  constructor(
    street: string,
    city: string,
    country: string,
    state: string,
    number: string,
    zipCode: string) {
    this.street = street;
    this.city = city;
    this.country = country;
    this.state = state;
    this.number = number;
    this.zipCode = zipCode;
  }
}
