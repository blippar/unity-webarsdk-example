mergeInto(LibraryManager.library, {

 InitSDK: function () {
     WEBARSDK.Init();
  },

 StartTracking: function () {
     WEBARSDK.StartTracking();
  },

StartTrackingParam: function (check) {
     WEBARSDK.StartTracking(check);
  },

 StopTracking: function () {
     WEBARSDK.StopTracking();
  },

 StopTrackingParam: function (check) {
     WEBARSDK.StopTracking(check);
  },

 SetTrackingAnimation: function (animURL) {
     WEBARSDK.SetGuideAnimation(UTF8ToString(animURL));
  },

 SetObjectAutoScale: function (enable) {
    if (enable)
        {
            WEBARSDK.SetAutoScale(true);
        }
    else
        {
            WEBARSDK.SetAutoScale(false);
        }
  },

  IsMobileBrowser: function () {
    return (/iPhone|iPad|iPod|Android/i.test(navigator.userAgent));
  },
});