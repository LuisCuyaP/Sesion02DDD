﻿using Dapper;
using Usuarios.Application.Abstractions.Data;
using Usuarios.Application.Abstractions.Messaging;
using Usuarios.Domain.Abstractions;

namespace Usuarios.Application.Usuarios.GetUsuario;

internal sealed class GetUsuarioQueryHandle : IQueryHandler<GetUsuarioQuery, UsuarioResponse>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetUsuarioQueryHandle(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<UsuarioResponse>> Handle(GetUsuarioQuery request, CancellationToken cancellationToken)
    {
        // Task<Result<UsuarioResponse>>

        /// Task  Representa una operación asíncrona que puede devolver un resultado.
        /// Result  Clase que encapsula el resultado de una operación, incluyendo éxito o error.
        /// UsuarioResponse La informacion   

        using var connection = _sqlConnectionFactory.CreateConnection();

        var sql = """
                SELECT 
                    u.id as Id,
                    u.nombres_persona as NombrePersona,
                    u.nombre_usuario as NombreUsuario,
                    u.apellido_paterno as ApellidoPaterno,
                    u.apellido_materno as ApellidoMaterno,
                    u.fecha_nacimiento as FechaNacimiento,
                    u.correo_electronico as CorreoElectronico,
                    u.direccion_pais as Pais,
                    u.direccion_departamento as Departamento,
                    u.direccion_provincia as Provincia,
                    u.direccion_distrito as Distrito,
                    u.direccion_calle as Calle,
                    r.nombre_rol as Rol,
                    u.fecha_ultimo_cambio as FechaUltimoCambio,
                    u.estado as Estado
                FROM usuarios u 
                INNER JOIN roles r ON u.rol_id = r.id
                WHERE u.id = @IdUsuario
                """;

        var usuario = await connection.QueryFirstOrDefaultAsync<UsuarioResponse>(sql, new { request.IdUsuario });

        return usuario!;

    }
}