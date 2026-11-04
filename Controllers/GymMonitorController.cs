using _4VGymEF.Models;
using _4VGymEF.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Text.RegularExpressions;

[ApiController]
[Route("gym-monitor")]
public class GymMonitorController : ControllerBase
{
    private readonly InMemoryGymMonitorRepository _repository;

    // Inyectamos la clase concreta del repositorio
    public GymMonitorController(InMemoryGymMonitorRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// GET /gym-monitors
    /// Devuelve el listado de monitores de 4VGym.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GymMonitor>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]

    public ActionResult<IEnumerable<GymMonitor>> FindGymMonitor([FromQuery(Name = "first_initial")] string? firstInitial)
    {
        try
        {
            IEnumerable<GymMonitor> instructors = _repository.GetByFirstInitial(firstInitial);

            return Ok(instructors);
        }
        catch (Exception)
        {
            return BadRequest(new ErrorResponse(1, "Any problem in the Server"));
        }
    }



    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GymMonitor), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public ActionResult<GymMonitor> FindGymMonitorById([FromRoute] int id)
    {
        try
        {
            GymMonitor gymMonitor = _repository.GetById(id);

            if (gymMonitor == null)
            {
                return BadRequest(new ErrorResponse(2, "No gymMonitor found"));
            }

            return Ok(gymMonitor);
        }
        catch (Exception)
        {
            return BadRequest(new ErrorResponse(1, "Any problem in the Server"));
        }
    }

    /// <summary>
    /// POST /gymMonitors
    /// Añade un nuevo monitor en 4VGym.
    /// </summary>
    /// <param name="gymMonitors">ObjetoGymMonitors enviado en el cuerpo de la petición (JSON).</param>
    [HttpPost]
    [ProducesResponseType(typeof(GymMonitor), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public ActionResult<GymMonitor> CreateGymMonitor([FromBody] GymMonitor gymMonitor)
    {
        List<ErrorResponse> miListaErrores = new List<ErrorResponse>();

        try
        {
            if (gymMonitor == null)
            {
                return BadRequest(new ErrorResponse(3, "Invalid Gym Monitor data"));
            }

            if (String.IsNullOrWhiteSpace(gymMonitor.Name))
            {
                miListaErrores.Add(new ErrorResponse(4, "The name of the Gym Monitor is mandatory"));
            }

            if (String.IsNullOrWhiteSpace(gymMonitor.Email))
            {
                miListaErrores.Add(new ErrorResponse(5, "The email of the Gym Monitor is mandatory"));
            }

            if (!GymMonitor.CheckEmail(gymMonitor.Email))
            {
                miListaErrores.Add(new ErrorResponse(6, "The email of the Gym Monitor is not valid"));
            }

            if (String.IsNullOrWhiteSpace(gymMonitor.Phone))
            {
                miListaErrores.Add(new ErrorResponse(7, "The phone of the Gym Monitor is mandatory"));
            }

            if (!GymMonitor.CheckPhone(gymMonitor.Phone))
            {
                miListaErrores.Add(new ErrorResponse(8, "The phone of the Gym Monitor is not valid"));
            }

            if (!Uri.TryCreate(gymMonitor.Image, UriKind.Absolute, out Uri? uri) ||
                (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
            {
                return BadRequest(new ErrorResponse(9, "The url image of the Gym Monitor is not valid"));
            }

            ErrorResponse? errorResponse;

            if (_repository.CheckIfExistsWithNameMail(gymMonitor))
            {
                miListaErrores.Add(new ErrorResponse(10, "The gym monitor already exists"));
            }

            if (miListaErrores.Count != 0)
            {
                return BadRequest(miListaErrores);
            }

            GymMonitor? createdGymMonitor = _repository.Add(gymMonitor, out errorResponse);

            if (errorResponse != null)
            {
                return BadRequest(errorResponse);
            }

            return StatusCode(StatusCodes.Status201Created, createdGymMonitor);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ErrorResponse(1, ex.Message));
        }
    }


}
