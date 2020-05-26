#!/usr/bin/env bash

# READ THIS BEFORE RUNNING!!!!!!!!!!!
# This will delete everything in a storage account... use at your own risk.

# Set storage account specific parameters
AZURE_RESOURCE_GROUP="openhack"
AZURE_STORAGE_ACCOUNT_NAME="ipopenhack"
AZURE_STORAGE_ACCOUNT_CONN=$(az storage account show-connection-string -g "$AZURE_RESOURCE_GROUP" -n "$AZURE_STORAGE_ACCOUNT_NAME" --query connectionString -o tsv)

# Delete list of containers in storage account
for container in $(az storage container list --connection-string "$AZURE_STORAGE_ACCOUNT_CONN" --query [].name -o tsv); do
    echo "Deleting container: $container"
    az storage container delete --name "$container" --connection-string "$AZURE_STORAGE_ACCOUNT_CONN"
done

# Delete list of tables in storage account
for table in $(az storage table list --connection-string "$AZURE_STORAGE_ACCOUNT_CONN" --query [].name -o tsv); do
    echo "Deleting table: $table"
    az storage table delete --name "$table" --connection-string "$AZURE_STORAGE_ACCOUNT_CONN"
done

# Delete list of queues in storage account
for queue in $(az storage queue list --connection-string "$AZURE_STORAGE_ACCOUNT_CONN" --query [].name -o tsv); do
    echo "Deleting queue: $queue"
    az storage queue delete --name "$queue" --connection-string "$AZURE_STORAGE_ACCOUNT_CONN"
done
