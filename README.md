# Better Prism Scheduler (BPScheduler)

## Overview
Better Prism Scheduler, also known as BPScheduler, is a sophisticated scheduling tool created for Blue Prism. It's designed to enhance the process scheduling efficiency within the Blue Prism environment. It offers scheduling based on cron, queues, and specific date/times, enabling you to manage your Blue Prism processes in a highly flexible and efficient way.

This project is licensed under the MIT License.

## Features

- A webinterface for managing day to day operations
- Queue-based scheduling with automatic scaling of processes
- Precise date/time scheduling for specific processes

## Components

The scheduler consists of a windows service that runs and monitors your Blue Prism environment. This service uses the AutomateC.exe command line utility to schedule processes inside the Blue Prism Environment.

The scheduler service also exposes a rest-api for management of the scheduler.

Lastly the scheduler package contains a web based UI for management.

## Installation

Important: The scheduler is an internal tool, it contains no security what so ever. Therefore install this and secure the ports with a firewall. Under NO CIRCUMSTANCES should this service be exposed to the internet.

### Prerequisites

In order to install the scheduler you will need:

- A Blue Prism installation using single signon (tested on versions 6.5.5 through 7.1)
- Node.js (v16.5.0 or higher)
- .NET Framework (v4.7.2 or higher)
- SQL Server (2017 or higher)
- Git

You will also need a windows service account that fullfill these criteria:

- Has permission to run processes on Blue Prism via AutomateC.exe
- Has ownership of the SQL server database for the Scheduler
- Has access to the Blue Prism database
- Has rights to open and listen on a port on the host machine

### Steps

#### Scheduler Windows Service

1. Clone the repository
    ```
    git clone https://github.com/mwulffn/betterprism-scheduler.git
    ```

2. Navigate into the cloned repository's directory
    ```bash
    cd betterprism-scheduler
    ```

3. Build the solution using MSBuild
    ```bash
    cd Scheduler
    MSBuild.exe /p:Configuration=Release Odk.Scheduler.sln
    ```

4. At this point you should edit the "odk.scheduler.exe.config" file in the "Odk.Scheduler\bin\Release" folder.

    ```xml
	<connectionStrings>
		<!-- The connection string for the schedulers database -->
		<add name="SchedulerContext" connectionString="Server=<YOUR SERVER>;Initial Catalog = <YOUR DB>;Integrated Security = true" providerName="System.Data.SqlClient" />

		<!-- The connection string for your Blue Prism database. This will only be read from -->
		<add name="BluePrism" connectionString="Server=<YOUR SERVER>;Initial Catalog = <YOUR DB>;Integrated Security = true" providerName="System.Data.SqlClient" />
	</connectionStrings>
    ```

    Also remember to set your licenses and your preferred port:

    ```xml
	<appSettings>
		<!-- This value tells the scheduler how many licenses are available for use -->
		<add key="Licenses" value="6" />

		<!-- The address on which the scheduler will listen -->
		<add key="Host" value="http://*:9000/" />
	</appSettings>
    ```


5. You can now test the service by running the windows service from the command line:
    ```bash
    Scheduler\odk.Scheduler\bin\release\odk.scheduler.exe
    ```
    Identify any problems and correct them. It is important to do this as your service account to get an accurate assesment if everything is setup properly. You may need to use a command prompt with administrator rights to open the port for listening.

6. Install the windows service:
    ```bash
    cd Scheduler\odk.Scheduler\bin\release
    Odk.Scheduler.exe install -username "YOUR USERNAME" -password "YOURPASSWORD"
    ```

7.  The service can now be controlled from the Windows services control panel.

#### Installing the scheduler frontend

1. Navigate to the 'Scheduler.ui' directory in the cloned repository and install dependencies.
    ```bash
    cd Scheduler.ui
    npm install
    ```

2. Create the config file for production:
    ```bash
    copy .env.development .env.production.local
    ```

3. Edit the .env file to set the ip address of the scheduler:
    ```javascript
    VUE_APP_APP_TITLE=Better Scheduler
    VUE_APP_SCHEDULER_API=http://your service-ip:9000/api/
    ```

4. At this point you can build the distribution using:
   
    ```bash
    npm run build
    ```

5. Copy the files from dist directory to your webserver of choice and point your browser at it. You should see the scheduler user interface.

## Troubleshooting

The process for getting the Better Prism scheduler running is rather complicated and can be daunting. Please open an issue if you believe you have encountered a bug.

We are actively working on establishing a user forum where users can help each other and provide tips and tricks.

## Contributing

We welcome contributions to Better Prism Scheduler. Before contributing, please read our [Contributing Guide](CONTRIBUTING.md).

## License

BPScheduler is licensed under the [MIT License](LICENSE.md).

## Support

If you encounter any problems or have any suggestions, please [file an issue](https://github.com/mwulffn/betterprism-scheduler/issues/new).

## Acknowledgements

We thank all the contributors who have been part of this project. 

Better Prism Scheduler would not have been possible without the support of the Municipality of Odense, Denmark. 