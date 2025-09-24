#!/usr/bin/env bash
# Git wrapper script — enforces repo/user permissions
# Arguments: <key-blob> (passed from authorized-keys)

KEY_BLOB="$1"
GIT_COMMAND="$SSH_ORIGINAL_COMMAND"

if [ -z "$GIT_COMMAND" ]; then
    echo "No command provided." >&2
    exit 1
fi

ACTION=$(echo "$GIT_COMMAND" | awk '{print $1}')
REPO=$(echo "$GIT_COMMAND" | awk -F"'" '{print $2}')

# Extract repository path (second argument to git-xxx-pack)
REPO_NAME="$(basename "$REPO")"
REPO_NAME_WO_EXT="$(basename "$REPO" .git)"
REAL_PATH="{{RepoRoot}}/$REPO_NAME"

# Call API to authorize this action
API_URL="{{AppUrl}}/api/git/authorize"

RESPONSE=$(/usr/bin/curl -s -o /dev/null \
    -w "%{http_code}" \
    -X POST "$API_URL" \
    -H "Content-Type: application/json" \
    -d "{\"keyBlob\":\"$KEY_BLOB\",\"action\":\"$ACTION\",\"repositoryName\":\"$REPO_NAME_WO_EXT\"}")

if [ "$RESPONSE" -eq 200 ]; then
    exec "$ACTION" "$REAL_PATH"
else
    echo "Permission denied for $ACTION on $REPO_NAME" >&2
    exit 1
fi
