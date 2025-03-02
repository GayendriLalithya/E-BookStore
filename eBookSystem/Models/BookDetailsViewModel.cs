using eBookSystem.Models;
namespace eBookSystem.Models
{
    public class BookDetailsViewModel
    {
        public Book  Book { get; set; }
        public List<Review> Reviews { get; set; }
    }
}