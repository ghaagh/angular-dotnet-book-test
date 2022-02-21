# Angular auto history logger including the front-end project
## Notes
- The Domain Project and Infrastructure project are implemented inside 2 Folders in application. They are completely separated from eachother.
- The SQL database dependency injection codes can be  found on 
[AddSqlExtension.cs](https://github.com/ghaagh/angular-dotnet-book-test/blob/master/Book.Application/Infrastructure/Sql/AddSqlExtension.cs). 
you can change the `Book and Author` database to anything Entity framework supports.
- the [`BookHistoryEventHandler`](https://github.com/ghaagh/angular-dotnet-book-test/blob/master/Book.Application/Infrastructure/Ado/ChangeHistory/BookHistoryHandler.cs)
is responsible for saving changed data to storage. I used ADO.NET in order to minimize the external database dependency,
but it can be changed to basically any database or file storage.
- You can find the code for extracting the changes from the entity framework in 
[ContextChangeHandler](https://github.com/ghaagh/angular-dotnet-book-test/blob/master/Book.Application/Domain/ChangeHistory/ContextChangeHandler.cs). 
It could be more generic so it would support logging changes for any table, but the time was limited.
- Exception Handling is `not` implemented.
## Version:
- Angular: `11.2.14`. 
- Angular CLI : `Angular CLI: 11.2.18`
- .NET Version: `6.0.101`
## Running the backend.
1. Select [Book.Application](https://github.com/ghaagh/angular-dotnet-book-test/tree/master/Book.Application) Project, 
2. Change connection string in [app.setting](https://github.com/ghaagh/angular-dotnet-book-test/blob/master/Book.Application/appsettings.json). 
3. Run the migration for creating an empty database. Or run
[TableAndData.sql](https://github.com/ghaagh/angular-dotnet-book-test/blob/master/Book.Application/Infrastructure/Sql/TableWithData.sql) against a newly created database to add tables and some random records.
## Running the front-end.
1. Change the address of back-end in [client-http.service.ts](https://github.com/ghaagh/angular-dotnet-book-test/blob/master/client-app/src/app/client-http.service.ts) File
2. Run `npm install`.
3. Run `ng serve` while the backend is running.


