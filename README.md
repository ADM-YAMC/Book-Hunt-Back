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
   
4. **BookHunt (UI o Capa de Presentación)**:
   - Es la capa más externa de la aplicación, responsable de interactuar con el mundo exterior, ya sea a través de APIs o interfaces de usuario.
   - Aquí se define la API REST que expone los servicios de la aplicación a los clientes externos.
   - Esta capa también contiene la configuración de la aplicación y el punto de entrada (`Program.cs` y `Startup.cs`).

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
