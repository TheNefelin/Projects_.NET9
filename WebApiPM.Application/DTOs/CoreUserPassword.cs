using System.ComponentModel.DataAnnotations;

namespace WebApiPM.Application.DTOs;

public class CoreUserPassword
{
    // Minimo 6 en la verificacion (uso). Nota: hay claves legacy creadas sin
    // validacion de longitud; si una clave existente tuviera menos de 6
    // caracteres, esta validacion la bloquearia (fuente de fallo conocida y
    // documentada en DEVELOPMENT.md). Destino futuro: subir a 8.
    [MinLength(6)]
    [MaxLength(50)]
    public required string Password { get; set; }

    public required CoreUserRequest CoreUser { get; set; }
}
