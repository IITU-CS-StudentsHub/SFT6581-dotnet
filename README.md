# Task Tracker Microservice

A Midterm Assignment for a "Task Management System" Microservice using ASP.NET Core 8


# Block 3 - Integration Design (AWS SQS)

I choose AWS SQS to connect the Task Service with the Notification Service. This is an asynchronous pattern


    1) No Server Management
				It is a managed service. I do not need to install or maintain any broker servers
    2) Data Safety
				Messages stay in the queue until they are processed. If the Notification Service is down, no data is lost
    3) Easy Scaling
				SQS handles any number of messages automatically. It works well with both small and large traffic
    4) Independence
				The Task Service and Notification Service are not connected directly. This makes the system more flexible and easier to update

When a task is completed, the Task Service sends a simple JSON message to the SQS queue. The Notification Service reads this message and sends an email


## API Endpoints

1) `GET /api/tasks` - Returns all tasks
2) `POST /api/tasks/bug` - Creates a new bug report
3) `PUT /api/tasks/{id}/complete` - Marks a task as complete and triggers the local logging handler

## Architectural Notes - Asynchronous Pattern vs Synchronous HTTP

1. Synchronous (HTTP) — Not Recommended
	It works like a phone call. The Task Service must wait for the Notification Service to answer

2. Asynchronous (Message Broker / SQS) — Recommended
	It works like sending a text message. The Task Service sends a "Task Done" message to the queue and moves on
