using System.ComponentModel.DataAnnotations.Schema;

namespace Task_management.Models
{

    [Table("status")]
    public class StatusModel
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("title")]
        public string Title { get; set; }

    }
}
