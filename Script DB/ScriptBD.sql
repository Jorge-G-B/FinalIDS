CREATE DATABASE FinalIDS;
use FinalIDS

CREATE TABLE candidato (

   id INT NOT NULL AUTO_INCREMENT,

   nombre VARCHAR(255) NOT NULL,
   
   Partido VARCHAR(255) NOT NULL,
   
   DPI VARCHAR(255) NOT NULL,

   PRIMARY KEY (id)

);

CREATE TABLE voto (

   id INT NOT NULL AUTO_INCREMENT,
   NombreVotante VARCHAR(255) NOT NULL,
   DPI VARCHAR(20) NOT NULL,
   FechaVoto DATE,
   IPOrigen VARCHAR(50),
   Estado INT,
   CandidatoVotado VARCHAR(255) NOT NULL,

   PRIMARY KEY (id)

);

CREATE TABLE Estadisticas (
    id INT NOT NULL AUTO_INCREMENT,
    Descripcion VARCHAR(255) NOT NULL,
    CantVotos INT NOT NULL,
	PRIMARY KEY (id)
);

CREATE TABLE Usuario(
    id INT NOT NULL AUTO_INCREMENT,
    Usuario VARCHAR(255) NOT NULL,
    Contraseña VARCHAR(255) NOT NULL,
	PRIMARY KEY (id)
);