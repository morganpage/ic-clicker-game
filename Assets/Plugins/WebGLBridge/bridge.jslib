mergeInto(LibraryManager.library, {
  Initialize: function () {
    window.addEventListener('message', function (event) {
      console.log(event);
      if (event.origin == "https://identity.ic0.app" || event.origin == "http://localhost:3000" || event.origin == "https://gl6wc-baaaa-aaaam-adb3a-cai.icp0.io") {
        console.log("here");
        var data = JSON.parse(event.data);
        if (data.hasOwnProperty("delegations")) {
          console.log(data.delegations);
          SendMessage('Login', 'HandleJsonDelegation', event.data);
        }
      }
    });
  },
  ShowLoginIframe: function (url) {
    var iframe = document.getElementById("loginIframe");
    iframe.style.display = "block";
    iframe.src = UTF8ToString(url);
  },
  HideLoginIframe: function () {
    var iframe = document.getElementById("loginIframe");
    iframe.style.display = "none";
  },


});
