window.getLocation = function (dotnetHelper) {
    console.log("Attempting to get geolocation...");
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(
            function (position) {
                console.log("Geolocation obtained: ", position.coords.latitude, position.coords.longitude);
                dotnetHelper.invokeMethodAsync('LocationReceived', position.coords.latitude, position.coords.longitude);
            },
            function (error) {
                console.error("Error obtaining geolocation: ", error.message);
            },
            {maximumAge:10000, timeout:5000, enableHighAccuracy:false}
        );
    } else {
        console.log("Geolocation is not supported by this browser.");
    }
};