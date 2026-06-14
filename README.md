## Використані технології

- **ASP.NET Core** — REST API та gRPC сервіси.
- **RabbitMQ** — брокер повідомлень для асинхронної взаємодії між сервісами.
- **MassTransit** — робота з RabbitMQ через Producer та Consumer.
- **gRPC** — синхронна взаємодія між сервісами.
- **Scalar** — тестування REST endpoint та перегляд OpenAPI документації.


## Потік виконання

1. Клієнт викликає REST endpoint:

```http
POST /api/transaction/start
```

2. ServiceA створює нову транзакцію та CorrelationId.
3. ServiceA відправляє повідомлення `StartACommand` та `StartBCommand` через RabbitMQ.
4. Сервіси A та B обробляють свої команди та відправляють події завершення.
5. Після отримання підтвердження від обох сервісів виконується gRPC виклик до ServiceC.
6. ServiceC повертає результат валідації.
7. ServiceA повертає фінальний результат через REST API.

## Результат виконання

Після завершення обробки повідомлень RabbitMQ та виклику ServiceC через gRPC, ServiceA повертає результат транзакції через REST API.

Приклад відповіді:

```json
{
  "correlationId": "42ebd684-f0f4-4de0-bdb3-6f3af4b5090b",
  "success": true
}
```

## Запуск проєкту

### RabbitMQ

```bash
docker-compose up -d
```

### Сервіси

```bash
dotnet run --project ServiceA
dotnet run --project ServiceB
dotnet run --project ServiceC
```

### Тестування

Відкрити Scalar UI та виконати запит:

```http
POST /api/transaction/start
```