export class ApartmentDto {
  id: string;
  name: string;
  description: string;
  address: AddressDto;
  price: number;
  cleaningFee: number;
  amenities: string[];
  imagePath: string;
  ownerId: string;

  constructor(
    id: string,
    name: string,
    description: string,
    address: AddressDto,
    price: number,
    cleaningFee: number,
    amenities: string[],
    imagePath: string,
    ownerId: string
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
  }
}

export class AddressDto {
  street: string;
  city: string;
  country: string;
  state: string;

  constructor(
    street: string,
    city: string,
    country: string,
    state: string) {
    this.street = street;
    this.city = city;
    this.country = country;
    this.state = state;
  }
}
