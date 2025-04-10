using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Hospitel_Project.Models
{
    public class PatientMedicament
    {
        [Key]
        public int Id { get; set; }

        public int PatientId { get; set; }
        public int MedicamentId { get; set; }

        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }

        [ForeignKey("MedicamentId")]
        public virtual Medicament Medicament { get; set; }
    }

}
