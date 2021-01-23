function putInCart(id)
{
    $.get('/MyCart/Add', { Id: id }, function(data) {
        $('#modal-info').html(data);
        $('#smallModal').modal({
            show: true
        });
    });
}

function search() {
    var searchInput = $('#searachString').val();

    $('#assortment').load('/Gadgets/Search', { searchName: searchInput });
}

function filter() {
    $('#assortment').load('/Gadgets/Filter', {
        GadgetType: $('#GadgetType').val(),
        Manufacturer: $('#Manufacturer').val(),
        Diagonal: $('#Diagonal').val(),
        ScreenResolution: $('#ScreenResolution').val(),
        Color: $('#Color').val(),
        CPU: $('#CPU').val(),
        Memory: $('#Memory').val(),
        RAM: $('#RAM').val()
    });
}