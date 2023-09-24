#!/usr/bin/env bash

# Setup Local Kind Cluster using config file
kind create cluster --config kind-config.yaml# sleep for 2 minutes

# Set kubectl context to kind cluster
kubectl cluster-info --context kind-kind

# Add eraser repo and update
helm repo add eraser https://eraser-dev.github.io/eraser/charts
helm repo update

# Install eraser with with scanner configured for critical and runs every minute
helm install -n eraser-system eraser eraser/eraser --create-namespace --values ./values-default.yaml

# Image with Critical CVE
trivy image alpine:3.7.3

# Deploy alpine:3.7.3 to kind cluster
kubectl run alpine-cve --image=alpine:3.7.3 --restart=Never

# Check if pod is running
kubectl get pods

# Show image is on kind cluster
docker exec kind-worker ctr -n k8s.io images list | awk '{print $1}' | grep alpine

# Remove pod
kubectl delete pod alpine-cve

# sleep for 2 minutes
for i in {120..1}; do
    echo "Sleeping for $i seconds..."
    sleep 1
done
echo "Done sleeping."

# Show image has been removed from kind cluster 
docker exec kind-worker ctr -n k8s.io images list | awk '{print $1}' | grep alpine

# =====================
# Test 2: No Critical CVE

# Image without Critical CVE, latest as of 20230924
trivy image alpine:3.18.3

# Deploy alpine:3.7.3 to kind cluster
kubectl run alpine-no-cve --image=alpine:3.18.3 --restart=Never

# Check if pod is running
kubectl get pods

# Show image is on kind cluster
docker exec kind-worker ctr -n k8s.io images list | awk '{print $1}' | grep alpine

# Remove pod
kubectl delete pod alpine-no-cve

# sleep for 2 minutes
for i in {120..1}; do
    echo "Sleeping for $i seconds..."
    sleep 1
done
echo "Done sleeping."

# Show image has not been removed from kind cluster 
docker exec kind-worker ctr -n k8s.io images list | awk '{print $1}' | grep alpine

# Remove helm release of eraser
helm uninstall -n eraser-system eraser

# =====================
# Test 3: Both Images, both removed, no scanner configured

# Install eraser with no scanner configured to delete every minute
helm install -n eraser-system eraser eraser/eraser --create-namespace --values ./values-no-scanner.yaml

# Deploy alpine:3.7.3 to kind cluster
kubectl run alpine-cve --image=alpine:3.7.3 --restart=Never

# Deploy alpine:3.7.3 to kind cluster
kubectl run alpine-no-cve --image=alpine:3.18.3 --restart=Never

# Check if both pods are running
kubectl get pods

# Show image is on kind cluster
docker exec kind-worker ctr -n k8s.io images list | awk '{print $1}' | grep alpine

# Remove pod
kubectl delete pod alpine-cve

# Remove pod
kubectl delete pod alpine-no-cve

# sleep for 2 minutes
for i in {120..1}; do
    echo "Sleeping for $i seconds..."
    sleep 1
done
echo "Done sleeping."

# Show both images have been removed from kind cluster 
docker exec kind-worker ctr -n k8s.io images list | awk '{print $1}' | grep alpine

# Delete kind cluster
kind delete cluster
