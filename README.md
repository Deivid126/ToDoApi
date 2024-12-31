# ToDoApi

This is a ToDo API built using **C#** and **.NET** following a **Clean Architecture** approach. The API allows users to manage tasks (create, update, retrieve, and delete tasks). It includes authentication with JWT tokens and ensures the secure handling of user passwords.

## Features

- **Task Management**: Create, update, retrieve, and delete tasks.
- **User Authentication**: Create users and log in with JWT tokens.
- **Secure Password Storage**: Passwords are encrypted before storing in the database.
- **Clean Architecture**: Follows the Clean Architecture principles for maintainability and testability.
- **Docker Compose**: Easily deploy the application with Docker Compose for containerized environments.
- **Unit and Integration Tests**: The application includes tests to ensure that the functionality works as expected.

## Technologies Used

- **C#** and **.NET Core**
- **Clean Architecture**
- **Entity Framework Core** for database operations
- **JWT** for authentication and authorization
- **Docker** for containerization
- **XUnit** and **FluentAssertions** for testing

## Running the Application

### Docker Compose

To run the application with Docker Compose, follow these steps:

1. **Clone the repository**:
    ```bash
    git clone https://github.com/your-username/ToDoApi.git
    cd ToDoApi
    ```

2. **Build and start the containers**:
    ```bash
    docker-compose up --build
    ```

    This command will build the Docker images and start the containers, including the API and the database.

3. The application will be available at `https://localhost:8081`.

### Running Tests

To run the tests, ensure you have **.NET SDK** installed and follow these steps:

1. **Navigate to the test project**:
    ```bash
    cd ToDoApi.Tests
    ```

2. **Run the tests**:
    ```bash
    dotnet test
    ```

    This command will run all unit and integration tests for the project. The results will be shown in the console.

## API Documentation

You can view the complete API documentation [here](https://documenter.getpostman.com/view/22840933/2sAYJ7fJtP).

## Clean Architecture Structure

The application follows the **Clean Architecture** principles, which include:

- **Core**: Contains business logic and entities.
- **Application**: Contains use cases and services.
- **Infrastructure**: Contains implementations for repositories, external services, and database context.
- **Web API**: Exposes the application functionality via REST endpoints.

## License

This project is licensed under the MIT License.

---

Feel free to clone the repository, customize it for your own needs, and contribute!
