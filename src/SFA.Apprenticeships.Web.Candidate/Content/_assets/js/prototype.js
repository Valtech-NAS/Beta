$(function() {
// ------------ Trainee search mockup ------------ //

  $('.trainee-input').on('keyup', function(){
    var $this   = $(this).val(),
        $input1 = $('.trainee-input-1').val(),
        $input2 = $('.trainee-input-2').val(),
        $input3 = $('.trainee-input-3').val(),
        $index  = $.jStorage.index();

    if($this.toLowerCase().indexOf('cust') > -1){
      $('.trainee-searchbtn').attr('href', 'search-results-customer.html');
    } else if($this.toLowerCase().indexOf('admin') > -1){
      $('.trainee-searchbtn').attr('href', 'search-results-admin.html');
    }

    $.jStorage.set('input1Key', $input1);
    $.jStorage.set('input2Key', $input2);
    $.jStorage.set('input3Key', $input3);

  });

  $('.trainee-result-input').on('keyup', function(){
    var $this   = $(this).val(),
        $input1 = $('.trainee-result-input-1').val(),
        $input2 = $('.trainee-result-input-2').val(),
        $input3 = $('.trainee-result-input-3').val();

    if($this.toLowerCase().indexOf('cust') > -1){
      $('.update-results-btn').attr('href', 'search-results-customer.html');
    } else if($this.toLowerCase().indexOf('admin') > -1){
      $('.update-results-btn').attr('href', 'search-results-admin.html');
    }

    $.jStorage.set('input1Key', $input1);
    $.jStorage.set('input2Key', $input2);
    $.jStorage.set('input3Key', $input3);
  });

  function changeSearchInputs() {
    var resultInput1 = $.jStorage.get('input1Key'),
        resultInput2 = $.jStorage.get('input2Key'),
        resultInput3 = $.jStorage.get('input3Key');

    $('.trainee-result-input-1').val(resultInput1);
    $('.trainee-result-input-2').val(resultInput2);
    $('.trainee-result-input-3').val(resultInput3);
  }

  changeSearchInputs();

// ------------ Apply for vacancy mockup ------------ //

  $('#saveQualification').on('click', function(){
    var $qualType    = $('#qual-type').val(),
        $qualSubject = $('#subject-name').val()
        $qualGrade   = $('#subject-grade').val()
        $isPredicted = $('#qual-predicted').is(':checked');

  });

// --------------- Remove for live code -------------- //
});