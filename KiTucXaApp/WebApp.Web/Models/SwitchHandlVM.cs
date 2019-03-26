using System.ComponentModel.DataAnnotations;

namespace WebApp.Web.Models
{
    public class SwitchHandlVM
    {
        [Required]
        [StringLength(127)]
        public string SwitchRequestId { get; set; }

        [Required]
        [StringLength(1023)]
        public string HandledNote { get; set; }

        public bool Status { get; set; }
    }
}