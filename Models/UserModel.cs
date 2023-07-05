using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechTest.Models
{
    public class UserModel
    {

        [Key]
        public int PersonId { get; set; }

        [DisplayName("Nombre de Usuario")]
        [Required(ErrorMessage = "El campo Nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El campo nombre no debe de tener más de 100 carácteres.")]
        public string PersonName { get; set; }

        [DisplayName("Correo Electrónico")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [Required(ErrorMessage = "El campo Correo es obligatorio.")]
        public string Email { get; set; }


        [DisplayName("Edad")]
        [RegularExpression(@"^\d*[1-9]\d*$", ErrorMessage = "No se permite edades en negativo o 0")]
        [Required(ErrorMessage = "El campo Edad es obligatorio.")]
        [Range(1, 100, ErrorMessage = "Valor fuera del rango (1 a 100)")]
        public int PersonAge { get; set; }

        [DisplayName("Estado")]
        [Required]
        public bool PersonStatus { get; set; }

        [DisplayName("Género")]
        public char? PersonSex { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [DisplayName("Fecha de Alta")]
        public DateTime PersonDisDate { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [DisplayName("Fecha de Modificación")]
        public DateTime PersonModDate { get; set; }


    }
}