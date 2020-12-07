https://docs.microsoft.com/en-us/azure/container-instances/tutorial-docker-compose

docker pull daprio/daprd:1.0.0-rc.1
docker tag daprd:1.0.0-rc.1 esmann.azurecr.io/samples/daprd:1.0.0-rc.1
docker push esmann.azurecr.io/samples/daprd:1.0.0-rc.1

