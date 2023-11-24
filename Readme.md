# Introduction

This readme file contains a list of environment variables used in the project along with their descriptions and
examples. These variables are used to configure various aspects of the application.

## Existing Environment Variables 

| ENV name                   | Scope (minimum) | Description                                                                                                                                      | Example                                                                                                                   |
|----------------------------|-----------------|--------------------------------------------------------------------------------------------------------------------------------------------------|---------------------------------------------------------------------------------------------------------------------------|
| ASPNETCORE_ENVIRONMENT     | `Development`   | For local use set variable to `Development`<br/>For k8 testing env set variable to `Staging`<br/>For production env set variable to `Production` | "Staging"                                                                                                                 |
| POSTGRES_CONNECTION_STRING | `Development`   | Connection string for the postgres database                                                                                                      | "Server=localhost;Port=5432;Database=pandatech_website;User Id=test;Password=test;Integrated Security=true;Pooling=true;" |
| RABBITMQ_URI               | `Development`   | RabbitMQ URI for connecting to the message broker                                                                                                | "amqp://test:test@localhost:5672"                                                                                         |
| RABBITMQ_EXCHANGE_NAME     | `Development`   | Name of the RabbitMQ exchange to be used                                                                                                         | "ca_audit_trail"                                                                                                          |
| RABBITMQ_QUEUE_NAME        | `Development`   | Name of the RabbitMQ queue to be used                                                                                                            | "ca_audit_entries"                                                                                                        |
| RABBITMQ_ROUTING_KEY       | `Development`   | Routing key for RabbitMQ messages                                                                                                                | "ca_audit_entries"                                                                                                        |
| RABBITMQ_QUEUE_NAME_DLX    | `Development`   | Name of the RabbitMQ queue holding dead letters                                                                                                  | "ca_dead_audit_entries"                                                                                                   |
| RABBITMQ_ROUTING_KEY_DLX   | `Development`   | Routing key for RabbitMQ messages in dead letter's queue                                                                                         | "ca_dead_audit_entries"                                                                                                   |
| REDIS_CONNECTION_STRING    | `Development`   | Redis connection string                                                                                                                          | "localhost:6379"                                                                                                          |
| ELASTIC_SEARCH_URL         | `Staging`       | ElasticSearch URL                                                                                                                                | "http://localhost:9200"                                                                                                   |
| ELASTIC_INDEX_NAME         | `Staging`       | ElasticSearch index name                                                                                                                         | "ca_audit_trail"                                                                                                          |
| BASE36_CHARS               | `Production`    | Base36 char order which should be a secret and same with other services!                                                                         | "0123456789abcdefghijklmnopqrstuvwqyz"                                                                                    |
| CORS_ALLOWED_ORIGINS       | `Production`    | Allowed origins for CORS. This variable should be set with correct syntax show in example, which also allows wildcard syntax.                    | "https://*pandatech.it, https://scan.easypay.am"                                                                          |

Please make sure to set these environment variables according to your specific configuration before running the
application. Refer to the descriptions and examples above for guidance.

If you need to add more environment variables in the future, please update this readme file accordingly for proper
documentation.
