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

 DisableAutoScale: function (check) {
     WEBARSDK.DisableAutoScale(check);
  },

});
