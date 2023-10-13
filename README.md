# Highly Available Azure Arc-enabled AKS Hybrid Messaging

[![Build on pull to main](https://github.com/ianlcurtis1/ha-aks-hybrid-poc/actions/workflows/build.yml/badge.svg)](https://github.com/ianlcurtis1/ha-aks-hybrid-poc/actions/workflows/build.yml)

# Introduction 
AKS Hybrid (https://learn.microsoft.com/en-us/azure/aks/hybrid/) is an on-premise implementation of Azure Kubernetes Service (AKS) orchestrator which automates running containerised applications at scale. It provides a solution for hosting highly available workloads on-premise. Azure Arc (https://learn.microsoft.com/en-us/azure/azure-arc/overview) is a cloud based control plane which can be used for managing on-premise AKS Hybrid instances. Flux (https://fluxcd.io/) is an open sourced set of continuous delivery solutions for Kubernetes. AKS and AKS Hybrid natively support Flux through their GitOps capabilities. 


This is a simple dotnet solution to demonstrate a highly available AKS configuration for messaging. The solution uses 2 instances of an architectural stamp - one hosted 'on premises' on Arc-enabled AKS Hybrid, the other hosted in the cloud on AKS. Between them, they provide high availability with the option (for on-premises installations) of a failover to cloud.

The deployed messaging code consists of 2 parts that will be hosted in both instances -
1. An asp dotnet core REST API that receives HTTP messages from a client app (not included) and persists them to a file share that represents a simple queuing mechanism.
2. A dotnet core console application that dequeues the messages from the file share and sends them to an event hub in Azure.												
The solution uses a circuit-breaker pattern to buffer messages and retry sending should the connection to the Event Hub be unavailable.

The code is non-production, and uses hard coded settings for simplicity. In a production scenario, these would be stored in a secure location such as [Arc-enabled] Azure Key Vault. The solution demonstrates how a single cloud-hosted control plane can be used to manage the deployments, whether on-premises, or in the cloud.

There are some simple GitHub workflows that:		
1. Build and test the solution when a PR is raised.			
2. Build Docker images and push them to Azure Container Registry when a new release is created.

![Imgur](https://github.com/ianlcurtis1/ha-aks-hybrid-poc/blob/main/PoCHLArchitecture.png)

# Getting Started
1. Create a GitHub account.
2. Create an Azure subscription.
3. Create a new Azure Container Registry (ACR) for storing your images: https://learn.microsoft.com/en-us/azure/container-registry/container-registry-get-started-portal?tabs=azure-cli.
4. Create a new Azure Event Hub to receive messages from your AKS clusters: https://learn.microsoft.com/en-us/azure/event-hubs/event-hubs-create
5. Fork the code to your own GitHub repo.

# Build, Test, and push images to ACR
1. Create an Azure service Principal with secrets: https://learn.microsoft.com/en-us/azure/developer/github/connect-from-azure?tabs=azure-portal%2Cwindows. This returns a json credentials object which is used as the GitHub secret.
2. Configure a secret called AZURE_CREDENTIALS to contain the json created above in the GitHub repo as described here in create GitHub secrets https://learn.microsoft.com/en-us/azure/developer/github/connect-from-azure?tabs=azure-cli%2Clinux#create-github-secrets.  
3. Create a branch for your changes.
4. Edit the appSettings.json in the message processor console app to point to your file share and event hub (these are hard-coded for simplicity).
5. Push changes to your branch and raise a PR. This will trigger the build and test workflow.
6. Once the PR is approved, merge it to main. 
7. Create a new release. This will trigger the build and push to ACR workflow.

# Deploy AKS Hybrid
1. Deploy an AKS Hybrid cluster - details of the process can be found here https://github.com/philljudge/aks-hybrid-api-poc.

# Deploy Images to AKS Hybrid
1. Now deploy the images pushed to ACR to your AKS Hybrid or AKS cluster. Details of the process can be found here https://github.com/philljudge/aks-hybrid-api-poc.

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

2. Alternatively, you can deploy the static test app in the `www` directory to an Azure blob storage account and use it as a test client. To do this:
   - Create a new Azure Storage Account and Blob Container, configure as a static website.
   - Edit the `js/site.js` file, in the `uris` const at the top of the file replace the existing uris with the urls to your AKS and AKS Hybrid instances.
   - Upload the contents of the `www` folder to the blob container.
   - Configure CORS on your AKS and AKS Hybrid instance to allow access from the blob container uri.

3. The API will enqueue the message to a file share and return a success message.

```json 
{
  "serverTimestamp": "2023-09-14T06:06:29.1009121Z",
  "senderId": "01",
  "correlationId": "967e9872-31f7-4613-a2b0-bf3383d7254d",
  "value": "Hello world!",
  "success": true
}
```

4. If you have configured the message processor console app correctly, it will pick up the message from the file share and send it to the event hub. You can monitor your event hub messages by:
   - Install extension for VSCode https://marketplace.visualstudio.com/items?itemName=Summer.azure-event-hub-explorer		
   - Edit the extension settings – add Event Hub Connection String and Hub Entity Name
   - Open the command palette, search for `Event` and select `Event hub: Start Monitoring Event Hub Message`								
   - When finished, select `Event hub: Stop Monitoring Event Hub Message`

5. You can test the circuit-breaker functionality by disabling/re-enabling the event hub in Azure. The message processor console app will buffer the messages locally and retry sending them to the event hub after a period of backing off.

THIS IS NOT COMPLETE, THERE MAY BE ERRORS AND OMMISSIONS, IT IS A WORK IN PROGRESS. COMPLETE INSTRUCTIONS COMING SOON
