# Book-Hunt-Back

Book-Hunt-Back es una aplicación backend que utiliza la arquitectura Onion con Code First en Entity Framework Core. Este README detalla los pasos para configurar, ejecutar y manejar la base de datos correctamente.

## Estructura del proyecto

El proyecto sigue la arquitectura Onion y está dividido en las siguientes capas:

1. **DomainLayer**:
   - Esta capa contiene la lógica de negocio y las entidades principales del dominio de la aplicación. Define las reglas del negocio sin tener dependencias en otras capas.
   - Aquí encontrarás las clases que representan los modelos de datos (`Book`, `Category`, `Author`, etc.) y sus relaciones.
   
2. **RepositoryLayer**:
   - Esta capa maneja la interacción con la base de datos. Implementa los repositorios que permiten acceder y modificar los datos del dominio.
   - Está encargada de implementar los patrones de repositorio y unidad de trabajo, lo que permite realizar operaciones sobre las entidades de la base de datos sin exponer la lógica de persistencia directamente.
   - Utiliza **Entity Framework Core** para el acceso a la base de datos.

3. **ApplicationLayer**:
   - Contiene la lógica de la aplicación y los servicios que orquestan el trabajo entre la capa de dominio y las interfaces de usuario o clientes externos.
   - Aquí se encuentran los servicios de la aplicación que ejecutan las operaciones del negocio usando los repositorios de la capa de datos, así como los casos de uso y las validaciones.
   
4. **BookHunt (UI o Capa de presentación)**:
   - Es la capa más externa de la aplicación, responsable de interactuar con el mundo exterior, ya sea a través de APIs o interfaces de usuario.
   - Aquí se define la API REST que expone los servicios de la aplicación a los clientes externos.
   - Esta capa también contiene la configuración de la aplicación y el punto de entrada (`Program.cs`).

Cada capa tiene su propia responsabilidad, facilitando la separación de preocupaciones y asegurando una arquitectura robusta y escalable.

## Requisitos previos

Antes de empezar, asegúrate de tener instalado:

- [Visual Studio](https://visualstudio.microsoft.com/) o cualquier IDE compatible con .NET.
- [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads).
- .NET SDK 6.0 o superior.

## Configuración de la base de datos

El proyecto utiliza **Code First** para manejar la base de datos, por lo que se necesita una configuración correcta en el archivo `appsettings.json`. Sigue los pasos a continuación:

1. Ve al archivo `appsettings.json` ubicado en la carpeta raíz del proyecto `BookHunt/`.
2. Modifica la cadena de conexión de la base de datos con el nombre de tu servidor local. Aquí tienes un ejemplo:

    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=.\\SQLEXPRESS;Database=BookHuntDB;Trusted_Connection=True;MultipleActiveResultSets=True;Encrypt=False;TrustServerCertificate=False"
    }
    ```

    - Cambia `Server=.\\SQLEXPRESS` por el nombre de tu servidor de SQL si es necesario.
    - La base de datos se llamará `BookHuntDB`.

3. Crea la base de datos manualmente desde SQL Server Management Studio o cualquier cliente SQL, asegurándote de que el nombre de la base de datos sea **BookHuntDB**.

## Migración y creación de tablas

El proyecto ya incluye una migración inicial. Sin embargo, para que las tablas se creen en la base de datos, debes seguir estos pasos:

1. En el archivo `Program.cs`, descomenta el siguiente bloque de código:

    ```csharp
    //using (var scope = app.Services.CreateScope())
    //{
    //    var context = scope.ServiceProvider.GetRequiredService<BookHuntDBContext>();
    //    context.Database.Migrate();
    //}
    ```

2. Ejecuta la aplicación. Esto ejecutará la migración existente y creará las tablas necesarias en la base de datos previamente creada.

3. **Nota importante**: Después de que las tablas se hayan creado, es recomendable comentar nuevamente este bloque de código para evitar que las tablas sean recreadas o eliminadas en futuras ejecuciones.

    ```csharp
    //using (var scope = app.Services.CreateScope())
    //{
    //    var context = scope.ServiceProvider.GetRequiredService<BookHuntDBContext>();
    //    context.Database.Migrate();
    //}
    ```

## Ejecución de la aplicación

1. Abre el proyecto en Visual Studio.
2. Asegúrate de que la configuración de la base de datos en `appsettings.json` sea correcta.
3. Si es la primera vez que ejecutas el proyecto, descomenta el bloque de código mencionado en el archivo `Program.cs` para que las tablas se creen correctamente.
4. Ejecuta la aplicación. La base de datos se actualizará y se crearán las tablas.
5. Comenta nuevamente el bloque de código antes de la próxima ejecución.

## Compilación del proyecto

Para compilar el proyecto nuevamente:

1. Asegúrate de que el bloque de código que ejecuta las migraciones esté comentado.
2. Ejecuta la aplicación desde Visual Studio o usa la línea de comandos:

    ```bash
    dotnet build
    dotnet run
    ```

## Notas adicionales

- Si deseas agregar más migraciones en el futuro, puedes usar el siguiente comando:

    ```bash
    Add-Migration YourMigrationName
    ```

    Luego, asegúrate de descomentar el bloque de código que ejecuta `Database.Migrate()` para aplicar los cambios.

---

Con esta configuración, deberías tener tu aplicación **Book-Hunt-Back** corriendo correctamente y con la base de datos lista para funcionar.

---

# Documentación para el consumo de la API

La API de **Book-Hunt-Back** está diseñada para gestionar el backend de la aplicación. Se utiliza **JWT (JSON Web Token)** para la autorización, lo que significa que la mayoría de las peticiones deben estar autenticadas con un token Bearer. En este documento se detallan los pasos para consumir la API, los endpoints disponibles, y cómo interpretar las respuestas del servidor.

## Autorización con JWT

Para consumir cualquier endpoint de la API, es necesario incluir el token de autorización en el encabezado de la solicitud, excepto en los dos endpoints abiertos mencionados a continuación. Este token se obtiene luego de un proceso de autenticación en la aplicación.

El encabezado debe tener el siguiente formato:

Authorization: Bearer `<tu_token_jwt>`


Donde `<tu_token_jwt>` es el token que obtuviste al autenticarte.

### Endpoints sin autorización

Los siguientes dos endpoints no requieren autenticación, ya que son necesarios para crear el primer usuario administrador y asignarle un rol:

- `POST /api/Users`: Crea el primer usuario de la aplicación.
- `POST /api/Role`: Crea los roles iniciales necesarios para la gestión de usuarios.

## Estructura de las respuestas

Todas las respuestas del servidor seguirán un formato estándar que se presenta de la siguiente manera:

```json
{
  "dataList": [],
  "singleData": {},
  "thereIsError": false,
  "entityId": 0,
  "successful": false,
  "message": ".",
  "errors": []
}
```
### dataList

- **Tipo:** Array (Lista de objetos)
- **Descripción:** Contiene una lista de objetos de datos, generalmente usada cuando la consulta devuelve múltiples resultados. Si la operación no devuelve una lista, este campo estará vacío.
- **Ejemplo:**
  ```json
  "dataList": [
    { "id": 1, "name": "Book1" },
    { "id": 2, "name": "Book2" }
  ]
   ```
### singleData

- **Tipo:** Objeto
- **Descripción:** Contiene un único objeto de datos cuando la operación afecta o devuelve solo un registro específico. Si no se devuelve un objeto específico, este campo será un objeto vacío.
- **Ejemplo:**
 ```json
"singleData": {
  "id": 1,
  "name": "Book1"
}
```
### thereIsError

- **Tipo:** Booleano
- **Descripción:** Indica si hubo algún error durante el procesamiento de la solicitud. Si es true, ocurrió un error; si es false, la operación se ejecutó correctamente en términos técnicos.
- **Ejemplo:**
 ```json
"thereIsError": false
   ```
### entityId

-**Tipo:** Entero (Integer)
- **Descripción:** Contiene el identificador de la entidad que se creó, modificó o afectó en una operación exitosa. Si no se ha afectado a una entidad específica, este valor será 0.
- **Ejemplo:**
 ```json
"entityId": 15
 ```
### message
-**Tipo:** Cadena de texto (String)
- **Descripción:** Proporciona un mensaje descriptivo sobre el resultado de la operación. Puede ser una confirmación de éxito, un mensaje informativo o una descripción del error.
- **Ejemplo:**
 ```json
"message": "La operación se completó exitosamente."
   ```
### errors
-**Tipo:** Array (Lista de cadenas de texto)
- **Descripción:** Contiene una lista de errores que ocurrieron durante la solicitud. Cada error en la lista explica qué salió mal. Si thereIsError es false, este array estará vacío.
- **Ejemplo:**
 ```json
"errors": [
  "El nombre del libro es obligatorio.",
  "El formato del ISBN no es válido."
]
   ```
## Autenticación y autorizaciones

### Obtener el token JWT

Para obtener un token JWT válido, debes autenticarte enviando una solicitud al endpoint de autenticación con las credenciales de usuario. La respuesta contendrá el token que deberás usar en las cabeceras de las siguientes peticiones.

### Ejemplo de cabecera con token JWT

 ```bash
GET /api/book
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
   ```
## EndPoint

### Autenticación

Para autenticarnos en la aplicación debemos usar el siguiente endpoint:
- `POST api/Auth/login`
Se debe enviar la siguiente solicitud:
 ```json
{
  "email": "string",
  "password": "string"
}
```
Este nos responderá de la siguiente forma si el resultado fue exitoso:

 ```json
{
  "dataList": [],
  "singleData": {
    "id": 2,
    "name": "Alexander",
    "lastName": "Moreta",
    "email": "alexander@moreta.com",
    "roleId": 2,
    "roleName": "Administrador",
    "isActive": true,
    "token": "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3ht..."
  },
  "thereIsError": false,
  "entityId": 0,
  "successful": true,
  "message": null,
  "errors": []
}
```
En caso contrario:
```json
{
  "dataList": [],
  "singleData": null,
  "thereIsError": false,
  "entityId": 0,
  "successful": false,
  "message": "El usuario o la contraseña son incorrectos...",
  "errors": []
}
```

### Refrecar el token

Para refrescar el token solo hay que hacer una solicitud al siguiente enpoint:
- `GET api/Auth/refresh-token?token={TU_TOKEN_ACTUAL}`
Este retornara:
```json
{
  "dataList": null,
  "singleData": "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZ… ",
  "thereIsError": false,
  "entityId": 0,
  "successful":true,
  "message": null,
  "errors": []
}
```
## Autor

### Estructura de datos

| Campo       | Tipo de dato | Descripción                                          |
|-------------|--------------|-----------------------------------------------------|
| `authorId`  | Integer      | Identificador único del autor.                      |
| `name`      | String       | Nombre del autor.                                   |
| `lastName`  | String       | Apellido del autor.                                 |
| `isActive`  | Boolean      | Indica si el autor está activo (true) o inactivo (false). |

### Posibles solicitudes

- `GET /api/Author: ` Obtiene a todos los autores:
```json
{
  "dataList": [
    {
      "authorId": 1,
      "name": "Yunior",
      "lastName": "Moreta",
      "isActive": true,
      "books": null
    },
    {
      "authorId": 2,
      "name": "Alexander",
      "lastName": "Carrera",
      "isActive": true,
      "books": null
    }
  ],
  "singleData": null,
  "thereIsError": false,
  "entityId": 0,
  "successful": true,
  "message": null,
  "errors": []
}
```
- `POST /api/Author` : Nos sirve para guardar un autor. Solo debemos enviar el siguiente json:
```json
{
  "name": "string",
  "lastName": "string",
  "isActive": true 
}
```
Nos dará la respuesta genérica en caso de ser o no exitoso.
- `GET /api/Author/{id}`: Busca a un autor por su identificador único `authorId`.
- `PUT /api/Author/{id}` Actualiza el autor. Debemos enviar el siguiente request body:
`/api/Author/1`
```json
{
   "authorId": 1,
   "name": "Yunior",
   "lastName": "Carreras",
   "isActive": true
}
```
Nos dará la respuesta genérica en caso de ser o no exitoso.
- `DELETE /api/Author/{id}`: Nos permite eliminar un autor. Solo debemos de pasar su identificador único `authorId`.

## Libros

### Obtener todos los libros

Nos retorna un arreglo de todos los libros con los autores y las categorías a las que pertenece el libro.

**Método:** `GET`  
**Endpoint:** `/api/Book`
Nos retornará algo similar a:
```json
{
  "dataList": [
    {
      "bookId": 1,
      "title": "La monja 2",
      "description": "Aventura",
      "publicationDate": "2024-10-05T23:22:06.889",
      "isActive": true,
      "authors": [
        {
          "authorId": 1,
          "name": "Yunior Moreta",
          "isActive": true
        }
      ],
      "categories": [
        {
          "categoryId": 1,
          "name": "Terror",
          "isActive": true
        },
        {
           ...
        }
      ]
    },
    {
      ...
    }
  ],
  "singleData": null,
  "thereIsError": false,
  "entityId": 0,
  "successful": true,
  "message": null,
  "errors": []
}
```
### Obtener un libro

Nos retorna un objeto de un libro específico libro con los autores y las categorías a las que pertenece el libro.

**Método:** `GET`  
**Endpoint:** `/api/Book/{id}`
Donde `id` es el identicador del libro. `bookId`
Nos retornará algo similar a:
```json
{
  "dataList": [],
  "singleData":  {
      "bookId": 1,
      "title": "La monja 2",
      "description": "Aventura",
      "publicationDate": "2024-10-05T23:22:06.889",
      "isActive": true,
      "authors": [
        {
          "authorId": 1,
          "name": "Yunior Moreta",
          "isActive": true
        }
      ],
      "categories": [
        {
          "categoryId": 1,
          "name": "Terror",
          "isActive": true
        },
        {
           ...
        }
      ]
    },
  "thereIsError": false,
  "entityId": 0,
  "successful": true,
  "message": null,
  "errors": []
}
```
### Guardar un libro

Para agregar un nuevo libro a la base de datos, utiliza el siguiente endpoint de la API:

**Método:** `POST`  
**Endpoint:** `/api/Book`

#### Parámetros

- **Request Body**: Debe enviarse en formato JSON con la siguiente estructura:

```json
{
  "title": "string",                  // Título del libro
  "description": "string",            // Descripción del libro
  "publicationDate": "2024-10-07T01:18:08.333Z", // Fecha de publicación del libro (formato yyyy-MM-dd)
  "isActive": true,                   // Indica si el libro está activo (true) o inactivo (false)
  "authorIds": [                      // Lista de identificadores de autores asociados al libro
    0                                 // Ejemplo: id del autor
  ],
  "categoryIds": [                    // Lista de identificadores de categorías asociadas al libro
    0                                 // Ejemplo: id de la categoría
  ]
}
```

| Campo            | Tipo de dato | Descripción                                          |
|------------------|--------------|-----------------------------------------------------|
| `title`          | String       | Título del libro.                                   |
| `description`    | String       | Descripción del libro.                              |
| `publicationDate`| DateTime     | Fecha de publicación del libro (formato yyyy-MM-dd). |
| `isActive`       | Boolean      | Indica si el libro está activo (true) o inactivo (false). |
| `authorIds`      | Array        | Lista de identificadores de autores asociados al libro. |
| `categoryIds`    | Array        | Lista de identificadores de categorías asociadas al libro. |

### Actualizar un libro

Para actualizar un nuevo libro a la base de datos, utiliza el siguiente endpoint de la API:

**Método:** `PUT`  
**Endpoint:** `/api/Book/{id}`

#### Parámetros

- **Path Parameter**: El ID del libro que deseas actualizar. Debe enviarse en la URL del endpoint.
- **Request Body**: Debe enviarse en formato JSON con la siguiente estructura. La estructura debera contener la información a actualizar:

```json
{
  "title": "string",                  // Título del libro
  "description": "string",            // Descripción del libro
  "publicationDate": "2024-10-07T01:18:08.333Z", // Fecha de publicación del libro (formato yyyy-MM-dd)
  "isActive": true,                   // Indica si el libro está activo (true) o inactivo (false)
  "authorIds": [                      // Lista de identificadores de autores asociados al libro
    0                                 // Ejemplo: id del autor
  ],
  "categoryIds": [                    // Lista de identificadores de categorías asociadas al libro
    0                                 // Ejemplo: id de la categoría
  ]
}
```

| Campo            | Tipo de dato | Descripción                                          |
|------------------|--------------|-----------------------------------------------------|
| `title`          | String       | Título del libro.                                   |
| `description`    | String       | Descripción del libro.                              |
| `publicationDate`| DateTime     | Fecha de publicación del libro (formato yyyy-MM-dd). |
| `isActive`       | Boolean      | Indica si el libro está activo (true) o inactivo (false). |
| `authorIds`      | Array        | Lista de identificadores de autores asociados al libro. |
| `categoryIds`    | Array        | Lista de identificadores de categorías asociadas al libro. |


### Eliminar un libro

Para eliminar un libro de la base de datos, utiliza el siguiente endpoint de la API:

**Método:** `DELETE`  
**Endpoint:** `/api/Book/{id}`

### Parámetros

- **Path Parameter**: El ID del libro que deseas eliminar. Debe enviarse en la URL del endpoint.

| Nombre | Descripción                       |
|--------|-----------------------------------|
| `id`   | Identificador único del libro a eliminar (requerido). Debe ser de tipo `integer`. |

**NOTA**: Todos los endpoint darán la respuesta genérica independientemente si es satisfactoria o no.

