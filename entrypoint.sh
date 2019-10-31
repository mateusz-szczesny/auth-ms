#!/bin/bash

set -e
run_cmd="dotnet Auth.dll"

until dotnet ef database update; do
>&2 echo "SQL Database is starting up..."
sleep 1
done

>&2 echo "SQL Database is up - running server"
exec $run_cmd
