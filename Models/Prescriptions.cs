using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Hospitel_Project.Models
{
    public class Prescription
    {
        [Key]
        public int PrescriptionId { get; set; }  

        [Required]
        public int PatientId { get; set; }  

        [Required]
        public int MedicamentId { get; set; }  

        [DataType(DataType.DateTime)]
        public DateTime DatePrescribed { get; set; } = DateTime.Now;  

        public int Quantity { get; set; }

        public string description { get; set; }  

        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }  

        [ForeignKey("MedicamentId")]
        public virtual Medicament Medicament { get; set; } 
    }
}
