# StarWars
A modular Blazor WebAssembly application displaying Star Wars data (e.g., Characters, Starships, Vehicles) from the Star Wars API (SWAPI).
It follows a clean architecture with separation into Core, Infrastructure, Presentation, and UI layers.

# Technologies Used

Blazor WebAssembly (.NET 8)

C# 12

MVVM (Model-View-ViewModel) pattern

Star Wars API (SWAPI) for data - https://swapi.info/

# Project Structure

Core: Contains interfaces and Models.

Infrastructure: Contains service implementations that call external APIs (SWAPI).

Presentation: Contains ViewModels that manage the UI logic.

UI: Contains Pages and Components.

# Steps to run the app:

# 1. Prerequisites

- Install .NET 8 SDK

- Use an IDE like:

  - Visual Studio 2022+

  - JetBrains Rider

# 2. Steps to Run
   
# Option 1: Using an IDE

- Download or Clone the repository.

- Open the solution file StarWars.sln in your IDE.

- Restore NuGet packages (automatic).

- Set the StarWarsSPA project as the startup project.

- Run the application (Ctrl+F5 or the Run button).

# Option 2: Using Terminal/Command Line

1. Clone the repository
   
- git clone https://github.com/giri2209/StarWars.git

3. Navigate to the StarWarsSPA project
   
- cd StarWars/StarWarsSPA

5. Restore the dependencies
   
- dotnet restore

7. Run the application
   
- dotnet run

# 3. Access the Application
After running, the app will open at:

- https://localhost:7288/


