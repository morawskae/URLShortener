# URL Shortener API

A REST API for a URL shortening service built with **ASP.NET Core Minimal API** and **.NET 8**.

The API allows users to create shortened URLs, redirect to original URLs using generated short codes, retrieve URL details and click statistics, and delete shortened URLs.

The project focuses on backend development using **Minimal APIs, Entity Framework Core, SQLite, DTOs, service-layer architecture, validation, URL expiration, click tracking, and automated testing**.

## Built With

* .NET 8
* ASP.NET Core Minimal API
* Entity Framework Core
* SQLite
* Swagger / Swashbuckle
* xUnit
* ASP.NET Core Integration Testing

## Getting Started

### Prerequisites

Make sure you have the following installed:

* [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
* Git

Check your installed .NET version:

```bash
dotnet --version
```

### Installation

Clone the repository:

```bash
git clone <repository-url>
cd URLShortener
```

Restore the project dependencies:

```bash
dotnet restore
```

Apply the Entity Framework Core migrations:

```bash
dotnet ef database update --project src/UrlShortenerApi
```

Start the API:

```bash
dotnet run --project src/UrlShortenerApi
```

The API will be available at the URL shown in the terminal.

Swagger UI is available at:

```text
http://localhost:<port>/swagger
```

## API Usage

The API can be tested using **Swagger**, **Postman**, `curl`, or another HTTP client.

### Create a Short URL

```http
POST /api/urls
```

Request:

```json
{
  "originalUrl": "https://example.com"
}
```

Optional expiration date:

```json
{
  "originalUrl": "https://example.com",
  "expiresAt": "2026-12-31T23:59:59Z"
}
```

Successful requests return:

```text
201 Created
```

with the created short URL information.

### Redirect Using a Short URL

```http
GET /{shortCode}
```

For example:

```text
GET /aB3xYz
```

If the short code exists and has not expired, the API redirects the client to the original URL.

A successful redirect returns:

```text
302 Found
```

Each successful redirect also increments the URL's click count.

### Get URL Details

```http
GET /api/urls/{shortCode}
```

Example response:

```json
{
  "id": "00000000-0000-0000-0000-000000000000",
  "shortCode": "aB3xYz",
  "originalUrl": "https://example.com",
  "createdAt": "2026-10-08T10:00:00Z",
  "expiresAt": null,
  "clickCount": 3
}
```

Returns:

* `200 OK` when the short URL exists
* `404 Not Found` when the short code does not exist

### Delete a Short URL

```http
DELETE /api/urls/{shortCode}
```

Returns:

```text
204 No Content
```

## API Endpoints

| Method   | Endpoint                | Description                         |
| -------- | ----------------------- | ----------------------------------- |
| `POST`   | `/api/urls`             | Create a shortened URL              |
| `GET`    | `/{shortCode}`          | Redirect to the original URL        |
| `GET`    | `/api/urls/{shortCode}` | Retrieve URL details and statistics |
| `DELETE` | `/api/urls/{shortCode}` | Delete a shortened URL              |

## Validation

The API validates URLs before creating a shortened URL.

The following rules apply:

* The original URL is required.
* The URL must be an absolute URL.
* Only `HTTP` and `HTTPS` URLs are accepted.
* The expiration date is optional.
* If an expiration date is provided, it must be in the future.
* Expired URLs cannot be used for redirects.

For example, this request is invalid:

```json
{
  "originalUrl": "not-a-valid-url"
}
```

The API returns:

```text
400 Bad Request
```

## Features

* Create shortened URLs
* Generate unique six-character short codes
* Redirect using short codes
* Validate original URLs
* Support HTTP and HTTPS URLs
* Optional URL expiration
* Prevent expired URLs from being redirected
* Track click counts
* Retrieve URL details
* Delete shortened URLs
* SQLite persistence
* Entity Framework Core migrations
* DTO-based API responses
* Swagger API documentation
* Service-layer architecture
* Automated unit/service tests
* API integration tests

## Architecture

The application follows a simple layered architecture:

```text
HTTP Request
     │
     ▼
Minimal API Endpoints
     │
     ▼
Service Layer
     │
     ▼
Entity Framework Core
     │
     ▼
SQLite Database
```

### Endpoints

The endpoint layer is responsible for handling HTTP requests and responses.

Examples:

* Reading route parameters
* Reading request DTOs
* Returning HTTP status codes
* Returning redirects

### Services

The service layer contains the application's business logic.

For example:

* URL validation
* Short-code generation
* Expiration checks
* Click counting
* Creating and deleting shortened URLs

### DTOs

DTOs are used to define the API request and response contracts separately from the database entities.

This prevents the database model from becoming the direct API contract.

### Entity Framework Core

Entity Framework Core handles communication between the application and the SQLite database.

The database schema is configured through `URLShortenerDbContext`.

## Database

The application uses **SQLite** for local persistence.

The main entity is `ShortUrl`, which contains:

| Property      | Description                    |
| ------------- | ------------------------------ |
| `Id`          | Unique identifier              |
| `ShortCode`   | Generated short URL code       |
| `OriginalUrl` | Original destination URL       |
| `CreatedAt`   | Creation timestamp             |
| `ExpiresAt`   | Optional expiration timestamp  |
| `ClickCount`  | Number of successful redirects |

The `ShortCode` column has a unique database index to prevent duplicate short codes.

## Database Migrations

Create a new migration:

```bash
dotnet ef migrations add <MigrationName> --project src/UrlShortenerApi
```

Apply migrations:

```bash
dotnet ef database update --project src/UrlShortenerApi
```

## Testing

Run all tests from the repository root:

```bash
dotnet test
```

The project contains both service-level tests and API integration tests.

### Service Tests

Service tests verify business logic such as:

* URL validation
* Expiration validation
* Short-code generation
* Redirect behavior
* Click-count updates
* URL details
* URL deletion

### Integration Tests

Integration tests use `WebApplicationFactory` to test the API through HTTP requests.

They cover scenarios such as:

* Creating a valid short URL
* Invalid URL requests
* Invalid expiration dates
* Successful redirects
* Invalid short codes
* Retrieving URL details
* Deleting URLs

## Project Structure

```text
URLShortener/
├── src/
│   └── UrlShortenerApi/
│       ├── Data/
│       │   └── URLShortenerDbContext.cs
│       ├── Dtos/
│       │   ├── RequestDto.cs
│       │   ├── ResponseDto.cs
│       │   └── ShortUrlDetailsDto.cs
│       ├── Endpoints/
│       │   └── ShortUrlEndpoints.cs
│       ├── Models/
│       │   └── ShortUrl.cs
│       ├── Services/
│       │   ├── IShortUrlService.cs
│       │   └── ShortUrlService.cs
│       ├── Migrations/
│       ├── Program.cs
│       ├── appsettings.json
│       └── README.md
│
└── tests/
    └── UrlShortenerTests/
        ├── IntegrationTests/
        ├── ShortUrlServiceTests.cs
        └── TestDbContextFactory.cs
```


