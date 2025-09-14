# MotoGP API Documentation

## Overview
This document provides comprehensive documentation for the MotoGP APIs implemented using Clean Architecture principles with CQRS pattern and Carter minimal APIs.

## Base URL
```
https://localhost:5258/api/carter/v1/motogp
```

## Authentication
All MotoGP endpoints require authorization. Include a valid JWT token in the Authorization header:
```
Authorization: Bearer <your-jwt-token>
```

## Rider Management API

### Create Rider
**POST** `/riders`

Creates a new MotoGP rider.

**Request Body:**
```json
{
  "firstName": "Valentino",
  "lastName": "Rossi",
  "racingNumber": 46,
  "countryCode": "IT",
  "countryName": "Italy",
  "countryFlag": "🇮🇹",
  "dateOfBirth": "1979-02-16T00:00:00Z",
  "height": 181.0,
  "weight": 67.0,
  "nickname": "The Doctor"
}
```

**Response:** `201 Created`

### Get Riders (Paginated with Filtering)
**GET** `/riders?searchTerm=rossi&isActive=true&countryCode=IT&pageIndex=1&pageSize=10&sortColumn=lastName&sortOrder=asc`

Retrieves paginated riders with optional filtering and sorting.

**Query Parameters:**
- `searchTerm` (optional): Search in name or nickname
- `isActive` (optional): Filter by active status
- `countryCode` (optional): Filter by country code
- `teamId` (optional): Filter by current team
- `pageIndex` (default: 1): Page number
- `pageSize` (default: 10): Items per page
- `sortColumn` (optional): Sort column (firstName, lastName, racingNumber, dateOfBirth)
- `sortOrder` (optional): Sort order (asc, desc)

**Response:** `200 OK`
```json
{
  "items": [
    {
      "id": "12345678-1234-1234-1234-123456789012",
      "firstName": "Valentino",
      "lastName": "Rossi",
      "fullName": "Valentino Rossi",
      "racingNumber": 46,
      "countryCode": "IT",
      "countryName": "Italy",
      "countryFlag": "🇮🇹",
      "dateOfBirth": "1979-02-16T00:00:00Z",
      "age": 44,
      "currentTeamId": null,
      "currentTeamName": null,
      "nickname": "The Doctor",
      "photo": null,
      "height": 181.0,
      "weight": 67.0,
      "isActive": false,
      "debutDate": "1996-08-31T00:00:00Z",
      "retirementDate": "2021-11-14T00:00:00Z",
      "yearsInMotoGP": 25,
      "isCurrentlyInTeam": false,
      "createdDate": "2024-01-10T10:00:00Z",
      "modifiedDate": null
    }
  ],
  "pageIndex": 1,
  "pageSize": 10,
  "totalCount": 1,
  "hasPreviousPage": false,
  "hasNextPage": false
}
```

### Get Rider by ID
**GET** `/riders/{id}`

**Response:** `200 OK` with rider details or `404 Not Found`

### Get Rider by Racing Number
**GET** `/riders/racing-number/{racingNumber}`

**Response:** `200 OK` with rider details or `404 Not Found`

### Update Rider Personal Info
**PUT** `/riders/{id}/personal-info`

**Request Body:**
```json
{
  "firstName": "Valentino",
  "lastName": "Rossi",
  "nickname": "The Doctor",
  "height": 182.0,
  "weight": 68.0
}
```

### Transfer Rider to Team
**PUT** `/riders/{id}/transfer`

**Request Body:**
```json
{
  "teamId": "87654321-4321-4321-4321-210987654321",
  "seasonId": "11111111-1111-1111-1111-111111111111",
  "joinDate": "2024-01-01T00:00:00Z"
}
```

### Retire Rider
**PUT** `/riders/{id}/retire`

**Request Body:**
```json
{
  "retirementDate": "2024-11-17T00:00:00Z"
}
```

### Rider Comeback
**PUT** `/riders/{id}/comeback`

Brings a retired rider back to active competition.

### Delete Rider
**DELETE** `/riders/{id}`

## Team Management API

### Create Team
**POST** `/teams`

**Request Body:**
```json
{
  "name": "Yamaha Factory Racing",
  "shortName": "Yamaha",
  "countryCode": "JP",
  "countryName": "Japan",
  "countryFlag": "🇯🇵",
  "foundedYear": "1955-01-01T00:00:00Z",
  "description": "Official Yamaha MotoGP factory team"
}
```

### Get Teams (Paginated with Filtering)
**GET** `/teams?searchTerm=yamaha&isActive=true&countryCode=JP&pageIndex=1&pageSize=10`

**Query Parameters:**
- `searchTerm` (optional): Search in name, short name, or description
- `isActive` (optional): Filter by active status
- `countryCode` (optional): Filter by country code
- `pageIndex` (default: 1): Page number
- `pageSize` (default: 10): Items per page
- `sortColumn` (optional): Sort column (name, shortName, foundedYear)
- `sortOrder` (optional): Sort order (asc, desc)

### Get Team by ID
**GET** `/teams/{id}`

### Get Team with Riders
**GET** `/teams/{id}/with-riders`

Returns team details including current riders.

## Response Patterns

### Success Response
All successful operations return appropriate HTTP status codes:
- `200 OK`: Successful retrieval
- `201 Created`: Successful creation
- `404 Not Found`: Resource not found

### Error Response
```json
{
  "title": "Validation Error",
  "type": "RacingNumber.NotAvailable",
  "detail": "Racing number 46 is already taken",
  "status": 400,
  "errors": [
    {
      "code": "RacingNumber.NotAvailable",
      "message": "Racing number 46 is already taken"
    }
  ]
}
```

## Domain Business Rules

### Riders
- Racing numbers must be between 1-99 and unique
- Minimum age for MotoGP is 16 years
- Riders can only be in one team at a time
- Retired riders can make comebacks
- Height and weight must be positive values

### Teams
- Team names and short names must be unique
- Teams can have maximum 2 riders (typical MotoGP limit)
- Inactive teams cannot have riders added

## Integration Examples

### Create and Transfer Rider Workflow
```bash
# 1. Create a new rider
curl -X POST "https://localhost:5258/api/carter/v1/motogp/riders" \
  -H "Authorization: Bearer $JWT_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "firstName": "Marc",
    "lastName": "Marquez",
    "racingNumber": 93,
    "countryCode": "ES",
    "countryName": "Spain",
    "countryFlag": "🇪🇸",
    "dateOfBirth": "1993-02-17",
    "height": 168.0,
    "weight": 59.0,
    "nickname": "MM93"
  }'

# 2. Create a team
curl -X POST "https://localhost:5258/api/carter/v1/motogp/teams" \
  -H "Authorization: Bearer $JWT_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Repsol Honda Team",
    "shortName": "Honda",
    "countryCode": "JP",
    "countryName": "Japan",
    "countryFlag": "🇯🇵",
    "foundedYear": "1982-01-01",
    "description": "Official Honda MotoGP factory team"
  }'

# 3. Transfer rider to team
curl -X PUT "https://localhost:5258/api/carter/v1/motogp/riders/{riderId}/transfer" \
  -H "Authorization: Bearer $JWT_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "teamId": "{teamId}",
    "seasonId": "{seasonId}",
    "joinDate": "2024-01-01"
  }'
```

## Architecture Notes

This implementation follows:
- **Clean Architecture** with clear separation of concerns
- **CQRS** pattern for command/query separation
- **Domain-Driven Design** with rich domain models
- **Result Pattern** for explicit error handling
- **Carter Minimal APIs** for lightweight endpoint definitions
- **AutoMapper** for DTO transformations
- **FluentValidation** ready for input validation
- **Authorization** requirements on all endpoints

## Extending the API

The foundation supports easy extension for:
- Race management endpoints
- Season management endpoints  
- Championship standings
- Statistics and analytics
- Real-time race updates
- Media management (photos, videos)