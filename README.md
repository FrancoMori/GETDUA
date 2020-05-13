# GETDUA
Script que permite la descarga de datos de las DUA´S - SUNAT

> Paquetes NUGET
- Selenium WebDriver API .NET Bindings
- nunit.framework

Extrae información de la plataforma [Exportación definitiva por aduana]
- Revisión 1.0
|
|- Detecta paginación de hojas y extrae en la carpeta C:\temp\DUA ~ Archivos *html* generaados por cada páginas
|- En caso no exista paginación guarda un archivo con todos los registros en la ruta C:\temp\DUA\TPage.html

Para ejecutar la clase, usar escenario de pruebas [Explorador de pruebas]
