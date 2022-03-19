
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Technical_Assessement_API.Models
{
    public class Author
    {
      
        [Key]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime DOB { get; set; }

        public string Bio { get; set; }

        public string Image { get; set; }

        [NotMapped]
        public List<int> Booksid { get; set; }

        public virtual ICollection<AuthorsBooks> AuthorsBooks { get; set; }
    }
}
