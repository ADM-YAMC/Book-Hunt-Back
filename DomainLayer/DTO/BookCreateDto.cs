using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.DTO
{
    public class BookCreateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PublicationDate { get; set; }
        public bool IsActive { get; set; }
        public List<int> AuthorIds { get; set; }
        public List<int> CategoryIds { get; set; }
    }
}
