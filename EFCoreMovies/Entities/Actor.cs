using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCoreMovies.Entities
{
    public class Actor : BaseEntity
    {

        private string _name;
        //Mapeo flexible => Podemos almazenar el valor del campo de una clase y no simplemente una propiedad
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = string.Join(' ',
                        value.Split(' ')
                        .Select(x => x[0].ToString().ToUpper() + x.Substring(1).ToLower()).ToArray());
            }
        }
        public string Biography { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public HashSet<MovieActor> MoviesActors { get; set; }

    }
}

