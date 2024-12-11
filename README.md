# BooKing Project

## Overview
The BooKing project is a microservices-based reservation system designed for managing apartment rentals. It leverages modern architectural principles, including Docker, RabbitMQ, SQL Server, and event-driven communication, to deliver a scalable and efficient solution.

This project was developed as a study initiative. The goal was to build an application from scratch while applying concepts learned and gaining hands-on experience with real-world challenges, such as system integration, performance optimization, and the management of microservices in a distributed environment.

The project has not been hosted yet, and there are still improvements to be made. However, I am happy with the outcome and the learning experience throughout the process.

![home](https://github.com/gajalves/booking/blob/main/docs/img/home.png?raw=true)

---

## Architecture
The BooKing system is built using the following core:

 ![architecture](https://github.com/gajalves/booking/blob/main/docs/img/arch.png?raw=true)
 
 ![solution](https://github.com/gajalves/booking/blob/main/docs/img/solution.png?raw=true)

### Services
- **Identity API**: Handles user authentication and management.
- **Apartments API**: Manages apartment data, including creation, editing, and deletion.
- **Reserve API**: Manages reservations, including status updates and cancellation.
- **Outbox Service**: Ensures reliable event delivery to RabbitMQ for event-driven communication.
- **Email Service**: Sends notifications to users about reservation statuses.
- **Reserve Service**: Handles reservation-specific business logic.

### Supporting Components
- **RabbitMQ**: Message broker for asynchronous communication.
- **SQL Server**: Relational database for storing core data.
- **Seq**: Centralized logging system for monitoring and debugging.
- **Portainer**: Docker management interface.
- **Event Store**: Stores event streams for event sourcing.

---

## Key Features
- **Authentication and Authorization**: JWT-based token authentication.
- **Reservation Management**: Reserve, confirm, and cancel reservations.
- **Asynchronous Event Handling**: Leveraging RabbitMQ and the Outbox pattern.
- **Event Sourcing**: Storing reservation events for audit.
- **Scalable Architecture**: Using Docker Compose to orchestrate multiple services.

---

## Swagger

### Identity
![Identity-swagger](https://github.com/gajalves/booking/blob/main/docs/img/swagger/identity.png?raw=true)
### Apartments
![Apartments-swagger](https://github.com/gajalves/booking/blob/main/docs/img/swagger/apartments.png?raw=true)
### Reserve
![Reserve-swagger](https://github.com/gajalves/booking/blob/main/docs/img/swagger/reserve.png?raw=true)

---

## Email

### Setup
Email Configuration
For the email service, if you want to receive emails set the **UseRealEmailService** to **true** and adjust the settings, I used *Mailtrap.io* for testing purposes.

![mailtrap](https://github.com/gajalves/booking/blob/main/docs/img/email/mailtrap.png?raw=true)
![email-appsettings](https://github.com/gajalves/booking/blob/main/docs/img/email/email-setup.png?raw=true)
![welcome](https://github.com/gajalves/booking/blob/main/docs/img/email/welcome.png?raw=true)
![reservation](https://github.com/gajalves/booking/blob/main/docs/img/email/reservation.png?raw=true)
---

## More Images

![reserve](https://github.com/gajalves/booking/blob/main/docs/img/reserve.png?raw=true)

![user-reservations](https://github.com/gajalves/booking/blob/main/docs/img/user-reservations.png?raw=true)

![reservation-detail](https://github.com/gajalves/booking/blob/main/docs/img/reservation-detail.png?raw=true)

![user-apartments](https://github.com/gajalves/booking/blob/main/docs/img/user-apartments.png?raw=true)

![rabbit](https://github.com/gajalves/booking/blob/main/docs/img/rabbit.png?raw=true)

![seq](https://github.com/gajalves/booking/blob/main/docs/img/seq.png?raw=true)
---

## Tests
Unit tests were developed for the main services created. Integration tests and other types of tests will be part of future development (maybe).

![test](https://github.com/gajalves/booking/blob/main/docs/img/coverage/tests.png?raw=true)
![BooKingApartmentApplicationServices](https://github.com/gajalves/booking/blob/main/docs/img/coverage/BooKingApartmentApplicationServices.png?raw=true)
![BooKingIdentityApplicationServices](https://github.com/gajalves/booking/blob/main/docs/img/coverage/BooKingIdentityApplicationServices.png?raw=true)
![BooKingReserveApplicationServices](https://github.com/gajalves/booking/blob/main/docs/img/coverage/BooKingReserveApplicationServices.png?raw=true)
---
## Technologies and Frameworks

The BooKing project uses the following versions of technologies and frameworks:

### Backend
- **.NET Core**: `7.0`

### Frontend
- **Angular**: `18.1.0`
- **Bootstrap**: `5.3.3`

---

## How to Run Locally

1. Clone the repository:
    ```bash
    git clone https://github.com/gajalves/booking.git
    cd booking
    ```

2. Start the services using Docker Compose:
    ```bash
    docker-compose up
    ```
4. Run the frontend:

    Navigate to the frontend folder and install the dependencies:

    ```bash
    cd BooKing.Web
    npm install
    ```

    Then, run the command to start the development server:

    ```bash
    npm start
    ```
4. Access the services:
    - **Identity API**: `http://localhost:5001`
    - **Apartments API**: `http://localhost:5002`
    - **Reserve API**: `http://localhost:5003`
    - **RabbitMQ Dashboard**: `http://localhost:15672`
    - **Seq Dashboard**: `http://localhost:8081`

5. Additional Information
   - When running the project, some apartments and users are created along with the migrations to facilitate testing and usage. The password for all of them is: secret123.
   - ![users](https://github.com/gajalves/booking/blob/main/docs/img/db/user.png?raw=true)
   - ![apartments](https://github.com/gajalves/booking/blob/main/docs/img/db/apartment-db.png?raw=true)

---
