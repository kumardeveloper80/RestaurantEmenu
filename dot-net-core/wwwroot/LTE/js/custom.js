function showTabs(cntrlId) {
    $("#login").hide();
    $("#register").hide();
    $("#recover").hide();
    $("#" + cntrlId).show();
}