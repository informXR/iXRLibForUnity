mergeInto(LibraryManager.library, {

    LoadIxrLib: function () {
        if (typeof window.ixrlib === "undefined") {
            const script = document.createElement("script");
            script.src = "ixrlib.min.js";
            script.type = "text/javascript";
            script.onload = function () {
                console.log("ixrlib.min.js loaded successfully!");
				window.alert("ixrlib.min.js loaded successfully!");
            };
            script.onerror = function () {
                console.error("Failed to load ixrlib.min.js.");
				window.alert("Failed to load ixrlib.min.js.");
            };
            document.head.appendChild(script);
        } else {
            console.log("ixrlib.min.js is already loaded.");
			window.alert("ixrlib.min.js is already loaded.");
        }
    },
	
  ixrAuth: async function () {
    const lib = window.iXRLib;
    const iXR = await lib.iXRInit({
                    appId: '471fd6fd-f5d0-4096-bc0c-17100c1c4fa0',
                    deviceId: 'iXRLibForWebXR_js',
                    deviceModel: 'iXRLibForWebXR_js', 
                    orgId : '5304ef74-423f-4bd4-87d9-cba4f19c3bdb',
                    authSecret : 'vEwWpJs5K2Kib3XeWBhXgQnQr43XNJCSyb5QJoGCU5ec590hFyb63vBSx6dX6Clj'
    });

    // Test basic functionality
    await iXR.LogInfo('Browser test successful');
    await iXR.Event('test_event', 'type=browser_test');
    window.alert("WORKING!");
  }
  
});