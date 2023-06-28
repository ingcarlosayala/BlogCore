
var datatable;

$(document).ready(function () {
    CargarTabla();
});

const CargarTabla = () => {
    datatable = $("#tblDatos").DataTable({
        "ajax": {
            "url": "/Admin/Articulos/ObtenerTodos"
        },
        "columns":[
            {"data": "nombre", "width": "15%"},
            {"data": "categoria.nombre", "width": "15%"},
            { "data": "fechaCreacion", "width": "15%"},
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                                <a href="/Admin/Articulos/Edit/${data}" class="btn btn-dark btn-sm"> <i class="bi bi-pen"></i> </a>
                                <a onclick=Delete("/Admin/Articulos/Delete/${data}") class="btn btn-danger btn-sm"> <i class="bi bi-trash"></i> </a>
                            </div>`;
                },"width": "15%"
            }
        ]
    });
}

const Delete = url => {
    Swal.fire({
        title: 'Estas Seguro?',
        text: "No podrás revertir esto!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'SI, Eliminar!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        datatable.ajax.reload();
                    } else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    })
}