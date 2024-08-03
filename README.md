The InformXR SDK for Unity

## Table of Contents

1. [Introduction](\#introduction)  
1. [Installation](\#installation)  
1. [Configuration](\#configuration)  
1. [Features](\#features)  
1. [FAQ](\#faq)  
1. [Troubleshooting](\#troubleshooting)  
1. [Contact](\#contact)

## Introduction

### Overview

Overview TBD

### Key Features

- HMD and Controller Tracking  
- Object Tracking  
- System Information Tracking  
- Event Tracking

## Installation

### Steps to Install

1. Open 'Package Manager' from the Window menu  
1. Click the \+ in the top left and select 'Add Package from git URL'  
1. Input `https://github.com/informXR/iXRLibUnitySDK.git`

## Configuration

### Initial Setup

iXRLib requires an initial configuration for authentication.

* In the Unity UI, click the 'InformXR' tab, then 'Configuration'  
* Application ID, Organization ID, and Authorization Secret can be obtained through the Web UI  
* Application ID is obtained from the WebUI by clicking on your published application, then clicking ‘AppID’  
* Organization ID and Authorization Secret are obtained from the WebUI by clicking `Settings->Organization Codes`

![alt text](https://github.com/informXR/iXRLibUnitySDK/blob/main/README-UISettingsButton.png?raw=true)
![alt text](https://github.com/informXR/iXRLibUnitySDK/blob/main/README-UISettingsScreen.png?raw=true)


## Features

### Feature 1: Headset Tracking

HMD and Controller Tracking. To use this feature, simply click 'Headset Tracking' in the Configuration UI

### Feature 2: Object Tracking

To use this feature
* Select the object you would like to Track
* In the Inspector Window, select 'Add Component'
* Select the 'InformXR'->'Track Object' component

### Feature 3: Event Tracking

To use this feature, add iXRSend.AddEvent() calls to your code

## FAQ

### Q: How do I \[TBD\]?

A: TBD

## Troubleshooting

### Common Issues

List common issues and their solutions.

1. **Issue 1**: Authentication failing due to network error.  
     
   - **Solution**: Uncheck ‘Force Remove Internet Permissions’ in Project Settings \-\> XR Plug-in Management \-\> OpenXR \-\> Meta Quest Support Settings.

## Contact

### Support

TBD

### Feedback

TBD  
