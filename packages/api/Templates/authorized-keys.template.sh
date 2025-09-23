#!/usr/bin/env bash
# Authorized Keys script for SSH
# User: {{GitUser}}

SSH_BLOB="$1"

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

BLOB_ENCODED=$(urlencode "$SSH_BLOB")

RESPONSE=$(curl -fsS "{{AppUrl}}/api/ssh-keys/lookup/$BLOB_ENCODED")

if [ -n "$RESPONSE" ]; then
    echo "$RESPONSE"
fi
