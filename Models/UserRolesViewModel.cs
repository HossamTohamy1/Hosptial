namespace Hospitel_Project.Models
{
    public class UserRolesViewModel
    {
        public ApplicationUser User { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}
