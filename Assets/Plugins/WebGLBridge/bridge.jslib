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

    // window.addEventListener("storage", function (event) {
    //   console.log(event);
    //   if (event.key === "delegation") {
    //     var message = event.newValue;
    //     console.log(message);
    //     var data = JSON.parse(message);
    //     SendMessage('Login', 'HandleJsonDelegation', message);
    //   }
    // });

    // if (document.getElementById("login-iframe")) {
    //   console.log("iframe exists");
    //   return;
    // }

  },
  CreateLoginIframe: function (url) {
    var iframe = document.createElement("iframe");
    const iframeUrl = new URL(UTF8ToString(url));
    iframe.id = "login-iframe";
    iframe.src = iframeUrl.href;
    document.body.appendChild(iframe);
  },
  PostMessage: function (message) {
    window.parent.postMessage(message, "*");
  },
  OpenWindow: function (url) {
    window.open(url, '_self');
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
