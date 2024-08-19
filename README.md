# Table of Contents

1. [Introduction](\#introduction)
2. [Installation](\#installation)
3. [Configuration](\#configuration)
4. [Sending Data](\#sending-data) 
5. [FAQ](\#faq)
6. [Troubleshooting](\#troubleshooting)
7. [Contact](\#contact)

## Introduction

### Overview

The informXR SDK for Unity empowers developers **for free** to seamlessly integrate advanced XR tracking, analytics, and data management into their applications. By leveraging informXR's comprehensive features _(see below)_, **developers like you** can **significantly enhance their product’s appeal to enterprise customers**. We are a cost-effective solution for your customers, making your product not just innovative, but also enterprise-ready.
* Seamless LMS and business intelligence integrations
* An advanced analytics platform
* Secure data storage solutions
* An AI proxy

### Key SDK Features

- **Event Tracking**: Get error logs and user interactions based events within your XR applications.
- **HMD and Controller Tracking**: Real-time tracking of head-mounted displays (HMD) and controllers for a comprehensive XR experience.   
- **Object Tracking**: Track specific objects within your XR environment to monitor interactions and movements.
- **System Information Tracking**: Capture system-level data, such as device specifications and performance metrics.  

## Installation

### Steps to Install

1. On the top menu choose `Window > Package Manager`.
2. Click the **\'+'** button in the top left and select 'Add Package from git URL'  
3. Input `https://github.com/informXR/iXRLibUnitySDK.git`
4. Once the package is installed, you should see informXR appear in your Unity toolbar.

## Configuration

### Initial Setup

To get started with the informXR SDK, you'll need to configure your application with the necessary authentication details.

1. On the top menu choose `informXR > Configuration`.
2. Enter the Application ID, Organization ID, and Authorization Secret. These can be retrieved from the [informXR Web Application](https://app.informxr.io/) which requires a **free account** to continue.
     * Organization ID and Authorization Secret: Available under `Settings > Organization Codes`.
     * Application ID: Available in the Web Dashboard under your application settings.
     * Follow the visual guides below for clarity.

### Organization ID and Authorization Secret Location - Web App
![Visual Tutorial to get to Settings](https://github.com/informXR/iXRLibUnitySDK/blob/main/READMEFiles/GotoSettings.png?raw=true "Go to Settings")
![Visual Tutorial to get to Organization Codes](https://github.com/informXR/iXRLibUnitySDK/blob/main/READMEFiles/goToOrganizationCodes.png?raw=true "Go to Organization Codes")

### Application ID Location - Web App

This tutorial will include the full steps to publishing your application with informXR and obtaining your Application ID.

![Visual Tutorial to Publish Application - Step 1](https://github.com/informXR/iXRLibUnitySDK/blob/main/READMEFiles/PublishApp1.png?raw=true "Publish an Application - Step 1")
![Visual Tutorial to Publish Application - Step 2](https://github.com/informXR/iXRLibUnitySDK/blob/main/READMEFiles/PublishApp2.png?raw=true "Publish an Application - Step 2")
![Visual Tutorial to Publish Application - Step 3](https://github.com/informXR/iXRLibUnitySDK/blob/main/READMEFiles/PublishApp3.png?raw=true "Publish an Application - Step 3")
![Visual Tutorial to Publish Application - Step 4](https://github.com/informXR/iXRLibUnitySDK/blob/main/READMEFiles/PublishApp4.png?raw=true "Publish an Application - Step 4")
![Visual Tutorial to Publish Application - Step 5](https://github.com/informXR/iXRLibUnitySDK/blob/main/READMEFiles/PublishApp5.png?raw=true "Publish an Application - Step 5")
![Published Application Tour - Step 1](https://github.com/informXR/iXRLibUnitySDK/blob/main/READMEFiles/PubAppTour1.png?raw=true "Published Application Tour - Step 1")
![Published Application Tour - Step 2](https://github.com/informXR/iXRLibUnitySDK/blob/main/READMEFiles/PubAppTour2.png?raw=true "Published Application Tour - Step 2")

## Sending Data

### Feature 1: Event Tracking & Debug Logging

Integrate event logging by calling the following methods from within your code:
- iXR.TelemetryEntry(string name, string data) - iXR.TelemetryEntry("Battery", "Level=100")
    - name (String) - a string that we will leave open (examples are OS version, XRDM Version, Geolocation, Battery Level, Battery Status, RAM Usage, CPU Usage, Object Tracking, FPS)
    - data (String) - a comma-separated string of name=value pairs of whatever the developer wants to provide

#### Event Recording
- iXR.Event(string message, string meta) - iXR.Event("Placement Complete", "Item=Ready")
    - name (String) - a string that we will leave open, but will provide recommendations for the value
    - meta (String)- a comma-separated string of name=value pairs of whatever the developer wants to provice
- iXR.Event(string message, string meta, GameObject gameObject)
    - Automatically adds the gameObject's coordinates to the meta

### Log Functions
- iXR.LogCritical(string text)
    - Logs a critical message
- iXR.LogError(string text)
    - Logs an error message
- iXR.LogWarn(string text)
    - Logs a warning message
- iXR.LogInfo(string text)
    - Logs an info message
- iXR.LogDebug(string text)
    - Logs a debug message

### Feature 2: Session Storage
- iXR.StorageSetEntry(name text, StorageEntry text)
    - name (String) - The name, `session` is a good option to start with
    - StorageEntry (String)- Store any data as a string value. JSON is a good solution in many cases

- iXR.StorageGetEntry(name text)
    - name (String) - The name, `session` is a good option to start with

- iXR.StorageRemoveEntry(name text)
    - name (String) - The name, `session` is a good option to start with

### Feature 3: Headset Tracking

Enable real-time tracking of headsets and controllers:
1. By default it is on. However, if not, in the Configuration UI click on Headset Tracking.
2. The SDK will automatically start tracking the HMD and controllers.

### Feature 4: Object Tracking
To track specific objects:
1. Select the object you want to track in your Unity scene.
2. In the Inspector window, click Add Component.
3. Choose `informXR > Track Object`.

### Feature 4: Other Services
- iXR.AIProxy(prompt text)
    - prompt (String) - The GTP prompt


## FAQ

### Q: How do I retrieve my Application ID and Authorization Secret?
A: Your Application ID can be found in the Web Dashboard under the application details. For the Authorization Secret, navigate to Settings > Organization Codes on the same dashboard.

### Q: How do I enable object tracking?
A: Object tracking can be enabled by adding the Track Object component to any GameObject in your scene via the Unity Inspector.


## Troubleshooting

### Common Issues

1. **Issue**: Authentication failing due to network error.  
     
   - **Solution**: Uncheck ‘Force Remove Internet Permissions’ in `Project Settings > XR Plug-in Management > OpenXR > Meta Quest Support Settings`.

2. **Issue**: Event data not appearing in the dashboard.

   - **Solution**: Verify that your Application ID, Organization ID, and Authorization Secret are correctly configured in the Unity SDK.

## Contact

### Support

For support, please reach out to our team at [info@informxr.com](mailto:info@informxr.com).

### Feedback

We value your contributions! If you'd like to suggest changes or improvements to our SDK, you can do so by creating a Pull Request (PR).

To submit a Pull Request:
1. Fork the repository to your GitHub account.
2. Clone the forked repository to your local machine.
3. Create a new branch for your changes.
4. Make your changes and commit them to your branch.
5. Push the changes to your fork on GitHub.
6. Go to the original repository and create a Pull Request from your forked branch.

Once submitted, our team will review your Pull Request. We may ask for additional information or changes, and once everything looks good, we'll approve and merge your changes into the main branch. If necessary, we might also deny the PR with feedback on why it was not accepted. 
