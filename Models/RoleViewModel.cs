using System.ComponentModel.DataAnnotations;

namespace Hospitel_Project.Models
{
    public class RoleViewModel
    {
        [Display(Name = "Role Name") ]
        public string RoleName { get; set; }
    }
}
