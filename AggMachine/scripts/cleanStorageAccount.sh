#!/usr/bin/env bash

AZURE_RESOURCE_GROUP="openhack"
AZURE_STORAGE_ACCOUNT_NAME="ipopenhack"
AZURE_STORAGE_ACCOUNT_CONN=$(az storage account show-connection-string -g "$AZURE_RESOURCE_GROUP" -n "$AZURE_STORAGE_ACCOUNT_NAME" --query connectionString -o tsv)
AZURE_STORAGE_CONTAINER_LIST=$(az storage container list --connection-string "$AZURE_STORAGE_ACCOUNT_CONN" --query [].name -o tsv)

for container in "$AZURE_STORAGE_CONTAINER_LIST"; do
    echo "$container"
done
