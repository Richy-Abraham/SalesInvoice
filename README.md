# Sales Invoice System

This project is a simple invoice management system built with ASP.NET Core. It allows users to create invoices, make payments, and process overdue invoices. The system is designed with a RESTful API architecture and includes unit tests for core functionalities.

## Table of Contents

- [Features](https://github.com/Richy-Abraham/SalesInvoice?tab=readme-ov-file#features)
- [Technologies Used](https://github.com/Richy-Abraham/SalesInvoice?tab=readme-ov-file#technologies-used)
- [Getting Started](https://github.com/Richy-Abraham/SalesInvoice?tab=readme-ov-file#getting-started)
- [Usage](https://github.com/Richy-Abraham/SalesInvoice?tab=readme-ov-file#usage)
- [Running the Tests](https://github.com/Richy-Abraham/SalesInvoice?tab=readme-ov-file#running-tests)
- [Assumptions](https://github.com/Richy-Abraham/SalesInvoice?tab=readme-ov-file#assumptions)
- [Future Enhancements](https://github.com/Richy-Abraham/SalesInvoice?tab=readme-ov-file#future-enhancements)
- [Contributions](https://github.com/Richy-Abraham/SalesInvoice?tab=readme-ov-file#contributions)

## Features

- **Create Invoice**: Allows the creation of new invoices.
- **Pay Invoice**: Allows making payments towards an invoice.
- **Process Overdue Invoices**: Automatically processes overdue invoices by either marking them as paid/void or generating new invoices with late fees.
- **Unit Testing**: Includes unit tests for domain models and handlers.

## Technologies Used

- **.NET Core 8.0**
- **Entity Framework Core**
- **AutoMapper**
- **Moq** (for unit testing)
- **XUnit** (for testing framework)

## Getting Started

### Prerequisites

- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/get-started) (for containerization)

### Installation

1. **Clone the repository:**
   ```sh
   git clone https://github.com/Richy-Abraham/SalesInvoice.git
   cd SalesInvoice
2. **Build the Docker container:**
   ```sh
   docker-compose build
3. **Run the application:**
   ```sh
   docker-compose up
The API will be accessible at http://localhost:8080.

## Usage
### API Endpoints
- **POST /invoices**
  * Create a new invoice.
  * **Request Body:**
    ```json
    {
      "amount": 199.99,
       "due_date": "2021-09-11"
    }
    ```
  * **Response:**
      * 201 Created
    ```json
    {
      "id": "1234"
    }
    ```

- **GET /invoices**
  * Retrieve all invoices.

  * **Response:**
      * 200 OK
    ```json
    [
      {
        "id": "1234",
        "amount": 199.99,
        "paid_amount": 0,
        "due_date": "2021-09-11",
        "status": "pending"
       }
    ]
    ```
- **POST /invoices/{id}/payments**
  * Make a payment on an invoice.
  * **Request Body:**
    ```json
    {
      "amount": 159.99
    }
    ```
  * **Response:**
      * 200 OK

- **POST /invoices/process-overdue**
  * Process overdue invoices and apply late fees.
  * **Request Body:**
    ```json
    {
      "late_fee": 10.5,
      "overdue_days": 10
    }
    ```
  * **Response:**
      * 200 OK


## Running Tests
Unit tests are included for the domain model and business logic.

1. Run tests using the .NET CLI:
   ```sh
   dotnet test
2. Run tests using Visual Studio:
   * Open the solution in Visual Studio.
   * Run all tests from the Test Explorer.


## Assumptions
* Invoices have a pending, paid, or void status.
* Payments are applied to the total invoice amount.
* Overdue invoices are processed based on a specified late fee and overdue days.


## Future Enhancements
 * Database Integration: Add persistent storage using a relational database.
 * Authentication: Implement user authentication and authorization.
 * Enhance the payment system to handle different payment methods.

## Contributions
Contributions are welcome! Please fork the repository and create a pull request with your changes.