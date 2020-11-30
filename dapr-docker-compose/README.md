
## The sample
This sample start a solution with the following components:

* website (+ dapr sitecar) 
* web api (+ dapr sitecar)
* dapr placement service
* redis
* MSSQL Server

The solution shows how to use pub/sub with redis, MSSQL, .NET 5 and docker-compose.

## Before running the sample:
This sample uses docker-compose to create the solution and all needed dapr components... therefore you need to uninstall the default docker components created when `dapr init` was called.

Uninstall dapr:
`dapr uninstall --all`

## Running the sample

`docker-compose up --build`

Website:
https://localhost

Web Api (swagger): 
http://localhost:5000

## Uninstall/remove sample app

`docker-compose down --remove-orphans`

(Please note that this does not remove the containers/images from docker locally)
