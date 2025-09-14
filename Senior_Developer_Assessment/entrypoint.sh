#!/bin/sh
set -e

echo "Waiting for SQL Server at $DB_HOST:$DB_PORT..."

until /opt/mssql-tools/bin/sqlcmd -S $DB_HOST,$DB_PORT -U $DB_USER -P $DB_PASS -Q "SELECT 1" >/dev/null 2>&1; do
  >&2 echo "SQL Server is unavailable - sleeping"
  sleep 2
done

>&2 echo "SQL Server is up - starting app"

exec dotnet Senior_Developer_Assessment.dll
