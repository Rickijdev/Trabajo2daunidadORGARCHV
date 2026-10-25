# BancoApp · Persona 2 (R)

Proyecto completo de ASP.NET Core / Blazor para .NET 8, con la navegación de Persona 2 integrada al avance recibido.

## Qué se incorporó

- Menú lateral blanco e índigo, acorde con la paleta existente.
- Inicio (Home), Counter, Weather y una página interna Acerca de.
- Iconos SVG locales, sección activa, efectos al pasar el cursor y foco visible.
- Menú contraíble en escritorio; recuerda la preferencia en el navegador.
- Panel móvil con fondo superpuesto, cierre al navegar y con Escape, y navegación con teclado.
- Barra superior con ubicación actual y pie de página académico.
- Presentación en Inicio y Acerca de con el logo de la Universidad Nacional de Trujillo, el curso Organización de Archivos, Ingeniería Informática, el profesor Max Leovildo y los cinco nombres indicados.

Los nombres se conservaron como se solicitaron, incluido KIARA sin apellidos.

## Abrir en Visual Studio

1. Extrae todo el ZIP.
2. En Visual Studio, elige **File > Open > Project/Solution**.
3. Abre **BancoApp.csproj**, dentro de la carpeta BancoApp.
4. Espera la restauración de paquetes NuGet y ejecuta el perfil **http**.
5. Abre http://localhost:5058 si el navegador no se abre automáticamente.

Necesitas el SDK de .NET 8 y la carga de trabajo ASP.NET de Visual Studio.

## Abrir en VS Code

Abre la carpeta que contiene BancoApp.csproj. En la terminal ejecuta, en este orden:

    dotnet restore
    dotnet build
    dotnet run --launch-profile http

Luego abre http://localhost:5058. Este proyecto se inicia con .NET; no necesita npm.

## Base de datos

Se conservaron Program.cs, BancoApp.csproj, la carpeta Data, appsettings.json y la lógica de las páginas existentes. Para consultar cuentas y operar con ellas necesitas la misma instancia SQL Server y base de datos que utiliza el equipo. Si tu computadora usa otra instancia, configura ConnectionStrings:DefaultConnection en tu entorno.

El ZIP original no contenía un respaldo de la base de datos ni un script de creación. Esta entrega completa la parte visual; no crea cuentas de prueba ni modifica la conexión del equipo.

## Archivos de Persona 2

Modificados:

- Components/Layout/MainLayout.razor y MainLayout.razor.css.
- Components/Layout/NavMenu.razor y NavMenu.razor.css.
- Components/App.razor: idioma español y carga del script del menú.
- .gitignore: exclusión de archivos generados.

Agregados:

- Components/Layout/NavIcon.razor.
- Components/Layout/AcademicPresentation.razor y AcademicPresentation.razor.css.
- Components/Pages/About.razor y About.razor.css.
- wwwroot/js/navigation.js.
- wwwroot/images/unt-logo.png.
- Este documento y FUENTES_RECURSOS.md.

Para integrar solo esta parte en tu repositorio, copia estos archivos respetando sus carpetas. El proyecto completo también incluye los avances originales. Home.razor, Counter.razor, Weather.razor y wwwroot/app.css se conservaron.

## Qué revisar al exponer

1. Muestra la presentación con universidad, curso, docente e integrantes.
2. Navega por Inicio, Counter, Weather y Acerca de: se destaca la opción actual.
3. Contrae y expande el menú con el botón superior izquierdo.
4. Reduce el ancho de la ventana para mostrar la versión móvil.
5. En Acerca de, usa los accesos a las herramientas o vuelve a Inicio.

El contador y la tabla Weather siguen siendo los componentes de demostración existentes. Weather muestra datos generados, no un pronóstico real.

Se excluyeron bin, obj, .vs y .git del ZIP: son archivos generados o metadatos locales, y se regeneran al compilar cuando corresponde.
