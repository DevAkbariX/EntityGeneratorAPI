Entity Generator API
This is an ASP.NET Core API designed to automate the process of generating C# models from a database schema. The API exposes endpoints for retrieving database schema details, generating models for specific tables, and downloading a collection of models as a ZIP file. It leverages Clean Architecture principles for maintainability and scalability.

Features:
Retrieve Database Schema: Get a list of tables and their structures from the database.
Generate Model for a Specific Table: Generate a C# model based on the structure of a given database table.
Download All Models: Download all generated models as a ZIP file for easy integration into a C# project.
Endpoints:
GET api/generator/schema: Retrieves the database schema.
GET api/generator/GetFullTables: Retrieves detailed information about the tables in the database.
GET api/generator/generate-model/{tableName}: Generates C# model code for the specified table.
GET api/generator/download-all-models/{namespaceName}: Downloads all generated models as a ZIP file with the given namespace.
Technologies Used:
ASP.NET Core for building the API.
Clean Architecture for scalable and maintainable code.
Entity Framework Core for interacting with the database.
How to Use:
Clone the repository to your local machine.
Set up your database connection in the appsettings.json file.
Run the application and access the API through the defined endpoints.
