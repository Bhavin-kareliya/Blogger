
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

## Screenshots
| ![image](https://github.com/Bhavin-kareliya/Blogger/assets/54073491/7295ceb7-81b0-488c-8f2f-6b65e8124051) |
|:--:| 
| *WEB API* |

| ![image](https://github.com/Bhavin-kareliya/Blogger/assets/54073491/1a4df396-cc36-4d6d-a592-05264046747d) |
|:--:| 
| *API Authorization* |

| ![image](https://github.com/Bhavin-kareliya/Blogger/assets/54073491/9960f154-1f23-44d1-8368-8809866727f7) |
|:--:| 
| *List of posts for everyone (Only published posts)* |

| ![image](https://github.com/Bhavin-kareliya/Blogger/assets/54073491/ac0c6192-f54a-4c3c-b4bd-e7d6147016e6) |
|:--:| 
| *Login page with validations* |

| ![image](https://github.com/Bhavin-kareliya/Blogger/assets/54073491/251a3e22-7970-40d2-9fd7-abdb4da6b52a) |
|:--:| 
| *User dashboard for post management (Published and draft both)* |

| ![image](https://github.com/Bhavin-kareliya/Blogger/assets/54073491/9dc003ba-1516-418f-bd42-53c8fe4c6e36) |
|:--:| 
| *Image post* |

![image](https://github.com/Bhavin-kareliya/Blogger/assets/54073491/20a6fefb-0fcd-433b-8389-f54a5ff10368)
|:--:| 
| *Video post* |


| ![image](https://github.com/Bhavin-kareliya/Blogger/assets/54073491/24508fe2-9b05-48e3-9388-2e5eb9ceb536) |
|:--:| 
| *Create/Edit post* |

| ![image](https://github.com/Bhavin-kareliya/Blogger/assets/54073491/4d5b1fb0-1c73-47bf-8b3d-affaf78985ca) |
|:--:| 
| *404 Error page* |

