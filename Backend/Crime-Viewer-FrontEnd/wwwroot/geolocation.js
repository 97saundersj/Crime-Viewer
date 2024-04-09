window.getLocation = function (dotnetHelper) {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function (position) {
            dotnetHelper.invokeMethodAsync('LocationReceived', position.coords.latitude, position.coords.longitude);
        });
    } else {
        dotnetHelper.invokeMethodAsync('LocationError', "Geolocation is not supported by this browser.");
    }
};