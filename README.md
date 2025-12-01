# ROMAR - Flujo de la aplicación por capas

Este documento explica de forma concisa cómo está organizada y cómo fluye la aplicación por capas en este repositorio.

## Estructura de proyectos
- `API` : Capa de presentación (Web API). Contiene controladores y configuración de DI.
- `Application` : Capa de lógica de aplicación. Contiene servicios, interfaces de servicios y orquestación de procesos (p. ej. `ExternalEtlService`).
- `Domain` : Capa de dominio. Contiene modelos (`Cliente`) e interfaces de repositorio (`IClienteRepository`).
- `Infraestructure` : Capa de infraestructura. Implementaciones concretas de acceso a datos (p. ej. `ClientRepository`) y contexto de acceso a BD (`DapperContext`).

## Flujo general (petición típica)
1. El cliente HTTP hace una petición al `API` al endpoint expuesto por un controlador (p. ej. `ClienteController`).
2. El `Controller` valida la entrada mínima y delega la operación al servicio correspondiente definido en la capa `Application` (p. ej. `IClienteService` → `ClienteService`).
3. El `Application Service` implementa la lógica de aplicación: validaciones adicionales, coordinación entre repositorios o servicios externos (p. ej. `ExternalEtlService`) y preparación de datos.
4. Para operaciones de persistencia el servicio llama a las interfaces del dominio (`IClienteRepository`).
5. En tiempo de ejecución el contenedor de DI (configurado en `API/Program.cs`) resuelve la implementación concreta proporcionada por la capa `Infraestructure` (p. ej. `ClientRepository`) que usa `DapperContext` para ejecutar consultas en la base de datos.
6. Los datos se devuelven en sentido inverso: repositorio → servicio → controlador → respuesta HTTP.

## Puntos clave
- Separación de responsabilidades: cada capa tiene responsabilidad única (presentación, aplicación, dominio, infraestructura).
- Inversión de dependencias: las capas superiores dependen de interfaces del `Domain` y las implementaciones concretas se registran en `API/Program.cs`.
- Acceso a datos: `Infraestructure` usa `DapperContext` para manejar la conexión y las consultas SQL.
- Configuración: cadenas de conexión y ajustes están en `API/appsettings.json`.

## Inyección de dependencias (DI) y lifetimes usados
En ASP.NET Core se registran servicios en un contenedor DI con distintos "lifetimes". Los principales son:

- `AddSingleton<T>()` — una única instancia para toda la aplicación. Se crea una vez y se reutiliza en todas las peticiones y hilos.
- `AddScoped<T>()` — una instancia por petición HTTP. Útil para dependencias que deben vivir durante la vida de una petición.
- `AddTransient<T>()` — nueva instancia cada vez que se solicita. Para objetos livianos y sin estado.

### Qué usa esta solución (revisar `API/Program.cs`)
- `DapperContext` está registrado como `AddSingleton<DapperContext>()`.
- `IClienteRepository` → `ClientRepository` registrado como `AddScoped<IClienteRepository, ClientRepository>()`.
- `IClienteService` → `ClienteService` registrado como `AddScoped<IClienteService, ClienteService>()`.
- `ExternalEtlService` está registrado con `AddHttpClient<ExternalEtlService>()` (cliente HTTP tipado gestionado por `IHttpClientFactory`).