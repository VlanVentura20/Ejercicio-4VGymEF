using _4VGymEF.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Numerics;
using System.Text.RegularExpressions;

namespace _4VGymEF.Repositories
{
    public class InMemoryGymMonitorRepository
    {

        private Dictionary<int,GymMonitor> _gymMonitors;
        private int _nextId = 1;
        public InMemoryGymMonitorRepository()
        {
            SeedData(); // Precargamos algunos datos de prueba
            _nextId = _gymMonitors.Count + 1;
        }
        private void SeedData()
        {
            _gymMonitors = new Dictionary<int, GymMonitor>
            {
                { 1, new GymMonitor { Name = "Izan Ventura", Email = "izan@gmail.com", Image = "izan.jpg", Phone = "600123456" } },
                { 2, new GymMonitor { Name = "Markel García", Email = "markel@gmail.com", Image = "markel.jpg", Phone = "611234567" } },
                { 3, new GymMonitor { Name = "Laura Martínez", Email = "laura@gmail.com", Image = "laura.jpg", Phone = "622345678" } },
                { 4, new GymMonitor { Name = "Ander López", Email = "ander@gmail.com", Image = "ander.jpg", Phone = "633456789" } }
            };

            foreach (var gymMonitor in _gymMonitors)
            {
                gymMonitor.Value.Id = gymMonitor.Key;
            }

        }
        // Método para obtener todos los tipos de actividades
        public IEnumerable<GymMonitor> GetAll()
        {
            return _gymMonitors.Values;
        }


        public GymMonitor GetById(int id)
        {
            foreach (var gymMonitor in _gymMonitors)
            {
                if (gymMonitor.Value.Id == id)
                {
                    return gymMonitor.Value;
                }
            }
            return null;
        }

        public IEnumerable<GymMonitor> GetByFirstInitial(string? firstInitial = null)
        {
            // 1. Caso base: si no hay filtro o viene vacío, devolvemos toda la lista
            if (string.IsNullOrWhiteSpace(firstInitial))
            {
                return GetAll();
            }

            // 2. Si hay filtro, creamos una nueva lista para almacenar las coincidencias
            List<GymMonitor> filteredInstructors = new List<GymMonitor>();

            // 3. Recorremos la colección manualmente sin usar LINQ
            foreach (GymMonitor instructor in _gymMonitors.Values)
            {
                if (instructor.Name != null &&
                    instructor.Name.StartsWith(firstInitial, StringComparison.OrdinalIgnoreCase))
                {
                    filteredInstructors.Add(instructor);
                }
            }

            return filteredInstructors;
        }


        public IEnumerable<GymMonitor> GetByFirstInitial2(string? firstInitial = null)
        {
            IEnumerable<GymMonitor> _allInstructors = _gymMonitors.Values;

            // Si se recibe la inicial, aplicamos el filtro sobre la colección
            if (!string.IsNullOrWhiteSpace(firstInitial))
            {
                IEnumerable<GymMonitor> filteredInstructors =
                    _allInstructors
                        .Where(instructor =>
                                     !string.IsNullOrEmpty(instructor.Name) &&
                                     instructor.Name.StartsWith(firstInitial, StringComparison.OrdinalIgnoreCase)
                        );

                return filteredInstructors;
            }

            return _allInstructors;

        }

        public GymMonitor? Add(GymMonitor gymMonitor, out ErrorResponse? errorResponse)
        {
            errorResponse = null;

            if (gymMonitor == null)
            {
                errorResponse = new ErrorResponse(3, "Invalid activity type data");
                return null;
            }
            if (gymMonitor.Name == null)
            {
                errorResponse = new ErrorResponse(4, "The name of the gym monitor is mandatory");
                return null;
            }

            gymMonitor.Id = _gymMonitors.Keys.Max() + 1;

            _gymMonitors.Add(gymMonitor.Id, gymMonitor);

            // Devolvemos el objeto completo tal como se ha guardado
            return gymMonitor;
        }

        public Boolean CheckIfExistsWithNameMail (GymMonitor gymMonitor)
        {
            return _gymMonitors.Values.Any(per =>
               per.Name.ToLower() == gymMonitor.Name.ToLower() &&
               per.Email.ToLower() == gymMonitor.Email.ToLower());
        }


    }
}
