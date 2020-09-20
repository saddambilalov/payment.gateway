
 The application was implemented with [.NET Core 3.1 SDK](https://github.com/dotnet/core/tree/master/release-notes/3.1) and [.NET Standard 2.0](https://docs.microsoft.com/en-us/dotnet/standard/net-standard).

## How to run locally? ##

#### Run the application as a whole.

After running the command below, the API can be accessed via http://localhost:3015/
```bash

docker-compose -f docker-compose.yml up --remove-orphans --force-recreate --renew-anon-volumes --build --abort-on-container-exit

```

#### The datastore for local development

```bash

docker-compose -f docker-compose.development.yml up --remove-orphans --force-recreate --renew-anon-volumes --build --abort-on-container-exit

```

## Decisions on the development side ##

### [The API](https://github.com/saddambilalov/payment.gateway/tree/master/src/Api/Payment.Gateway.Api)

**[The API Abstraction Layer](https://github.com/saddambilalov/payment.gateway/tree/master/src/Api/Payment.Gateway.Api.Abstractions)** 

It was created to facilitate possible integration into the system. It only contains request and response models. It can be useful when building an API client with [**Refit**](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-3.1) .

The API was documented with the **Open API (Swagger)** support.

**[The Application Logging](https://github.com/saddambilalov/payment.gateway/blob/6975ca5d89754ad246a86f540135cbc13873c119/src/Api/Payment.Gateway.Api/Program.cs#L22)**

The console logging provider was used but can be integrated with **ELK** or another external provider (e.g. [**GrayLog**](https://www.graylog.org/)).

**[The Application Metrics (APM)](https://github.com/saddambilalov/payment.gateway/blob/28f419ae25250c858edfc415d9e69d4bed74c049/src/Api/Payment.Gateway.Api/Program.cs#L18)**

[app-metrics.io](https://www.app-metrics.io/web-monitoring/aspnet-core/) was used to serve APM metrics with the url http://localhost:3015/metrics. It can be integrated into [Prometheus](https://prometheus.io/) and [Grafana](https://grafana.com/grafana/dashboards?search=app+metrics).

**[Authentication.](https://github.com/saddambilalov/payment.gateway/tree/master/src/Api/Payment.Gateway.Api/Authentication)**

It was implemented using API key logic, but the list of keys was provided by every external vault system. Saving the API keys directly is not recommended unless they are not rotated frequently during the Vault integration.

[HashiCorp's Vault,](https://www.vaultproject.io/) [Azure Key Vault](https://azure.microsoft.com/en-us/services/key-vault/), and [AWS Key Management Service](https://aws.amazon.com/kms/) may be better suited for production because of the rotation support.

The most reliable authentication system can be OAuth or JWT standards. It can be expired, updated, etc.

**[Encryption.](https://github.com/saddambilalov/payment.gateway/blob/master/src/Payment.Gateway.Infrastructure/Services/CipherService.cs)**

The card details were encrypted before being written to the database. It was serialized using BinaryFormatter. **MongoDb** also supports [client-side encryption](https://docs.mongodb.com/manual/core/security-client-side-encryption/) but it is not a good practice to be dependent on the data store that created a tightly coupled system.

**[The Bank Simulator](https://github.com/saddambilalov/payment.gateway/tree/master/simulator)**

It was implemented with Express.js to make the possible integration easy later.

When calling the bank simulator API the issues were handled with Polly [Advanced](https://github.com/App-vNext/Polly/wiki/Advanced-Circuit-Breaker) (to have a circuit breaker) and [Jitter](https://github.com/App-vNext/Polly/wiki/Retry-with-jitter) (to avoid retries bunching into further spikes of load) retry policies. To make external integration easier and clearer, [**Refit**](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-3.1) was used.

**[The Domain Layer](https://github.com/saddambilalov/payment.gateway/tree/master/src/Payment.Gateway.Domain)**

It resesents the concerpt of business and follows the [Persistence Ignorance](https://deviq.com/persistence-ignorance/) and the [Infrastructure Ignorance](https://ayende.com/blog/3137/infrastructure-ignorance) principles. The entities at this layer are [POCO](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/ddd-oriented-microservice) and do not contain an attribute.

**[The infrastructure layer](https://github.com/saddambilalov/payment.gateway/tree/master/src/Payment.Gateway.Infrastructure)**

This layer contains the implementation of data persistence and repository.

## Note. There are some important cases left in order to be implemented in the future: ##
1. Instead of feeding data from the external API, it can be better to have the publish/subscriber pattern. When something has changed on the show and cast side they can publish an event that will be executed by a subscription in the reading model. It can also be the same as the [CQRS](https://learning.oreilly.com/library/view/designing-event-driven-systems/9781492038252/ch07.html) implementation.
2. The index is missing in MongoDB.
3. The application was not covered with the unit tests as a whole due to the time limitation.
4. And so on...
