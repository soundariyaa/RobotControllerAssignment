# RobotControllerAssignment

This project is developed using below tools and software libraries

1. Visual studio 2022
2. Asp.Net Core
3. C#
4. (.Net) 8.0
5. Microsoft SQL Server Management Studio 20.2.30.0

## SQL Table Creation
### Database Name : RobotDB

- Queries for table Creation
- Rooms, Robots

```
CREATE TABLE Rooms (
  Id INT PRIMARY KEY IDENTITY(1,1),
  Width Int NOT NULL,
  Height INT NOT NULL);

  CREATE TABLE Robots (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL DEFAULT 'Robo1',
    RoomId INT NOT NULL,
    X INT NOT NULL,
    Y INT NOT NULL,
    Facing NVARCHAR(1) NOT NULL CHECK (Facing IN ('N', 'E', 'S', 'W')),   
    ExecutedAt DATETIME2 NOT NULL,

    CONSTRAINT FK_Robots_Rooms FOREIGN KEY (RoomId)
        REFERENCES Rooms(Id)
        ON DELETE CASCADE
);

```

## To Run the Project 
- Copy your Sql connectionstring and change it in appsettings.json
    - example : 

    ```
     "ConnectionStrings": {
   "DefaultConnection": "Server=<Name>;Database=RobotDB;Integrated Security=True;TrustServerCertificate=True;"
    },

   ```
- Build and run the project

## Example RestApi request for api/RobotExecution/SaveRobot 

```
{
 "name": "Robo1",
  "roomId": 1,
  "x": 0,
  "y": 0,
  "facing": "E",
  "room": {
    "id": 1,
    "width": 5,
    "height": 5
  },
  "commands": "RFLFFLRF"
}
```
### Run Unit test

- Click the play button to run the test cases in Test Explorer in Visual studio 
- or Open the test project in the command prompt and use "dotnet test" command to run the Unit test

- Example:
```
C:\Users\sound\source\repos\RobotControllerAssignment\RobotControllerApi\RobotControllerApi.Core.Test>dotnet test 
```

### Improvements needed:

- Separate endpoint has to be created to saving Robots and ExecuteCommands, Robot can have access to many rooms based on the UI Requorements. 
- Create separate table to store the reports that capture the robot's final position and facing direction after command execution.
- SQL connection string is visible in the "appsettings.json" , It should be stored either in SecretManager or in Azure Keyvalut 
- TurnLeft,TrunRight,MoveForward Functions in RobotService has to moved to seperate classes 

### Additional Tasks
-  Postman test collections to perform the automated functional test can be added 