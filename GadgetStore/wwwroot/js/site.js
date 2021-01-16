function putInCart(id)
{
   /* $.ajax({
        method: 'GET',
        url: 'MyCart/Add',
        dataType: 'HTML',
        data: { 'Id': id },
        success: (data) => {
                
                $('#modal-info').html(data);
                $('#smallModal').modal({
                    show: true
                });
            }
    });
    */

    $.get('MyCart/Add', { Id: id }, function(data) {
        $('#modal-info').html(data);
        $('#smallModal').modal({
            show: true
        });
    });
}