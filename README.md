# Crezco Technical Challenge

Design a micro-service that would use any of the 3rd party location-from-IP address service. The service that you’ll build must expose a REST interface.
- Service should maintain a cache
- Service should maintain a persistent store of the looked-up values
- Unit tests
- Integration tests

## Test Dependencies
- [Docker](https://www.docker.com/)
- [Docker Compose](https://docs.docker.com/compose/)

Spin up the dependencies via docker-compose.
```bash
docker-compose up
```
Then run the tests
```bash
dotnet test
```