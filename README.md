# Table of Contents

1. [Introduction](#introduction)
2. [Installation](#installation)
3. [Configuration](#configuration)
4. [Sending Data](#sending-data) 
5. [FAQ](#faq)
6. [Troubleshooting](#troubleshooting)
7. [Contact](#contact)

## Introduction

### Overview

The informXR SDK for Unity empowers developers **for free** to seamlessly integrate advanced XR tracking, analytics, and data management into their applications. By leveraging informXR's comprehensive features _(see below)_, **developers like you** can **significantly enhance their product's appeal to enterprise customers**. We are a cost-effective solution for your customers, making your product not just innovative, but also enterprise-ready.
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
2. Click the **'+'** button in the top left and select 'Add Package from git URL'  
3. Input `https://github.com/informXR/iXRLibForUnity.git`
4. Once the package is installed, you should see informXR appear in your Unity toolbar.

## Configuration

### Initial Setup

To get started with the informXR SDK, you'll need to configure your application with the necessary authentication details.

1. On the top menu choose `informXR > Configuration`.
2. Enter the Application ID, Organization ID, and Authorization Secret. These can be retrieved from the [informXR Web Application](https://app.informxr.io/) which requires a **free account** to continue.
     * Organization ID and Authorization Secret: Available under `Settings > Organization Codes`.
     * Application ID: Available in the Web Dashboard under your application settings. Please use the 'Get Started' tutorial button on the Home page and then choose the 'Content Developer' path for step-by-step instructions.
     * Follow the visual guides below for clarity.

### Organization ID and Authorization Secret Location - Web App
Follow the visual instructions below for clarification on how to get to the Organization ID and Authorization Secret in Settings.
![Visual Tutorial to get to Settings](https://github.com/informXR/iXRLibForUnity/blob/main/READMEFiles/GotoSettings.png?raw=true "Go to Settings")
![Visual Tutorial to get to Organization Codes](https://github.com/informXR/iXRLibForUnity/blob/main/READMEFiles/goToOrganizationCodes.png?raw=true "Go to Organization Codes")

### Application ID Location - Web App
Simply use the provided tutorials with the 'Get Started Button' shown below, and choose the 'Content Developer' path.
![Visual Tutorial to get App ID](https://github.com/informXR/iXRLibForUnity/blob/main/READMEFiles/PubAppTour1.png?raw=true "Press Get Started")

## Sending Data

### Event Methods
The Event Methods are designed to track user progress and activity throughout the experience. These functions allow developers to record specific actions, milestones, or interactions within the application, providing valuable insights into user behavior and engagement. By leveraging these methods, developers can create a detailed log of a user's journey, enabling comprehensive analysis and performance tracking.

#### Event
```csharp
public void iXR.Event( string name) 
    
public void iXR.Event( string name, Dictionary<string, string> meta = null)

public void iXR.Event( string name, Dictionary<string, string> meta = null, Vector3 location_data = null)
```
Records an event with optional metadata and location data.

**Parameters:**
- `name` (string): The name of the event. Use snake_case for better analytics processing.
- `meta` (Dictionary<string, string>): Optional. Additional key-value pairs describing the event.
- `location_data` (Vector3): Optional. The (x, y, z) coordinates of the event in 3D space.

**Note:** The system automatically includes a timestamp and origin ("user" by default, "system" for lib-generated events) with each event.

### Event Wrapper Functions
The Event Wrapper Functions are specialized versions of the Event method, tailored for common scenarios in XR experiences. These functions help enforce consistency in event logging across different parts of the application and are crucial for powering integrations with Learning Management System (LMS) platforms. By using these standardized wrapper functions, developers ensure that key events like starting or completing levels, assessments, or interactions are recorded in a uniform format. This consistency not only simplifies data analysis but also facilitates seamless communication with external educational systems, enhancing the overall learning ecosystem.

#### EventLevelStart
```csharp
public void iXR.EventEventLevelStart( string levelName ) 
    
public void iXR.EventLevelStart( string levelName, Dictionary<string, string> meta = null )
public void iXR.EventLevelStart( string levelName, metaString = "" )
```
Note: The meta/metaString parameter is optional and can be a string or a dictionary.


#### EventLevelStart
```csharp
iXR.EventLevelStart(string levelName)
iXR.EventLevelStart(string levelName, Dictionary<string, string> meta)
```

#### EventLevelComplete
```csharp
public void iXR.EventLevelComplete( string levelName, int score )
 
public void iXR.EventLevelComplete( string levelName, int score, Dictionary<string, string> meta = null )
public void iXR.EventLevelComplete( string levelName, int score, metaString = "" )
```
Note: If you use iXR.EventLevelStart first, it will automatically record the duration of the level, and give metrics for users who start but never end the level.

#### Assessments (LMS Compatible)
Assessments are intended to track the overall performance of a learner across multiple Objectives and Interactions. 
* Think of it as the learner's score for a specific course or curriculum.
* When the Assessment is complete, it will automatically record and close out the Assessment in the various LMS platforms we support.

#### EventAssessmentStart
```csharp
public void iXR.EventAssessmentStart( string assessmentName ) 
    
public void iXR.EventAssessmentStart( string assessmentName, Dictionary<string, string> meta = null )
public void iXR.EventAssessmentStart( string assessmentName, metaString = "" )
```

#### EventAssessmentComplete
```csharp
public enum ResultOptions
{
    Pass,
    Fail,
    Complete,
    Incomplete
}

iXR.EventAssessmentComplete(string assessmentName, int score, ResultOptions result = ResultOptions.Complete )

iXR.EventAssessmentComplete(string assessmentName, int score, ResultOptions result = ResultOptions.Complete, Dictionary<string, string> meta)
iXR.EventAssessmentComplete(string assessmentName, int score, ResultOptions result = ResultOptions.Complete, metaString = "" )
```

#### Objectives (LMS Compatible)
Objectives are intended to track the performance of a learner on a specific task or objective. 
* Think of it as the learner's score for a specific section of a course or curriculum.
* You can have multiple Objectives per Assessment. Note: SCORM limits to 255 Objectives per Assessment.

#### EventObjectiveStart
```csharp
iXR.EventObjectiveStart(string objectiveName)

iXR.EventObjectiveStart(string objectiveName, Dictionary<string, string> meta)
iXR.EventObjectiveStart(string objectiveName, metaString = "" )
```

#### EventObjectiveComplete
```csharp
public enum ResultOptions
{
    Pass,
    Fail,
    Complete,
    Incomplete
}

iXR.EventObjectiveComplete(string objectiveName, int score )

iXR.EventObjectiveComplete(string objectiveName, int score, ResultOptions result = ResultOptions.Complete )

iXR.EventObjectiveComplete(string objectiveName, int score, ResultOptions result = ResultOptions.Complete, Dictionary<string, string> meta)
iXR.EventObjectiveComplete(string objectiveName, int score, ResultOptions result = ResultOptions.Complete, metaString = "" )
```

#### Interactions (LMS Compatible)
Interactions are intended to track the performance of a learner on a specific task or interaction. 
* This gives you the ability to record the learner's performance on a specific interaction and choices they make.
* Because of this the options are a bit different than Assessments or Objectives, and you should look at our documentation carefully to use Interactions to their full advantage.
* You can have multiple Interactions per Assessment. Note: SCORM limits to 255 Interactions per Assessment.

#### EventInteractionStart
```csharp
iXR.EventInteractionStart(string interactionName)

iXR.EventInteractionStart(string interactionName, Dictionary<string, string> meta)
iXR.EventInteractionStart(string interactionName, metaString = "" )
```

#### EventInteractionComplete
```csharp
public enum InteractionType
{
   Bool, // 1 or 0
   Select, // true or false and the result_details value should be a single letter or for multiple choice a,b,c
   Text, // a string 
   Rating, // a single digit value
   Number // integer
}

iXR.EventInteractionComplete(string interactionName, string result)

iXR.EventInteractionComplete(string interactionName, string result, string result_details = null)

iXR.EventInteractionComplete(string interactionName, string result, string result_details = null, InteractionType type = InteractionType.Text = null)

iXR.EventInteractionComplete(string interactionName, string result, string result_details = null, InteractionType type = InteractionType.Text = null, Dictionary<string, string> meta = null )
iXR.EventInteractionComplete(string interactionName, string result, string result_details = null, InteractionType type = InteractionType.Text = null, metaString = "" )
```

**Parameters for all Event Wrapper Functions:**
- `levelName/assessmentName/objectiveName/interactionName` (string): The identifier for the level, assessment, objective, or interaction.
- `score` (int): The numerical score achieved. While typically between 1-100, any integer is valid. In metadata, you can also set a minScore and maxScore to define the range of scores for this objective.
- `result` (ResultOptions for Assessment and Objective): The basic result of the assessment or objective.
- `result` (Interactions): The result for the interaction are based on the InteractionType.
- `result_details` (string): Optional. Additional details about the result. For interactions this can be a single character or a string. For example: "a", "b", "c" or "correct", "incorrect".
- `type` (InteractionType): Optional. The type of interaction for this event.
- `meta` (Dictionary<string, string>): Optional. Additional key-value pairs describing the event.

**Note:** For all "Complete" events, the duration is automatically calculated if the corresponding "Start" event was recorded with the same name.

### Log Methods
The Log Methods provide straightforward logging functionality, similar to syslogs. These functions are available to developers by default, even across enterprise users, allowing for consistent and accessible logging across different deployment scenarios.

#### Log
```csharp
iXR.Log(LogLevel level, string message)
```

**Parameters:**
- `level` (LogLevel): The severity of the log (Debug, Info, Warn, Error, Critical).
- `message` (string): The content of the log message.

#### Log Wrapper Functions
```csharp
iXR.LogDebug(string message)
iXR.LogInfo(string message)
iXR.LogWarn(string message)
iXR.LogError(string message)
iXR.LogCritical(string message)
```

**Parameters:**
- `message` (string): The content of the log message.

### Storage Methods
The Storage Methods enable developers to store and retrieve learner/player progress, facilitating the creation of long-form training content. When users log in using ArborXR's facility or the developer's in-app solution, these methods allow users to continue their progress on different headsets, ensuring a seamless learning experience across multiple sessions or devices.

#### SetStorageEntry
```csharp
iXR.SetStorageEntry(Dictionary<string, string> data, string name = "state", bool keep_latest = true, string origin = null, bool session_data = false)
```

**Parameters:**
- `data` (Dictionary<string, string>): The key-value pairs to store.
- `name` (string): Optional. The identifier for this storage entry. Default is "state".
- `keep_latest` (bool): Optional. If true, only the most recent entry is kept. If false, entries are appended. Default is true.
- `origin` (string): Optional. The source of the data (e.g., "system").
- `session_data` (bool): Optional. If true, the data is specific to the current session. Default is false.

#### GetStorageEntry
```csharp
iXR.GetStorageEntry(string name = "state", string origin = null, string[] tags_any = null, string[] tags_all = null, bool user_only = false)
```

**Parameters:**
- `name` (string): Optional. The identifier of the storage entry to retrieve. Default is "state".
- `origin` (string): Optional. Filter entries by their origin ("system", "user", or "admin").
- `tags_any` (string[]): Optional. Retrieve entries matching any of these tags.
- `tags_all` (string[]): Optional. Retrieve entries matching all of these tags.
- `user_only` (bool): Optional. If true, retrieve data for the current user across all devices for this app. Default is false.

**Returns:** A dictionary containing the retrieved storage entry.

#### RemoveStorageEntry
```csharp
iXR.RemoveStorageEntry(string name = "state")
```

**Parameters:**
- `name` (string): Optional. The identifier of the storage entry to remove. Default is "state".

#### GetAllStorageEntries
```csharp
iXR.GetAllStorageEntries()
```

**Returns:** A dictionary containing all storage entries for the current user/device.

### Telemetry Methods
The Telemetry Methods provide comprehensive tracking of the XR environment. By default, they capture headset and controller movements, but can be extended to track any custom objects in the virtual space. These functions also allow collection of system-level data such as frame rates or device temperatures. This versatile tracking enables developers to gain deep insights into user interactions and application performance, facilitating optimization and enhancing the overall XR experience.

#### Telemetry
```csharp
iXR.Telemetry(string name, Dictionary<string, string> data)
```

**Parameters:**
- `name` (string): The type of telemetry data (e.g., "OS_Version", "Battery_Level", "RAM_Usage").
- `data` (Dictionary<string, string>): Key-value pairs of telemetry data.

### AI Integration Methods
The Integrations Methods offer developers access to additional services, enabling customized experiences for enterprise users. Currently, this includes access to GPT services through the AIProxy method, allowing for advanced AI-powered interactions within the XR environment. More integration services are planned for future releases, further expanding the capabilities available to developers for creating tailored enterprise solutions.

#### AIProxy
```csharp
iXR.AIProxy(string prompt, string past_messages = "", string bot_id = "")
```

**Parameters:**
- `prompt` (string): The input prompt for the AI.
- `past_messages` (string): Optional. Previous conversation history for context.
- `bot_id` (string): Optional. An identifier for a specific pre-defined chatbot.

**Returns:** The AI-generated response as a string.

**Note:** AIProxy calls are processed immediately and bypass the cache system. However, they still respect the SendRetriesOnFailure and SendRetryInterval settings.

### Authentication Methods

#### SetUserId
```csharp
iXR.SetUserId(string userId)
```

**Parameters:**
- `userId` (string): The User ID used during authentication (setting this with trigger re-authentication).

## Debug Window
The Debug Window is a little bonus feature from the iXRLib developers. 
To help with general debugging, this feature routes a copy of all iXRLib messages (Logs, Events, etc) to a window within the VR space that you can easily attached to an object like one of the controllers. This enables developers to view logs in VR without having to repeatedly take on and off your headset while debugging. If enabled, this feature can be disabled at runtime by pressing the `Primary Controller Button`.

### Setup
To use this feature, simply drag the `DebugCanvas` Prefab from `iXRLib for Unity/Assets/Prefabs`, to whatever object in the scene you want this window attached to (i.e. `Left Controller`).

## FAQ

### Q: How do I retrieve my Application ID and Authorization Secret?
A: Your Application ID can be found in the Web Dashboard under the application details. For the Authorization Secret, navigate to Settings > Organization Codes on the same dashboard.

### Q: How do I enable object tracking?
A: Object tracking can be enabled by adding the Track Object component to any GameObject in your scene via the Unity Inspector.


## Troubleshooting

### Common Issues

1. **Issue**: Authentication failing due to network error.  
     
   - **Solution**: Uncheck 'Force Remove Internet Permissions' in `Project Settings > XR Plug-in Management > OpenXR > Meta Quest Support Settings`.

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
