CREATE DATABASE Practica1_DS2
GO
USE Practica1_DS2
GO

SET DATEFORMAT DMY
GO
CREATE TABLE PERSONA(
	Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
	Nombre VARCHAR(80),
	Apellido VARCHAR(80),
	TipoDocumento VARCHAR(50),	
	DNI CHAR(8) NULL,
	CarnetExtranjero VARCHAR(12) NULL,
	Pasaporte VARCHAR(12) NULL,
	FechaNacimiento DATE,
	--FechaRegistro DATETIME
	 )
GO

INSERT INTO PERSONA(Nombre, Apellido, TipoDocumento,DNI,FechaNacimiento)
--VALUES (' Al ','Ponce de Leon','DNI','72545404','05/10/2003',GETDATE())
VALUES ('Fernando','Ponce Robles','DNI','72575791','07/05/2023')
SELECT* FROM PERSONA

--Mostrar Personas
	SELECT
    Nombre,
    TipoDocumento,
    Apellido,
    DNI,
    CarnetExtranjero,
    Pasaporte,
    FORMAT(FechaNacimiento, 'dd MMMM yyyy') AS FechaNacimientoConMes,
    YEAR(GETDATE()) - YEAR(FechaNacimiento) - 
    CASE
        WHEN MONTH(GETDATE()) < MONTH(FechaNacimiento) OR (MONTH(GETDATE()) = MONTH(FechaNacimiento) AND DAY(GETDATE()) < DAY(FechaNacimiento))
        THEN 1
        ELSE 0
    END AS Edad    
FROM
    PERSONA
----------------------------------------------------------------------------------------------------------------------------------------------------------------------


