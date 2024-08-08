# Travel and Accommodation Booking Platform

We are thrilled to present to you a hands-on project where you'll be responsible for designing and implementing the APIs for an advanced online hotel booking system. Your task is to develop a series of APIs that will drive the core functionalities of various components of the platform, including the Login Page, Home Page, Search Results, Hotel Details, Secure Checkout, and Admin Management.

We encourage you to thoroughly read and understand the project features outlined in the documentation. Your challenge is to translate these features into efficient, secure, and well-structured APIs. These APIs should follow RESTful principles and be designed with clean code practices in mind.

Key aspects of your development should include robust error handling, secure JWT authentication, effective user permissions management, and comprehensive unit testing. These elements are crucial for ensuring the reliability and maintainability of your code.

Additionally, your ability to manage this project effectively using tools like Jira will be crucial. This project management aspect is vital for tracking progress, managing tasks, and ensuring timely delivery of your work.

This project is not just about coding; it's an opportunity for you to engage in the full lifecycle of software development, from conception to deployment. We are excited to see how you interpret the requirements and look forward to your innovative approaches to these challenges.

## Prerequisites

- ASP.NET Core 7 SDK installed
- Docker installed

## Setup Guide

1. **Clone the Project**: Clone the project repository to your local machine using Git.

    ```bash
    git clone github.com/mhammadHmouda/TAB.git
    ```

2. **Run the Project**: Navigate to the project directory and run the following commands to start the project.

    ```bash
    docker-compose -f docker-compose.yml -f docker-compose.override.yml build
    ```

    ```bash
    docker-compose -f docker-compose.yml -f docker-compose.override.yml up
    ```

3. **Stop the Project**: To stop the project, run the following command.

    ```bash
    docker-compose -f docker-compose.yml -f docker-compose.override.yml down
    ```

## Environment Variables

You should find a file named `appsettings.json` in the `src/TAB.WebApi` directory of the project. This file contains the environment variables used by the project. You can change the values of these variables to suit your needs.

## API Documentation

You can find the API documentation Swagger UI for Development at [http://localhost:7072/swagger/index.html](http://localhost:7072/swagger/index.html) OR for Production at [https://foothill-tab.azurewebsites.net/swagger/index.html](https://foothill-tab.azurewebsites.net/swagger/index.html). You can also find the Postman collection directly from [here](https://documenter.getpostman.com/view/29769959/2sA3s1nrqm). My Postman collection is also available via my public workspace [here](https://www.postman.com/hmoudah/workspace/tab-api-s/collection/29769959-1da9a23c-9247-4ede-9737-957b0224bea9?action=share&creator=29769959&active-environment=29769959-9df2b079-d9a6-477d-8817-54ba8ac7ba41).

## Database Diagram

![Database-Diagram](https://github.com/user-attachments/assets/a02fda57-7c77-493b-92ad-25b76b15f02f)

## Project Structure

This project uses the Clean Architecture approach to structure the project into layers. Domain-Driven Design (DDD) is used to design the domain model and business logic. The project is divided into the following layers:

- **Web Api Layer (`src/TAB.WebApi`)**: 
  - This layer contains the API controllers responsible for handling HTTP requests and returning responses. It acts as the entry point for all client interactions and communicates with the Application Layer to perform business logic operations.

- **Application Layer (`src/TAB.Application`)**:
  - This layer defines the business use cases and orchestrates the flow of data between the outer layers. It contains service classes, command/query handlers, and business logic that operates on domain entities. The Application Layer is independent of external services or frameworks, ensuring the business logic can be tested in isolation.

- **Domain Layer (`src/TAB.Domain`)**:
  - This layer contains the core business logic, including domain models, entities, value objects, and domain services. The Domain Layer encapsulates the business rules and maintains the integrity of the domain model. It also defines interfaces for repository contracts that are implemented in the Persistence Layer.

- **Infrastructure Layer (`src/TAB.Infrastructure`)**:
  - This layer contains the implementations of external service interfaces, such as email services, payment gateways, and third-party APIs. It serves as a bridge between the application and external systems.

- **Persistence Layer (`src/TAB.Persistence`)**:
  - This layer contains the database context, repositories, and configuration settings required to interact with the database. It implements the repository interfaces defined in the Domain Layer and manages data access logic, including querying and persisting entities.

- **Contracts Layer (`src/TAB.Contracts`)**:
  - This layer contains shared data transfer objects (DTOs), requests, and responses used for communication between different layers. It defines the contract for data that moves across the boundaries of different layers, ensuring consistency and decoupling.

## Testing Strategy

The project includes a comprehensive testing strategy to ensure the reliability and maintainability of the codebase. Tests are divided into the following categories:

- **Unit Tests (`tests/ApplicationUnitTests`)**:
  - These tests focus on individual components or classes within the Application and Domain layers. Unit tests verify that each unit of code performs as expected, with all external dependencies mocked or stubbed out. The goal is to ensure that business logic behaves correctly under various conditions.

- **Architecture Tests (`tests/ArchitectureTests`)**:
  - These tests are designed to enforce architectural principles and constraints within the project. Architecture tests verify that the project adheres to Clean Architecture guidelines, such as ensuring dependencies flow in the correct direction and that layer boundaries are respected. These tests help maintain the integrity of the overall project structure as the codebase evolves.

## Notes

- To check the payment feature demo, follow these steps:
    - Create a booking using the "Book Room" API, setting the booking status to `Pending`.
    - The admin should confirm this booking using the "Confirm Booking" API, changing the booking status to `Confirmed`. The user will receive an email with a checkout link.
    - Use the "Checkout" API to obtain a URL for the payment session page.
    - I've set up a demo for testing the payment feature. Use the card number `4242 4242 4242 4242` with any future date and any CVC code.
    - Locate the HTML page in the `src/PaymentDemo` directory of the project. Replace the hardcoded JWT token with a new token. Open the page, enter the ID for the booking confirmed and ready for checkout, then choose the payment method (Stripe, PayPal), and press the "Pay" button.
