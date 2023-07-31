# SQL Server Message Broker Listener

This repository contains the source and scripts folders for a .NET Core 6 worker service application that implements a simple SQL Server Message Broker Listener.

## Source Folder

The source folder contains the .NET Core 6 worker service application, which is a sample implementation of SQL Server Message Broker Listener. The worker service listens for changes in a specified SQL Server table and executes actions in response to those changes.

## Scripts Folder

The scripts folder contains an SQL file that enables SQL Server Message Broker in the target database. This is a prerequisite for the worker service to function correctly. You can execute the SQL script in your SQL Server database to enable the necessary features for message broker communication.

### Usage

1. Clone or download this repository.
2. Execute the SQL script in your target SQL Server database to enable the SQL Server Message Broker feature.
3. Open the .NET Core 6 worker service application in your preferred development environment.
4. Configure the worker service application with the appropriate connection string and table to listen for changes.
5. Build and run the application to start listening for changes in the specified SQL Server table.
6. Upon changes in the table, the worker service will execute the defined actions in response.

### Contributing

Contributions are welcome! If you find any issues or have suggestions for improvements, feel free to submit a pull request or create an issue.

### License

This repository is licensed under the MIT License. See the [LICENSE](/LICENSE) file for details.
