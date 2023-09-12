# Highly Available Azure Arc-enabled AKS Hybrid Messaging

[![Build on pull to main](https://github.com/ianlcurtis1/p-ha-aks-hybrid-poc/actions/workflows/build.yml/badge.svg)](https://github.com/ianlcurtis1/p-ha-aks-hybrid-poc/actions/workflows/build.yml)

# Introduction 
AKS Hybrid (https://learn.microsoft.com/en-us/azure/aks/hybrid/) is an on-premise implementation of Azure Kubernetes Service (AKS) orchestrator which automates running containerised applications at scale. It provides a solution for hosting highly available workloads on-premise. Azure Arc (https://learn.microsoft.com/en-us/azure/azure-arc/overview) is a cloud based control plane which can be used for managing on-premise AKS Hybrid instances.


This is a simple dotnet solution to demonstrate a highly available Azure Arc-enabled AKS Hybrid configuration for messaging. 
It consists of 2 parts that will be hosted on AKS Hybrid (on premises) and/or AKS (cloud) -
1. An asp dotnet core REST API that receives HTTP messages from a [local] client app (not included) and persists them to a file share that represents a simple queuing mechanism.
2. A dotnet core console application that dequeues the messages from the file share and sends them to an event hub in Azure.												
The solution uses a circuit-breaker pattern to buffer messages locally and retry sending should the connection to the cloud be unavailable. The code is non-production, and uses hard coded settings for simplicity. In a production scenario, these would be stored in a secure location such as [Arc-enabled] Azure Key Vault.

There are some simple GitHub workflows that:		
1. Build and test the solution when a PR is raised.			
2. Build Docker images and push them to Azure Container Registry when a new release is created.

# Getting Started
1. Create a GitHub account.
2. Create an Azure subscription. Then create a new Azure Container Registry (ACR) for storing your images.
3. Fork the code to your own GitHub repo.

# Prerequisites
1. An AKS Hybrid or AKS cluster. Details of the process to build this coming soon.

# Build and Test
1. Create an Azure service Principal with secrets: https://learn.microsoft.com/en-us/azure/developer/github/connect-from-azure?tabs=azure-portal%2Cwindows. This returns a json credentials object which is used as the GitHub secret.
2. Configure a secret called AZURE_CREDENTIALS to contain the json created above in the GitHub repo as described here in create GitHub secrets https://learn.microsoft.com/en-us/azure/developer/github/connect-from-azure?tabs=azure-cli%2Clinux#create-github-secrets.  
3. Create a branch for your changes.
4. Edit the appSettings.json in the message processor console app to point to your file share and event hub (these are hard-coded for simplicity).
5. Push changes to your branch and raise a PR. This will trigger the build and test workflow.
6. Once the PR is approved, merge it to main. 
7. Create a new release. This will trigger the build and push to ACR workflow.
8. Once the images are in ACR, you can deploy them to your AKS Hybrid or AKS cluster. Details of the process to do this coming soon.

# Run
1. Use curl or Postman to send a 'Hello world!' message to the API.
```c#
curl -X 'POST' \
  'https://[YOUR CONTAINER IP]/ServiceAPI/message' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d '{
  "senderId": "01",
  "value": "Hello world!"
}'
```

2. The API will enqueue the message to a file share and return a success message.

`08/09/2023 08:00:28 enqueue message senderId:01 correlationId:e46f1091-0e22-4cf7-ac29-4b0a8695bdb3 message:Hello world! successful.`

3. If you have configured the message processor console app correctly, it will pick up the message from the file share and send it to the event hub. You can monitor your event hub messages by:
a. Install extension for VSCode https://marketplace.visualstudio.com/items?itemName=Summer.azure-event-hub-explorer
b. Edit the extension settings – add Event Hub Connection String and Hub Entity Name
c. Open the command palette, search for `Event` and select `Event hub: Start Monitoring Event Hub Message`								
e. When finished, select `Event hub: Start Monitoring Event Hub Message`



THIS IS NOT COMPLETE, THERE MAY BE ERRORS AND OMMISSIONS, IT IS A WORK IN PROGRESS. COMPLETE INSTRUCTIONS COMING SOON
