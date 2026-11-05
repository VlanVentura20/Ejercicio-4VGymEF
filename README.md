# 4VGymEF 🏋️

API REST para la gestión de actividades deportivas, tipos de actividad e instructores de un gimnasio, desarrollada con **ASP.NET Core** y **Entity Framework Core**.

## Tecnologías

- C# y ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- OpenAPI 3.1.1 / Swagger

## Ejecución

1. Abrir la solución en Visual Studio.
2. Configurar la cadena de conexión a SQL Server en `appsettings.json` según el entorno.
3. Restaurar las dependencias NuGet.
4. Aplicar las migraciones de Entity Framework Core, si corresponde:

   ```bash
   dotnet ef database update
   ```

5. Ejecutar la aplicación.

La URL HTTPS indicada en el documento OpenAPI es `https://localhost:7065/`. El puerto puede variar según la configuración local. Si Swagger UI está habilitado, normalmente se puede consultar en `/swagger`.

## Endpoints

| Método | Ruta | Descripción | Respuestas documentadas |
|---|---|---|---|
| GET | `/activity-types` | Obtener tipos de actividad | 200, 400 |
| POST | `/activity-types` | Crear un tipo de actividad | 201, 400, 500 |
| GET | `/instructors` | Listar instructores; admite filtro `first_initial` | 200, 400 |
| GET | `/instructors/{id}` | Obtener un instructor por ID | 200, 400 |
| POST | `/instructors` | Crear un instructor | 201, 400, 500 |
| POST | `/activities` | Crear una actividad | 201, 400, 500 |

> Los endpoints y códigos HTTP anteriores proceden de la especificación OpenAPI facilitada. No se documentan endpoints de modificación o eliminación.

## Tipos de actividad

### `GET /activity-types`

Devuelve una lista de tipos de actividad. Cada elemento puede incluir `id`, `name`, `numberInstructors` y `listaAtividades` (relación con actividades).

### `POST /activity-types`

Ejemplo de cuerpo JSON:

```json
{
  "name": "Zumba",
  "number-monitors": 2
}
```

El esquema de creación se llama `ActivityTypeCreateDto` y contiene `id`, `name` y `number-monitors`. La respuesta de creación documentada es **201 Created**.

## Instructores

### `GET /instructors`

Devuelve la lista de instructores. Se puede filtrar por inicial del nombre:

```http
GET /instructors?first_initial=a
```

### `GET /instructors/{id}`

Consulta un instructor por su identificador:

```http
GET /instructors/2
```

### `POST /instructors`

Ejemplo de cuerpo JSON:

```json
{
  "name": "Ana García",
  "email": "ana@example.com",
  "photo_url": "https://example.com/ana.jpg",
  "phone": "600123456"
}
```

El esquema `InstructorCreateDto` define los campos `id`, `name`, `email`, `photo_url` y `phone`. El modelo de lectura `Instructor` usa `photoUrl` y puede incluir la colección `actividades`.

## Actividades

### `POST /activities`

Permite crear una actividad vinculada a un tipo de actividad y a uno o varios instructores.

```json
{
  "activity_type_id": 1,
  "instructors_id": [1, 2],
  "max_particpants": 20,
  "date_start": "2027-01-15T10:00:00",
  "date_end": "2027-01-15T11:00:00"
}
```

Campos del esquema `ActivityCreateDto`:

| Campo | Tipo | Restricciones documentadas |
|---|---|---|
| `id` | Entero | Opcional en el esquema |
| `activity_type_id` | Entero o null | Propiedad requerida |
| `instructors_id` | Array de enteros | Requerido; mínimo 1 elemento |
| `max_particpants` | Entero | Entre 1 y 25, si se proporciona |
| `date_start` | Fecha y hora | Requerida; formato `date-time` |
| `date_end` | Fecha y hora | Requerida; formato `date-time` |

**Importante:** `max_particpants` está escrito así en el OpenAPI (sin la segunda `i` de `participants`). Se mantiene exactamente para que los ejemplos sean compatibles con la API documentada.

El modelo `Activity` también contempla `listaInstructores` y `tipoActividad`.

## Respuestas y errores

| Código | Significado |
|---|---|
| `200 OK` | Consulta completada |
| `201 Created` | Recurso creado |
| `400 Bad Request` | Solicitud no válida o error de negocio |
| `500 Internal Server Error` | Error interno del servidor |

Los errores se representan mediante el esquema `ErrorResponse`:

```json
{
  "code": 30,
  "description": "Descripción del error"
}
```

Este JSON es **ilustrativo**, no un error concreto garantizado por el OpenAPI. Según el endpoint, las respuestas 400 pueden contener un único `ErrorResponse` o una lista de ellos.

## Estructura de datos

La especificación incluye los siguientes modelos:

- `ActivityType`: tipo de actividad, número de instructores y actividades relacionadas.
- `Instructor`: nombre, correo, fotografía, teléfono y actividades relacionadas.
- `Activity`: tipo de actividad, aforo, fechas e instructores asignados.
- `ActivityTypeCreateDto`, `InstructorCreateDto` y `ActivityCreateDto`: objetos utilizados para las operaciones de creación.
- `ErrorResponse`: código y descripción del error.

## Documentación de la API

La API está descrita con **OpenAPI 3.1.1**, versión **1.0.0**, bajo el título `4VGymEF | v1`.

Se pueden probar las rutas mediante Swagger UI (si está configurado), Visual Studio con archivos `.http` o clientes HTTP como Postman.

---

**Nota:** este README se basa en el OpenAPI suministrado. Las tecnologías y los pasos de ejecución se corresponden con la configuración conocida del proyecto; la especificación por sí sola no confirma todas las reglas de negocio ni el estado de la base de datos.
