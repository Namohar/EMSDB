// BlockUI setup

$.extend({
  // Block ui during ajax post back
  initializeUiBlocking: function () {
    if (typeof ($.blockUI) != 'undefined') {
      $.blockUI.defaults.message = 'LOADING';
      $.blockUI.defaults.overlayCSS = {};
      $.blockUI.defaults.css = {};

      var request = Sys.WebForms.PageRequestManager.getInstance();
      request.add_initializeRequest(function (sender, args) {
        request.get_isInAsyncPostBack() && args.set_cancel(true);
      });
      request.add_beginRequest(function () { $.blockUI(); });
      request.add_endRequest(function () { $.unblockUI(); });
    }
  }
});

$(document).ready(function () {
  $.initializeUiBlocking();
});