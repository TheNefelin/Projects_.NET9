using System.ComponentModel.DataAnnotations;

namespace WebApiPM.Application.DTOs;

public class CoreUserPasswordCreate
{
    // Minimo 8 en la creacion de la clave maestra (mas estricto que la
    // verificacion, que se queda en 6 por compatibilidad con las claves ya
    // creadas).
    [MinLength(8)]
    [MaxLength(50)]
    public required string Password { get; set; }

    public required CoreUserRequest CoreUser { get; set; }
}
