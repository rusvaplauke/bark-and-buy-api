# Bark And Buy API

## Overview 🐕

This project implements a RESTful API for order functionality in an online marketplace. The project is containerized using Docker for easy deployment.
The project is written in .NET8, using PostgreSQL for the database and Liquibase for migrations.

## Functionality ✨

- An order is created when a user wants to buy an item on the marketplace.
- A seller can deliver the item and mark the order as completed.
- Orders that are not paid within 2 hours of creation are automatically deleted from the system. The cleanup service runs every 2 minutes.
- A user can retrieve all of their orders.

## How to Run 🏃

1. Ensure you have Docker and Docker Compose installed on your machine.
2. Build and run the Docker containers:

    ```bash
    docker-compose up
    ```

   The API will be accessible at [http://localhost:8080/swagger](http://localhost:8080/swagger) (http) and at [https://localhost:8081/swagger](https://localhost:8081/swagger) (https).

3. To stop the containers, use:

    ```bash
    docker-compose down
    ```
   Make sure to stop the containers before making changes to the configuration or when you're done using the API.

## Assumptions 💡

- **Seeded Data**: Possible order status values are `1 - Pending`, `2 - Paid`, `3 - Delivered`, `4 - Completed`. 
- **Seeded Data**: The `seller` table is pre-populated (refer to Migration file 003_seed_data.sql for values).
- **Assumption**: There is no `item_id` in orders table or `items` table. Each seller provides only 1 item, for example, a marketplace for wedding cake taster boxes where each seller can only offer 1 type of product.

## Limitations 🧠
- Only CreateAsync method was covered with unit tests due to limited time resources.
- Integration tests where skipped due to limited time resources.
- Some values were hardcoded for faster implementation (status ids and values, pending order deletion period). They could be retrieved dynamically in order to assure easier maintenance. 
- The assumed model "1 seller - 1 item" / "1 item per order" is not flexible for general online marketplace needs. Luckily, the current model is easily extendible to account for more complex structures.
- Users could be cached in database in order to reduce the number of API calls and minimize dependency on external API.
- Additional checks could be implemented to ensure that order status changes are logical and to prevent fraud (i.e., pending -> paid -> delivered -> completed).

## Documentation & manual testing 🌻

The application can be tested using Visual Studio's HTTP files.
