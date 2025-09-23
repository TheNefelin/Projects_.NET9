USE db_testing

-- Tables -------------------------------------------------------
-- --------------------------------------------------------------

CREATE TABLE GG_Users (
	User_Id UNIQUEIDENTIFIER PRIMARY KEY,
	Email VARCHAR(100) UNIQUE NOT NULL,
	GoogleSUB VARCHAR(256),
	GoogleJTI VARCHAR(256),
	SqlToken UNIQUEIDENTIFIER
)
GO

CREATE TABLE GG_Games (
	Game_Id INT PRIMARY KEY IDENTITY(1,1),
	Name VARCHAR(50) NOT NULL,
	Description VARCHAR(256) NOT NULL,
	ImgUrl VARCHAR(256) NOT NULL,
	IsEnabled BIT NOT NULL
)
GO

CREATE TABLE GG_Characters (
	Character_Id INT PRIMARY KEY IDENTITY(1,1),
	Name VARCHAR(50) NOT NULL,
	Description VARCHAR(256) NOT NULL,
	ImgUrl VARCHAR(256) NOT NULL,
	Game_Id INT NOT NULL,
	FOREIGN KEY (Game_Id) REFERENCES GG_Games(Game_Id)
)
GO

CREATE TABLE GG_Sources (
	Source_Id INT PRIMARY KEY IDENTITY(1,1),
	Name VARCHAR(50) NOT NULL,
	Url VARCHAR(256) NOT NULL,
	Game_Id INT NOT NULL,
	FOREIGN KEY (Game_Id) REFERENCES GG_Games(Game_Id)
)
GO

CREATE TABLE GG_BackgroundImgs (
	BackgroundImg_Id INT PRIMARY KEY IDENTITY(1,1),
	ImgUrl VARCHAR(256) NOT NULL,
	Game_Id INT NOT NULL,
	FOREIGN KEY (Game_Id) REFERENCES GG_Games(Game_Id)
)
GO

CREATE TABLE GG_Guides (
	Guide_Id INT PRIMARY KEY IDENTITY(1,1),
	Name VARCHAR(100) NOT NULL,
	Sort INT NOT NULL,
	Game_Id INT NOT NULL,
	FOREIGN KEY (Game_Id) REFERENCES GG_Games(Game_Id),
)
GO

CREATE TABLE GG_Adventures (
	Adventure_Id INT PRIMARY KEY IDENTITY(1,1),
	Description VARCHAR(800) NOT NULL,
	IsImportant BIT NOT NULL,
	Sort INT NOT NULL,
	Guide_Id INT NOT NULL,
	FOREIGN KEY (Guide_Id) REFERENCES GG_Guides(Guide_Id),
)
GO

CREATE TABLE GG_AdventureImgs (
	AdventureImg_Id INT PRIMARY KEY IDENTITY(1,1),
	ImgUrl VARCHAR(256) NOT NULL,
	Sort INT NOT NULL,
	Adventure_Id INT NOT NULL,
	FOREIGN KEY (Adventure_Id) REFERENCES GG_Adventures(Adventure_Id),
)
GO

CREATE TABLE GG_UserGuides (
	User_Id UNIQUEIDENTIFIER NOT NULL,
	Guide_Id INT NOT NULL,
	IsCheck BIT NOT NULL,
	PRIMARY KEY (Guide_Id, User_Id),
	FOREIGN KEY (Guide_Id) REFERENCES GG_Guides(Guide_Id),
)
GO

CREATE TABLE GG_UserAdventures (
	User_Id UNIQUEIDENTIFIER NOT NULL,
	Adventure_Id INT NOT NULL,
	IsCheck BIT NOT NULL,
	PRIMARY KEY (Adventure_Id, User_Id),
	FOREIGN KEY (Adventure_Id) REFERENCES GG_Adventures(Adventure_Id),
)
GO

DROP TABLE __EFMigrationsHistory
GO
DROP TABLE GG_AdventureImgs
GO
DROP TABLE GG_UserAdventures
GO
DROP TABLE GG_Adventures
GO
DROP TABLE GG_UserGuides
GO
DROP TABLE GG_Guides
GO
DROP TABLE GG_BackgroundImgs
GO
DROP TABLE GG_Sources
GO
DROP TABLE GG_Characters
GO
DROP TABLE GG_Games
GO
DROP TABLE GG_Users
GO

-- Data ---------------------------------------------------------
-- --------------------------------------------------------------

SET IDENTITY_INSERT GG_Games ON
GO
INSERT INTO GG_Games
	(Game_Id, Name, Description, ImgUrl, IsEnabled)
VALUES
	(1, 'Chrono Cross', 'Lejos el mejor RPG en mi opinion personal. Precisamente, este juego, me inspiro hacer esta pagina.', 'cover_cc_617x617.webp', 1),
	(2, 'Comrades', 'Creo que soy el �nico que juega a esto, los servidores siempre est�n vac�os.', 'cover_com_617x617.webp', 1),
	(3, 'Darksiders 2', 'Un MANJAR de los dioses.', 'cover_ds2_617x617.webp', 1),
	(4, 'FFIX', 'Uno de los trofeos mas duros de conseguir', 'cover_ffix_617x617.webp', 1);
GO
SET IDENTITY_INSERT GG_Games OFF
GO

SET IDENTITY_INSERT GG_Characters ON
GO
INSERT INTO GG_Characters
	(Character_Id, Name, Description, ImgUrl, Game_Id)
VALUES
	(1,'Serge','Se obtiene al inciar la aventura','character_cc_serge_288x288.webp',1),
	(2,'Mojo','Se obtiene con el Shark Tooth Amulet','character_cc_mojo_288x288.webp',1),
	(3,'Leena','Se obtiene rechazando a Kid 3 veces','character_cc_leena_288x288.webp',1),
	(4,'Poshul','Se obtiene rechazando a Kid 3 veces o en Arni Village dandole el hueso','character_cc_poshul_288x288.webp',1),
	(5,'Kid','Se obtiene en Termina o Cape Howl','character_cc_kid_288x288.webp',1),
	(6,'Guile','Se obtiene en Termina hablando con �l en el bar','character_cc_guile_288x288.webp',1),
	(7,'Nikki','Se obtiene en Termina hablando con Miki en el barco de Magical Dreamers','character_cc_nikki_288x288.webp',1),
	(8,'Pierre','Se obtiene en Termina entregandole el Heros Medal en la herreria','character_cc_pierre_288x288.webp',1),
	(9,'Glenn','Se obtiene en Termina al decidir No salvar a Kid','character_cc_glenn_288x288.webp',1),
	(10,'Macha','Se obtiene en Termina al decidir No salvar a Kid','character_cc_macha_288x288.webp',1),
	(11,'Doc','Se obtiene en Termina al decidir No salvar a Kid','character_cc_doc_288x288.webp',1),
	(12,'Korcha','Se obtiene en Termina al decidir salvar a Kid','character_cc_korcha_288x288.webp',1),
	(13,'Greco','Se obtiene en Termina al decidir salvar a Kid','character_cc_greco_288x288.webp',1),
	(14,'Razzly','Se obtiene en Hydra Marsh al decidir salvar a Kid','character_cc_razzly_288x288.webp',1),
	(15,'Mel','Se obtiene en Guldove, despue se capturarla y salir de la isla','character_cc_mel_288x288.webp',1),
	(16,'Pip','Se obtiene en el barco fantasma','character_cc_pip_288x288.webp',1),
	(17,'Luccia','Se obtiene en Viper Manor al ir nuevamente','character_cc_luccia_288x288.webp',1),
	(18,'Lynx','Se obtiene despues del evento en Fort Dragonia','character_cc_lynx_288x288.webp',1),
	(19,'Sprigg','Se obtiene en el Mundo Abstracto','character_cc_sprigg_288x288.webp',1),
	(20,'Harle','Se obtiene en el Mundo Abstracto','character_cc_harle_288x288.webp',1),
	(21,'Radius','Se obtiene al vencerlo en Arni Village','character_cc_radius_288x288.webp',1),
	(22,'Zappa','Se obtiene en Termina con Lynx','character_cc_zappa_288x288.webp',1),
	(23,'Van','Se obtiene en Termina con Lynx','character_cc_van_288x288.webp',1),
	(24,'Norris','Se obtiene en Viper Manor si Radius esta en tu equipo','character_cc_norris_288x288.webp',1),
	(25,'Starky','Se obtiene venciendolo en la isla de Sky Dragon','character_cc_starky_288x288.webp',1),
	(26,'Janice','Se obtiene en el Zelbess al ganar el SLAM de combate','character_cc_janice_288x288.webp',1),
	(27,'Sneff','Se obtiene en Zelbess despues de derrotar al sabio','character_cc_sneff_288x288.webp',1),
	(28,'Irenes','Se obtiene en Zelbess despues de derrotar al sabio','character_cc_irenes_288x288.webp',1),
	(29,'Miki','Se obtiene en Zelbess despues de derrotar al sabio','character_cc_miki_288x288.webp',1),
	(30,'Zoah','Se obtiene en el bar de Termina, habitacion oculta','character_cc_zoah_288x288.webp',1),
	(31,'Karsh','Se obtiene en el bar de Termina, habitacion oculta','character_cc_karsh_288x288.webp',1),
	(32,'Orcha','Se obtiene en Viper Manor despues de rescatar a Riddel','character_cc_orcha_288x288.webp',1),
	(33,'Grobyc','Se obtiene en Viper Manor despues del combate con el Mecha','character_cc_grobyc_288x288.webp',1),
	(34,'Skelly','Se obtiene al conseguir todos sus huesos, son 6 en total','character_cc_skelly_288x288.webp',1),
	(35,'Riddel','Se obtiene al rescatarla del ejercito en Viper Manor','character_cc_riddel_288x288.webp',1),
	(36,'Viper','Se obtiene despues de rescatar a Riddel en Viper Manor','character_cc_viper_288x288.webp',1),
	(37,'Fargo','Se obtiene despues de rescatar a Riddel en Viper Manor','character_cc_fargo_288x288.webp',1),
	(38,'Marcy','Se obtiene despues de rescatar a Riddel en Viper Manor','character_cc_marcy_288x288.webp',1),
	(39,'Turnip','Se obtiene en Hermits Hideaway (AW), (HW), utilizando Ice Gun o Ice Breath con Poshul en tu equip','character_cc_turnip_288x288.webp',1),
	(40,'Funguy','Se obtiene en Shadow Forest al darle el Mushroom al hombre de la cueva en la cascada','character_cc_funguy_288x288.webp',1),
	(41,'Neofio','Se obtiene en la pileta de Viper Manor con la Life Sparkle','character_cc_neofio_288x288.webp',1),
	(42,'Leah','Se obtiene al llegar a la isla de Gaeas Navel','character_cc_leah_288x288.webp',1),
	(43,'Steena','Se obtiene en Guldove al mostrarle el Dragon Emblem','character_cc_steena_288x288.webp',1),
	(44,'Draggy','Se obtiene al poner el huevo gigante en Fort Dragonia','character_cc_draggy_288x288.webp',1),
	(45,'Orlha','Se obtiene en Guldove devolviendole el Sapphire Brooch como Serge','character_cc_orlha_288x288.webp',1)
GO
SET IDENTITY_INSERT GG_Characters OFF
GO

SET IDENTITY_INSERT GG_Sources ON
GO
INSERT INTO GG_Sources
	(Source_Id, Name, Url, Game_Id)
VALUES
	(1,'Guia','https://guiamania.com/41154/',1),
	(2,'Window Frame','https://www.ign.com/wikis/chrono-cross/Window_Frames/',1),
	(3,'Finales','https://game8.co/games/Chrono-Cross-Radical-Dreamers-Edition/archives/375630/',1),
	(4,'Items','https://game8.co/games/Chrono-Cross-Radical-Dreamers-Edition/archives/371977/',1),
	(6,'Dragones','https://chrono.fandom.com/wiki/Chronopedia/',1),
	(7,'Tecnicas','https://guiltybit.com/como-conseguir-las-tecnicas-definitivas-de-todos-los-personajes-en-chrono-cross/',1),
	(8,'Criosphinx','https://chrono.fandom.com/wiki/Criosphinx/',1),
	(9,'Bend of Time','https://chrono.fandom.com/wiki/Bend_of_Time/',1),
	(10,'Triple Tech Delta Force','https://www.trueachievements.com/a356989/deadly-delta-achievement/',1),
	(11,'Triple Tech Z-Slash','https://www.trueachievements.com/a356990/z-one-and-only-achievement/',1),
	(12,'Guia Paso a Paso','https://www.youtube.com/watch?v=ZVTfcjFNVcU&list=PLUEaSJ4rKZoL5SJjw7ryjUI2NAztObT8F&index=1',4),
	(13,'Trofeo: Mister Nice Guy (  angelo noctis)','https://www.youtube.com/watch?v=RWI-uaZsYAY&t=912s',4),
	(14,'Trofeo: Beating the rigtime blues (angelo noctis)','https://www.youtube.com/watch?v=RWI-uaZsYAY&lc=UgxWdOT4ZJzoZmeEJix4AaABAg.9xkQXwbhDhM9xkVq_TWzy0',4),
	(15,'Trofeo: A clean bill of health (chibikei)','https://www.youtube.com/watch?v=Vrh5KILchfc',4)	
GO
SET IDENTITY_INSERT GG_Sources OFF
GO

SET IDENTITY_INSERT GG_BackgroundImgs ON
GO
INSERT INTO GG_BackgroundImgs
	(BackgroundImg_Id, ImgUrl, Game_Id)
VALUES
	(1,'background_cc_01_1280x720.webp',1),
	(2,'background_cc_02_1280x720.webp',1),
	(3,'background_com_01_1280x720.webp',2),
	(4,'background_com_02_1280x720.webp',2),
	(5,'background_ds2_01_1280x720.webp',3),
	(6,'background_ds2_02_1280x720.webp',3),
	(7,'background_ffix_01_1280x720.webp',4),
	(8,'background_ffix_02_1280x720.webp',4),
	(9,'map_ds2_01.webp',3),
	(10,'map_ds2_02.webp',3),
	(11,'map_ds2_03.webp',3),
	(12,'map_ds2_04.webp',3)
GO
SET IDENTITY_INSERT GG_BackgroundImgs OFF
GO

SET IDENTITY_INSERT GG_Guides ON
GO
INSERT INTO GG_Guides
	(Guide_Id, Name, Sort, Game_Id)
VALUES
	(1,'El Sueño',1,1),
	(2,'Final oculto: Programmers Ending (Final 1/11)',2,1),
	(3,'(HW) Arni Village',3,1),
	(4,'(HW) Lizard Rock',4,1),
	(5,'(AW) Another World',5,1),
	(6,'(AW) Hydra Swamp',6,1),
	(7,'(AW) Fossil Valley',7,1),
	(8,'(AW) Termina',8,1),
	(9,'Final oculto: General Kid (Final 2/11)',9,1),
	(10,'(AW) Infiltración a Viper Manor',10,1),
	(11,'(AW) Viper Manor',11,1),
	(12,'Final oculto: The True Hero (Final 3/11)',12,1),
	(13,'(AW) Guldove',13,1),
	(14,'(HW) Bosque Hydra',14,1),
	(15,'(AW) Piratas Fantasmas',15,1),
	(16,'(AW) Guldove | Termina | Viper Manor',16,1),
	(17,'(HW) Arni Village',17,1),
	(18,'(AW) Costa Este de El Nido',18,1),
	(19,'Final oculto: The Magical Dreamers (Final 4/11)',19,1),
	(20,'(AW) Fort Dragonia',20,1),
	(21,'Mundo Abstracto y el Más Allá',21,1),
	(22,'Final oculto: New Beginnings (Final 5/11)',22,1),
	(23,'(HW) Termina',23,1),
	(24,'(HW) Viper Manor',24,1),
	(25,'(HW) Nido Triangle | Sky Dragon Isle | Marbule',25,1),
	(26,'(HW) El Zelbess de Fargo',26,1),
	(27,'(HW) Mar del Edén',27,1),
	(28,'(HW) Isla Maldita',28,1),
	(29,'(HW) Mar del Edén',29,1),
	(30,'(AW) Hydra Swamp | Termina',30,1),
	(31,'(AW) Viper Manor',31,1),
	(32,'Final oculto: Onward Dragoons (Final 6/11)',32,1),
	(33,'(AW) Guldove | Water Dragon Isle | Isle of the Damned | Termina',33,1),
	(34,'(AW) Hermits Hideaway',34,1),
	(35,'Final oculto: Darkened Fate (Final 7/11)',35,1),
	(36,'(AW) Isla del Dragón del Cielo',36,1),
	(37,'(HW) Preparación',37,1),
	(38,'(HW) Dragón Verde',38,1),
	(39,'(HW) Dragón de Agua',39,1),
	(40,'(HW) Dragón de Tierra',40,1),
	(41,'(AW) Dragón de Fuego',41,1),
	(42,'(AW) Dragón Negro',42,1),
	(43,'(AW) Dragón del Cielo',43,1),
	(44,'(AW) Criosphinx',44,1),
	(45,'(AW) Guldove',45,1),
	(46,'(HW) Fuerte Dragonian',46,1),
	(47,'(AW) La Búsqueda de la Masamune',47,1),
	(48,'Final oculto: Career Change (Final 8/11)',48,1),
	(49,'(HW) Mar de Edén',49,1),
	(50,'(HW) Chronopolis',50,1),
	(51,'(AW) Cascadas Divinas de los Dragones',51,1),
	(52,'Cosas por Hacer',52,1),
	(53,'(AW) Hermits Hideaway',53,1),
	(54,'(AW) El Nido Triangle',54,1),
	(55,'Final oculto: Return of the Downtrodden (Final 9/11)',55,1),
	(56,'(AW) Dinopolis y Terra Tower',56,1),
	(57,'(HW) Playa Opassa',57,1),
	(58,'Final: True Ending (Final 10/11)',58,1),
	(59,'Final: Bad Ending (Final 11/11)',59,1),
	(60,'Consideraciones',60,1),
	(61,'Lanza - Bigote de Dragón (1)',1,2),
	(62,'Lanza - Bigote de Dragón (2)',2,2),
	(63,'Mazo - Mjolnir',3,2),
	(64,'Katana - Mumeito (1)',4,2),
	(65,'Katana - Mumeito (2)',5,2),
	(66,'Escudo - Égida',6,2),
	(67,'SET Equipo de combate',7,2),
	(68,'The Forge Land',1,3),
	(69,'The Forge Land - The Cauldron',2,3),
	(70,'The Forge Land - The Drenchfort',3,3),
	(71,'The Forge Land - The Foundry',4,3),
	(72,'The Forge Land - The Lost Temple',5,3),
	(73,'The Forge Land - The Nook',6,3),
	(74,'The Forge Land - The Scar',7,3),
	(75,'The Forge Land - The Shattered Forge',8,3),
	(76,'The Forge Land - The Weeping Crag',9,3),
	(77,'The Forge Land - Tri-Stone',10,3),
	(78,'Kingdom of the Dead',11,3),
	(79,'Kingdom of the Dead - Boneriven',12,3),
	(80,'Kingdom of the Dead - Breach',13,3),
	(81,'Kingdom of the Dead - City of the Dead',14,3),
	(82,'Kingdom of the Dead - Eternal Throne',15,3),
	(83,'Kingdom of the Dead - Gilded Arena',16,3),
	(84,'Kingdom of the Dead - Judicators Tomb',17,3),
	(85,'Kingdom of the Dead - Lair of the Deposed King',18,3),
	(86,'Kingdom of the Dead - Phariseers Tomb',19,3),
	(87,'Kingdom of the Dead - Psychameron',20,3),
	(88,'Kingdom of the Dead - Sentinels Gaza',21,3),
	(89,'Bezswietle',22,3),
	(90,'Bezswietle - Earth',23,3),
	(91,'Bezswietle - The Ivory Citadel',24,3),
	(92,'Shadows Edge',25,3),
	(93,'Shadows Edge - The Black Stone',26,3),
	(94,'El concurso de Ragtime',1,4)
GO
SET IDENTITY_INSERT GG_Guides OFF
GO

GO
SET IDENTITY_INSERT GG_Adventures ON
GO
INSERT INTO GG_Adventures
	(Adventure_Id, Description, IsImportant, Sort, Guide_Id)
VALUES
	(1,'Obs: (HW) = Home World, (AW) = Another World debo validar (Turnip y Neofio)',0,1,1),
	(2,'Sigue el camino hasta el pilar de luz',0,2,1),
	(3,'Ponte encima de la plataforma magenta',0,3,1),
	(4,'Entra por la puerta',0,4,1),
	(5,'Inicias el juego con estos marcos (Marco 1/15) (Marco 2/15) (Marco 3/15)',0,5,1),
	(6,'Cuando despiertes crea un guardado independiente de la partida principal, cuando termines el juego carga esta partida con el Continue+. Vencer al Time Devourer inmediatamente sin reclutar companeros',1,1,2),
	(7,'Despiertas con Serge (Personaje 1/45)',0,1,3),
	(8,'Habla con Marge (Mama)',0,2,3),
	(9,'Ve al puerto y habla con Leena',0,3,3),
	(10,'Obten la Shellfish Window Frame (Marco 4/15) de la tienda de elements',0,4,3),
	(11,'Consigue la Komodo Scale (Item /56) dandole la razon al vendedor y regalasela al muchacho',0,5,3),
	(12,'Coge el Shark Tooth Amulet (Item /56) de uno de los tipos de las casas de la derecha',0,6,3),
	(13,'Consigue el Heckran Bone (Item /56) de debajo de la cama del bar',0,7,3),
	(14,'Habla con la camarera del bar la de los poemas y finalmente NO reclutes a Poshul aun',0,8,3),
	(15,'Empuja la roca enfrente de la cueva, y lucha con el primer Komodo',0,1,4),
	(16,'Usa el saliente para llegar el segundo Komodo',0,2,4),
	(17,'Debes perseguir, para cazar y luego luchar con el tercer Komodo, y luego a Mama Komodo',0,3,4),
	(18,'Sal por la izquierda de la playa, y sigue recto',0,4,4),
	(19,'Ve a Arni y habla con Marge y Leena',0,1,5),
	(20,'Habla con la camarera del bar la de los poemas',0,2,5),
	(21,'Ve a Cape Howl y examina la tumba',0,3,5),
	(22,'Lucharas con Karsh, Solt y Peppor',0,4,5),
	(23,'Nombra a Kid pero NO la reclutes aun (rechazala 3 veces)',0,5,5),
	(24,'Leena (Personaje 3/45) y Poshul (Personaje 4/45) se te uniran ahora, (es la unica manera de conseguir a Leena)',0,6,5),
	(25,'Ve donde conseguiste el Shark Tooth Amulet y muestraselo al predicador, se te unira Mojo (Personaje 2/45) al salir de alli',0,7,5),
	(26,'Ve al noroeste, encuentra a un chico que te dara el Safety Gear (Item /56)',0,1,6),
	(27,'Vete por ahora al Fossil Valley',0,2,6),
	(28,'Obten el Heavy Skull (Item /56) de Skelly',0,1,7),
	(29,'Obten las Bellflower (Item /56)',0,2,7),
	(30,'Obten el Big Egg (Item /56) cerca del pajaro',0,3,7),
	(31,'Continua el camino hacia el norte y lucha contra Solt y Peppor',0,4,7),
	(32,'Ve a la estatua en lo mas alto del pueblo, habla con el tipo que la esta limpiando, y consigue a Kid (Personaje 5/45) y obtendras el Tele Porter (Item /56)',0,1,8),
	(33,'Obten el marco Tea for Three (Marco 5/15)',0,2,8),
	(34,'Ve a la parte mas al este del pueblo, dale las flores a Glenn',0,3,8),
	(35,'Aqui debes decidir cual de los 3 personajes quieres reclutar para infiltrarte en el Viper Manor, No los puedes conseguir a todos en una partida, deberas iniciar una partida (NG+) para los 2 que falten',0,4,8),
	(36,'Opcion 1: Consigue a Guile (Personaje 6/45), Habla con el en el bar. Ve al cenemterio de Termina, encuentra el barco de Korcha y sube por el rompeolas',1,5,8),
	(37,'Opcion 2: Consigue a Pierre (Personaje 8/45), en la Herreria, primero debes conseguir el Heros Medal (Item /56) del del nino que juega afuera y devolverselo a Pierre, Ve a la puerta principal de Viper Manor',1,6,8),
	(38,'Opcion 3: Consigue a Nikki (Personaje 7/45), habla con todos en el barco de Magical Dreamers hasta que llegue Miki. Ve al Bosque Shadow, consigue la Aroma Pouch (Item /56) y usalo para mover a los monstruitos hacia el monstruo-planta gigante, en la cueva de la cascada puedes encontrar el Skullduggery (Marco 6/15) y recoge la Angry Scapula (Item /56) de Skelly, avanza por el camino que bloqueaba el mounstruo, mueve la roca para detener a los insectos, avanza hasta salir por el pozo',1,7,8),
	(39,'Crea un guardado independiente de la partida principal, cuando termines el juego carga esta partida con el Continue+. Vencer al Time Devourer antes de infiltrarte en Viper Manor',1,1,9),
	(40,'La puerta principal esta cerrada',0,1,10),
	(41,'La llave Manor Key (Item /56) esta en la parte superior derecha del establo de alimentacion de dragones',0,2,10),
	(42,'Debes jugar al juego de alimentar dragones al menos una vez, si quieres alimentarlos todos sin fallar, puedes usar el Time Shifter (Item /56) que puedes obtener en la partida (NG+)',0,3,10),
	(43,'Gira la estatua de la serpiente, y cae en la trampa',0,1,11),
	(44,'Lucha con los guardias, coge su ropa y entra en las habitaciones de la parte izquierda',0,2,11),
	(45,'En la cocina, habla con Glenn, ve a su habitacion, y te escribira la combinacion de la estatua',0,3,11),
	(46,'En las habitaciones de la derecha, recoge todos los articulos y el pendiente para caer en otra trampa',0,4,11),
	(47,'Lucha con los bichos de Luccia',0,5,11),
	(48,'Libera al criatura que esta enjaulada',0,6,11),
	(49,'La habitacion que esta al lado del laboratorio podras conseguir el Decor Shield (Item /56), si la colocas en la armadura sin escudo encontraras una habitacion secreta',0,7,11),
	(50,'Vuelve a la entrada y usa la combinacion apropiada en la estatua de la serpiente',0,8,11),
	(51,'Ve al oeste, a la biblioteca, veras a Balthasar (Chrono Trigger), lucha con Marcy',0,9,11),
	(52,'Obten el marco Porres Furnace (Marco 7/15)',0,10,11),
	(53,'Habla con el viejo, vuelve a la habitacion grande, busca en las columnas un boton que accione el ascensor',0,11,11),
	(54,'Lucha con los guardias, toma el ascensor y pasa la puerta',0,12,11),
	(55,'Lucha contra Lynx, habra unas cuantas escenas',0,13,11),
	(56,'Este final tiene 2 variantes, las variantes dependeran de que decision tomas en Guldove',0,1,12),
	(57,'Opcion 1: No salvar a Kid. Crea un guardado independiente de la partida principal, cuando termines el juego carga esta partida con el Continue+, Vencer al Time Devourer inmediatamente despues de que Kid quede envenenada',1,2,12),
	(58,'Opcion 2: Salvar a Kid. Crea un guardado independiente de la partida principal, cuando termines el juego carga esta partida con el Continue+, Vencer al Time Devourer inmediatamente despues de encontrar la cura en Hydra Marsh y Kid se haya recuperado',1,3,12),
	(59,'Tendras que eleguir entre salvar o NO salvar a kid recibiras el Astral Amulet (Item /56), dependiendo de la decicion podras obtener nuevos personajes distintos en ambos casos',0,1,13),
	(60,'En ambos casos debes hablar con el Shaman y el enano del bar el cual te dara el Green Tinkler (Item /56)',0,2,13),
	(61,'Opcion 1: No salvar a Kid: Habla con Korcha y luego con Macha, toma el barco de Kocha hacia Termina, en la entrada veras a Glenn. Vuelve al barco, conseguiras a Glenn (Personaje 9/45) y Macha (Personaje 10/45), Macha te dara el Tropical Paradise (Marco 8/15). Haz el capitulo: 15.- (AW) Piratas Fantasmas, luego vuelve a Guldove para ver al Doc y Kid. Se te unira el Doc (Personaje 11/45). Finalmente puedes hacer el capitulo: 14.- (HW) Bosque Hydra',1,3,13),
	(62,'Opcion 2: Salvar a Kid: Se te une Korcha (Personaje 12/45), Toma el barco hacia Termina, en la entrada veras a Glenn, Ve a la Playa Opassa, Haz el capitulo: 14.- (HW) Bosque Hydra, vuelve a Guldove, asegurate de darle el medicamento a Kid y luego trata de captura a Mel (Personaje 13/45) si es que las quieres conseguir, ella te da el Guldovian Stitch (Marco 9/15), para ello deberas hablar con Orla y el portero del Shaman luego ve al escondite de Mel. Viaja a Termina, escucha a Glenn Vuelve al bote aceptando a Korcha NUEVAMENTE y viaja a Guldove a la casa de Mel (Por fin se une la bastarda). Finalmente ve al capitulo: 15.- (AW) Piratas Fantasmas',1,4,13),
	(63,'Lega a la playa y viaja al (HW) con el Astral Amulet de Kid',0,1,14),
	(64,'Ve al Hydra Marsh, lucha con Beeba, obten la Beeba Flute (Item /56) y usala al norte de todo, Lucha contra el Wingapede y cae por el agujero en donde acabas de pelear',0,2,14),
	(65,'No Salvar a Kid: Al caer por el agujero veras que el pulpo ya se comio a Razzly',1,3,14),
	(66,'Salvar a Kid: Lucha con el pulpo y consigue a Razzly el hada (Personaje 14/45), sigue por la parte norte luego este, lucha contra la Hydra (SIN Razzly en tu equipo), obtendras la Hydra Humour (Item /56)',1,4,14),
	(67,'Ve a la zona nublosa entre El Nido y el The Dead Sea',0,1,15),
	(68,'Cuando ataquen los fantasmas, observa que la puerta principal esta cerrada',0,2,15),
	(69,'Mata a todos los esqueletos y habla con los piratas para encontrar una llave',0,3,15),
	(70,'Durante el recorrido encontraras a la criatura que liberaste, si la capturas reclutaras a Pip (Personaje 15/45)',0,4,15),
	(71,'Llega a la cubierta, lucha con el Diamond Jar',0,5,15),
	(72,'Ve a guldove y consigue la Pelvic Bone (Item /56) de Skelly de la nina comerciante cerca del barco',0,1,16),
	(73,'Ve a Termina y consigue a Greco (Personaje 16/45) en la casa del cementerio',0,2,16),
	(74,'Ve a Viper Manor y busca Luccia (Personaje 17/45) para reclutarla',0,3,16),
	(75,'Ve a Arni Village habla con la camarera del bar y te dara el Book of Poems (Item /56)',0,1,17),
	(76,'Habla con el viejo pescador cerca de Leena, te llevara a la Isla del Dragon de Agua',0,2,17),
	(77,'Aqui, ve hasta el Dragon de Agua y te dara el Ice Breath (Item /56)',0,3,17),
	(78,'Cuando vuelvas a (AW) ve a Arni Village y entregale a la camarera el Book of Poems',0,4,17),
	(79,'Ve en bote al refugio del hermitano en (AW) y usa el Ice Breath en el suelo arido',0,5,17),
	(80,'Cruza el Monte Piro, usa el Ice Breath para congelar la lava',0,1,18),
	(81,'Te recomiendo recoger todos los cofres primero antes de congelar todo',0,2,18),
	(82,'Cruza todo el Monte hasta llegar a Fort Dragonia',0,3,18),
	(83,'Antes de terminar la Fortaleza, crea un guardado independiente de la partida principal, cuando termines el juego carga esta partida con el Continue+. Vencer al Time Devourer con la siguiente formacion Nikki, Razzly y Kid, Para reemplazar a Serge del euipo puede usar el Relief Charm (Item /56) que obtienes al iniciar la partida NG+',1,1,19),
	(84,'Resuelve todos los puzzles y jefes',0,1,20),
	(85,'Debes vencer al SOL (Hincha Pelotas), toma el acensor',0,2,20),
	(86,'Lynx y Viper estaran aqui, continua la historia...',0,3,20),
	(87,'Hola Lynx (Personaje 18/45), toma una manzana del arbol y Sprigg vendra',0,1,21),
	(88,'Entra en su casa y podras reclutar a Sprigg (Personaje 19/45)',0,2,21),
	(89,'Ve al norte, sigue a Serge a cada puerta que vaya (en tu piso)',0,3,21),
	(90,'Harle (Personaje 20/45) se te une, avanza, escapa por el agujero y apareceras en Hydra Marsh',0,4,21),
	(91,'Crea un guardado independiente de la partida principal, cuando termines el juego carga esta partida con el Continue+. Vencer al Time Devourer con Lynx',1,1,22),
	(92,'Intenta cruzar el portal para ir a (HW) en la playa luego intenta cruzar el Fossil Valley',0,1,23),
	(93,'Ve a Arni Village, habla con Marge y lucha contra Radius (Personaje 21/45) para reclutarlo',0,2,23),
	(94,'Cruza el Fossil Valley y consigue el Infrared Vision (Marco 10/15)',0,3,23),
	(95,'Ve Termina para reclutara Zappa (Personaje 22/45) este te data el Smith Spirit (Item /56) y a Van (Personaje 23/45)',0,4,23),
	(96,'Con Radius ve a Termina a buscar a Norris, luego entra por el Shadow Forest, despues de hablar con el nino salvar a un hombre ,golpeando a un insecto ahogado, y te dara el Mushroom (Item /56). Puedes obtener Funguy (Personaje 40/45) si le das el champinon al hombre de la cueva',0,1,24),
	(97,'Ve por las mazmorras, si no puedes pasar tendras que ir por la entrada principal y bajar por el pozo. Habla con Norris (Personaje 24/45), se te unira si Radius esta contigo',0,2,24),
	(98,'Ve a Termina, ahora podras usar un bote inflable donde estaba el barco de Nikki',0,3,24),
	(99,'Ve al Nido Triangle, nada hasta el fondo y obten el Star Fragment (Item /56)',0,1,25),
	(100,'Ve a Sky Dragon Isle e intenta abrir el cofre, lucha contra Starky (Personaje 25/45) y vencelo para reclutarlo, obten Our Favorite Martian (Marco 11/15)',0,2,25),
	(101,'Ve Marbule sin ningun humano en tu equipo, habla con Toma, haz todas las preguntas y habla con el otro tipo, y obten el Valencian Cloth (Marco 12/15), duerme en la posada, habla con Irenes por la noche',0,3,25),
	(102,'Intenta cruzar la puerta del fondo del barco, Fargo te debe dar permiso, abre la puerta siguiente y veras a un orejon que hace aseo, hablale',0,1,26),
	(103,'Ve a pedirle a fargo permiso para cruzar la puerta, te hara juega a la Roulette y perderas. ve a la primera puerta debajo del casino, veras algo sospechozo',0,2,26),
	(104,'Mira el show de Sneff, convertira a alguien en gato, sale y vuelve a entrar para que Sneff te convierta en gato',0,3,26),
	(105,'Mientras seas gato y le hablas al gato de la cocina podras obtener el Monsters Mouth (Marco 13/15)',0,4,26),
	(106,'Entra en el en la habitacion que esta debajo de la ruleta, roba el Handle (Item /56)',0,5,26),
	(107,'Ve donde Sneff y que te vuelva a convertir, juega a la Roulette con Fargo otra vez. Perdera, ahora cruza esa puerta',0,6,26),
	(108,'Si ganas los 3 combates del SLAM podras reclutar a Janice (Personaje 26/45)',0,7,26),
	(109,'Lucha contra el Sabio y obten la Fiddler Crab (Item /56)',0,8,26),
	(110,'Ve al casino donde esta la ruleta, luego ve a la segunda habitacion bajando las escaleras y reclutaras a Sneff (Personaje 27/45)',0,9,26),
	(111,'Sube a la cubierta y luego por el mastil, aqui se unira Irenes (Personaje 28/45)',0,10,26),
	(112,'ve al The Dead Sea y usa la Fiddler Crab, entra y veras la Masamune',0,1,27),
	(113,'Despues de hablar, ve a la Hermits Hideaway (HW), habla con Radius y recibe el Garais Keepsake (Item /56)',0,2,27),
	(114,'Llega al espejo de la izquierda del todo (bajando por el esqueleto serpiente)',0,1,28),
	(115,'Lucha con Garai (a llorar a la playa)',0,2,28),
	(116,'Obten la Einlanzer (Item /56) y usala para destruir la Masamune en Dead Sea',0,3,28),
	(117,'Ve al The Dead Sea, recoge todos los cofres',0,1,29),
	(118,'Habla con el fantasma y se le caera algo, recogelo y Obten el Station Pass (Item /56) con este item podra obtener Obten el Medical Book (Item /56)',0,2,29),
	(119,'Obten el Prop Sword (Item /56)',0,3,29),
	(120,'Lucha con Miguel, y la ciudad sera destruida (el Dragon del Cielo te salvara)',0,4,29),
	(121,'Viaja por el portal de la playa a (AW)',0,1,30),
	(122,'ve a Hydra Marshes y explora todo lucha con jefes obtendras una habilidad especial, mas adelante en una lagunilla obten el Life Sparkle (Item /56) y Busca el Good Backbone (Item /56) de Skelly en una cueva (por la zona este)',0,2,30),
	(123,'Ve al bar de Termina, hay una habitacion secreta, aqui podras reclutar a Zoah (Personaje 30/45) o Karsh (Personaje 31/45), siquieres obtener a los 2 deberas terminar el juego e iniciar (NG+) Pero si eliges a Karsh, Zoha se te unira en el S.S Zelbess tambien obtendras el Tear of Hate (Item /56)',0,3,30),
	(124,'Opcion 1: Consigue a Zoah',1,4,30),
	(125,'Opcion 2: Consigue a Karsh',1,5,30),
	(126,'Ve a Viper Manor, habla con Norris en la habitacion del oeste de la entrada y te dara la Prison Key (Item /56)',0,1,31),
	(127,'Si vas a la carcel, encontraras una la Parlor Key (Item /56)',0,2,31),
	(128,'Ve a las alcantarillas desde la cocina, encuentra y apaga la valvula',0,3,31),
	(129,'Vuelve a la primera seccion, ahora ya puedes cruzar, y apaga la segunda valvula',0,4,31),
	(130,'En la siguiente pantalla, sube la valvula y vuelve al otro lado',0,5,31),
	(131,'Bajala, procede, y lucharas contra un bicho gordo, procede subir las escaleras',0,6,31),
	(132,'Lucharas contra Dark Orcha para rescatar a Riddel, luego se te unira Orcha (Personaje 32/45)',0,7,31),
	(133,'Sigue recto hacia la biblioteca, lucharas con Grovyc y Mecha',0,8,31),
	(134,'Eapa hacia la biblioteca, lucha con Mecha otra vez, y escapa por la ventana',0,9,31),
	(135,'Grobyc (Personaje 33/45) se unira a tu grupo',0,10,31),
	(136,'Crea un guardado independiente de la partida principal, cuando termines el juego carga esta partida con el Continue+. Vencer al Time Devourer inmediatamente despues de rescatar a Riddel',1,1,32),
	(137,'Ve a Guldove y Lucha con Orlha y conseguiras su Sapphire Brooch (Item /56), devuelvesela cuando seas Serge de nuevo, y se te unira',0,1,33),
	(138,'Dale al Doc el libro para desbloquear su habilidad',0,2,33),
	(139,'Ve Water Dragon Isle y a Un tipo te dara el Sturdy Ribs (Item /56) de Skelly',0,3,33),
	(140,'Ve a Isle of the Damned, recoge los Mixed Bones (Item /56) de Skelly en la cueva al noroeste de la primera pantalla',0,4,33),
	(141,'Si ya tienes todas las 6 partes de Skelly (Heavy Skull, Pelvic Bone, Mixed Bones, Angry Scapula, Good Backbone, Sturdy Ribs) ve a Termina a la casa de la derecha de la estatua de Viper, y se te unira Skelly (Personaje 34/45) si hablas con la abuela',0,5,33),
	(142,'Habla con Riddel (Personaje 35/45) y se te unira',0,1,34),
	(143,'Se te uniran Viper (Personaje 36/45), Fargo (Personaje 37/45) y Mercy (Personaje 38/45)... (Zoah o Karsh)',0,2,34),
	(144,'Crea un guardado independiente de la partida principal, cuando termines el juego carga esta partida con el Continue+. Vencer al Time Devourer despues de que Harle abandona tu grupo',1,1,35),
	(145,'Hay 8 dragones, uno por cada elemento',0,1,36),
	(146,'Recuerda robar a los dragones (con Fargo), cada dragon tiene una armadura que bloquea dicho elemento',0,2,36),
	(147,'Son enemigos bastante basicos, usa magias de elementos opuestos',0,3,36),
	(148,'Personajes del elemento innato opuesto hacen y reciben el doble de dano',0,4,36),
	(149,'Personajes del mismo elemento que dicho dragon hacen y reciben la mitad de dano',0,5,36),
	(150,'Elementos del mismo color los curaran',0,6,36),
	(151,'No hay un orden especifico para derrotarlos',0,7,36),
	(152,'Para activar la mision de los dragones hay q ir a hablar con el dragon del cielo',0,8,36),
	(153,'Primero ve al S.S Zelbess (HW) lleva a Fargo a verse asimismo',0,1,37),
	(154,'Despues del concierto ve a Marbule y vence a todos los monstruos, cuando vuelvas podras reclutar a Miki (Personaje 29/45)',0,2,37),
	(155,'Ve a Hydra Marshes (HW) y consigue la Ancient Fruit (Item /56), si no te la da el bicho ve a (AW) al mismo lugar para que te la de, luego vuelve',0,3,37),
	(156,'En Hydra Marshes, usa la Beeba Flute cerca del segundo Beeba',0,1,38),
	(157,'El bicho Hydra te llevara a Gaeas Navel, aqui reclutas a Leah (Personaje 42/45)',0,2,38),
	(158,'Roba la armadura cuando este debilitado, al vencerlo te dara la Green Relic (Item /56)',0,3,38),
	(159,'Ve a la Isla del Dragon de Agua (Water Dragon Isle), esta al final del todo',0,1,39),
	(160,'Roba la armadura cuando este debilitado, al vencerlo te dara la Blue Relic (Item /56)',0,2,39),
	(161,'Ve a Cleared Earth Dragon Isle, entra por las arenas movedizas',0,1,40),
	(162,'Debes ir a la izquierda y abajo de la cueva, obten los Explosive (Item /56) del NPC, si el NPC no esta trata de mover a la tortuga, luego sale de la isla y vuelve a entrar',0,2,40),
	(163,'Debes matar y poner cada tortuga en los agujeros geiser con los explosivos',0,3,40),
	(164,'Salta dentro del nuevo agujero y lucha con el dragon',0,4,40),
	(165,'Roba la armadura cuando este debilitado, al vencerlo te dara la Yellow Relic (Item /56)',0,5,40),
	(166,'Ve al monte del Dragon de Fuego en Mount Pyre (AW)',0,1,41),
	(167,'Ve a la cascada de lava, baja a la primera cascada y entra por la puerta',0,2,41),
	(168,'Roba la armadura cuando este debilitado, al vencerlo te dara la Red Relic (Item /56)',0,3,41),
	(169,'Ve a Marbule (AW), el dragon estara en la cueva, habla con el NPC y te dara el Quill and Papyrus (Marco 15/15)',0,1,42),
	(170,'Si tienes a Pierre, no tendras que luchar con el para la Black Crest',0,2,42),
	(171,'Roba la armadura cuando este debilitado, al vencerlo te dara la Black Relic (Item /56)',0,3,42),
	(172,'Ve a Sky Dragon Isle y lucha con el dragon',0,1,43),
	(173,'Roba la armadura cuando este debilitado, al vencerlo te dara la White Relic (Item /56)',0,2,43),
	(174,'Esta Mision es opcional (No es Importante) debes ir a Earth Dragon Isle y... (a llorar a la playa) sino esperar a la partida (NG+)',1,1,44),
	(175,'Este dragon es el jefe mas dificil del juego, aun mas que el jefe final',0,2,44),
	(176,'El truco para vencerlo es responder correctamente sus acertijo o luchando a fuerza bruta',0,3,44),
	(177,'Si lograste robar la armadura con Fargo al dragon de tierra (Coraza Dorada) lo venceras en cuestion de tiempo',0,4,44),
	(178,'De lo contrario sera un jefe dificil, debes llevar muchas curas y trampas de elemento como Terremoto y Tormenta Electrica',0,5,44),
	(179,'Si lo vences te dara las Gafas de Sol, uno de los mejores articulos de euipamiento del juego',0,6,44),
	(180,'Muestra el cristal roto a Direa y Steena, y consigue el Dragon Emblem (Item /56)',0,1,45),
	(181,'Ensenaselo al guardia de la tienda de Steena en Guldove (HW) y ella te dara el Dragon Tear (Item /56)',0,2,45),
	(182,'Ahora Steena (Personaje 43/45) se te unira, Lleva a Steena a Fort Dragonia',0,3,45),
	(183,'Ve a Fort Dragonia',0,1,46),
	(184,'Pon el amuleto en la ranura de la entrada',0,2,46),
	(185,'Lucha contra Dark Serge en la sala del ascensor',0,3,46),
	(186,'Baja, pon el huevo gigante en el centro y y obtendras a Draggy (Personaje 44/45)',0,4,46),
	(187,'Sube a la habitacion principal y abre la puerta, aqui obtendras el Tear of Love (Item /56)',0,5,46),
	(188,'Esta mision es opcional y dificil, Necesitaras a Riddel y Karsh',0,1,47),
	(189,'Lucha con Solt y Peppor en Isle of the Damned (AW) y obtendras el Memento Pendant (Item /56)',0,2,47),
	(190,'Ve a la isla escondida al noreste de El Nido (HW)',0,3,47),
	(191,'Usa el pendiente y esto hara que luches con Dario (a llorar a la playa)',0,4,47),
	(192,'La Masamune se transformara en la la Mastermune, la mejor arma de Serge',0,5,47),
	(193,'Pon a Glenn en posicion principal y llevalo a la tumba de Dario',0,6,47),
	(194,'Ve a Termina y procura de que Glen NO tenga equipada la Einlanzer, ve a la tumba familiar de Glen y obten la Einlanzer doble',0,7,47),
	(195,'Crea un guardado independiente de la partida principal, cuando termines el juego carga esta partida con el Continue+. Vencer al Time Devourer despues de vencer a Dario y antes de que aparezca Terra Tower',1,1,48),
	(196,'Si quieres reclutar a Orlha (Personaje 45/45), ve a Guldove (AW) y dale el Sapphire Brooch',0,1,49),
	(197,'Si tienes a Glenn, ahora le puedes equipar la Einlanzer',0,2,49),
	(198,'Lleva a Steena al Sea of Eden y entra en uno de los agujeros',0,3,49),
	(199,'Ve a cada una de las islas',0,4,49),
	(200,'En la tercera tendrias que luchar contra los Destinos',0,5,49),
	(201,'Se vienen los Mecha',0,1,50),
	(202,'Encuentra la entrada a la sala de ordenadores y ve a la base',0,2,50),
	(203,'Usa el control remoto robot para accionar el puente',0,3,50),
	(204,'En la habitacion principal, ve a la derecha hacia la habitacion de las escaleras',0,4,50),
	(205,'Hay un objeto importante para Grobyc en el cofre del fondo de todo',0,5,50),
	(206,'En el segundo piso hay una habitacion donde puedes activar el elevador',0,6,50),
	(207,'En el tercero y cuarto piso, observalo todo, pero no puedes entrar en una habitacion',0,7,50),
	(208,'En el primer piso, en el muelle, habla con el fantasma azul',0,8,50),
	(209,'Ve a la habitacion donde no pudiste entrar y coge la Card Key (Item /56)',0,9,50),
	(210,'Ahora que el Septimo dragon esta aqui, el septimo elemento es activado',0,10,50),
	(211,'Ve al Divine Dragon Falls que estan justo al este del Fossil Valley',0,1,51),
	(212,'Usa los 2 fragmentos de cristal en los pilares, el Tear of Hate y el Tear of Love',0,2,51),
	(213,'Ahora conseguiras el Chrono Cross (Item /56), y el elemento. Ponla en la primera ranura de Serge',0,3,51),
	(214,'Presta atencion en las bolas de colores que aparecen ahora en batalla, son importantes',0,4,51),
	(215,'Ve a la casa de casa de Radius en Hermits Hideaway (HW) con Poshul en tu equipo',0,1,52),
	(216,'Podras reclutar a Turnip (Personaje 39/45) para esto debes haber usa el Ice Breath o la Ice Gun en el suelo en Hermits Hideaway (AW)',0,2,52),
	(217,'Ve a la pileta de Viper Manor (AW) con el Life Sparkle y obtendras a Neofio (Personaje 41/45)',0,3,52),
	(218,'Ve con Viber en tu equipo podras conseguir los cofres de la sala secreta, despues de leer la carta en la oficina secreta',0,4,52),
	(219,'Para entrar a la oficina secreta, debes mover la cabeza de la estatua de la izquierda',0,5,52),
	(220,'Ve a Gaeas Navel (AW) vence al cavernicola del arbol y consigues el Snakes & Orbs (Marco 14/15)',0,6,52),
	(221,'Ve a Hermits Hideaway, veras a Kid',0,1,53),
	(222,'Si tienes la Mastermune, entraras en un sueno',0,2,53),
	(223,'En casa de Lucca, busca por ahi y hay una puerta trampilla en la habitacion de arriba derecha',0,3,53),
	(224,'En la primera habitacion, pulsa L2, Triangulo, R2, y Triangulo en la maquina',0,4,53),
	(225,'Obtendras el Ataque de nivel 7 de Kid',0,5,53),
	(226,'Necesitaras la Ice gun (Item /56) de Lucca para apagar las llamas',0,6,53),
	(227,'Rescata a todos los ninos hasta que alcances a Kid y el sueno se acabara',0,7,53),
	(228,'Ve con Starky, y lucha con el pez gigante en el fondo del mar',0,1,54),
	(229,'El Pez gigante es solo debil al fuego, no a los ataques fisicos',0,2,54),
	(230,'Ahora vuela con el bote hacia Dinopolis',0,3,54),
	(231,'Crea un guardado independiente de la partida principal, cuando termines el juego carga esta partida con el Continue+. Vencer al Time Devourer antes de terminar Terra Tower',1,1,55),
	(232,'Venciendo a los seis guardianes elementales',0,1,56),
	(233,'Se recomienda usar elementos trap para robar las invocaciones',0,2,56),
	(234,'Al final, te encontraras al Dragon Lunar',0,3,56),
	(235,'Atiende al orden de colores a los que cambia, y anotalos XD, igual los anote por ti',0,4,56),
	(236,'Si lo vences, la Llama Congelada se perdera en una corriente temporal',0,5,56),
	(237,'Obtendras el Time Egg (Item /56), este item lo obtendras en la partida NG+',0,6,56),
	(238,'Ve a la playa que gatillo todos los eventos de tu historia',0,1,57),
	(239,'Habla con los Chrono, Lucca y Marle',0,2,57),
	(240,'Aca podras sacar 2 finales Distintos uno bueno y el otro malo, crea un guardado independiente de la partida principal, cuando termines el juego NO guardes la partida nueva, sino que carga esta partida de forma normal para sacar el otro final.',0,3,57),
	(241,'Usa el Time Egg encima del agujero de gusano lila para la batalla final y al terminar el juego recuperaras el Time Egg en el (NG+)',0,4,57),
	(242,'Salva a Schala del Time Devourer tocando el Chrono Cross',0,1,58),
	(243,'Para sto debes crear la secuencia de colores que realizo el Dragon Lunar',0,2,58),
	(244,'Lanza elementos en este orden: Amarillo, Rojo, Verde, Azul, Negro, Blanco, y Chrono Cross',1,3,58),
	(245,'Ayuda si esperas a que todos puedan usar hasta la octava fila',0,4,58),
	(246,'Usa magias de curacion de modo que el Time Devourer no lo cuente ni interrumpa tu orden de bolas',0,5,58),
	(247,'Mata al Time Devourer junto a Schala! Sin usar la secuencia de colores',1,1,59),
	(248,'Bend of Time, es un area oculta que solo se puede acceder en una partida NG+, aqui puedes luchar encontra Ozzie, Slash y Flea, personajes de Chrono Trigger, los puedes capturar para el Dopellgang de Sprigg',1,1,60),
	(249,'Los 10 portales del Bend of Time son los siguientes. 1: Lizard Rock. 2: Viper Manor, Viper Manor Bluffs. 3: Water Dragon Isle, Hydra Marshes, S. S. Invincible. 4: Fort Dragonia, Temporal Vortex. 5: Shadow Forest, Fossil Valley, 6: Mt. Pyre, Isle of the Damned. 7: Dead Sea, 8: Marbule, Gaeas Navel, Earth Dragon Isle. 9: Chronopolis, Sea of Eden. 10: Terra Tower, Luccas House.',1,2,60),
	(250,'Para obtener a los 46 personajes se debe recorrer el juego al amenos 3 veces seguida usando el NG+ y usar el Chrono Cross en la casa de Sprigg',1,3,60),
	(251,'Con el Chrono Cross puedes recuperar todos los personajes de las partidas anteriores PERO, para poder considerar a Harle, debes terminar el juego con Lynx y Harle e iniciar el NG+',1,4,60),
	(252,'Para obtener Guile, Nikki y Pierre se debe hacer en 3 pasadas, no se puede conseguir en una partida',1,5,60),
	(253,'Para obtener Korcha, Mel ,Razzly o Macha, Glen, Doc se debe hacer en 2 pasadas, no se puede conseguir en una partida',1,6,60),
	(254,'Leena solo se puede obtener rechazando a Kid 3 veces en Cape Howl',1,7,60),
	(255,'Arma: Corsesca - 30 (Vendedor de Gaus)',1,1,61),
	(256,'Material: (1) Sensor Laser',0,2,61),
	(257,'Material: (1) Cuerno de Dragon',0,3,61),
	(258,'Material: (1) Lagrima de Begimo, Habilidad: Rompe Defensas X',0,4,61),
	(259,'Material: (2) Cuerno Hueco Curvo',0,5,61),
	(260,'Arma: Corsesca II - 60',1,6,61),
	(261,'Material: (8) Sensor Laser',0,7,61),
	(262,'Material: (3) Cuerno Hueco Curvo',0,8,61),
	(263,'Arma: Bigote de Dragon - 99',1,9,61),
	(264,'Material: (8) Cuerno Hueco Curvo',0,10,61),
	(265,'Arma: Bigote de Dragon - 120',1,11,61),
	(266,'Material: (2) Meteoritos',0,12,61),
	(267,'Material: (3) Cuerno Hueco Curvo',0,13,61),
	(268,'Material: (1) Cuerno de Begimo',0,14,61),
	(269,'Material: (2) Joya de Fuerza',0,15,61),
	(270,'Arma: Corsesca - 30 (Vendedor de Gaus)',1,1,62),
	(271,'Material: (1) Sensor Laser',0,2,62),
	(272,'Material: (1) Cuerno de Dragon',0,3,62),
	(273,'Material: (1) Perone Escarlata, Habilidad: Potenciador de Trachyon II',0,4,62),
	(274,'Material: (2) Cuerno Hueco Curvo',0,5,62),
	(275,'Arma: Corsesca II - 60',1,6,62),
	(276,'Material: (4) Fragmento igneo',0,7,62),
	(277,'Material: (3) Fragmento Gelido',0,8,62),
	(278,'Material: (4) Fragmento de Tormenta',0,9,62),
	(279,'Material: (10) Sensor Laser',0,10,62),
	(280,'Arma: Bigote de Dragon - 99',1,11,62),
	(281,'Material: (21) Sensor Laser',0,12,62),
	(282,'Arma: Bigote de Dragon - 120',1,13,62),
	(283,'Material: (14) Sensor Laser',0,14,62),
	(284,'Material: (1) Joya de Fuerza',0,15,62),
	(285,'Arma: Garrote Mancillado - 50 (Mision Burgos Infestados)',1,1,63),
	(286,'Material: (8) Cuerno de Dragon',0,2,63),
	(287,'Arma: Arma: Mjolnir - 99',1,3,63),
	(288,'Material: (11) Cuerno de Dragon',0,4,63),
	(289,'Arma: Mjolnir - 120',1,5,63),
	(290,'Material: (2) Meteoritos',0,6,63),
	(291,'Material: (3) Pico de Ze',0,7,63),
	(292,'Material: (1) Mandibula de Monstruo',0,8,63),
	(293,'Material: (1) Fragmento de Entereza',0,9,63),
	(294,'Material: (2) Cuerno de Dragon',0,10,63),
	(295,'Arma: Huerfana - 30 (Vendedor de Gaus)',1,1,64),
	(296,'Material: (4) Cuerno de Dragon',0,2,64),
	(297,'Arma: Sennin-Giri - 60',1,3,64),
	(298,'Material: (10) Octopedo',0,4,64),
	(299,'Material: (3) Cuerno de Dragon',0,5,64),
	(300,'Arma: Mumeito - 99',1,6,64),
	(301,'Material: (8) Cuerno de Dragon',0,7,64),
	(302,'Arma: Mumeito - 120',1,8,64),
	(303,'Material: (2) Meteoritos',0,9,64),
	(304,'Material: (4) Cuerno de Dragon',0,10,64),
	(305,'Material: (1) Perone Escarlata',0,11,64),
	(306,'Material: (1) Mandibula de Monstruo',0,12,64),
	(307,'Material: (1) Garra Retorcida, Habilidad: Hielo++',0,13,64),
	(308,'Arma: Huerfana - 30 (Vendedor de Gaus)',1,1,65),
	(309,'Material: (6) Aguijon Venenoso',0,2,65),
	(310,'Material: (6) Mandibula de Monstruo',1,3,65),
	(311,'Arma: Sennin-Giri - 60',0,4,65),
	(312,'Material: (2) Piel Endurecida',0,5,65),
	(313,'Material: (5) Cuerno Hueco Curvo',1,6,65),
	(314,'Arma: Mumeito - 99',0,7,65),
	(315,'Material: (20) Sensor Laser',1,8,65),
	(316,'Material: (1) Cilindro Hidraulico, Habilidad: Potenciador de Trachyon X',0,9,65),
	(317,'Arma: Mumeito - 120',0,10,65),
	(318,'Material: (10) Sensor Laser',0,11,65),
	(319,'Material: (1) Tela Antigua',0,12,65),
	(320,'Material: (1) Joya de Fuerza',0,13,65),
	(321,'Arma: Disco Oxidado - 50 (Coliseo Begimo Extremo)',1,1,66),
	(322,'Material: (3) Cuerno de Dragon',0,2,66),
	(323,'Material: (5) Pico de Ze',0,3,66),
	(324,'Arma: Escudo Sangriento - 99',1,4,66),
	(325,'Material: (4) Cuerno de Dragon',0,5,66),
	(326,'Material: (6) Fragmento igneo',0,6,66),
	(327,'Material: (1) Joya de la Escarcha',0,7,66),
	(328,'Material: (3) Fragmento Gelido',0,8,66),
	(329,'Material: (6) Fragmento de Tormenta',0,9,66),
	(330,'Material: (2) Gema de Oscuridad',0,10,66),
	(331,'Material: (2) Armadura de Placa',0,11,66),
	(332,'Material: (1) Placa Resistente',0,12,66),
	(333,'Material: (3) Pico de Ze',0,13,66),
	(334,'Arma: Egida - 120',1,14,66),
	(335,'Material: (2) Meteoritos',0,15,66),
	(336,'Material: (6) Pico de Ze',0,16,66),
	(337,'SET (Mixto)',1,1,67),
	(338,'Katana - Mumeito (1)',0,2,67),
	(339,'Mazo - Mjolnir',0,3,67),
	(340,'Lanza - Bigote de Dragon (1)',0,4,67),
	(341,'Escudo - Egida',0,5,67),
	(342,'SET (Fisico)',1,6,67),
	(343,'Katana - Mumeito (2)',0,7,67),
	(344,'Lanza - Bigote de Dragon (2)',0,8,67),
	(345,'Lanza - Bigote de Dragon (1)',0,9,67),
	(346,'Escudo - Egida',0,10,67),
	(347,'Boatman Coin #1',1,1,68),
	(348,'Boatman Coin #2',1,2,68),
	(349,'Boatman Coin #3',1,3,68),
	(350,'Boatman Coin #4',1,4,68),
	(351,'Boatman Coin #6',1,5,68),
	(352,'Boatman Coin #7',1,6,68),
	(353,'Boatman Coin #8',1,7,68),
	(354,'Boatman Coin #14',1,8,68),
	(355,'Boatman Coin #15',1,9,68),
	(356,'Boatman Coin #16',1,10,68),
	(357,'Boatman Coin #17',1,11,68),
	(358,'Boatman Coin #18',1,12,68),
	(359,'Boatman Coin #19',1,13,68),
	(360,'Boatman Coin #20',1,14,68),
	(361,'Boatman Coin #41',1,15,68),
	(362,'Boatman Coin #42',1,16,68),
	(363,'Boatman Coin #43',1,17,68),
	(364,'Book of the Dead Page #1',1,18,68),
	(365,'Book of the Dead Page #2',1,19,68),
	(366,'Book of the Dead Page #22',1,20,68),
	(367,'Book of the Dead Page #35',1,21,68),
	(368,'Stone of Power #1',1,22,68),
	(369,'Stone of Power #2',1,23,68),
	(370,'Stone of Power #5',1,24,68),
	(371,'Stone of Mystic #1',1,25,68),
	(372,'Stone of Mystic #2',1,26,68),
	(373,'Stone of Mystic #3',1,27,68),
	(374,'Stone of Mystic #5',1,28,68),
	(375,'Stone of Mystic #6',1,29,68),
	(376,'Stone of Mystic #7',1,30,68),
	(377,'Stone of Mystic #8',1,31,68),
	(378,'Stone of Mystic #9',1,32,68),
	(379,'Stone of Mystic #10',1,33,68),
	(380,'Stone of Mystic #11',1,34,68),
	(381,'Stone of Mystic #15',1,35,68),
	(382,'Stone of Resistance #9',1,36,68),
	(383,'Boatman Coin #9',1,1,69),
	(384,'Boatman Coin #10',1,2,69),
	(385,'Boatman Coin #11',1,3,69),
	(386,'Boatman Coin #12',1,4,69),
	(387,'Boatman Coin #13',1,5,69),
	(388,'Book of the Dead Page #3',1,6,69),
	(389,'Book of the Dead Page #4',1,7,69),
	(390,'Stone of Mystic #12',1,1,70),
	(391,'Book of the Dead Page #5',1,2,70),
	(392,'Book of the Dead Page #6',1,3,70),
	(393,'Boatman Coin #21',1,4,70),
	(394,'Boatman Coin #22',1,5,70),
	(395,'Boatman Coin #23',1,6,70),
	(396,'Boatman Coin #24',1,7,70),
	(397,'Boatman Coin #25',1,8,70),
	(398,'Stone of Resistance #1',1,1,71),
	(399,'Book of the Dead Page #10',1,2,71),
	(400,'Boatman Coin #35',1,3,71),
	(401,'Boatman Coin #36',1,4,71),
	(402,'Boatman Coin #37',1,5,71),
	(403,'Boatman Coin #38',1,6,71),
	(404,'Boatman Coin #39',1,7,71),
	(405,'Stone of Power #3',1,1,72),
	(406,'Stone of Mystic #14',1,2,72),
	(407,'Book of the Dead Page #8',1,3,72),
	(408,'Book of the Dead Page #9',1,4,72),
	(409,'Boatman Coin #29',1,5,72),
	(410,'Boatman Coin #30',1,6,72),
	(411,'Boatman Coin #31',1,7,72),
	(412,'Boatman Coin #32',1,8,72),
	(413,'Boatman Coin #33',1,9,72),
	(414,'Stone of Mystic #13',1,1,73),
	(415,'Book of the Dead Page #11',1,2,73),
	(416,'Boatman Coin #5',1,3,73),
	(417,'Boatman Coin #27',1,4,73),
	(418,'Boatman Coin #28',1,5,73),
	(419,'Boatman Coin #40',1,6,73),
	(420,'Boatman Coin #44',1,1,74),
	(421,'Book of the Dead Page #13',1,2,74),
	(422,'Boatman Coin #26',1,1,75),
	(423,'Book of the Dead Page #7',1,2,75),
	(424,'Boatman Coin #94',1,1,76),
	(425,'Book of the Dead Page #12',1,2,76),
	(426,'Stone of Power #4',1,3,76),
	(427,'GnoMADs Gnome #3',1,4,76),
	(428,'Boatman Coin #34',1,1,77),
	(429,'Stone of Mystic #4',1,2,77),
	(430,'Boatman Coin #49',1,1,78),
	(431,'Boatman Coin #50',1,2,78),
	(432,'Boatman Coin #51',1,3,78),
	(433,'Boatman Coin #56',1,4,78),
	(434,'Boatman Coin #57',1,5,78),
	(435,'Boatman Coin #62',1,6,78),
	(436,'Boatman Coin #63',1,7,78),
	(437,'Boatman Coin #64',1,8,78),
	(438,'Boatman Coin #100',1,9,78),
	(439,'Relic Renagoth #2',1,10,78),
	(440,'Relic Renagoth #3',1,11,78),
	(441,'Relic Etu-Goth #1',1,12,78),
	(442,'Relic Etu-Goth #7',1,13,78),
	(443,'Relic Khagoth #1',1,14,78),
	(444,'Stone of Power #7',1,15,78),
	(445,'Stone of Power #10',1,16,78),
	(446,'Stone of Power #11',1,17,78),
	(447,'Stone of Power #25',1,18,78),
	(448,'Book of the Dead Page #14',1,19,78),
	(449,'Book of the Dead Page #18',1,20,78),
	(450,'Stone of Mystic #16',1,21,78),
	(451,'Stone of Mystic #20',1,22,78),
	(452,'Stone of Mystic #34',1,23,78),
	(453,'Soul Arbiters Scroll #8',1,24,78),
	(454,'Soul Arbiters Scroll #20',1,25,78),
	(455,'Soul Arbiters Scroll #5',1,26,78),
	(456,'Stone of Resistance #4',1,27,78),
	(457,'Boatman Coin #68',1,1,79),
	(458,'Boatman Coin #69',1,2,79),
	(459,'Stone of Mystic #24',1,3,79),
	(460,'Soul Arbiters Scroll #11',1,4,79),
	(461,'Soul Arbiters Scroll #12',1,5,79),
	(462,'Boatman Coin #47',1,1,80),
	(463,'Boatman Coin #48',1,2,80),
	(464,'Stone of Resistance #3',1,3,80),
	(465,'Boatman Coin #70',1,1,81),
	(466,'Boatman Coin #71',1,2,81),
	(467,'Boatman Coin #72',1,3,81),
	(468,'Boatman Coin #73',1,4,81),
	(469,'Book of the Dead Page #23',1,5,81),
	(470,'Book of the Dead Page #24',1,6,81),
	(471,'Book of the Dead Page #25',1,7,81),
	(472,'Book of the Dead Page #26',1,8,81),
	(473,'Stone of Power #14',1,9,81),
	(474,'Stone of Resistance #5',1,10,81),
	(475,'GnoMADs Gnome #1',1,11,81),
	(476,'Soul Arbiters Scroll #13',1,12,81),
	(477,'Soul Arbiters Scroll #14',1,13,81),
	(478,'Relic Etu-Goth #10',1,14,81),
	(479,'Relic Renagoth #5',1,15,81),
	(480,'Boatman Coin #52',1,1,82),
	(481,'Stone of Resistance #2',1,2,82),
	(482,'Relic Etu-Goth #4',1,3,82),
	(483,'Boatman Coin #53',1,1,83),
	(484,'Boatman Coin #54',1,2,83),
	(485,'Boatman Coin #55',1,3,83),
	(486,'Book of the Dead Page #15',1,4,83),
	(487,'Stone of Mystic #17',1,5,83),
	(488,'Stone of Mystic #18',1,6,83),
	(489,'Stone of Mystic #19',1,7,83),
	(490,'Stone of Power #8',1,8,83),
	(491,'Soul Arbiters Scroll #3',1,9,83),
	(492,'Soul Arbiters Scroll #4',1,10,83),
	(493,'Relic Renagoth #1',1,11,83),
	(494,'Relic Etu-Goth #3',1,12,83),
	(495,'Boatman Coin #65',1,1,84),
	(496,'Boatman Coin #66',1,2,84),
	(497,'Boatman Coin #67',1,3,84),
	(498,'Book of the Dead Page #19',1,4,84),
	(499,'Book of the Dead Page #20',1,5,84),
	(500,'Book of the Dead Page #21',1,6,84),
	(501,'Stone of Mystic #22',1,7,84),
	(502,'Stone of Power #12',1,8,84),
	(503,'Soul Arbiters Scroll #9',1,9,84),
	(504,'Soul Arbiters Scroll #10',1,10,84),
	(505,'Relic Etu-Goth #8',1,11,84),
	(506,'Relic Khagoth #2',1,12,84),
	(507,'Boatman Coin #45',1,1,85),
	(508,'Boatman Coin #46',1,2,85),
	(509,'Stone of Power #6',1,3,85),
	(510,'Relic Etu-Goth #2',1,4,85),
	(511,'Soul Arbiters Scroll #1',1,5,85),
	(512,'Soul Arbiters Scroll #2',1,6,85),
	(513,'Boatman Coin #59',1,1,86),
	(514,'Boatman Coin #60',1,2,86),
	(515,'Boatman Coin #61',1,3,86),
	(516,'Stone of Power #9',1,4,86),
	(517,'Relic Etu-Goth #6',1,5,86),
	(518,'Book of the Dead Page #16',1,6,86),
	(519,'Book of the Dead Page #17',1,7,86),
	(520,'Stone of Mystic #21',1,8,86),
	(521,'Soul Arbiters Scroll #6',1,9,86),
	(522,'Soul Arbiters Scroll #7',1,10,86),
	(523,'Stone of Mystic #23',1,1,87),
	(524,'Relic Etu-Goth #9',1,2,87),
	(525,'Relic Renagoth #4',1,3,87),
	(526,'Boatman Coin #58',1,1,88),
	(527,'Stone of Power #13',1,2,88),
	(528,'Relic Etu-Goth #5',1,3,88),
	(529,'Relic Renagoth #6',1,1,89),
	(530,'Relic Etu-Goth #11',1,2,89),
	(531,'Book of the Dead Page #27',1,3,89),
	(532,'Stone of Power #15',1,4,89),
	(533,'Stone of Mystic #25',1,5,89),
	(534,'Stone of Mystic #26',1,6,89),
	(535,'Stone of Mystic #27',1,7,89),
	(536,'Boatman Coin #74',1,8,89),
	(537,'Boatman Coin #75',1,9,89),
	(538,'Boatman Coin #76',1,10,89),
	(539,'Boatman Coin #84',1,11,89),
	(540,'Boatman Coin #85',1,12,89),
	(541,'Boatman Coin #77',1,1,90),
	(542,'Boatman Coin #78',1,2,90),
	(543,'Boatman Coin #79',1,3,90),
	(544,'Boatman Coin #80',1,4,90),
	(545,'Boatman Coin #81',1,5,90),
	(546,'Boatman Coin #82',1,6,90),
	(547,'Boatman Coin #83',1,7,90),
	(548,'Book of the Dead Page #28',1,8,90),
	(549,'Book of the Dead Page #29',1,9,90),
	(550,'Book of the Dead Page #30',1,10,90),
	(551,'Book of the Dead Page #31',1,11,90),
	(552,'Book of the Dead Page #32',1,12,90),
	(553,'Stone of Mystic #28',1,13,90),
	(554,'Stone of Mystic #29',1,14,90),
	(555,'Stone of Mystic #30',1,15,90),
	(556,'Stone of Mystic #31',1,16,90),
	(557,'Stone of Power #16',1,17,90),
	(558,'Stone of Power #17',1,18,90),
	(559,'Stone of Resistance #6',1,19,90),
	(560,'Soul Arbiters Scroll #15',1,20,90),
	(561,'Soul Arbiters Scroll #16',1,21,90),
	(562,'Relic Etu-Goth #12',1,22,90),
	(563,'Relic Renagoth #7',1,23,90),
	(564,'Relic Renagoth #8',1,24,90),
	(565,'Trapped Hellguard #1',1,25,90),
	(566,'Trapped Hellguard #2',1,26,90),
	(567,'Trapped Hellguard #3',1,27,90),
	(568,'Trapped Hellguard #4',1,28,90),
	(569,'Trapped Hellguard #5',1,29,90),
	(570,'Trapped Hellguard #6',1,30,90),
	(571,'Trapped Hellguard #7',1,31,90),
	(572,'Trapped Hellguard #8',1,32,90),
	(573,'Trapped Hellguard #9',1,33,90),
	(574,'Trapped Hellguard #10',1,34,90),
	(575,'Boatman Coin #86',1,1,91),
	(576,'Boatman Coin #87',1,2,91),
	(577,'Boatman Coin #88',1,3,91),
	(578,'Boatman Coin #89',1,4,91),
	(579,'Boatman Coin #90',1,5,91),
	(580,'Boatman Coin #91',1,6,91),
	(581,'Boatman Coin #92',1,7,91),
	(582,'Boatman Coin #93',1,8,91),
	(583,'Book of the Dead Page #33',1,9,91),
	(584,'Book of the Dead Page #34',1,10,91),
	(585,'Stone of Mystic #32',1,11,91),
	(586,'Stone of Mystic #33',1,12,91),
	(587,'Stone of Power #18',1,13,91),
	(588,'Stone of Power #19',1,14,91),
	(589,'Stone of Resistance #7',1,15,91),
	(590,'Stone of Resistance #8',1,16,91),
	(591,'Soul Arbiters Scroll #17',1,17,91),
	(592,'Relic Etu-Goth #13',1,18,91),
	(593,'Relic Etu-Goth #14',1,19,91),
	(594,'Relic Khagoth #3',1,20,91),
	(595,'GnoMADs Gnome #2',1,21,91),
	(596,'Relic Khagoth #4',1,1,92),
	(597,'Relic Khagoth #5',1,2,92),
	(598,'Book of the Dead Page #36',1,3,92),
	(599,'Stone of Power #23',1,4,92),
	(600,'Stone of Power #24',1,5,92),
	(601,'Boatman Coin #95',1,1,93),
	(602,'Boatman Coin #96',1,2,93),
	(603,'Boatman Coin #97',1,3,93),
	(604,'Boatman Coin #98',1,4,93),
	(605,'Boatman Coin #99',1,5,93),
	(606,'Book of the Dead Page #37',1,6,93),
	(607,'Book of the Dead Page #38',1,7,93),
	(608,'Book of the Dead Page #39',1,8,93),
	(609,'Book of the Dead Page #40',1,9,93),
	(610,'Stone of Power #20',1,10,93),
	(611,'Stone of Power #21',1,11,93),
	(612,'Stone of Power #22',1,12,93),
	(613,'Stone of Resistance #10',1,13,93),
	(614,'GnoMADs Gnome #4',1,14,93),
	(615,'Soul Arbiters Scroll #18',1,15,93),
	(616,'Soul Arbiters Scroll #19',1,16,93),
	(617,'Relic Etu-Goth #15',1,17,93),
	(618,'(F) La decimoquinta Guerra de Lindblum fue en 1600',1,1,94),
	(619,'(V) El bosque de los Chocobos esta entre Lindblum y la Puerta Sur',1,2,94),
	(620,'(F) El barco teatro Prima Vista fue hecho por los Astilleros Aubert',1,3,94),
	(621,'(F) Quiero ser tu canario es una obra de Lord Eibon',1,4,94),
	(622,'(V) Los taxis neumaticos de Lindblum funcionan las 24 horas del dia',1,5,94),
	(623,'(V) Existen Mu amistosos que no atacan',1,6,94),
	(624,'(V) El Castillo de Lindblum es mas grande que el de Alexandria',1,7,94),
	(625,'(F) El Pasaje de los Fosiles une Treno y Alexandria',1,8,94),
	(626,'(V) El barco teatro Prima Vista utiliza un motor a niebla',1,9,94),
	(627,'(V) El carro de hierro Berkmea fue inaugurado hace 8 anos',1,10,94),
	(628,'(F) Conde Petie es un pueblo de duendes',1,11,94),
	(629,'(F) Se dice que el popodori es un ave que trae riqueza',1,12,94),
	(630,'(V) El Cafe Bella Carta de Treno es un local privado',1,13,94),
	(631,'(F) Prima Vista significa el estreno de una obra',1,14,94),
	(632,'(F) En el mundo solo hay un desierto',1,15,94),
	(633,'(V) Es posible derrotarme a mi, el Ragtime',1,16,94),
	(634,'(V) La Guerra de Lindblum comenzo en 1500',1,17,94)
GO
SET IDENTITY_INSERT GG_Adventures OFF
GO

GO
SET IDENTITY_INSERT GG_AdventureImgs ON
GO
INSERT INTO GG_AdventureImgs
	(AdventureImg_Id, ImgUrl, Sort, Adventure_Id)
VALUES
	(1,'frame_cc_arnian_wood_1216x855.webp',1,5),
	(2,'frame_cc_simple_line_1216x855.webp',2,5),
	(3,'frame_cc_iron_plate_1216x855.webp',3,5),
	(4,'final_cc_01_1536x1080.webp',1,6),
	(5,'adventure_cc_03_01_1536x1080.webp',1,10),
	(6,'frame_cc_shellfish_1216x855.webp',2,10),
	(7,'adventure_cc_03_02_1536x1080.webp',1,11),
	(8,'adventure_cc_03_03_1536x1080.webp',2,11),
	(9,'item_cc_komodo_scale_1536x1080.webp',3,11),
	(10,'adventure_cc_03_04_1536x1080.webp',4,11),
	(11,'adventure_cc_03_05_1536x1080.webp',1,12),
	(12,'item_cc_shark_tooth_1536x1080.webp',2,12),
	(13,'adventure_cc_03_06_1536x1080.webp',1,13),
	(14,'item_cc_heckran_1536x1080.webp',2,13),
	(15,'adventure_cc_03_07_1536x1080.webp',1,14),
	(16,'adventure_cc_04_01_1536x1080.webp',1,18),
	(17,'adventure_cc_04_02_1536x1080.webp',2,18),
	(18,'adventure_cc_04_03_1536x1080.webp',3,18),
	(19,'adventure_cc_05_01_1536x1080.webp',1,22),
	(20,'adventure_cc_05_02_1536x1080.webp',2,22),
	(21,'adventure_cc_05_03_1536x1080.webp',1,23),
	(22,'adventure_cc_05_04_1536x1080.webp',2,23),
	(23,'adventure_cc_05_05_1536x1080.webp',3,23),
	(24,'adventure_cc_05_06_1536x1080.webp',1,24),
	(25,'adventure_cc_05_07_1536x1080.webp',2,24),
	(26,'adventure_cc_05_08_1536x1080.webp',1,25),
	(27,'adventure_cc_05_09_1536x1080.webp',2,25),
	(28,'adventure_cc_06_01_1536x1080.webp',1,26),
	(29,'item_cc_safety_gear_1536x1080.webp',2,26),
	(30,'adventure_cc_07_01_1536x1080.webp',1,28),
	(31,'adventure_cc_07_02_1536x1080.webp',2,28),
	(32,'item_cc_heavy_skull_1536x1080.webp',3,28),
	(33,'adventure_cc_07_03_1536x1080.webp',1,29),
	(34,'adventure_cc_07_04_1536x1080.webp',2,29),
	(35,'item_cc_bellflower_1536x1080.webp',3,29),
	(36,'adventure_cc_07_05_1536x1080.webp',1,30),
	(37,'item_cc_big_egg_1536x1080.webp',2,30),
	(38,'adventure_cc_08_01_1536x1080.webp',1,32),
	(39,'adventure_cc_08_02_1536x1080.webp',2,32),
	(40,'adventure_cc_08_03_1536x1080.webp',3,32),
	(41,'item_cc_tele_porterr_1536x1080.webp',4,32),
	(42,'adventure_cc_08_04_1536x1080.webp',1,33),
	(43,'frame_cc_tea_for_three_1216x855.webp',2,33),
	(44,'adventure_cc_08_05_1536x1080.webp',1,34),
	(45,'adventure_cc_08_06_1536x1080.webp',2,34),
	(46,'adventure_cc_08_07_1536x1080.webp',1,36),
	(47,'adventure_cc_08_08_1536x1080.webp',2,36),
	(48,'adventure_cc_08_09_1536x1080.webp',3,36),
	(49,'adventure_cc_08_10_1536x1080.webp',1,37),
	(50,'adventure_cc_08_11_1536x1080.webp',2,37),
	(51,'item_cc_heros_medalr_1536x1080.webp',3,37),
	(52,'adventure_cc_08_12_1536x1080.webp',4,37),
	(53,'adventure_cc_08_13_1536x1080.webp',5,37),
	(54,'adventure_cc_08_14_1536x1080.webp',1,38),
	(55,'adventure_cc_08_15_1536x1080.webp',2,38),
	(56,'adventure_cc_08_16_1536x1080.webp',3,38),
	(57,'adventure_cc_08_17_1536x1080.webp',4,38),
	(58,'item_cc_angry_scapular_1536x1080.webp',5,38),
	(59,'adventure_cc_08_18_1536x1080.webp',6,38),
	(60,'item_cc_aroma_pouchr_1536x1080.webp',7,38),
	(61,'frame_cc_skullduggery_1216x855.webp',8,38),
	(62,'adventure_cc_08_19_1536x1080.webp',9,38),
	(63,'adventure_cc_08_20_1536x1080.webp',10,38),
	(64,'final_cc_02_1536x1080.webp',1,39),
	(65,'adventure_cc_10_01_1536x1080.webp',1,41),
	(66,'adventure_cc_10_02_1536x1080.webp',2,41),
	(67,'item_cc_manor_key_1536x1080.webp',3,41),
	(68,'item_cc_time_shifter_1536x1080.webp',1,42),
	(69,'adventure_cc_11_01_1536x1080.webp',1,43),
	(70,'adventure_cc_11_02_1536x1080.webp',1,45),
	(71,'adventure_cc_11_03_1536x1080.webp',2,45),
	(72,'adventure_cc_11_04_1536x1080.webp',1,48),
	(73,'adventure_cc_11_05_1536x1080.webp',2,48),
	(74,'adventure_cc_11_06_1536x1080.webp',3,48),
	(75,'adventure_cc_11_08_1536x1080.webp',4,48),
	(76,'adventure_cc_11_08_1536x1080.webp',1,49),
	(77,'item_cc_decor_shield_1536x1080.webp',2,49),
	(78,'adventure_cc_11_09_1536x1080.webp',3,49),
	(79,'adventure_cc_11_10_1536x1080.webp',4,49),
	(80,'adventure_cc_11_11_1536x1080.webp',1,52),
	(81,'adventure_cc_11_12_1536x1080.webp',2,52),
	(82,'frame_cc_porres_furnace_1216x855.webp',3,52),
	(83,'adventure_cc_11_13_1536x1080.webp',1,55),
	(84,'adventure_cc_11_14_1536x1080.webp',2,55),
	(85,'final_cc_03_1536x1080.webp',1,56),
	(86,'adventure_cc_13_01_1536x1080.webp',1,59),
	(87,'item_cc_astral_amulet_1536x1080.webp',2,59),
	(88,'adventure_cc_13_02_1536x1080.webp',1,60),
	(89,'adventure_cc_13_03_1536x1080.webp',2,60),
	(90,'item_cc_green_tinkler_1536x1080.webp',3,60),
	(91,'adventure_cc_13_04_1536x1080.webp',1,61),
	(92,'adventure_cc_13_07_1536x1080.webp',2,61),
	(93,'adventure_cc_13_08_1536x1080.webp',3,61),
	(94,'adventure_cc_13_09_1536x1080.webp',4,61),
	(95,'adventure_cc_13_10_1536x1080.webp',5,61),
	(96,'adventure_cc_13_11_1536x1080.webp',6,61),
	(97,'adventure_cc_15_01_1536x1080.webp',7,61),
	(98,'adventure_cc_13_12_1536x1080.webp',8,61),
	(99,'adventure_cc_13_13_1536x1080.webp',9,61),
	(100,'frame_cc_tropical_paradise_1216x855.webp',10,61),
	(101,'adventure_cc_13_05_1536x1080.webp',1,62),
	(102,'adventure_cc_13_06_1536x1080.webp',2,62),
	(103,'adventure_cc_13_09_1536x1080.webp',3,62),
	(104,'adventure_cc_14_01_1536x1080.webp',4,62),
	(105,'adventure_cc_13_14_1536x1080.webp',5,62),
	(106,'adventure_cc_13_15_1536x1080.webp',6,62),
	(107,'adventure_cc_13_16_1536x1080.webp',7,62),
	(108,'adventure_cc_13_17_1536x1080.webp',8,62),
	(109,'adventure_cc_13_18_1536x1080.webp',9,62),
	(110,'frame_cc_guldovian_stitch_1216x855.webp',10,62),
	(111,'adventure_cc_14_01_1536x1080.webp',1,64),
	(112,'adventure_cc_14_02_1536x1080.webp',2,64),
	(113,'item_cc_beeba_flute_1536x1080.webp',3,64),
	(114,'adventure_cc_14_03_1536x1080.webp',4,64),
	(115,'adventure_cc_14_04_1536x1080.webp',1,65),
	(116,'adventure_cc_14_05_1536x1080.webp',1,66),
	(117,'adventure_cc_14_06_1536x1080.webp',2,66),
	(118,'adventure_cc_14_07_1536x1080.webp',3,66),
	(119,'adventure_cc_14_08_1536x1080.webp',4,66),
	(120,'item_cc_hydra_humour_1536x1080.webp',5,66),
	(121,'adventure_cc_15_01_1536x1080.webp',1,68),
	(122,'adventure_cc_15_02_1536x1080.webp',2,68),
	(123,'adventure_cc_15_03_1536x1080.webp',1,69),
	(124,'adventure_cc_15_04_1536x1080.webp',2,69),
	(125,'adventure_cc_15_05_1536x1080.webp',1,70),
	(126,'adventure_cc_16_01_1536x1080.webp',1,72),
	(127,'item_cc_pelvic_bone.webp',2,72),
	(128,'adventure_cc_16_02_1536x1080.webp',1,73),
	(129,'adventure_cc_16_03_1536x1080.webp',2,73),
	(130,'adventure_cc_16_04_1536x1080.webp',1,74),
	(131,'adventure_cc_16_05_1536x1080.webp',2,74),
	(132,'adventure_cc_16_06_1536x1080.webp',3,74),
	(133,'adventure_cc_16_07_1536x1080.webp',4,74),
	(134,'adventure_cc_17_01_1536x1080.webp',1,75),
	(135,'item_cc_book_of_poems.webp',2,75),
	(136,'adventure_cc_17_02_1536x1080.webp',1,77),
	(137,'adventure_cc_17_03_1536x1080.webp',2,77),
	(138,'item_cc_ice_breath.webp',3,77),
	(139,'adventure_cc_17_04_1536x1080.webp',4,77),
	(140,'adventure_cc_17_05_1536x1080.webp',1,78),
	(141,'adventure_cc_17_06_1536x1080.webp',1,79),
	(142,'adventure_cc_17_07_1536x1080.webp',2,79),
	(143,'adventure_cc_18_01_1536x1080.webp',1,82),
	(144,'adventure_cc_18_02_1536x1080.webp',2,82),
	(145,'adventure_cc_18_03_1536x1080.webp',3,82),
	(146,'adventure_cc_18_04_1536x1080.webp',4,82),
	(147,'final_cc_04_1536x1080.webp',1,83),
	(148,'item_cc_relief_charm_1536x1080.webp',2,83),
	(149,'adventure_cc_20_01_1536x1080.webp',1,86),
	(150,'adventure_cc_20_02_1536x1080.webp',2,86),
	(151,'adventure_cc_21_01_1536x1080.webp',1,90),
	(152,'adventure_cc_21_02_1536x1080.webp',2,90),
	(153,'final_cc_05_1536x1080.webp',1,91),
	(154,'adventure_cc_23_01_1536x1080.webp',1,93),
	(155,'adventure_cc_23_02_1536x1080.webp',2,93),
	(156,'frame_cc_infrared_vision_1216x855.webp',1,94),
	(157,'adventure_cc_23_03_1536x1080.webp',1,95),
	(158,'adventure_cc_23_04_1536x1080.webp',2,95),
	(159,'adventure_cc_23_05_1536x1080.webp',3,95),
	(160,'item_cc_smith_spirit_1536x1080.webp',4,95),
	(161,'adventure_cc_23_06_1536x1080.webp',5,95),
	(162,'adventure_cc_23_07_1536x1080.webp',6,95),
	(163,'adventure_cc_24_01_1536x1080.webp',1,96),
	(164,'adventure_cc_24_02_1536x1080.webp',2,96),
	(165,'item_cc_mushroom_1536x1080.webp',3,96),
	(166,'adventure_cc_24_03_1536x1080.webp',4,96),
	(167,'adventure_cc_24_04_1536x1080.webp',5,96),
	(168,'adventure_cc_24_05_1536x1080.webp',1,97),
	(169,'adventure_cc_24_06_1536x1080.webp',2,97),
	(170,'adventure_cc_24_07_1536x1080.webp',3,97),
	(171,'adventure_cc_24_08_1536x1080.webp',4,97),
	(172,'adventure_cc_24_09_1536x1080.webp',1,98),
	(173,'adventure_cc_25_01_1536x1080.webp',1,99),
	(174,'adventure_cc_25_02_1536x1080.webp',2,99),
	(175,'item_cc_star_fragment_1536x1080.webp',3,99),
	(176,'adventure_cc_25_03_1536x1080.webp',1,100),
	(177,'adventure_cc_25_04_1536x1080.webp',2,100),
	(178,'frame_cc_our_favorite_martian_1216x855.webp',3,100),
	(179,'adventure_cc_25_05_1536x1080.webp',1,101),
	(180,'frame_cc_valencian_cloth_1216x855.webp',2,101),
	(181,'adventure_cc_25_06_1536x1080.webp',3,101),
	(182,'adventure_cc_26_01_1536x1080.webp',1,103),
	(183,'adventure_cc_26_02_1536x1080.webp',2,103),
	(184,'adventure_cc_26_03_1536x1080.webp',3,103),
	(185,'adventure_cc_26_04_1536x1080.webp',4,103),
	(186,'adventure_cc_26_05_1536x1080.webp',1,107),
	(187,'adventure_cc_26_06_1536x1080.webp',2,107),
	(188,'frame_cc_monsters_mouth_1216x855.webp',3,107),
	(189,'adventure_cc_26_07_1536x1080.webp',4,107),
	(190,'item_cc_handle_1536x1080.webp',5,107),
	(191,'adventure_cc_26_08_1536x1080.webp',6,107),
	(192,'adventure_cc_26_09_1536x1080.webp',1,111),
	(193,'adventure_cc_26_10_1536x1080.webp',2,111),
	(194,'adventure_cc_26_11_1536x1080.webp',3,111),
	(195,'item_cc_fiddler_crab_1536x1080.webp',4,111),
	(196,'adventure_cc_26_12_1536x1080.webp',5,111),
	(197,'adventure_cc_26_13_1536x1080.webp',6,111),
	(198,'adventure_cc_26_14_1536x1080.webp',7,111),
	(199,'adventure_cc_27_01_1536x1080.webp',1,113),
	(200,'adventure_cc_27_02_1536x1080.webp',2,113),
	(201,'adventure_cc_27_03_1536x1080.webp',3,113),
	(202,'adventure_cc_27_04_1536x1080.webp',4,113),
	(203,'adventure_cc_27_05_1536x1080.webp',5,113),
	(204,'item_cc_garais_keepsake_1536x1080.webp',6,113),
	(205,'adventure_cc_28_01_1536x1080.webp',1,116),
	(206,'adventure_cc_28_02_1536x1080.webp',2,116),
	(207,'item_cc_einlanzer_1536x1080.webp',3,116),
	(208,'adventure_cc_28_03_1536x1080.webp',4,116),
	(209,'adventure_cc_29_01_1536x1080.webp',1,118),
	(210,'item_cc_station_pass_1536x1080.webp',2,118),
	(211,'adventure_cc_29_02_1536x1080.webp',3,118),
	(212,'item_cc_medical_book_1536x1080.webp',4,118),
	(213,'adventure_cc_29_03_1536x1080.webp',1,119),
	(214,'item_cc_prop_sword_1536x1080.webp',2,119),
	(215,'adventure_cc_30_01_1536x1080.webp',1,122),
	(216,'adventure_cc_30_02_1536x1080.webp',2,122),
	(217,'item_cc_life_sparkle_1536x1080.webp',3,122),
	(218,'adventure_cc_30_03_1536x1080.webp',4,122),
	(219,'item_cc_good_backbone_1536x1080.webp',5,122),
	(220,'adventure_cc_30_04_1536x1080.webp',1,123),
	(221,'adventure_cc_30_05_1536x1080.webp',2,123),
	(222,'item_cc_tear_of_hate_1536x1080.webp',3,123),
	(223,'adventure_cc_30_06_1536x1080.webp',4,123),
	(224,'adventure_cc_31_01_1536x1080.webp',1,126),
	(225,'item_cc_prison_key_1536x1080.webp',2,126),
	(226,'adventure_cc_31_02_1536x1080.webp',1,127),
	(227,'item_cc_parlor_key_1536x1080.webp',2,127),
	(228,'adventure_cc_31_03_1536x1080.webp',1,131),
	(229,'adventure_cc_31_04_1536x1080.webp',2,131),
	(230,'adventure_cc_31_05_1536x1080.webp',3,131),
	(231,'adventure_cc_31_06_1536x1080.webp',4,131),
	(232,'adventure_cc_31_07_1536x1080.webp',1,135),
	(233,'adventure_cc_31_08_1536x1080.webp',2,135),
	(234,'final_cc_06_1536x1080.webp',1,136),
	(235,'adventure_cc_33_01_1536x1080.webp',1,138),
	(236,'item_cc_sapphire_brooch_1536x1080.webp',2,138),
	(237,'adventure_cc_33_02_1536x1080.webp',3,138),
	(238,'adventure_cc_33_03_1536x1080.webp',4,138),
	(239,'adventure_cc_33_04_1536x1080.webp',1,139),
	(240,'item_cc_sturdy_ribs_1536x1080.webp',2,139),
	(241,'adventure_cc_33_05_1536x1080.webp',1,140),
	(242,'item_cc_mixed_bones_1536x1080.webp',2,140),
	(243,'adventure_cc_33_06_1536x1080.webp',1,141),
	(244,'adventure_cc_33_07_1536x1080.webp',2,141),
	(245,'adventure_cc_34_01_1536x1080.webp',1,143),
	(246,'adventure_cc_34_02_1536x1080.webp',2,143),
	(247,'adventure_cc_34_03_1536x1080.webp',3,143),
	(248,'adventure_cc_34_04_1536x1080.webp',4,143),
	(249,'final_cc_07_1536x1080.webp',1,144),
	(250,'adventure_cc_37_01_1536x1080.webp',1,153),
	(251,'adventure_cc_37_02_1536x1080.webp',2,153),
	(252,'adventure_cc_37_03_1536x1080.webp',1,154),
	(253,'adventure_cc_37_04_1536x1080.webp',2,154),
	(254,'adventure_cc_37_05_1536x1080.webp',3,154),
	(255,'adventure_cc_37_06_1536x1080.webp',1,155),
	(256,'item_cc_ancient_fruit_1536x1080.webp',2,155),
	(257,'adventure_cc_38_01_1536x1080.webp',1,157),
	(258,'adventure_cc_38_02_1536x1080.webp',2,157),
	(259,'adventure_cc_38_03_1536x1080.webp',1,158),
	(260,'item_cc_green_relic_1536x1080.webp',2,158),
	(261,'adventure_cc_38_04_1536x1080.webp',3,158),
	(262,'adventure_cc_39_01_1536x1080.webp',1,160),
	(263,'adventure_cc_39_02_1536x1080.webp',2,160),
	(264,'item_cc_blue_relic_1536x1080.webp',3,160),
	(265,'adventure_cc_40_01_1536x1080.webp',1,162),
	(266,'adventure_cc_40_02_1536x1080.webp',2,162),
	(267,'adventure_cc_40_03_1536x1080.webp',3,162),
	(268,'item_cc_explosive_1536x1080.webp',4,162),
	(269,'adventure_cc_40_04_1536x1080.webp',1,165),
	(270,'item_cc_yellow_relic_1536x1080.webp',2,165),
	(271,'adventure_cc_41_01_1536x1080.webp',1,168),
	(272,'adventure_cc_41_02_1536x1080.webp',2,168),
	(273,'item_cc_red_relic_1536x1080.webp',3,168),
	(274,'adventure_cc_42_01_1536x1080.webp',1,169),
	(275,'frame_cc_quill_and_papyrus_1216x855.webp',2,169),
	(276,'adventure_cc_42_02_1536x1080.webp',1,171),
	(277,'item_cc_black_relic_1536x1080.webp',2,171),
	(278,'adventure_cc_43_01_1536x1080.webp',1,173),
	(279,'adventure_cc_43_02_1536x1080.webp',2,173),
	(280,'item_cc_white_relic_1536x1080.webp',3,173),
	(281,'adventure_cc_44_01_1536x1080.webp',1,179),
	(282,'adventure_cc_45_01_1536x1080.webp',1,180),
	(283,'item_cc_dragon_emblem_1536x1080.webp',2,180),
	(284,'adventure_cc_45_02_1536x1080.webp',1,182),
	(285,'adventure_cc_45_03_1536x1080.webp',2,182),
	(286,'item_cc_dragon_tear_1536x1080.webp',3,182),
	(287,'adventure_cc_45_04_1536x1080.webp',4,182),
	(288,'adventure_cc_46_01_1536x1080.webp',1,187),
	(289,'adventure_cc_46_02_1536x1080.webp',2,187),
	(290,'adventure_cc_46_03_1536x1080.webp',3,187),
	(291,'item_cc_tear_of_love_1536x1080.webp',4,187),
	(292,'adventure_cc_47_01_1536x1080.webp',1,189),
	(293,'adventure_cc_47_02_1536x1080.webp',2,189),
	(294,'item_cc_memento_pendant_1536x1080.webp',3,189),
	(295,'adventure_cc_47_03_1536x1080.webp',1,194),
	(296,'adventure_cc_47_04_1536x1080.webp',2,194),
	(297,'adventure_cc_47_05_1536x1080.webp',3,194),
	(298,'final_cc_08_1536x1080.webp',1,195),
	(299,'adventure_cc_49_01_1536x1080.webp',1,200),
	(300,'adventure_cc_49_02_1536x1080.webp',2,200),
	(301,'adventure_cc_49_03_1536x1080.webp',3,200),
	(302,'adventure_cc_49_04_1536x1080.webp',4,200),
	(303,'adventure_cc_50_01_1536x1080.webp',1,205),
	(304,'adventure_cc_50_02_1536x1080.webp',2,205),
	(305,'adventure_cc_50_03_1536x1080.webp',1,209),
	(306,'adventure_cc_50_04_1536x1080.webp',2,209),
	(307,'item_cc_card_key_1536x1080.webp',3,209),
	(308,'adventure_cc_50_05_1536x1080.webp',4,209),
	(309,'adventure_cc_51_01_1536x1080.webp',1,213),
	(310,'adventure_cc_51_02_1536x1080.webp',2,213),
	(311,'adventure_cc_51_03_1536x1080.webp',3,213),
	(312,'item_cc_chrono_cross_1536x1080.webp',4,213),
	(313,'adventure_cc_52_01_1536x1080.webp',1,216),
	(314,'adventure_cc_52_02_1536x1080.webp',1,217),
	(315,'adventure_cc_52_03_1536x1080.webp',2,217),
	(316,'adventure_cc_52_04_1536x1080.webp',1,219),
	(317,'adventure_cc_52_05_1536x1080.webp',2,219),
	(318,'adventure_cc_52_06_1536x1080.webp',1,220),
	(319,'adventure_cc_52_07_1536x1080.webp',2,220),
	(320,'adventure_cc_52_08_1536x1080.webp',3,220),
	(321,'adventure_cc_53_01_1536x1080.webp',1,227),
	(322,'adventure_cc_53_02_1536x1080.webp',2,227),
	(323,'adventure_cc_53_03_1536x1080.webp',3,227),
	(324,'item_cc_ice_gun_1536x1080.webp',4,227),
	(325,'adventure_cc_54_01_1536x1080.webp',1,230),
	(326,'final_cc_09_1536x1080.webp',1,231),
	(327,'adventure_cc_56_01_1536x1080.webp',1,237),
	(328,'adventure_cc_56_02_1536x1080.webp',2,237),
	(329,'item_cc_time_egg_1536x1080.webp',3,237),
	(330,'final_cc_10_1536x1080.webp',4,237),
	(331,'adventure_cc_57_01_1536x1080.webp',1,241),
	(332,'weapon_com_sword_01.webp',1,269),
	(333,'weapon_com_sword_02.webp',1,284),
	(334,'weapon_com_hammer.webp',1,294),
	(335,'weapon_com_katana_01.webp',1,307),
	(336,'weapon_com_katana_02.webp',1,320),
	(337,'weapon_com_shield.webp',1,336),
	(338,'set_com_01_a.webp',1,341),
	(339,'set_com_01_b.webp',2,341),
	(340,'set_com_01_c.webp',3,341),
	(341,'set_com_02_a.webp',1,346),
	(342,'set_com_02_b.webp',2,346)
GO
SET IDENTITY_INSERT GG_AdventureImgs OFF
GO

-- Stored Procedure ---------------------------------------------
-- --------------------------------------------------------------

CREATE OR ALTER PROCEDURE GG_Login
    @Email VARCHAR(100),
    @GoogleSUB VARCHAR(256),
    @GoogleJTI VARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;

    -- Validar par�metros de entrada
    IF @Email IS NULL OR @GoogleSUB IS NULL OR @GoogleJTI IS NULL
		BEGIN
			SELECT 
				0 AS IsSuccess, 
				400 AS StatusCode, 
				'Parámetros Obligatorios' AS Message, 
				NULL AS User_Id, 
				NULL AS SqlToken
			RETURN
		END

	BEGIN TRANSACTION

	BEGIN TRY
		IF EXISTS (SELECT User_Id FROM GG_Users WHERE Email = @Email)
			BEGIN
				IF NOT EXISTS (SELECT User_Id FROM GG_Users WHERE Email = @Email AND GoogleSUB = @GoogleSUB)
					BEGIN
						SELECT 
							0 AS IsSuccess, 
							401 AS StatusCode, 
							'Usuario No Autorizado' AS Message, 
							NULL AS User_Id, 
							NULL AS SqlToken
						RETURN
					END

				UPDATE GG_Users SET 
					GoogleJTI = @GoogleJTI,
					SqlToken = NEWID()
				WHERE Email = @Email
			END
		ELSE
			BEGIN
				INSERT INTO GG_Users 
					(User_Id, Email, GoogleSUB, GoogleJti, SqlToken)
				VALUES 
					(NEWID(), @Email, @GoogleSUB, @GoogleJTI, NEWID())
			END

		SELECT 
			1 AS IsSuccess, 
			200 AS StatusCode, 
			'Ok' AS Message, 
			User_Id, 
			SqlToken 
		FROM GG_Users WHERE Email = @Email	

		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION

	    SELECT 
			0 AS IsSuccess, 
			ERROR_STATE() AS StatusCode, 
			ERROR_MESSAGE() AS Message, 
			NULL AS User_Id, 
			NULL AS SqlToken
	END CATCH

END
GO

CREATE OR ALTER PROCEDURE GG_UserGuides_Set
    @User_Id UNIQUEIDENTIFIER,
    @Guide_Id INT,
    @IsCheck BIT
AS
BEGIN
    SET NOCOUNT ON;

    -- Validar parámetros de entrada
    IF @User_Id IS NULL OR @Guide_Id IS NULL OR @IsCheck IS NULL
    BEGIN
        SELECT 0 AS IsSuccess, 400 AS StatusCode, 'Parámetros Obligatorios' AS Message;
        RETURN;
    END;

    -- Verificar si el usuario existe
    IF NOT EXISTS (SELECT User_Id FROM GG_Users WHERE User_Id = @User_Id)
    BEGIN
        SELECT 0 AS IsSuccess, 404 AS StatusCode, 'No se Encontró el Usuario' AS Message;
        RETURN;
    END;

	-- Verificar si la guia existe
    IF NOT EXISTS (SELECT Guide_Id FROM GG_Guides WHERE Guide_Id = @Guide_Id)
    BEGIN
        SELECT 0 AS IsSuccess, 404 AS StatusCode, 'No se Encontró la Guia' AS Message;
        RETURN;
    END;

    BEGIN TRANSACTION;

    BEGIN TRY
        -- Usar MERGE para manejar la inserción o actualización
        MERGE GG_UserGuides AS target
        USING (SELECT @Guide_Id AS Guide_Id, @User_Id AS User_Id) AS source
        ON target.Guide_Id = source.Guide_Id AND target.User_Id = source.User_Id
        WHEN MATCHED THEN
            UPDATE SET IsCheck = @IsCheck
        WHEN NOT MATCHED THEN
            INSERT (User_Id, Guide_Id, IsCheck)
            VALUES (@User_Id, @Guide_Id, @IsCheck);

        -- Confirmar la transacci�n
        COMMIT TRANSACTION;

        -- Devolver Éxito
        SELECT 1 AS IsSuccess, 200 AS StatusCode, 'Ok' AS Message;
    END TRY
    BEGIN CATCH
        -- Revertir la transacción en caso de error
        ROLLBACK TRANSACTION;

        -- Devolver error
        SELECT 0 AS IsSuccess, ERROR_STATE() AS StatusCode, ERROR_MESSAGE() AS Message;
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE GG_UserAdventures_Set
    @User_Id UNIQUEIDENTIFIER,
    @Adventure_Id INT,
    @IsCheck BIT
AS
BEGIN
    SET NOCOUNT ON;

    -- Validar parámetros de entrada
    IF @User_Id IS NULL OR @Adventure_Id IS NULL OR @IsCheck IS NULL
    BEGIN
        SELECT 0 AS IsSuccess, 400 AS StatusCode, 'Parámetros Obligatorios' AS Message;
        RETURN;
    END;

    -- Verificar si el usuario existe
    IF NOT EXISTS (SELECT User_Id FROM GG_Users WHERE User_Id = @User_Id)
    BEGIN
        SELECT 0 AS IsSuccess, 404 AS StatusCode, 'No se Encontró el Usuario' AS Message;
        RETURN;
    END;

	-- Verificar si la aventura existe
    IF NOT EXISTS (SELECT Adventure_Id FROM GG_Adventures WHERE Adventure_Id = @Adventure_Id)
    BEGIN
        SELECT 0 AS IsSuccess, 404 AS StatusCode, 'No se Encontró la Aventura' AS Message;
        RETURN;
    END;

    BEGIN TRANSACTION;

    BEGIN TRY
        -- Usar MERGE para manejar la inserción o actualización
        MERGE GG_UserAdventures AS target
        USING (SELECT @Adventure_Id AS Adventure_Id, @User_Id AS User_Id) AS source
        ON target.Adventure_Id = source.Adventure_Id AND target.User_Id = source.User_Id
        WHEN MATCHED THEN
            UPDATE SET IsCheck = @IsCheck
        WHEN NOT MATCHED THEN
            INSERT (User_Id, Adventure_Id, IsCheck)
            VALUES (@User_Id, @Adventure_Id, @IsCheck);

        -- Confirmar la transacción
        COMMIT TRANSACTION;

        -- Devolver Éxito
        SELECT 1 AS IsSuccess, 200 AS StatusCode, 'Ok' AS Message;
    END TRY
    BEGIN CATCH
        -- Revertir la transacción en caso de error
        ROLLBACK TRANSACTION;

        -- Devolver error
        SELECT 0 AS IsSuccess, ERROR_STATE() AS StatusCode, ERROR_MESSAGE() AS Message;
    END CATCH;
END;
GO

DROP PROCEDURE GG_Login
GO
DROP PROCEDURE GG_GuidesUser_Set
GO
DROP PROCEDURE GG_AdventuresUser_Set
GO

-- Query --------------------------------------------------------
-- --------------------------------------------------------------

SELECT * FROM GG_Users
SELECT * FROM GG_Games
SELECT * FROM GG_Characters
SELECT * FROM GG_Sources
SELECT * FROM GG_BackgroundImgs
SELECT * FROM GG_Guides
SELECT * FROM GG_Adventures
SELECT * FROM GG_AdventureImgs
SELECT * FROM GG_UserGuides
SELECT * FROM GG_UserAdventures

EXECUTE GG_Login 'b@b', '123', '456'
EXECUTE GG_UserGuides_Set 'FBAA1ED3-3EF4-4211-8DAC-9C21890AC481', 1, 1
EXECUTE GG_UserAdventures_Set 'FBAA1ED3-3EF4-4211-8DAC-9C21890AC481', 1, 1

-- --------------------------------------------------------------
-- --------------------------------------------------------------
