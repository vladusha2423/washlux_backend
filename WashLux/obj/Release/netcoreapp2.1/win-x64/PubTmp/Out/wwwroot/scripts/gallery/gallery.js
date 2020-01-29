$('#gallery .phone-button').on('mouseenter', function(){
    $('.gallery-title-phone').animate({
        width: '160px'
    })
});
$('#gallery .gallery-title').on('mouseleave', function(){
    setTimeout(function(){
        $('.gallery-title-phone').animate({
            width: '0'
        })
    }, 4000);

});