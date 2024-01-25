# NewshoreAir
Newshore Air API es una solución en .NET que permite conectar viajes. Esta API recibe como parámetros el origen y el destino del viaje del usuario, consulta los vuelos asociados, y devuelve la ruta de viaje si es posible o un mensaje informando que la ruta no puede ser calculada. Además, guarda las rutas previamente consultadas en un sistema de persistencia para su fácil recuperación. A su vez opcionalmente se puede limitar la cantidad máxima de vuelos / escalas.

## Detalles del Proyecto
El proyecto está finalizado, y se puede utilizar para calcular rutas de vuelo. 
- Se utilizo el nivel más avanzado (Rutas múltiples y de retorno https://recruiting-api.newshore.es/api/flights/2).
- Se optimizo la cantidad de peticiones.
- Base de datos con SQL Lite, nombre de la base NewshoreAirDB, la misma se encientra destro de la capa de servicio / API.
- Posibilidad de configurar el número máximo de vuelos que puede tener una ruta, en caso de existir.
- Sistema de log, usando NLog.
- Unit Tests

Como extra realice un simple FrontEnd de la aplicacion / funcionalidad, el mismo esta contruido en Angular 16, dejo el link del repo:
https://github.com/VarasMD/NewshoreAir.FrontEnd

## Capturas de Pantalla o Demostración
![NewshoreAir](https://github.com/VarasMD/NewshoreAir/assets/69024396/afcc980e-4a1e-466c-b1f0-7912601f12cb)

## Uso
La API se puede utilizar mediante solicitudes GET. 
Aquí hay un ejemplo de solicitud: GET /api/journeys/MDE/MAD
En caso de indicar un máximo de vuelos: GET /api/journeys/MDE/MAD?maxFlights=2

## Author
Matias Varas 

