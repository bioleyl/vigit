#!/usr/bin/env bash
GIT_USER="{{GitUser}}"
USER=$(whoami)
COMMAND="$SSH_ORIGINAL_COMMAND"

if [[ "$USER" != "$GIT_USER" ]]; then
    echo "Access denied: only $GIT_USER can execute this"
    exit 1
fi

if [[ -z "$COMMAND" ]]; then
    echo "No command provided"
    exit 1
fi

# Call the app to authorize this command
RESPONSE=$(curl -s -o /dev/stderr -w "%{http_code}" \
    -X POST "{{AppUrl}}/api/ssh/authorize" \
    -H "Content-Type: application/json" \
    -d "{\"user\":\"$USER\",\"command\":\"$COMMAND\"}")

if [[ "$RESPONSE" != "200" ]]; then
    echo "Access denied by policy"
    exit 1
fi

# Execute the Git command if allowed
exec $COMMAND
