using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodingChallenge.Api.Models
{
    [Table("RX_Job")]
    public class Job
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int JobId { get; set; }
        public string Status { get; set; }
        public int Floor { get; set; }
        public JobStatus StatusNum { get; set; }
    }
}
