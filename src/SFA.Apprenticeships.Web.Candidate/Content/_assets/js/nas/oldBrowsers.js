$(function() {

  //-- Faking details behaviour

  $('.no-details').on('click', 'summary', function() {
    $(this).parent().toggleClass('open');
  });

});