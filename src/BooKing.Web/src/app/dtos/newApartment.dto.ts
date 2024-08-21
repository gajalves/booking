export class NewApartmentDto {
  name: string;
  description: string;
  address: AddressDto;
  price: number;
  cleaningFee: number;
  amenities: string[];
  imagePath: string;

  constructor(
    name: string,
    description: string,
    address: AddressDto,
    price: number,
    cleaningFee: number,
    amenities: string[],
    imagePath: string
  ) {
    this.name = name;
    this.description = description;
    this.address = address;
    this.price = price;
    this.cleaningFee = cleaningFee;
    this.amenities = amenities;
    this.imagePath = imagePath;
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
