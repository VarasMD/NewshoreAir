# NewshoreAir (español)
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

-------------------------------------------------------------

# NewshoreAir (English)
Newshore Air API is a .NET solution that allows to connect trips. This API receives as parameters the origin and destination of the user's trip, queries the associated flights, and returns the travel route if possible or a message informing that the route cannot be calculated. In addition, it saves the previously consulted routes in a persistence system for easy retrieval. The maximum number of flights/stops can be optionally limited.

## Project details
The project is finished, and can be used to calculate flight routes. 
- The most advanced level (multiple and return routes https://recruiting-api.newshore.es/api/flights/2) was used.
- The number of requests was optimized.
- Database with SQL Lite, database name NewshoreAirDB, it is located in the service layer / API.
- Possibility to configure the maximum number of flights that a route can have, if any.
- Log system, using NLog.
- Unit Tests

As an extra I made a simple FrontEnd of the application / functionality, it is built in Angular 16, I leave the link to the repo:
https://github.com/VarasMD/NewshoreAir.FrontEnd

## Screenshots or Demonstration
![NewshoreAir](https://github.com/VarasMD/NewshoreAir/assets/69024396/afcc980e-4a1e-466c-b1f0-7912601f12cb)

## Use
The API can be used via GET requests.
Here is an example request: GET /api/journeys/MDE/MAD
If you specify a maximum number of flights: GET /api/journeys/MDE/MAD?maxFlights=2

##Author
Matias Varas
