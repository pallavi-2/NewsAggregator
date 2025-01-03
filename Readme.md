﻿## News Aggregator App 📊
### Overview
News aggregator is an application that displays the news articles according to the user desired categories. The user can save a list of their interest and the articles will be displayed accordingly. The user can also save the articles so they can be viewd later on. The news articles are obtained from [CurrentsApi](https://currentsapi.services/en)
### Features 🌟  
| Feature              | Description                                      |
|----------------------|--------------------------------------------------|
| Get Articles         | Get the articles according to your saved interest    |
| Add Interest         | Add the categories from which you want your articles |
| Save Articles        | Save the articles to be read later                   |
| Get All Articles     | Get all the articles                                 |

### Getting Started  
Follow these steps to set up the app locally.

### Prerequisites  
- .NET 6.0+  
- SQL Server  
- Api from NewsApi 

### Restore Dependencies
Restore NuGet packages required by the application:

dotnet restore

### Configure the Database
Set up the database connection string in appsettings.json:

"ConnectionStrings": {
    "DefaultConnection": "YourDatabaseConnectionStringHere"
}