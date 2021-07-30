USE tempdb
go

--DROP DATABASE VitalCareRxDB
-- Crear la base de datos
CREATE DATABASE VitalCareRxDB
GO

USE VitalCareRxDB
GO
-- ESQUEMAS
CREATE SCHEMA Personas
GO

CREATE SCHEMA Consultas
GO

CREATE SCHEMA Historial
GO


--TABLAS
CREATE TABLE Personas.Sexo 
(
	idSexo INT NOT NULL IDENTITY,
	descripcionSexo varchar(10) NOT NULL,

	CONSTRAINT PK_Sexo_idSexo
		PRIMARY KEY CLUSTERED (idSexo)
)


CREATE TABLE Personas.TipoSangre
(
	idTipoSangre INT NOT NULL IDENTITY,
	descripcionTipoSangre varchar(15) NOT NULL,

	CONSTRAINT PK_TipoSangre_idTipoSangre
		PRIMARY KEY CLUSTERED (idTipoSangre)
)

CREATE TABLE Personas.Paciente 
(
	idPaciente INT NOT NULL IDENTITY,
	numeroIdentidad VARCHAR(16) NOT NULL,
	primerNombre VARCHAR(25) NOT NULL,
	segundoNombre VARCHAR(25),
	primerApellido VARCHAR(25) NOT NULL,
	segundoApellido VARCHAR(25),
	direccion VARCHAR(200) NOT NULL,
	celular VARCHAR(8),
	fechaNacimiento DATE NOT NULL,
	peso FLOAT NOT NULL,
	estatura FLOAT NOT NULL,
	estado BIT NOT NULL,
	idTipoSangre INT NOT NULL,
	idSexo INT NOT NULL,
	

	CONSTRAINT PK_Paciente_idIdentidad
		PRIMARY KEY CLUSTERED (idPaciente),
	CONSTRAINT FK_Paciente$Existe$TipoSangre
		FOREIGN KEY (idTipoSangre) REFERENCES Personas.TipoSangre(idTipoSangre),
	CONSTRAINT FK_Paciente$Existe$Sexo
		FOREIGN KEY (idSexo) REFERENCES Personas.Sexo(idSexo)
)

CREATE TABLE Personas.Puesto
(

	idPuesto INT NOT NULL IDENTITY,
	puesto VARCHAR(25) NOT NULL

	CONSTRAINT PK_Puesto_idPuesto
		PRIMARY KEY CLUSTERED (idPuesto)

)

CREATE TABLE Personas.Empleado
(
	idEmpleado INT NOT NULL IDENTITY,
	primerNombre VARCHAR(25) NOT NULL, 
	segundoNombre VARCHAR(25),
	primerApellido VARCHAR(25) NOT NULL,
	segundoApellido VARCHAR(25),
	celular VARCHAR(8) NOT NULL,
	correo VARCHAR(70),
	fechaNacimiento DATE NOT NULL,
	idSexo INT NOT NULL,
	idPuesto INT NOT NULL,
	nombreUsuario VARCHAR(30) NOT NULL,
	contrasenia VARBINARY(MAX) NOT NULL,
	estado BIT NOT NULL
	

	CONSTRAINT PK_Empleado_idEmpleado
		PRIMARY KEY CLUSTERED (idEmpleado),

	CONSTRAINT FK_Empleado$Existe$Puesto
		FOREIGN KEY (idPuesto) REFERENCES Personas.Puesto(idPuesto),

	CONSTRAINT FK_Empleado$Existe$Sexo
		FOREIGN KEY (idSexo) REFERENCES Personas.Sexo(idSexo)
)

CREATE TABLE Historial.Bitacora
(
	idBitacora INT NOT NULL IDENTITY,
	idEmpleado INT NOT NULL,
	pcUsuario VARCHAR(70) NOT NULL,
	accion NVARCHAR(200) NOT NULL,
	fecha DATETIME NOT NULL

	CONSTRAINT PK_Bitacora_idBitacora
		PRIMARY KEY CLUSTERED (idBitacora),

	CONSTRAINT FK_Bitacora$Existe$Empleado
		FOREIGN KEY (idEmpleado) REFERENCES Personas.Empleado(idEmpleado),

)

CREATE TABLE Historial.ControlEmpleado
(
	idControlEmpleado INT NOT NULL IDENTITY,
	idEmpleado INT NOT NULL,
	fechaEntrada DATETIME NOT NULL,
	fechaSalida DATETIME NULL

	CONSTRAINT PK_ControlEmpleado_idControlEmpleado
		PRIMARY KEY CLUSTERED (idControlEmpleado),

	CONSTRAINT FK_ControlEmpleado$Existe$Empleado
		FOREIGN KEY (idEmpleado) REFERENCES Personas.Empleado(idEmpleado),
)

CREATE TABLE Consultas.Consulta
(
	idConsulta INT NOT NULL IDENTITY,
	fechaConsulta DATE NOT NULL,
	motivoConsulta VARCHAR(300) NOT NULL,
	diagnosticoConsulta VARCHAR(300) NOT NULL,
	temperatura FLOAT NOT NULL,
	presionArterial VARCHAR(9) NOT NULL,
	idEmpleado INT NOT NULL,
	idPaciente INT NOT NULL
	

	CONSTRAINT PK_Consulta_idConsulta
		PRIMARY KEY CLUSTERED (idConsulta),

	CONSTRAINT FK_Consulta$Existe$Empleado
		FOREIGN KEY (idEmpleado) REFERENCES Personas.Empleado(idEmpleado),

	CONSTRAINT FK_Consulta$Existe$Paciente
		FOREIGN KEY (idPaciente) REFERENCES Personas.Paciente(idPaciente)	 
)



CREATE TABLE Consultas.RecetaMedica
(
	idRecetaMedica INT NOT NULL IDENTITY,
	idConsulta INT NOT NULL,

	CONSTRAINT PK_RecetaMedica_idRecetaMedica
		PRIMARY KEY CLUSTERED (idRecetaMedica),
	CONSTRAINT FK_RecetaMedica$Existe$Consulta
		FOREIGN KEY (idConsulta) REFERENCES Consultas.Consulta(idConsulta)
)

CREATE TABLE Consultas.Farmaco            
(										
	idFarmaco INT NOT NULL IDENTITY,	
	descripcionFarmaco varchar(60) NOT NULL,	
	informacionPrecaucion varchar(200) NOT NULL,

	CONSTRAINT PK_Farmaco_idFarmaco
		PRIMARY KEY CLUSTERED (idFarmaco),

)

CREATE TABLE Consultas.DetalleFarmaco
(
	idFarmaco INT NOT NULL,
	fechaVencimiento DATE NOT NULL,
	cantidad INT NOT NULL

	CONSTRAINT PK_DetalleFarmaco_idFarmaco_fechaVencimiento
		PRIMARY KEY CLUSTERED (idFarmaco, fechaVencimiento),
	CONSTRAINT FK_DetalleFarmaco$Existe$Farmaco
		FOREIGN KEY (idFarmaco) REFERENCES Consultas.Farmaco (idFarmaco)

)





CREATE TABLE Consultas.DetalleRecetaMedica
(
	idRecetaMedica INT NOT NULL,
	idFarmaco INT NOT NULL,
	cantidad INT NOT NULL,
	duracionTratamiento varchar(100) NOT NULL,
	indicaciones varchar(200) NOT NULL,

	CONSTRAINT PK_DetellaRecetaMedica_idRecetaMedica_idFarmaco
		PRIMARY KEY CLUSTERED (idRecetaMedica,idFarmaco),
	CONSTRAINT FK_DetellaRecetaMedica$Existe$RecetaMedica
		FOREIGN KEY (idRecetaMedica) REFERENCES Consultas.RecetaMedica(idRecetaMedica),
	CONSTRAINT FK_DetellaRecetaMedica$Existe$idFarmaco
		FOREIGN KEY (idFarmaco) REFERENCES Consultas.Farmaco(idFarmaco)

)




-- RESTRICCIONES

-- Tabla Personas.Paciente

ALTER TABLE Personas.Paciente WITH CHECK
		ADD CONSTRAINT CHK_Personas_Paciente$VerificarLongitudDNI
		CHECK (len(numeroIdentidad) = 13)
GO


ALTER TABLE Personas.Paciente WITH CHECK
		ADD CONSTRAINT CHK_Personas_Paciente$VerificarLongitudCelular 
		CHECK (len(celular) = 8)
GO

ALTER TABLE Personas.Paciente WITH CHECK
		ADD CONSTRAINT CHK_Personas_Paciente$VerificarFechaNacimiento
		CHECK (fechaNacimiento <= CONVERT (date, GETDATE()))
GO

ALTER TABLE Personas.Paciente WITH CHECK
		ADD CONSTRAINT CHK_Personas_Paciente$VerificarPeso
		CHECK (peso > 0)
GO

ALTER TABLE Personas.Paciente WITH CHECK
		ADD CONSTRAINT CHK_Personas_Paciente$VerificarEstatura
		CHECK (estatura > 0)
GO

ALTER TABLE Personas.Paciente
	ADD CONSTRAINT CHK_Personas_Paciente$EstadoPaciente
	CHECK (estado IN(0, 1))
GO

ALTER TABLE Personas.Paciente
		ADD CONSTRAINT AK_Personas_numeroIdentidad
		UNIQUE NONCLUSTERED (numeroIdentidad)
GO


-- Tabla Personas.Sexo
ALTER TABLE Personas.Sexo
		ADD CONSTRAINT AK_Personas_Sexo_descripcionSexo
		UNIQUE NONCLUSTERED (descripcionSexo)
GO

ALTER TABLE Personas.Sexo
	ADD CONSTRAINT CHK_Personas_Sexo$SexoPersonas
	CHECK (descripcionSexo IN('Masculino', 'Femenino'))
GO
-- TABLA Personas.TipoSangre
ALTER TABLE Personas.TipoSangre
		ADD CONSTRAINT AK_Personas_TipoSexo_descripcionTipoSangre
		UNIQUE NONCLUSTERED (descripcionTipoSangre)
GO

ALTER TABLE Personas.TipoSangre
	ADD CONSTRAINT CHK_Personas_TipoSangre$TipoSangrePersonas
	CHECK (descripcionTipoSangre IN('A+', 'A-', 'AB-', 'AB+','B-','B+', 'O-', 'O+'))
GO

--TABLA Personas.Puesto
ALTER TABLE Personas.Puesto
	ADD CONSTRAINT CHK_Personas_Puesto$PuestoEmpleado
	CHECK (puesto IN('Administrador', 'Doctor')) 




-- TABLA Personas.Empleado

ALTER TABLE Personas.Empleado WITH CHECK
		ADD CONSTRAINT CHK_Personas_Empleado$VerificarLongitudCelular 
		CHECK (len(celular) = 8)
GO

ALTER TABLE Personas.Empleado
		ADD CONSTRAINT AK_Personas_Empleado_nombreUsuario
		UNIQUE NONCLUSTERED (nombreUsuario)
GO

ALTER TABLE Personas.Empleado WITH CHECK
		ADD CONSTRAINT CHK_Personas_Empleado$VerificarLongitudContrasenia
		CHECK (len(contrasenia) >= 8)
GO

ALTER TABLE Personas.Empleado WITH CHECK
		ADD CONSTRAINT CHK_Personas_Empleado$VerificarFechaNacimiento
		CHECK (fechaNacimiento <= CONVERT (date, GETDATE()))
GO

ALTER TABLE Personas.Empleado 
	ADD CONSTRAINT AK_Personas_Empleado_correo
	UNIQUE NONCLUSTERED (correo)
GO



-- Tabla Consultas.Consulta

ALTER TABLE Consultas.Consulta WITH CHECK
		ADD CONSTRAINT CHK_Consultas_Consulta$VerificarTemperatura
		CHECK (temperatura > 0)
GO

ALTER TABLE Consultas.Consulta WITH CHECK
		ADD CONSTRAINT CHK_Consultas_Consulta$VerificarFechaConsulta
		CHECK (CONVERT(DATE,fechaConsulta) = CONVERT(DATE,GETDATE()))
GO


-- Tabla Consultas.DetalleRecetaMedica
ALTER TABLE Consultas.DetalleRecetaMedica WITH CHECK
		ADD CONSTRAINT CHK_Consultas_DetalleRecetaMedica$VerificarCantidadFarmacos
		CHECK (cantidad > 0)
GO

-- Tabla Consultas.DetalleFarmaco
ALTER TABLE Consultas.DetalleFarmaco WITH CHECK
		ADD CONSTRAINT CHK_Consultas_DetalleFarmaco$VerificarCantidadFarmacos
		CHECK (cantidad >= 0)
GO

ALTER TABLE Consultas.DetalleFarmaco WITH CHECK
		ADD CONSTRAINT CHK_Consultas_DetalleFarmaco$VerificarFechaVencimiento
		CHECK (fechaVencimiento > CONVERT (date, DATEADD(day, 7, GETDATE())))
GO


-- Datos tabla sexo
INSERT [Personas].[Sexo]  VALUES ('Femenino')

INSERT [Personas].[Sexo]  VALUES ('Masculino')


-- Datos tabla TipoSangre
INSERT [Personas].[TipoSangre]  VALUES ( 'A+')

INSERT [Personas].[TipoSangre] VALUES ( 'A-')

INSERT [Personas].[TipoSangre]  VALUES ( 'AB-')

INSERT [Personas].[TipoSangre]  VALUES ( 'AB+')

INSERT [Personas].[TipoSangre]  VALUES ( 'B-')

INSERT [Personas].[TipoSangre]  VALUES ( 'B+')

INSERT [Personas].[TipoSangre]  VALUES ( 'O-')

INSERT [Personas].[TipoSangre]  VALUES ( 'O+')


-- Datos tabla Farmaco

INSERT [Consultas].[Farmaco]  VALUES ('Simvastatina', 'Para controlar el colesterol')INSERT [Consultas].[Farmaco] VALUES ('Aspirina', ' Casi para todo')INSERT [Consultas].[Farmaco]  VALUES ('Omeprazol', 'Para la acidez de estómago')INSERT [Consultas].[Farmaco]  VALUES ('Lexotiroxina sódica', 'Para reemplazar la tiroxina')INSERT [Consultas].[Farmaco]  VALUES ('Ramipril', 'Para la hipertensión')INSERT [Consultas].[Farmaco] VALUES ('Amlodipina', 'Para la hipertensión y la angina')INSERT [Consultas].[Farmaco] VALUES ('Paracetamol ', 'Para aliviar el dolor')INSERT [Consultas].[Farmaco]  VALUES ('Salbutamol', 'Para el asma')

-- Datos tabla puesto
INSERT INTO [Personas].[Puesto] VALUES ('Administrador');

INSERT INTO [Personas].[Puesto] VALUES ('Doctor');


-- Datos de un usuario tipo Administrador.

INSERT INTO [Personas].[Empleado] VALUES ('Admin', 'Admin', 'Admin', 'Admin', '12345678', 'Admin@gmail.com', '1990-02-02', 1, 1, 'admin', (ENCRYPTBYPASSPHRASE('ecrypt07','admin123')), 1)
GO
-- PROECEDIMIENTOS ALMACENADOS

-- SP_UsuarioActual
CREATE PROCEDURE sp_usuarioActual@value sql_variant,@key sysnameASBEGIN	EXEC sp_set_session_context @key, @valueEND
GO

-- sp_restaurarPassword
CREATE PROCEDURE sp_restaurarPassword
	@idEmpleado INT,
	@Contrasenia VARCHAR(30)

AS
DECLARE
	@password VARBINARY(max)
BEGIN

	SET @password = (ENCRYPTBYPASSPHRASE('ecrypt07',@Contrasenia));

	UPDATE [Personas].[Empleado]
	SET contrasenia = @password
	WHERE idEmpleado = @idEmpleado 
END
GO

-- sp_Pacientes
CREATE PROCEDURE sp_Pacientes
@idPaciente INT = NULL,
@numeroIdentidad VARCHAR(16)  = NULL,
@primerNombre VARCHAR(25)   = NULL,
@segundoNombre VARCHAR(25)  = NULL,
@primerApellido VARCHAR(25)  = NULL,
@segundoApellido VARCHAR(25)  = NULL,
@direccion VARCHAR(200)   = NULL,
@celular VARCHAR(8)  = NULL ,
@fechaNacimiento DATE  = NULL,
@peso FLOAT  = NULL,
@estatura FLOAT   = NULL,
@estado BIT  = NULL,
@idTipoSangre INT   = NULL,
@idSexo INT   = NULL,
@accion VARCHAR(50),
@nombrePaciente VARCHAR(80)  = NULL

AS
BEGIN
	
	IF @accion = 'insertar'
	BEGIN
	INSERT INTO [Personas].[Paciente] VALUES (@numeroIdentidad,@primerNombre,@segundoNombre,@primerApellido,@segundoApellido,@direccion, @celular,@fechaNacimiento,
						@peso,@estatura,@estado, @idTipoSangre,@idSexo)
	END
	ELSE IF @accion = 'modificar'
	BEGIN
		UPDATE [Personas].[Paciente] 
                                SET numeroIdentidad = @numeroIdentidad, primerNombre = @primerNombre, segundoNombre = @segundoNombre, primerApellido = @primerApellido,
                                segundoApellido = @segundoApellido, direccion = @direccion, celular = @celular, fechaNacimiento = @fechaNacimiento, peso = @peso,
                                estatura = @estatura, idTipoSangre = @idTipoSangre, idSexo = @idSexo, estado = @estado
                                WHERE idPaciente = @idPaciente
	END
	ELSE IF @accion = 'banear'
	BEGIN
		UPDATE [Personas].[Paciente] 
                SET estado = 0
                WHERE idPaciente = @idPaciente
	END
	ELSE IF @accion = 'verPaciente'
	BEGIN
		SELECT P.numeroIdentidad Identidad,primerNombre,segundoNombre,PrimerApellido,segundoApellido, P.idTipoSangre, P.idSexo, P.idPaciente,
		CONCAT(P.primerNombre, ' ', P.segundoNombre, ' ', P.primerApellido, ' ', P.segundoApellido) Paciente,
		P.direccion 'Dirección', P.celular Celular, P.fechaNacimiento 'Fecha de nacimiento', P.peso 'Peso (lbs)', P.estatura 'Estatura (cm)', P.estado Estado,
		T.descripcionTipoSangre 'Tipo de sangre', S.descripcionSexo Sexo
		FROM [Personas].[Paciente] P INNER JOIN [Personas].[TipoSangre] T
		ON P.idTipoSangre = T.idTipoSangre
		INNER JOIN [Personas].[Sexo] S
		ON P.idSexo = S.idSexo
		WHERE P.estado = @estado
	END
	ELSE IF @accion = 'verUnPaciente'
	BEGIN
	SELECT P.numeroIdentidad Identidad,primerNombre,segundoNombre,PrimerApellido,segundoApellido, P.idTipoSangre, P.idSexo, P.idPaciente,
    CONCAT(P.primerNombre, ' ', P.segundoNombre, ' ', P.primerApellido, ' ', P.segundoApellido) Paciente,
    P.direccion 'Dirección', P.celular Celular, P.fechaNacimiento 'Fecha de nacimiento', P.peso 'Peso (lbs)', P.estatura 'Estatura (cm)', P.estado Estado,
    T.descripcionTipoSangre 'Tipo de sangre', S.descripcionSexo Sexo
    FROM [Personas].[Paciente] P INNER JOIN [Personas].[TipoSangre] T
    ON P.idTipoSangre = T.idTipoSangre
    INNER JOIN [Personas].[Sexo] S
    ON P.idSexo = S.idSexo
    WHERE P.estado = @estado and CONCAT(P.primerNombre, ' ', P.segundoNombre, ' ', P.primerApellido, ' ', P.segundoApellido) LIKE CONCAT('%', @nombrePaciente,'%')
	END
	ELSE IF @accion = 'existeID'
	BEGIN
		SELECT numeroIdentidad FROM [Personas].[Paciente] WHERE [numeroIdentidad] = @numeroIdentidad
	END
END
GO

-- sp_Empleados

CREATE PROCEDURE sp_Empleados
@idEmpleado INT = NULL,
@primerNombre VARCHAR(25)   = NULL,
@segundoNombre VARCHAR(25)  = NULL,
@primerApellido VARCHAR(25)  = NULL,
@segundoApellido VARCHAR(25)  = NULL,
@celular VARCHAR(8)  = NULL,
@correo VARCHAR(70)  = NULL,
@fechaNacimiento DATE  = NULL,
@idSexo INT   = NULL,
@idPuesto INT   = NULL,
@nombreUsuario VARCHAR(30) = NULL,
@contrasenia VARCHAR(30) = NULL,
@estado BIT  = NULL,
@accion VARCHAR(50),
@nombreEmpleado VARCHAR(80)  = NULL


AS
DECLARE
@password VARBINARY(max)
BEGIN


	IF @accion = 'insertar'
	BEGIN
		SET @password = (ENCRYPTBYPASSPHRASE('ecrypt07',@contrasenia));
		INSERT INTO [Personas].[Empleado] VALUES 
                (@primerNombre,@segundoNombre,@primerApellido,
                @segundoApellido,@celular, @correo, @fechaNacimiento, @idSexo, @idPuesto, @nombreusuario,@password, @estado)
	END
	ELSE IF @accion = 'modificar'
	BEGIN
		SET @password = (ENCRYPTBYPASSPHRASE('ecrypt07',@contrasenia));
		UPDATE [Personas].[Empleado]
            SET primerNombre = @primerNombre, segundoNombre = @segundoNombre, primerApellido = @primerApellido, segundoApellido = @segundoApellido,
            celular = @celular, correo = @correo, fechaNacimiento = @fechaNacimiento, idSexo = @idSexo, idPuesto = @idPuesto, nombreUsuario = @nombreusuario, contrasenia =  @password, estado = @estado
            WHERE idEmpleado =  @idEmpleado
	END
	ELSE IF @accion = 'desactivar'
	BEGIN
		
		UPDATE [Personas].[Empleado]
            SET  estado = 0
            WHERE idEmpleado =  @idEmpleado
	END
	ELSE IF @accion = 'buscarUsuario'
	BEGIN
		SELECT * FROM [Personas].[Empleado] WHERE nombreUsuario = @Nombreusuario
	END
	ELSE IF @accion = 'buscarUsuarioID'
	BEGIN
		SELECT * FROM [Personas].[Empleado] WHERE idEmpleado = @idEmpleado
	END
	ELSE IF @accion = 'buscarUsuarioCorreo'
	BEGIN
		SELECT * FROM [Personas].[Empleado] WHERE correo = @correo
	END
	ELSE IF @accion = 'codigoMayor'
	BEGIN
		SELECT MAX(idEmpleado) idEmpleado FROM [Personas].[Empleado]
	END	
	ELSE IF @accion = 'verEmpleados'
	BEGIN
		SELECT idEmpleado 'Código empleado', primerNombre,segundoNombre,PrimerApellido,segundoApellido,
		CONCAT(primerNombre, ' ', segundoNombre, ' ', primerApellido, ' ', segundoApellido) Empleado, celular Celular,
		correo Correo, S.idSexo, CONVERT(VARCHAR,DECRYPTBYPASSPHRASE('ecrypt07',contrasenia)) contrasenia, S.descripcionSexo Sexo, fechaNacimiento 'Fecha de nacimiento', (cast(datediff(dd,fechaNacimiento,GETDATE()) / 365.25 as int)) Edad,	nombreUsuario 'Nombre de usuario', '••••••••' Contraseña,
		estado Estado
		FROM [Personas].[Empleado] E INNER JOIN [Personas].[Sexo] S
		ON E.idSexo = s.idSexo								
		WHERE idPuesto = 2 and estado = @estado
		
	END
	ELSE IF @accion = 'verUnEmpleado'
	BEGIN
		SELECT idEmpleado 'Código empleado', primerNombre,segundoNombre,PrimerApellido,segundoApellido,
		CONCAT(primerNombre, ' ', segundoNombre, ' ', primerApellido, ' ', segundoApellido) Empleado, celular Celular,
		correo Correo, S.idSexo, CONVERT(VARCHAR,DECRYPTBYPASSPHRASE('ecrypt07',contrasenia)) contrasenia, S.descripcionSexo Sexo, fechaNacimiento 'Fecha de nacimiento', (cast(datediff(dd,fechaNacimiento,GETDATE()) / 365.25 as int)) Edad,	nombreUsuario 'Nombre de usuario', '••••••••' Contraseña,
		estado Estado
		FROM [Personas].[Empleado] E INNER JOIN [Personas].[Sexo] S
		ON E.idSexo = s.idSexo	
		WHERE CONCAT(primerNombre, ' ', segundoNombre, ' ', primerApellido, ' ', segundoApellido) LIKE CONCAT('%', @nombreEmpleado,'%') and idPuesto = 2 and estado = @estado
		
	END
	ELSE IF @accion = 'existeUsuario'
	BEGIN
		SELECT nombreUsuario FROM [Personas].[Empleado] WHERE [nombreUsuario] = @nombreUsuario
	END
	ELSE IF @accion = 'obtenerPass'
	BEGIN
		 SELECT CONVERT(VARCHAR,DECRYPTBYPASSPHRASE('ecrypt07',contrasenia)) contrasenia FROM [Personas].[Empleado] WHERE idEmpleado = @idEmpleado
	END

END
GO

-- sp_RecetasMedicas

CREATE PROCEDURE sp_RecetasMedicas
@idRecetaMedica int = NULL,
@Idconsulta int = NULL,
@Idfarmaco int = NULL,
@Idfarmaco2 int = NULL,
@cantidad int = NULL,
@duracion varchar(100) = NULL,
@indicacion varchar(200) = NULL,
@accion varchar(60)

AS

BEGIN
	IF @accion = 'ValidarCrearRecetaMedica'
	BEGIN
		SELECT idConsulta FROM[Consultas].[Consulta] 
		WHERE idConsulta IN(SELECT idConsulta FROM[Consultas].[RecetaMedica]) and idConsulta = @idConsulta
	END

	ELSE IF @accion = 'InsertarReceta'
	BEGIN
		INSERT INTO[Consultas].[RecetaMedica] VALUES(@idConsulta)
	END

	ELSE IF @accion = 'InsertarFarmaco'
	BEGIN 
		INSERT INTO [Consultas].[DetalleRecetaMedica] 
		VALUES (@idRecetaMedica,@idFarmaco,@cantidad,@duracion, @indicacion)
	END

	ELSE IF @accion = 'EliminarFarmaco'
	BEGIN
		DELETE [Consultas].[DetalleRecetaMedica] 
		WHERE idRecetaMedica = @idRecetaMedica AND idFarmaco = @idFarmaco
	END

	ELSE IF @accion = 'ModificarFarmaco'
	BEGIN
		UPDATE [Consultas].[DetalleRecetaMedica]
                                SET idFarmaco = @idFarmaco2, cantidad = @cantidad , duracionTratamiento = @duracion, indicaciones = @indicacion
                                WHERE idRecetaMedica = @idRecetaMedica and idFarmaco = @idFarmaco
		
	END

	ELSE IF @accion = 'MostrarFarmacos'
	BEGIN
		SELECT  F.descripcionFarmaco 'Fármaco',
		       DR.cantidad 'Cantidad', DR.duracionTratamiento 'Duración', DR.indicaciones 'Indicaciones'
                            FROM [Consultas].[DetalleRecetaMedica] DR INNER JOIN [Consultas].[Farmaco] F
                            ON DR.idFarmaco = F.idFarmaco
                            INNER JOIN [Consultas].[RecetaMedica] R
                            ON DR.idRecetaMedica = R.idRecetaMedica
                            INNER JOIN [Consultas].[Consulta] C
                            ON R.idConsulta = C.idConsulta
                            WHERE C.idConsulta = @idConsulta
	END

	ELSE IF @accion = 'ValidarFarmacoEnReceta'
	BEGIN
		SELECT idFarmaco FROM [Consultas].[DetalleRecetaMedica] 
		WHERE idRecetaMedica = @idRecetaMedica and idFarmaco = @idFarmaco
	END
	ELSE IF @accion = 'ValidarDetalleReceta'
	BEGIN
		SELECT idFarmaco FROM [Consultas].[DetalleRecetaMedica] 
		WHERE idRecetaMedica = @idRecetaMedica 
	END
END
GO

-- sp_LlenarComboBox
CREATE PROCEDURE sp_LlenarComboBox
@accion varchar(60)
AS

BEGIN
	IF @accion = 'CargarEstado'
	BEGIN
		SELECT '1' id, 'Activos' estado
        UNION
        SELECT '0', 'Inactivos'
	END

	ELSE IF @accion = 'CargarSexo'
	BEGIN
		SELECT * FROM [Personas].[Sexo]
	END

	ELSE IF @accion = 'CargarTipoSangre'
	BEGIN
		SELECT * FROM [Personas].[TipoSangre]
	END

	ELSE IF @accion = 'CargarFarmacos'
	BEGIN
		SELECT * FROM [Consultas].[Farmaco]
	END

	
	ELSE IF @accion = 'cargarEmpleados'
	BEGIN
		SELECT idEmpleado, CONCAT(primerNombre, ' ', primerApellido) Empleado FROM [Personas].[Empleado]
	END	

	ELSE IF @accion = 'cargarPacientes'
	BEGIN
		SELECT idPaciente, CONCAT(numeroIdentidad, ' - ', primerNombre, ' ', primerApellido) Paciente FROM [Personas].[Paciente]
		ORDER BY numeroIdentidad
	END	
	ELSE IF @accion = 'anios'	BEGIN		SELECT DISTINCT(YEAR(CE.fechaEntrada)) 'Anios' FROM Historial.ControlEmpleado CE	END	ELSE IF @accion = 'meses'	BEGIN		SET LANGUAGE Spanish;		with Months as 		( 			select month(GETDATE()) as Numero, datename(month, GETDATE()) as Nombre, 1 as number			union all			select month(dateadd(month,number,(GETDATE()))) Monthnumber ,datename(month, dateadd(month,number,(GETDATE()))) as name, number+1 			from Months 			where number<12		)   		select Numero, Nombre 		from Months 		order by Numero	END



END
GO

-- sp_Farmacos
CREATE PROCEDURE sp_Farmacos
@idFarmaco int = NULL,
@descripcionFarmaco varchar(60) = NULL,
@informacionPrecaucion varchar(200) = NULL,
@fechaVencimiento DATE = NULL, 
@fechaVencimiento2 DATE = NULL,
@cantidad INT = NULL,
@cantInventario INT = NULL,
@accion varchar(70)

AS

BEGIN 

	IF @accion = 'Insertar'
	BEGIN
		INSERT INTO [Consultas].[Farmaco] VALUES (@descripcionFarmaco, @informacionPrecaucion)
	END

	ELSE IF @accion = 'Modificar'
	BEGIN
		UPDATE [Consultas].[Farmaco]
                                SET descripcionFarmaco = @descripcionFarmaco, informacionPrecaucion = @informacionPrecaucion
                                where idFarmaco = @idFarmaco
	END

	ELSE IF @accion = 'Buscar'
	BEGIN 
		SELECT idFarmaco as 'Código Fármaco', descripcionFarmaco as 'Fármaco', informacionPrecaucion as 'Información del fármaco' 
		FROM [Consultas].[Farmaco] WHERE descripcionFarmaco LIKE CONCAT('%', @descripcionFarmaco, '%')
	END

	ELSE IF @accion = 'Mostrar'
	BEGIN
		SELECT F.idFarmaco as 'Código Fármaco', descripcionFarmaco as 'Fármaco', informacionPrecaucion as 'Información del fármaco', ISNULL(SUM(DF.Cantidad), 0) Cantidad
		FROM [Consultas].[Farmaco] F LEFT JOIN [Consultas].[DetalleFarmaco] DF
		ON F.idFarmaco = DF.idFarmaco
		GROUP BY  F.idFarmaco, descripcionFarmaco, informacionPrecaucion
	END
	ELSE IF  @accion = 'InsertarDetalle'
	BEGIN
		INSERT INTO [Consultas].[DetalleFarmaco] VALUES (@idFarmaco, @fechaVencimiento, @cantidad);
	END
	ELSE IF  @accion = 'MostrarDetalleFarmaco'
	BEGIN
		SELECT fechaVencimiento 'Fecha de vencimiento', cantidad Cantidad, DATEDIFF(DAY,  GETDATE(), fechaVencimiento) 'Días restantes para vencer'
		FROM [Consultas].[DetalleFarmaco] 
		WHERE idFarmaco = @idFarmaco and fechaVencimiento >  GETDATE()
	END
	ELSE IF  @accion = 'MostrarDetalleFarmacoFiltro'
	BEGIN
		SELECT fechaVencimiento 'Fecha de vencimiento', cantidad Cantidad, DATEDIFF(DAY,  GETDATE(), fechaVencimiento) 'Días restantes para vencer'
		FROM [Consultas].[DetalleFarmaco] 
		WHERE idFarmaco = @idFarmaco and fechaVencimiento = @fechaVencimiento
	END
	ELSE IF  @accion = 'ActualizarDetalle'
	BEGIN
		UPDATE [Consultas].[DetalleFarmaco]
		SET fechaVencimiento = @fechaVencimiento2, cantidad = @cantidad 
		WHERE idFarmaco = @idFarmaco and fechaVencimiento = @fechaVencimiento
	
	END
	ELSE IF  @accion = 'SumarCantidad'
	BEGIN
		UPDATE [Consultas].[DetalleFarmaco]
		SET  cantidad = (SELECT cantidad FROM [Consultas].[DetalleFarmaco] WHERE idFarmaco = @idFarmaco and fechaVencimiento = @fechaVencimiento) + @cantidad 
		WHERE idFarmaco = @idFarmaco and fechaVencimiento = @fechaVencimiento
	
	END
	ELSE IF  @accion = 'EliminarDetalle'
	BEGIN
		DELETE [Consultas].[DetalleFarmaco] WHERE idFarmaco = @idFarmaco and fechaVencimiento = @fechaVencimiento
	
	END
	ELSE IF  @accion = 'VerificarFecha'
	BEGIN
		SELECT * FROM [Consultas].[DetalleFarmaco] WHERE idFarmaco = @idFarmaco and fechaVencimiento = @fechaVencimiento	
	
	END
	ELSE IF @accion = 'fecha'
	BEGIN
	Select TOP 1  fechaVencimiento
		FROM [Consultas].[DetalleFarmaco]
		WHERE cantidad > 0 and idFarmaco = @idFarmaco and fechaVencimiento > CONVERT (date, DATEADD(day, 7, GETDATE()))
		Order by fechaVencimiento asc
	END
	ELSE IF @accion = 'cantInventario'
	BEGIN
		Select TOP 1 cantidad
            FROM [Consultas].[DetalleFarmaco]
            WHERE cantidad > 0 and idFarmaco = @idFarmaco and fechaVencimiento > CONVERT (date, DATEADD(day, 7, GETDATE()))		
            Order by fechaVencimiento asc
			
	END
	ELSE IF @accion = 'RetirarInventario'
	BEGIN
		UPDATE [Consultas].[DetalleFarmaco]
        SET cantidad = @cantInventario - @cantidad
        WHERE idFarmaco = @idFarmaco and fechaVencimiento = @fechaVencimiento
			
	END
	ELSE IF @accion = 'VerificarCantidad'
	BEGIN
		SELECT ISNULL(SUM(cantidad), 0) cantidad
		FROM [Consultas].[DetalleFarmaco]        
        WHERE idFarmaco = @idFarmaco and fechaVencimiento > CONVERT (date, DATEADD(day, 7, GETDATE()))
			
	END
END
GO

-- sp_Consultas

CREATE PROCEDURE sp_Consultas
@idPaciente INT = NULL,
@idConsulta int = NULL,
@motivoConsulta varchar(300) = NULL,
@diagnosticoConsulta varchar(300) = NULL,
@temperatura float = NULL,
@presionArterial varchar(9) = NULL,
@idEmpleado int = NULL,
@nombrePaciente varchar (70) = NULL,
@accion varchar(60)

AS

BEGIN
	IF @accion = 'Insertar'
	BEGIN
		INSERT INTO [Consultas].[Consulta] 
		VALUES (getdate(), @motivoConsulta,@diagnosticoConsulta,@temperatura,@presionArterial,@idEmpleado, @idPaciente)
	END

	ELSE IF @accion = 'Modificar'
	BEGIN
		UPDATE [Consultas].[Consulta]
                                SET  motivoConsulta = @motivoConsulta, diagnosticoConsulta = @diagnosticoConsulta,
								temperatura = @temperatura,presionArterial = @presionArterial, idEmpleado = @idEmpleado, idPaciente = @idPaciente						
                                WHERE idConsulta = @idConsulta
	END

	ELSE IF @accion = 'Mostrar'
	BEGIN

		SELECT P.idPaciente, C.idConsulta 'Código de consulta', c.fechaConsulta 'Fecha', CONCAT(P.primerNombre, ' ', P.segundoNombre, ' ', P.primerApellido, ' ', P.segundoApellido)
                Paciente, C.motivoConsulta Motivo, C.diagnosticoConsulta 'Diagnóstico', C.temperatura Temperatura, C.presionArterial AS 'Presión arterial', 
				CONCAT(E.primerNombre, ' ', E.segundoNombre, ' ', E.primerApellido, ' ', E.segundoApellido) Empleado							
                FROM[Consultas].[Consulta] C
                INNER JOIN[Personas].[Paciente] P
                ON C.idPaciente = P.idPaciente
                INNER JOIN[Personas].[Empleado] E
                ON C.idEmpleado = e.idEmpleado
		
	END

	ELSE IF @accion = 'MostrarConsultaPaciente'
	BEGIN
		SELECT CO.idConsulta 'Código de consulta', CO.fechaConsulta 'Fecha', CO.motivoConsulta Motivo, CO.diagnosticoConsulta 'Diagnóstico', CO.temperatura Temperatura,
				CO.presionArterial AS 'Presión arterial',CONCAT(E.primerNombre, ' ', E.segundoNombre, ' ', E.primerApellido, ' ', E.segundoApellido) Empleado				
				FROM [Consultas].[Consulta] CO
				INNER JOIN [Personas].[Paciente] P
				ON CO.idPaciente = P.idPaciente
				INNER JOIN [Personas].[Empleado] E
				ON CO.idEmpleado = e.idEmpleado
				WHERE P.idPaciente = @idPaciente

	END

	ELSE IF @accion = 'CapturarIdRecetaMedica'
	BEGIN
		SELECT idRecetaMedica FROM[Consultas].[RecetaMedica] WHERE idConsulta = @idConsulta
	END

	ELSE IF @accion = 'Buscar'
	BEGIN
		SELECT P.idPaciente, C.idConsulta 'Código de consulta', c.fechaConsulta 'Fecha', CONCAT(P.primerNombre, ' ', P.segundoNombre, ' ', P.primerApellido, ' ', P.segundoApellido)
                Paciente, C.motivoConsulta Motivo, C.diagnosticoConsulta 'Diagnóstico', C.temperatura Temperatura, C.presionArterial AS 'Presión arterial', 
				CONCAT(E.primerNombre, ' ', E.segundoNombre, ' ', E.primerApellido, ' ', E.segundoApellido) Empleado
							
                FROM[Consultas].[Consulta] C
                INNER JOIN[Personas].[Paciente] P
                ON C.idPaciente = P.idPaciente
                INNER JOIN[Personas].[Empleado] E
                ON C.idEmpleado = e.idEmpleado
                WHERE CONCAT(P.primerNombre, ' ', P.segundoNombre, ' ', P.primerApellido, ' ', P.segundoApellido) LIKE CONCAT('%', @nombrePaciente,'%')

	END

END 
GO

-- sp_Aportes

CREATE PROCEDURE sp_Aportes
@fecha date =  NULL,
@idEmpleado int = NULL,
@HoraEntrada datetime = NULL,
@HoraSalida datetime = NULL,
@idControlEmpleado int = NULL,
@anio int = NULL,
@mes int = NULL,
@accion varchar(50)

  AS

BEGIN
	IF @accion = 'MostrarBitacora'
	BEGIN
		select CONCAT(E.primerNombre, ' ', E.primerApellido) 'Nombre del empleado', E.nombreUsuario 'Nombre del Usuario', B.pcUsuario 'Nombre PC', B.accion 'Acción realizada' ,B.fecha 'Fecha'  
		from [Historial].[Bitacora] B INNER JOIN [Personas].[Empleado] E
		on E.idEmpleado = B.idEmpleado
		ORDER BY B.idBitacora DESC
	END

	ELSE IF @accion = 'MostrarBitacoraParametros'
	BEGIN
		select E.nombreUsuario 'Nombre del Usuario', B.pcUsuario 'Nombre PC', B.accion 'Acción realizada' ,B.fecha 'Fecha'  
		from [Historial].[Bitacora] B INNER JOIN [Personas].[Empleado] E
		on E.idEmpleado = B.idEmpleado
		WHERE CONVERT(DATE,B.fecha) = @fecha OR B.idEmpleado = @idEmpleado
		ORDER BY B.idBitacora DESC

	END
	ELSE IF @accion = 'MostrarBitacoraParametrosAND'
	BEGIN
		select E.nombreUsuario 'Nombre del Usuario', B.pcUsuario 'Nombre PC', B.accion 'Acción realizada' ,B.fecha 'Fecha'  
		from [Historial].[Bitacora] B INNER JOIN [Personas].[Empleado] E
		on E.idEmpleado = B.idEmpleado
		WHERE CONVERT(DATE,B.fecha) = @fecha AND B.idEmpleado = @idEmpleado
		ORDER BY B.idBitacora DESC

	END

	ELSE IF @accion = 'MostrarControl'
	BEGIN
		select Concat(E.primerNombre, ' ', E.primerApellido) 'Nombre del Empleado', CONVERT(DATE,CE.fechaEntrada) 'Fecha', 
		CONVERT(nvarchar(5), CE.fechaEntrada, 108) 'Hora de Entrada', CONVERT(nvarchar(5), CE.fechaSalida, 108) 'Hora de Salida'  from [Historial].[ControlEmpleado] CE 
		INNER JOIN [Personas].[Empleado] E 
		on E.idEmpleado = CE.idEmpleado

	END
	ELSE IF @accion = 'MostrarControlPorEmpleados'	BEGIN		set Language spanish		select YEAR(CE.fechaEntrada) 'Año', DATENAME(MONTH,CE.fechaEntrada) 'Mes',DAY(CE.fechaEntrada) 'Día',CONVERT(nvarchar(5), CE.fechaEntrada, 108) 'Hora de Entrada', 		CONVERT(nvarchar(5), CE.fechaSalida, 108) 'Hora de Salida'  from [Historial].[ControlEmpleado] CE 		WHERE CE.idEmpleado = @idEmpleado	END


	ELSE IF @accion = 'MostrarControlParametros'	BEGIN		select Concat(E.primerNombre, ' ', E.primerApellido) 'Nombre del Empleado', DAY(CE.fechaEntrada) 'Día',CONVERT(nvarchar(5), CE.fechaEntrada, 108) 'Hora de Entrada', 		CONVERT(nvarchar(5), CE.fechaSalida, 108) 'Hora de Salida'  from [Historial].[ControlEmpleado] CE 		INNER JOIN [Personas].[Empleado] E 		on E.idEmpleado = CE.idEmpleado		WHERE CE.idEmpleado = @idEmpleado OR YEAR(CE.fechaEntrada) = @anio AND MONTH(CE.fechaEntrada) = @mes	END

	ELSE IF @accion = 'MostrarControlParametrosAmbos'
	BEGIN
		select DAY(CE.fechaEntrada) 'Día',CONVERT(nvarchar(5), CE.fechaEntrada, 108) 'Hora de Entrada', 
		CONVERT(nvarchar(5), CE.fechaSalida, 108) 'Hora de Salida'  from [Historial].[ControlEmpleado] CE 
		WHERE CE.idEmpleado = @idEmpleado AND YEAR(CE.fechaEntrada) = @anio AND MONTH(CE.fechaEntrada) = @mes
	END


	ELSE IF @accion = 'InsertarEntrada'
	BEGIN
		INSERT INTO [Historial].[ControlEmpleado] VALUES (@idEmpleado, @HoraEntrada,@HoraSalida)
	END

	ELSE IF @accion = 'InsertarSalida'
	BEGIN
		UPDATE [Historial].[ControlEmpleado] SET
		fechaSalida = @HoraSalida
		WHERE idControlEmpleado = @idControlEmpleado

	END
	ELSE IF @accion = 'CapturarIdControl'	BEGIN		SELECT TOP 1 idControlEmpleado FROM  [Historial].[ControlEmpleado]		WHERE idEmpleado = @idEmpleado AND CONVERT(DATE, fechaEntrada) = CONVERT(DATE, GETDATE()) 	END
	ELSE IF @accion = 'mensajeFecha'
	BEGIN
		SELECT TOP 1 CONCAT('Actualmente lleva trabajando ', CONVERT(INT, DATEDIFF(MINUTE, fechaEntrada, GETDATE()) / 60), ' horas y ', CONVERT(INT, ROUND(((DATEDIFF(MINUTE, fechaEntrada, 
			GETDATE()) / 60.00) - CONVERT(INT,DATEDIFF(MINUTE, fechaEntrada, GETDATE()) / 60.00)) * 60, 0)),' minutos') mensaje
			FROM [Historial].[ControlEmpleado] 
			WHERE idEmpleado = @idEmpleado and CONVERT(DATE, fechaEntrada) = CONVERT(DATE, GETDATE())
	END
	ELSE IF @accion = 'validarEntradaSalida'
	BEGIN
		SELECT * 
		FROM [Historial].[ControlEmpleado] 
		WHERE idEmpleado = @idEmpleado and CONVERT(DATE, fechaEntrada) = CONVERT(DATE, GETDATE())
	END


END
GO

-- TRIGGERS
CREATE TRIGGER InsertarPaciente
ON [Personas].[Paciente] AFTER INSERT
AS
BEGIN 

declare @id sql_variant
	set @id = (select SESSION_CONTEXT(N'user_id'));
	insert into [Historial].[Bitacora]
	select cast(@id as int), 
SYSTEM_USER,
CONCAT('Inserción de paciente ',CONCAT(primerNombre, ' ' ,primerApellido), ' con ID ', idPaciente),
GETDATE()
from inserted

END

GO

CREATE TRIGGER ModificarPaciente
ON [Personas].[Paciente] AFTER UPDATE
AS
BEGIN 

declare @id sql_variant
	set @id = (select SESSION_CONTEXT(N'user_id'));
	insert into [Historial].[Bitacora]
	select cast(@id as int), 
SYSTEM_USER,
CONCAT('Modificación de paciente ',CONCAT(primerNombre, ' ' ,primerApellido), ' con ID ', idPaciente),
GETDATE()
from inserted

END

Go


CREATE TRIGGER InsertarEmpleado
ON [Personas].[Empleado] AFTER INSERT
AS
BEGIN 

declare @id sql_variant
	set @id = (select SESSION_CONTEXT(N'user_id'));
	insert into [Historial].[Bitacora]
	select cast(@id as int), 
SYSTEM_USER,
CONCAT('Inserción de empleado ',CONCAT(primerNombre, ' ' ,primerApellido), ' con ID ', idEmpleado),
GETDATE()
from inserted

END

GO

CREATE TRIGGER ModificarEmpleado
ON [Personas].[Empleado] AFTER UPDATE
AS
BEGIN 

declare @id sql_variant
	set @id = (select SESSION_CONTEXT(N'user_id'));
	insert into [Historial].[Bitacora]
	select cast(@id as int), 
SYSTEM_USER,
CONCAT('Modificación de empleado ',CONCAT(primerNombre, ' ' ,primerApellido), ' con ID ', idEmpleado),
GETDATE()
from inserted

END

GO


CREATE TRIGGER InsertarRecetaMedica
ON [Consultas].[RecetaMedica] AFTER INSERT
AS
BEGIN 

declare @id sql_variant
	set @id = (select SESSION_CONTEXT(N'user_id'));
	insert into [Historial].[Bitacora]
	select cast(@id as int), 
SYSTEM_USER,
CONCAT('Inserción de Receta con ID ', idRecetaMedica),
GETDATE()
from inserted

END


GO

---------------------
------------------
--------------------

CREATE TRIGGER InsertarDetalleRecetaON [Consultas].[DetalleRecetaMedica] AFTER INSERTASBEGIN declare @id sql_variant	set @id = (select SESSION_CONTEXT(N'user_id'));	insert into [Historial].[Bitacora]	select cast(@id as int), SYSTEM_USER,CONCAT('Inserción de farmaco ',(SELECT A.descripcionFarmaco FROM [Consultas].[Farmaco] A INNER JOIN [Consultas].[DetalleRecetaMedica] BON A.idFarmaco = B.idFarmaco WHERE A.idFarmaco = (select idFarmaco from inserted) and B.idRecetaMedica = (select idRecetaMedica from inserted)) , ' en la receta con el ID ', idRecetaMedica),GETDATE()from insertedEND
GO

CREATE TRIGGER ModificarDetalleRecetaON [Consultas].[DetalleRecetaMedica] AFTER UPDATEASBEGIN declare @id sql_variant	set @id = (select SESSION_CONTEXT(N'user_id'));	insert into [Historial].[Bitacora]	select cast(@id as int), SYSTEM_USER,CONCAT('Modificación de farmaco ',(SELECT A.descripcionFarmaco FROM [Consultas].[Farmaco] A INNER JOIN [Consultas].[DetalleRecetaMedica] BON A.idFarmaco = B.idFarmaco  WHERE A.idFarmaco = (select idFarmaco from inserted) and B.idRecetaMedica = (select idRecetaMedica from inserted)) , ' en la receta con el ID ', idRecetaMedica),GETDATE()from insertedEND
GO



CREATE TRIGGER EliminarDetalleRecetaON [Consultas].[DetalleRecetaMedica] AFTER DELETEASBEGIN declare @id sql_variant	set @id = (select SESSION_CONTEXT(N'user_id'));	insert into [Historial].[Bitacora]	select cast(@id as int), SYSTEM_USER,CONCAT('Eliminación de farmaco ',(SELECT A.descripcionFarmaco FROM [Consultas].[Farmaco] A where A.idFarmaco = (Select d.idFarmaco from deleted d) ) , ' en la receta con el ID ', idRecetaMedica),GETDATE()from deletedEND
GO



--------------------
---------------------
----------------------
CREATE TRIGGER InsertarConsulta
on [Consultas].[Consulta] after insert

as

BEGIN

declare @id sql_variant	set @id = (select SESSION_CONTEXT(N'user_id'));	insert into [Historial].[Bitacora]	select cast(@id as int),
	SYSTEM_USER,
	CONCAT('Inserción de la consulta con ID ',idConsulta),
	GETDATE()
	from inserted	

END
GO


CREATE TRIGGER ModificarConsulta
on [Consultas].[Consulta] after update

as

BEGIN

declare @id sql_variant	set @id = (select SESSION_CONTEXT(N'user_id'));	insert into [Historial].[Bitacora]	select cast(@id as int),
	SYSTEM_USER,
	CONCAT('Modificación a la consulta con ID ',idConsulta),
	GETDATE()
	from inserted	

END
GO

-------------------
-----------------


CREATE TRIGGER InsertarDetalleFarmaco
on [Consultas].[DetalleFarmaco] after insert

as

BEGIN

declare @id sql_variant	set @id = (select SESSION_CONTEXT(N'user_id'));	insert into [Historial].[Bitacora]	select cast(@id as int),	SYSTEM_USER,	CONCAT('Inserción de lote del fármaco ', (select F.descripcionFarmaco from [Consultas].[DetalleFarmaco] DF 	INNER JOIN [Consultas].[Farmaco] F	on F.idFarmaco = DF.idFarmaco 	WHERE F.idFarmaco = (select I.idFarmaco from inserted I) and DF.fechaVencimiento = (select I.fechaVencimiento from inserted I ) ), 	' con fecha de vencimiento ', fechaVencimiento ),	GETDATE()	from inserted

END
GO


CREATE TRIGGER ModificarDetalleFarmaco
on [Consultas].[DetalleFarmaco] after update

as

BEGIN

declare @id sql_variant	set @id = (select SESSION_CONTEXT(N'user_id'));	insert into [Historial].[Bitacora]	select cast(@id as int),	SYSTEM_USER,	CONCAT('Modificación de lote del fórmaco ', (select F.descripcionFarmaco from [Consultas].[DetalleFarmaco] DF 	INNER JOIN [Consultas].[Farmaco] F	on F.idFarmaco = DF.idFarmaco 	WHERE F.idFarmaco = (select I.idFarmaco from inserted I) and DF.fechaVencimiento = (select I.fechaVencimiento from inserted I ) ), 	' con fecha de vencimiento ', fechaVencimiento ),	GETDATE()	from inserted

END
GO



CREATE TRIGGER EliminarDetalleFarmaco
on [Consultas].[DetalleFarmaco] after delete

as

BEGIN

declare @id sql_variant	set @id = (select SESSION_CONTEXT(N'user_id'));	insert into [Historial].[Bitacora]	select cast(@id as int),	SYSTEM_USER,	CONCAT('Eliminacion de lote del fármaco ', (select F.descripcionFarmaco from [Consultas].[Farmaco] F	WHERE F.idFarmaco = (select D.idFarmaco from deleted D)), 	' con fecha de vencimiento ', fechaVencimiento ),	GETDATE()	from deleted

END
GO

----------------------------------------------
----------------------------------------------
-------------------------------------------------

CREATE TRIGGER InsertarFarmaco
on [Consultas].[Farmaco] after insert

as

BEGIN

declare @id sql_variant	set @id = (select SESSION_CONTEXT(N'user_id'));	insert into [Historial].[Bitacora]	select cast(@id as int),
	SYSTEM_USER,
	CONCAT('Inserción del fármaco ', descripcionFarmaco , ' con ID ', idFarmaco),
	GETDATE()
	from inserted	

END
GO



CREATE TRIGGER ModificarFarmaco
on [Consultas].[Farmaco] after update

as

BEGIN

declare @id sql_variant	set @id = (select SESSION_CONTEXT(N'user_id'));	insert into [Historial].[Bitacora]	select cast(@id as int),
	SYSTEM_USER,
	CONCAT('Modificación del fármaco ', descripcionFarmaco , ' con ID ', idFarmaco),
	GETDATE()
	from inserted	

END
GO