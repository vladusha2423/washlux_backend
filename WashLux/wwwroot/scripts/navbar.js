let isActive = false;
function open(){
    $('.nav-mobile').animate({height: "100%"})
    isActive = true;
}
function close(){
    $('.nav-mobile').animate({height: "0"})
    isActive = false;
}
$('.nav-mobile-trigger').click(function(){
    if (!isActive)
        open();
    else
        close();
});