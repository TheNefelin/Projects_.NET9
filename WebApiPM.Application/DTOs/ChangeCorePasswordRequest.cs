using System.ComponentModel.DataAnnotations;

namespace WebApiPM.Application.DTOs;

public class ChangeCorePasswordRequest
{
    // Minimo 6 en la verificacion de la clave actual, igual que en get-iv.
    // Nota: hay claves legacy creadas sin validacion de longitud; si una clave
    // existente tuviera menos de 6 caracteres, este control la bloquearia y el
    // cambio de clave maestra no serviria como salida (fuente de fallo
    // conocida, documentada en DEVELOPMENT.md). Destino futuro: subir a 8.
    [MinLength(6)]
    [MaxLength(50)]
    public required string OldPassword { get; set; }

    [MinLength(8)]
    [MaxLength(50)]
    public required string NewPassword { get; set; }

    // Nueva sal/IV (base64 de 16 bytes) generada por el cliente. Es material
    // publico del cifrado client-side, y su largo se valida en el servicio para
    // descartar valores invalidos o malformados.
    [Required]
    public required string Salt { get; set; }

    public required CoreUserRequest CoreUser { get; set; }

    // Registros re-cifrados por el cliente con la nueva clave y la nueva sal.
    // Se reemplazan de forma atomica junto con la rotacion de la clave.
    public required List<CoreDataReplacement> Records { get; set; }
}
