#!/usr/bin/env bash
# Authorized Keys script for SSH
# User: {{GitUser}}

SSH_KEY="$1"

# URL-encode function (pure Bash)
urlencode() {
    local string="${1}"
    local strlen=${#string}
    local encoded=""
    local pos c o

    for (( pos=0 ; pos<strlen ; pos++ )); do
        c=${string:$pos:1}
        case "$c" in
            [a-zA-Z0-9.~_-]) o="$c" ;;
            *) printf -v o '%%%02X' "'$c"
        esac
        encoded+="$o"
    done
    echo "$encoded"
}

KEY_ENCODED=$(urlencode "$SSH_KEY")

# Call the API to check if the key exists
RESPONSE=$(/usr/bin/curl -fsS "{{AppUrl}}/api/ssh-keys/lookup/$KEY_ENCODED")

# If API returns 200 OK, print the key
if [ -n "$RESPONSE" ]; then
    # echo "$RESPONSE"
    echo "command=\"{{ScriptPath}}/git-wrapper $SSH_KEY\",no-port-forwarding,no-X11-forwarding,no-agent-forwarding,no-pty $RESPONSE"
fi
# If API returns 404 or any error, do nothing (deny access)
