#!/usr/bin/env bash
# wait-for-it.sh - wait for a TCP host and port to be available

set -e

HOST="$1"
PORT="$2"
shift 2
CMD="$@"

echo "Waiting for $HOST:$PORT to be ready..."

while ! nc -z "$HOST" "$PORT"; do
  sleep 2
done

echo "$HOST:$PORT is available! Starting command..."
exec $CMD
