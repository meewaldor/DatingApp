using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;

namespace DatingAPI.Models
{
    public class UserLike
    {
        public User SourceUser { get; set; }
        public int SourceUserId { get; set; }
        public User TargetUser { get; set; }
        public int TargetUserId { get; set; }

    }
}
