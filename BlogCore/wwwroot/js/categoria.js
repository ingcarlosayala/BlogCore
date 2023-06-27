
var datatable;

$(document).ready(function () {
    CargarTabla();
});

const CargarTabla = () => {
    datatable = $("#tblDatos").DataTable({
        "ajax": {
            "url": "/Admin/Categorias/ObtenerTodos"
        },
        "columns": [
            { "data": "nombre", "width": "20%" },
            {
                "data": "estado",
                "render": function (data) {
                    if (data) {
                        return "Activo"
                    } else {
                        return "Inactivo"
                    }
                },"width": "20"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                                <a href="/Admin/Categorias/Edit/${data}" class="btn btn-dark btn-sm"> <i class="bi bi-pen"></i> </a>
                                <a onclick=Delete("/Admin/Categorias/Delete/${data}") class="btn btn-danger btn-sm"> <i class="bi bi-trash"></i> </a>
                            </div>`;
                },"width": "20"
            }
        ]
    });
}

const Delete = url => {
    Swal.fire({
        title: 'Estas Seguro?',
        text: "¡No podrás revertir esto!!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Si, Eliminar!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data) {
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