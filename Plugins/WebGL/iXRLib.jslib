mergeInto(LibraryManager.library, {
    ProcessInteractionType: function(enumValue) {
        if (enumValue === 1) return iXRLib.InteractionType.eBool;
        if (enumValue === 2) return iXRLib.InteractionType.eSelect;
        if (enumValue === 3) return iXRLib.InteractionType.eText;
        if (enumValue === 4) return iXRLib.InteractionType.eRating;
        if (enumValue === 5) return iXRLib.InteractionType.eNumber;
        return iXRLib.InteractionType.eNull;
    },
    ProcessDictString: function(dictPtr) {
        const dictString = UTF8ToString(dictPtr);
        return JSON.parse(dictString); // Convert to JavaScript object
    },
    // ---
    iXRLibLoad: function () {
        if (typeof window.ixrlib === "undefined") {
            const script = document.createElement("script");
            script.src = "iXRLibForWebXR.js";
            script.type = "text/javascript";
            script.onload = function () {
                console.log("iXRLibForWebXR.js loaded successfully!");
				window.alert("iXRLibForWebXR.js loaded successfully!");
            };
            script.onerror = function () {
                console.error("Failed to load iXRLibForWebXR.js.");
				window.alert("Failed to load iXRLibForWebXR.js.");
            };
            document.head.appendChild(script);
        } else {
            console.log("iXRLibForWebXR.js is already loaded.");
			window.alert("iXRLibForWebXR.js is already loaded.");
        }
    },
	JSAuthenticate: async function (ptrAppId, ptrOrgId, ptrDeviceId, ptrAuthSecret, ptrPartner) {
        const appId = UTF8ToString(ptrAppId);
        const orgId = UTF8ToString(ptrOrgId);
        const deviceId = UTF8ToString(ptrDeviceId);
        const authSecret = UTF8ToString(ptrAuthSecret);
        const partner = UTF8ToString(ptrPartner);
        await iXRLib.iXRLibInit.Authenticate(appId, orgId, deviceId, authSecret, partner);
	},
    JSReAuthenticate: async function (obtainAuthSecret) {
        await iXRLib.iXRLibInit.ReAuthenticate(obtainAuthSecret);
    },
    JSTokenExpirationImminent: function () {
        if (typeof iXRLib == 'undefined') return false;
        return iXRLib.iXRLibInit.m_ixrLibAuthentication.TokenExpirationImminent();
    },
    JSForceSendUnsent: async function () {
        await iXRLib.iXRLibInit.ForceSendUnsent();
    },
    // ---
    JSLogDebug: async function(ptrText, ptrMeta) {
        const text = UTF8ToString(ptrText);
        const metaDict = ProcessDictString(ptrMeta);
        await iXRLib.iXRLibSend.LogDebug(text, metaDict);
    },
    JSLogInfo: async function(ptrText, ptrMeta) {
        const text = UTF8ToString(ptrText);
        const metaDict = ProcessDictString(ptrMeta);
        await iXRLib.iXRLibSend.LogInfo(text, metaDict);
    },
    JSLogWarn: async function(ptrText, ptrMeta) {
        const text = UTF8ToString(ptrText);
        const metaDict = ProcessDictString(ptrMeta);
        await iXRLib.iXRLibSend.LogWarn(text, metaDict);
    },
    JSLogError: async function(ptrText, ptrMeta) {
        const text = UTF8ToString(ptrText);
        const metaDict = ProcessDictString(ptrMeta);
        await iXRLib.iXRLibSend.LogError(text, metaDict);
    },
    JSLogCritical: async function(ptrText, ptrMeta) {
        const text = UTF8ToString(ptrText);
        const metaDict = ProcessDictString(ptrMeta);
        await iXRLib.iXRLibSend.LogCritical(text, metaDict);
    },
    // ---
    JSEvent: async function(ptrName, ptrMeta) {
        const name = UTF8ToString(ptrName);
        const metaDict = ProcessDictString(ptrMeta);
        await iXRLib.iXRLibSend.Event(name, metaDict);
    },
    // ---
    JSAddTelemetryEntry: async function(ptrName, ptrMeta) {
        const name = UTF8ToString(ptrName);
        const metaDict = ProcessDictString(ptrMeta);
        await iXRLib.iXRLibSend.AddTelemetryEntry(name, metaDict);
    },
    // ---
    JSStorageGetDefaultEntry: function() {
        iXRLib.iXRLibStorage.StorageGetEntryAsString();
    },
    JSStorageGetEntry: function(ptrName) {
        const name = UTF8ToString(ptrName);
        iXRLib.iXRLibStorage.StorageGetEntryAsString(name);
    },
    JSStorageSetDefaultEntry: async function(ptrEntry, keepLatest, ptrOrigin, sessionData) {
        const entry = UTF8ToString(ptrEntry);
        const origin = UTF8ToString(ptrOrigin);
        await iXRLib.iXRLibStorage.StorageSetEntry(entry, keepLatest, origin, sessionData);
    },
    JSStorageSetEntry: async function(ptrName, ptrEntry, keepLatest, ptrOrigin, sessionData) {
        const name = UTF8ToString(ptrName);
        const entry = UTF8ToString(ptrEntry);
        const origin = UTF8ToString(ptrOrigin);
        await iXRLib.iXRLibStorage.StorageSetEntry(entry, keepLatest, origin, sessionData, name);
    },
    JSStorageRemoveDefaultEntry: async function() {
        await iXRLib.iXRLibStorage.StorageRemoveEntry();
    },
    JSStorageRemoveEntry: async function(ptrName) {
        const name = UTF8ToString(ptrName);
        await iXRLib.iXRLibStorage.StorageRemoveEntry(name);
    },
    JSStorageRemoveMultipleEntries: async function(sessionOnly) {
        await iXRLib.iXRLibStorage.StorageRemoveMultipleEntries(sessionOnly);
    },
    // ---
    JSAddAIProxy0: async function(ptrPrompt, ptrLlmProvider) {
        const prompt = UTF8ToString(ptrPrompt);
        const llmProvider = UTF8ToString(ptrLlmProvider);
        await iXRLib.iXRLibAnalytics.AddAIProxy0(prompt, llmProvider);
    },
    JSAddAIProxy1: async function(ptrPrompt, ptrPastMessages, ptrLlmProvider) {
        const prompt = UTF8ToString(ptrPrompt);
        const pastMessages = UTF8ToString(ptrPastMessages);
        const llmProvider = UTF8ToString(ptrLlmProvider);
        await iXRLib.iXRLibAnalytics.AddAIProxy1(prompt, pastMessages, llmProvider);
    },
    // ---
    JSEventAssessmentStart: async function(ptrAssessmentName, ptrMeta) {
        const assessmentName = UTF8ToString(ptrAssessmentName);
        const metaDict = ProcessDictString(ptrMeta);
        await iXRLib.iXRLibSend.EventAssessmentStart(assessmentName, metaDict);
    },
    JSEventAssessmentComplete: async function(ptrAssessmentName, ptrScore, ptrMeta, resultOption) {
        const assessmentName = UTF8ToString(ptrAssessmentName);
        const score = UTF8ToString(ptrScore);
        const metaDict = ProcessDictString(ptrMeta);
        const eResultOption = ProcessResultOption(resultOption);
        await iXRLib.iXRLibSend.EventAssessmentComplete(assessmentName, score, eResultOption, metaDict);
    },
    JSEventObjectiveStart: async function(ptrObjectiveName, ptrMeta) {
        const objectiveName = UTF8ToString(ptrObjectiveName);
        const metaDict = ProcessDictString(ptrMeta);
        await iXRLib.iXRLibSend.EventObjectiveStart(objectiveName, metaDict);
    },
    JSEventObjectiveComplete: async function(ptrObjectiveName, ptrScore, ptrMeta, resultOption) {
        const objectiveName = UTF8ToString(ptrObjectiveName);
        const score = UTF8ToString(ptrScore);
        const metaDict = ProcessDictString(ptrMeta);
        const eResultOption = ProcessResultOption(resultOption);
        await iXRLib.iXRLibSend.EventObjectiveComplete(objectiveName, score, eResultOption, metaDict);
    },
    JSEventInteractionStart: async function(ptrInteractionName, ptrMeta) {
        const interactionName = UTF8ToString(ptrInteractionName);
        const metaDict = ProcessDictString(ptrMeta);
        await iXRLib.iXRLibSend.EventInteractionStart(interactionName, metaDict);
    },
    JSEventInteractionComplete: async function(ptrInteractionName, ptrResult, ptrResultDetails, interactionType, ptrMeta) {
        const interactionName = UTF8ToString(ptrInteractionName);
        const result = UTF8ToString(ptrResult);
        const resultDetails = UTF8ToString(ptrResultDetails);
        const eInteractionType = ProcessInteractionType(interactionType);
        const metaDict = ProcessDictString(ptrMeta);
        return await iXRLib.iXRLibSend.EventInteractionComplete(interactionName, result, resultDetails, eInteractionType, metaDict);
    },
    JSEventLevelStart: async function(ptrLevelName, ptrMeta) {
        const levelName = UTF8ToString(ptrLevelName);
        const metaDict = ProcessDictString(ptrMeta);
        await iXRLib.iXRLibSend.EventLevelStart(levelName, metaDict);
    },
    JSEventLevelComplete: async function(ptrLevelName, ptrScore, ptrMeta) {
        const levelName = UTF8ToString(ptrLevelName);
        const score = UTF8ToString(ptrScore);
        const metaDict = ProcessDictString(ptrMeta);
        await iXRLib.iXRLibSend.EventLevelComplete(levelName, score, metaDict);
    },
    JSCaptureTimeStamp: function() {
        iXRLib.iXRBase.CaptureTimeStamp();
    },
    JSUnCaptureTimeStamp: function() {
        iXRLib.iXRBase.UnCaptureTimeStamp();
    }
});
