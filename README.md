# Realtime Database Sync

## Overview

This project is a proof of concept for a database sync between SQLServer and MySQL.

The API has a full CRUD for a `Product` entity in SQLServer
and two READ endpoints for the same entity in MySQL.

The API syncs the data from SQLServer to MySQL through a background service that runs every 1 minute.

## Setup

Make sure you have the following installed:

- .NET Core 7.0
- SQLServer
- MySQL

## Migrations

cd into the `API.DbSync` project and run the following commands:

### SQLServer

```bash
dotnet ef database update --context SQLServerDbContext
```

### MySQL

```bash
dotnet ef database update --context MySQLDbContext
```

## Running the API

cd into the `API.DbSync` project and run the following command:

```bash
dotnet run
```

open the URL where the API is running and go to `/swagger` to see the documentation and test the endpoints.

