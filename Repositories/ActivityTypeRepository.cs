using _4VGymEF.Models;

namespace _4VGymEF.Repositories
{
    public class ActivityTypeRepository
    {
        private List<ActivityType> _activityTypes = new();
        private int _nextId = 1;
        public ActivityTypeRepository()
        {
            SeedData(); // Precargamos algunos datos de prueba
        }
        private void SeedData()
        {
            _activityTypes = new List<ActivityType>()
            {
                new ActivityType (10, "BodyPump",  2 ),
                new ActivityType ( 20, "Spinning",  1 ),
                new ActivityType { Id = 30, Name = "Pilates", NumberInstructors = 1 }
            };
            _activityTypes.Add(new ActivityType(40, "CrossFit", 3));
        }
        // Método para obtener todos los tipos de actividades
        public IEnumerable<ActivityType> GetAll()
        {
            return _activityTypes;
        }

        public ActivityType? GetByName(string name)
        {
            return _activityTypes.SingleOrDefault(at =>
          string.Equals(at.Name, name, StringComparison.OrdinalIgnoreCase));
        }

        public ActivityType? Add(ActivityType activityType, out ErrorResponse? errorResponse)
        {
            errorResponse = null; // Inicializamos a null para cuando no hay error

            // Validaciones
            if (activityType == null)
            {
                errorResponse = new ErrorResponse(3, "Invalid activity type data");
                return null;

            }
            if (activityType.Name == null)
            {
                errorResponse = new ErrorResponse(4, "The name of the activity is mandatory");
                return null;
            }
            if (activityType.NumberInstructors < 1)
            {
                activityType.NumberInstructors = 1;
            }
            // No puede haber tipos de actividad con el mismo nombre
            if (this.GetByName(activityType.Name) != null)
            {
                errorResponse = new ErrorResponse(6, "The name is unique, there is already another ActivityType with the same Name");
                return null;
            }

            // Asignamos el ID autoincremental en la capa de datos, o sea que machacamos el que venia
            activityType.Id = _nextId++;

            _activityTypes.Add(activityType);

            // Devolvemos el objeto completo tal como se ha guardado
            return activityType;
        }

        public ActivityType? GetById(long id)
        {
            return _activityTypes.FirstOrDefault(activityType => activityType.Id == id);
        }


    }

}
