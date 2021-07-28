FROM redis:latest
# Copy dapr configurations and components files to image
# removing the need for a fileshare
WORKDIR /redis
COPY . .
WORKDIR /data