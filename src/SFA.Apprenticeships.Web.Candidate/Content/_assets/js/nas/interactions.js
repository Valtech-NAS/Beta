$(function() {
  FastClick.attach(document.body);

  $('.menu-trigger').on('click', function() {
    $(this).next('.menu').toggleClass('menu-open');
    $(this).toggleClass('triggered');
    return false;
  });

  $('.mob-collpanel-trigger').on('click', function() {
    $(this).next('.mob-collpanel').toggleClass('panel-open');
    $(this).toggleClass('triggered');
    return false;
  });

  $('.collpanel-trigger').on('click', function() {
    $(this).next('.collpanel').toggleClass('panel-open');
    $(this).toggleClass('triggered');
    return false;
  });

  $(".block-label").each(function(){

    // Add focus
    $(".block-label input").focus(function() {
      $("label[for='" + this.id + "']").addClass("add-focus");
      }).blur(function() {
      $("label").removeClass("add-focus");
    });
    // Add selected class
    $('input:checked').parent().addClass('selected');

  });

  // Create linked input fields (For using email address as username)
  $('.linked-input-master').keyup(function() {
    var masterVal = $(this).val();
    $('.linked-input-slave').val(masterVal);
  });

  // Add/remove selected class
  $('.block-label').find('input[type=radio], input[type=checkbox]').click(function() {

    $('input:not(:checked)').parent().removeClass('selected');
    $('input:checked').parent().addClass('selected');
    // Show data-toggle content
    var target = $(this).parent().attr('data-target');
    $('#'+target).show();
    
    // Hide open data-toggle content
    if($(this).prop('checked') != true) {
      $(this).parent().next('.toggle-content').hide();
    }
    // $('.toggle-content').hide();
  });

  $('.summary-trigger').on('click', function() {
    $('.summary-box').toggle();
  });

  $('.summary-close').on('click', function() {
    $('.summary-box').toggle();
  });

  $("#password-input").keyup(function() {
    initializeStrengthMeter();
  });

  function initializeStrengthMeter() {
    var username = $('#username-input').val();
    $("#pass_meter").pwStrengthManager({
      password: $("#password-input").val(),
      minChars : "8",
      blackList : [username],
      advancedStrength: true
    });
  }

  $('.pw-masktoggle').on("click", function() {
    changePassType();
    toggleShowHide();
  });

  function changePassType() {
    var password = document.getElementById('password-input');
    if (password.type == 'password') {
      password.type = 'text';
    } else {
      password.type = 'password';
    }
  }

  function toggleShowHide() {
    var showOrHide = $('.pw-masktoggle').text();
    if (showOrHide == 'Show') {
      $('.pw-masktoggle').text('Hide');
    } else {
      $('.pw-masktoggle').text('Show');
    }
  }

});