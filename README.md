## Drone

New Technology

### Introduction/explanation

A REST API that allows clients to communicate with the drones, The following service is exposed

- registering a drone;
- uploading medication image;
- loading a drone with medication items;
- checking loaded medication items for a given drone;
- checking available drones for loading;
- check drone battery level for a given drone;

### Requirements

- It require docker to be installed
- port 80 should be open

### How to build / run the project

```
cd into root project
docker-compose build
docker-compose up -d

```

### Note.

- after docker-compose up, you can view the service on http://localhost/swagger/index.html
- every expose service is explained on swagger.
