using _4VGymEF.Models;
using _4VGymEF.Repositories;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("activity-type")]
public class ActivityTypeController : ControllerBase
{
    private readonly ActivityTypeRepository _repository;

    // Inyectamos la clase concreta del repositorio
    public ActivityTypeController(ActivityTypeRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// GET /activity-types
    /// Devuelve el listado de tipos de actividades de 4VGym.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ActivityType>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public ActionResult<IEnumerable<ActivityType>> FindActivityTypes()
    {
        try
        {
            IEnumerable<ActivityType> activityTypes = _repository.GetAll();

            if (activityTypes == null || activityTypes.Count() == 0)
            {
                return BadRequest(new ErrorResponse(2, "No activity types found"));
            }

            return Ok(activityTypes);
        }
        catch (Exception)
        {
            return BadRequest(new ErrorResponse(1, "Any problem in the Server"));
        }
    }

    /// <summary>
    /// POST /activity-types
    /// Añade un nuevo tipo de actividad en 4VGym.
    /// </summary>
    /// <param name="activityType">ObjetoActivityType enviado en el cuerpo de la petición (JSON).</param>
    [HttpPost]
    [ProducesResponseType(typeof(ActivityType), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public ActionResult<ActivityType> CreateActivityType([FromBody] ActivityType activityType)
    {
        try
        {
            if (activityType == null)
            {
                return BadRequest(new ErrorResponse(3, "Invalid activity type data"));
            }

            if (String.IsNullOrWhiteSpace(activityType.Name))
            {
                return BadRequest(new ErrorResponse(4, "The name of the activity is mandatory"));
            }

            ErrorResponse? errorResponse;

            ActivityType? createdActivityType = _repository.Add(activityType, out errorResponse);

            if (errorResponse != null)
            {
                return BadRequest(errorResponse);
            }

            return StatusCode(StatusCodes.Status201Created, createdActivityType);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                              new ErrorResponse(1, "An internal server error occurred"));
        }
    }



}
