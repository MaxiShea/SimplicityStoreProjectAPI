# SimplicityStoreProjectAPI

## Descripción

SimplicityStoreProjectAPI es una API para el e-commerce de suplementos deportivos, diseñada para ofrecer una plataforma en línea que permita a los clientes comprar productos relacionados con la nutrición y el rendimiento deportivo. La API proporciona funcionalidades para la gestión de productos, pedidos y usuarios, así como para la autenticación.

## Administrador del Sitio

### Gestión de Inventario

- Administrar productos, precios y usuarios.

### Operaciones CRUD

- Agregar, actualizar y eliminar productos.

### Gestión de Pedidos

- Actualizar el estado de los pedidos y gestionar su procesamiento.

## Interacción del Sistema

### Gestión de Inventario

- Actualización automática del inventario cuando se realiza un pedido.

### Autenticación y Autorización

- La API utiliza JWT para la autenticación. Incluye el token JWT en el encabezado de autorización de tus solicitudes:

## Instalación

1.Clona el repositorio:

```bash
   git clone https://github.com/MaxiShea/SimplicityStoreProjectAPI.git

2.Navega al directorio del proyecto:

\`cd SimplicityStoreProjectAPI`

3.Restaura los paquetes necesarios:

-dotnet restore 

4.Ejecuta la aplicación:

- dotnet run
