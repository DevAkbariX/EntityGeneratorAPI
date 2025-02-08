# Entity Generator API

Entity Generator API is a web service that automatically generates C# models from a database schema. The API supports retrieving database schema information, generating models for specific tables, and downloading all generated models as a ZIP file. It follows Clean Architecture principles to ensure a scalable and maintainable codebase.

## Features

- Retrieves database schema details.
- Generates C# models for specific database tables.
- Downloads all generated models as a ZIP file.
- Prevents duplicate model generation.
- Implements logging for tracking the process.
- Follows Clean Architecture principles for a maintainable structure.
- Uses Entity Framework Core for database interaction.

## Technologies Used
- ASP.NET Core
- Entity Framework Core for database operations
- System.IO.Compression for creating ZIP files
- Microsoft.Extensions.Logging for logging
- Clean Architecture for scalable code structure

## Setup and Installation

1. Clone the repository:

    ```bash
    git clone https://github.com/yourusername/EntityGeneratorAPI.git
    ```

2. Install dependencies:

    ```bash
    dotnet restore
    ```

3. Set up the database connection in `appsettings.json`.

4. Run the application:

    ```bash
    dotnet run
    ```

The API will now be running and accessible at `http://localhost:5000` by default.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
