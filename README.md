# Blogger

ASP.NET Web app for blog management using ADO.NET with SQL Server

## Setup

- Make sure you have .NET 7 SDK and SQL Server
- Create new database in SQL Server name it as "BloggerDB".
- Now restore "BloggerDB.bak" file from "Blogger.Data => App_Data".
- Replace your connection string in the appsettings.json file

```json
{
  "ConnectionStrings": {
    "BloggerDBConnection": "Data Source=[DBSOURCENAME];Initial Catalog=BloggerDB;Persist Security Info=True;User ID=[YOURUSERID];Password=[******];TrustServerCertificate=True"
  }
}
```

## Application User detail

#### User-1

- Email: nonstop@gmail.com
- Pass: 123

#### User-2

- Email: bhavin@gmail.com
- Pass: 123
