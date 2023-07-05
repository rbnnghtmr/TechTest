using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechTest.Models
{
    public class LoginModel
    {

        [Key]
        public int UserId { get; set; }


        [DisplayName("Correo Electrónico")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [Required(ErrorMessage = "El campo Correo es obligatorio.")]
        public string Email { get; set; }

        [DisplayName("Contraseña")]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "Contraseña demasiado débil")]
        [Required(ErrorMessage = "El campo contraseña es obligatorio")]
        public string UserPassword { get; set; }


        [DisplayName("Estado")]
        [Required]
        public bool UserStatus { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [DisplayName("Fecha de Alta")]
        public DateTime UserDisDate { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [DisplayName("Fecha de Modificación")]
        public DateTime UserModDate { get; set; }


    }
}
