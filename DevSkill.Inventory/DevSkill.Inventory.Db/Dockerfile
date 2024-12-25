# Stage 1: Build wrapper.so
FROM debian:buster-slim AS build0
WORKDIR /root

# Update sources and install required packages
RUN apt-get update && apt-get install -y binutils gcc

# Add the C source file to the container
ADD wrapper.c /root/

# Compile the wrapper
RUN gcc -shared -ldl -fPIC -o wrapper.so wrapper.c

# Stage 2: Run SQL Server with the compiled wrapper
FROM mcr.microsoft.com/mssql/server:2019-latest
WORKDIR /root

# Copy the compiled wrapper.so from the build stage
COPY --from=build0 /root/wrapper.so /root/

# Set LD_PRELOAD and start SQL Server
CMD ["bash", "-c", "LD_PRELOAD=/root/wrapper.so /opt/mssql/bin/sqlservr"]
