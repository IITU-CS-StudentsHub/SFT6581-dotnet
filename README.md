# Task Tracker Microservice

A Midterm Assignment for a "Task Management System" Microservice using ASP.NET Core 8 following Clean Architecture principles.

## Structure

The solution consists of two projects demonstrating Clean Architecture:
- `TaskTracker.Core`: Contains the domain models (`BaseTask`, `BugReportTask`, `FeatureRequestTask`), enums (`SeverityLevel`), interfaces (`ITaskRepository`), and the domain logic service (`TaskFilterService`).
- `TaskTracker.Web`: Contains the REST API endpoints (`TasksController`), the infrastructure implementation (`InMemoryTaskRepository`), and Dependency Injection configurations.

## Running the Application

### Using Docker Compose
A multi-stage Dockerfile (optimized for size using Alpine) is provided.

```bash
docker-compose up --build
```
The API will be accessible at `http://localhost:5000`. Swagger UI is available at `http://localhost:5000/swagger`.

### Using .NET CLI
```bash
cd TaskTracker.Web
dotnet run
```

## API Endpoints

- `GET /api/tasks` - Returns all tasks.
- `POST /api/tasks/bug` - Creates a new bug report.
- `PUT /api/tasks/{id}/complete` - Marks a task as complete and triggers the local logging handler.

## Architectural Notes: Asynchronous Pattern vs. Synchronous HTTP

In the context of scaling a microservice architecture, consider integrating a future `NotificationService` that handles sending emails or push notifications when a task is marked as completed. 

**Using an asynchronous pattern (such as RabbitMQ, Kafka, or an Azure Service Bus) is vastly superior to making synchronous HTTP calls for the following reasons:**

1. **Decoupling:** The `TaskTracker` microservice doesn't need to know the location, API contract, or even the existence of the `NotificationService`. It simply publishes a `TaskCompletedEvent` to the message broker. 
2. **Reliability & Resilience:** If the `NotificationService` goes offline or requires maintenance, the message broker queues the events. When the service comes back online, it will process the backlog. With synchronous HTTP, the `TaskTracker` would either fail to complete the task (tight coupling of failures) or lose the notification event entirely.
3. **Performance (Latency):** Publishing an event to a message broker is incredibly fast compared to awaiting an HTTP response over the network. It frees up the threads on the `TaskTracker` API to handle more incoming requests rather than blocking to wait for the Notification Service to acknowledge the email was sent.
4. **Scalability:** If notifications require complex processing, you can horizontally scale the `NotificationService` consumers independently without changing the producer logic. Multiple services can also subscribe to the same event (e.g., an Analytics service and a Notification service) without modifying the `TaskTracker` code.
