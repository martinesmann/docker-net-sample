https://docs.microsoft.com/en-us/azure/container-instances/tutorial-docker-compose

docker pull daprio/daprd:1.0.0-rc.1
docker tag daprd:1.0.0-rc.1 esmann.azurecr.io/samples/daprd:1.0.0-rc.1
docker push esmann.azurecr.io/samples/daprd:1.0.0-rc.1

notes:

Usage of ./daprd:
  -allowed-origins string
    	Allowed HTTP origins (default "*")
  -app-id string
    	A unique ID for Dapr. Used for Service Discovery and state
  -app-max-concurrency int
    	Controls the concurrency level when forwarding requests to user code (default -1)
  -app-port string
    	The port the application is listening on
  -app-protocol string
    	Protocol for the application: grpc or http (default "http")
  -app-ssl
    	Sets the URI scheme of the app to https and attempts an SSL connection
  -components-path string
    	Path for components directory. If empty, components will not be loaded. Self-hosted mode only
  -config string
    	Path to config file, or name of a configuration object
  -control-plane-address string
    	Address for a Dapr control plane
  -dapr-grpc-port string
    	gRPC port for the Dapr API to listen on (default "50001")
  -dapr-http-port string
    	HTTP port for Dapr API to listen on (default "3500")
  -dapr-internal-grpc-port string
    	gRPC port for the Dapr Internal API to listen on
  -enable-mtls
    	Enables automatic mTLS for daprd to daprd communication channels
  -enable-profiling
    	Enable profiling
  -kubeconfig string
    	(optional) absolute path to the kubeconfig file (default "/.kube/config")
  -log-as-json
    	print log as JSON (default false)
  -log-level string
    	Options are debug, info, warning, error, or fatal (default "info")
  -metrics-port string
    	The port for the metrics server (default "9090")
  -mode string
    	Runtime mode for Dapr (default "standalone")
  -placement-host-address string
    	Addresses for Dapr Actor Placement servers
  -profile-port string
    	The port for the profile server (default "7777")
  -sentry-address string
    	Address for the Sentry CA service
  -version
    	Prints the runtime version