In order to run this app, you need Docker installed.

= Setup =
Copy `WebToKindle\secrets\mssql_sa_password.example` to `WebToKindle\secrets\mssql_sa_password` and enter a password in `mssql_sa_password`.
Copy the existing `example.Resources.cs.example` and `example.Resources.resx.example` in the Properties directory to `Resources.Designer.cs` and `Resources.resx`.
Open the solution in Visual Studio
Modify the `DbConnectionString` in `Resources` to include the password you entered in `mssql_sa_password`
Start the project!

= Seeding data =
Once the project has started, navigate to `/api/Books/create` to insert an example book.
You can now navigate to `/api/Books/update/{id}` to download all chapters for this book.