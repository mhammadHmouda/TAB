# Travel and Accommodation Booking Platform

This project implements a comprehensive hotel booking system with a discount management feature using ASP.NET Core API. It allows users to browse hotels, view room details, make reservations, and apply discount codes to reduce their booking costs. Hotel owners can manage hotel information, room types, and discounts through API endpoints.

## Prerequisites

- ASP.NET Core 7 SDK installed
- Docker installed

## Setup Guide

1. Clone the Project: Clone the project repository to your local machine using Git.

```bash
git clone github.com/mhammadHmouda/TAB.git
```

2. Run the Project: Navigate to the project directory and run the following commands to start the project.

```bash
docker-compose -f docker-compose.yml -f docker-compose.override.yml build
```

```bash
docker-compose -f docker-compose.yml -f docker-compose.override.yml up
```

3. Access the Project: Open your browser and navigate to `http://localhost:7072` or `https://localhost` to access the project.

4. Stop the Project: To stop the project, run the following command.

```bash
docker-compose -f docker-compose.yml -f docker-compose.override.yml down
```

## Environment Variables

You should find a file with `appsettings.json` name in the `src/TAB.WebApi` directory of the project. This file contains the environment variables used by the project. You can change the values of these variables to suit your needs.

## API Documentation

You can find the API documentation Swagger UI for Development at ([http://localhost:7072/swagger/index.html](http://localhost:7072/swagger/index.html)) OR for Production at ([https://foothill-tab.azurewebsites.net/swagger/index.html](https://foothill-tab.azurewebsites.net/swagger/index.html)). You can also find the Postman collection directly from ([here](https://documenter.getpostman.com/view/29769959/2sA3s1nrqm)). You can also find my postman collection via my public workspace ([here](https://www.postman.com/hmoudah/workspace/tab-api-s/collection/29769959-1da9a23c-9247-4ede-9737-957b0224bea9?action=share&creator=29769959&active-environment=29769959-9df2b079-d9a6-477d-8817-54ba8ac7ba41)).

## Database Diagram

![Database-Diagram](https://github.com/user-attachments/assets/9989b023-703e-4a46-8cad-1efa8c43f57a)

## Project Structure

This Project used the Clean Architecture approach to structure the project into layers. DDD is used to design the domain model and the business logic. The project is divided into the following layers:

- **API Layer**: This layer contains the API controllers.

- **Application Layer**: This layer defines the business use cases responsible for coordinating the operations of your application and orchestrates the flow of data between the outer layers.

- **Domain Layer**: This layer contains the domain models and the domain services and bussiness rules.

- **Infrastructure Layer**: This layer contains the implementations of external services interfaces.

- **Persistence Layer**: This layer contains the database context and the repositories and all configuration required.

- **Contracts Layer**: This layer contains the shared contracts between the layers contains dtos, requests and responses.

## Notes

- To check the payment feature demo, just go through these steps:
    - Create booking from book room api to be the booking status `Pending`.
    - Yhe admin should confirm this booking from confirm booking api to be the booking status `Confirmed` and the use recieved email with checkout link.
    - Then use the checkout API, which will return a url for payment session page.
    - I've set up a demo for testing the payment feature. You can utilize the card number `4242 4242 4242 4242` with any future date and any CVC code.
    - Locate the HTML page in the src/PaymentDemo directory of the project replace the jwtToken hardcoded via new token. Open page, enter the id for booking confirmed and ready for checkout then choose the payment method you need (Stripe, Paypal), then press the "Pay" button.
