# Inter Service Transaction

Test task demonstrating communication between microservices using:

* .NET
* MassTransit
* RabbitMQ
* gRPC

## Architecture

Client sends a REST request to Service A.

Service A starts a transaction and asynchronously communicates with Service A Worker and Service B through RabbitMQ.

After both responses are received, Service A synchronously calls Service C via gRPC.

## Services

### Service A

* REST API
* Transaction orchestration
* RabbitMQ producer/consumer
* gRPC client

### Service B

* RabbitMQ consumer
* Returns transaction result

### Service C

* gRPC server
* Returns validation result

## Technologies

* ASP.NET Core
* MassTransit
* RabbitMQ
* gRPC
* Docker Compose

## Run

```bash
docker compose up -d
```

Then start:

* ServiceA
* ServiceB
* ServiceC
