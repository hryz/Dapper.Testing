#!/usr/bin/env bash

# source: https://github.com/microsoft/mssql-docker/issues/11#issuecomment-565158034

# Launch MSSQL and send to background
/opt/mssql/bin/sqlservr &
pid=$!
# Wait for it to be available
echo "⏳ Waiting for MS SQL to be available ⏳"
/opt/mssql-tools/bin/sqlcmd -l 30 -S localhost -h-1 -V1 -U sa -P $SA_PASSWORD -Q "SET NOCOUNT ON SELECT \"🟢 We are up 🟢\" , @@servername"
is_up=$?
while [[ ${is_up} -ne 0 ]] ; do 
  echo -e $(date) 
  /opt/mssql-tools/bin/sqlcmd -l 30 -S localhost -h-1 -V1 -U sa -P $SA_PASSWORD -Q "SET NOCOUNT ON SELECT \"🟢 We are up 🟢\" , @@servername"
  is_up=$?
  sleep 5 
done
# Run every script in /scripts
for foo in /scripts/*.sql
  do /opt/mssql-tools/bin/sqlcmd -U sa -P $SA_PASSWORD -l 30 -e -i ${foo}
done
echo "🚀 Init scripts are applied 🚀"
wait ${pid}