$(function() {

    $('input[id=EmailAddress').focusout(function() {
        var request = new UsernameModel();

        alert(JSON.stringify(request));
    });

});

function UsernameModel() {
    var self = this;
    self.email = $('input[id=EmailAddress').val();
}