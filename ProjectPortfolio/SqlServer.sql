USE db_testing
﻿
-- Tablas -------------------------------------------------------
-- --------------------------------------------------------------

CREATE TABLE PF_Projects (
	Id INT PRIMARY KEY IDENTITY(1,1),
	Name VARCHAR(50) NOT NULL,
	ImgUrl VARCHAR(100) NOT NULL,
	RepoUrl VARCHAR(256),
	AppUrl VARCHAR(256)
)
GO

CREATE TABLE PF_Languages (
	Id INT PRIMARY KEY IDENTITY(1,1),
	Name VARCHAR(50) NOT NULL,
	ImgUrl VARCHAR(100) NOT NULL,
)
GO

CREATE TABLE PF_Technologies (
	Id INT PRIMARY KEY IDENTITY(1,1),
	Name VARCHAR(50) NOT NULL,
	ImgUrl VARCHAR(100) NOT NULL,
)
GO

CREATE TABLE PF_Pro_Lang (
	Id_Project INT,
	Id_Language INT,
	PRIMARY KEY (Id_Project, Id_Language),
	FOREIGN KEY (Id_Project) REFERENCES PF_Projects(Id),
	FOREIGN KEY (Id_Language) REFERENCES PF_Languages(Id)
)
GO

CREATE TABLE PF_Pro_Tech (
	Id_Project INT,
	Id_Technology INT,
	PRIMARY KEY (Id_Project, Id_Technology),
	FOREIGN KEY (Id_Project) REFERENCES PF_Projects(Id),
	FOREIGN KEY (Id_Technology) REFERENCES PF_Technologies(Id)
)
GO

CREATE TABLE PF_UrlGrp (
	Id INT PRIMARY KEY IDENTITY(1,1),
	Name VARCHAR(50) NOT NULL,
	IsEnable BIT NOT NULL,
	UNIQUE (Name),
)
GO

CREATE TABLE PF_Url (
	Id INT PRIMARY KEY IDENTITY(1,1),
	Name VARCHAR(50) NOT NULL,
	Link VARCHAR(256) NOT NULL,
	IsEnable BIT NOT NULL,
	Id_UrlGrp INT NOT NULL,
	UNIQUE (Name),
	FOREIGN KEY (Id_UrlGrp) REFERENCES PF_UrlGrp(Id),
)
GO

DROP TABLE PF_Pro_Tech
GO
DROP TABLE PF_Pro_Lang
GO
DROP TABLE PF_Technologies
GO
DROP TABLE PF_Languages
GO
DROP TABLE PF_Projects
GO
DROP TABLE PF_UrlGrp
GO
DROP TABLE PF_Url
GO

-- Data ---------------------------------------------------------
-- --------------------------------------------------------------

SET IDENTITY_INSERT PF_Projects ON
GO
INSERT INTO PF_Projects
	(Id, Name, ImgUrl, RepoUrl, AppUrl)
VALUES
	(1, 'Transbank POS Integration', 'project_pos_863x568.webp', 'https://github.com/TheNefelin/Transbank_POS_v1', null),
	(2, 'Arduino DHT Temperature Monitoring by Network', 'project_dht_863x568.webp', 'https://github.com/TheNefelin/DHT', null),
	(3, 'El Cubo v2.0', 'project_cube2_863x568.webp', null, null),
	(4, 'Portafolio v1.0', 'project_portfolio1_863x568.webp', 'https://github.com/TheNefelin/Portafolio-NextJS', 'https://portafolio-next-js-sand.vercel.app'),
	(5, 'Portafolio v2.0', 'project_portfolio2_863x568.webp', 'https://github.com/TheNefelin/Portafolio-Angular-ssr', 'https://portafolio-angular-ssr.vercel.app'),
	(6, 'Bier Heart Page', 'project_bierheart_863x568.webp', 'https://github.com/TheNefelin/BierHeart_Vue', 'https://www.bierheart.cl'),
	(7, 'WebP Converter', 'project_webp_863x568.webp', 'https://github.com/TheNefelin/WebPConverter_.NETCore', null),
	(8, 'Trueke Mobile App', 'project_trueke_863x568.webp', 'https://github.com/TheNefelin/Kambio_.NetCore', null),
	(9, 'Password Manager Mobile App', 'project_pm_863x568.webp', 'https://github.com/TheNefelin/Portfolio_.NETCore/tree/master/MauiAppAdmin', null),
	(10, 'Guides for Games (NextJS)', 'project_ggv1_863x568.webp', 'https://github.com/TheNefelin/game-guides-nextjs', 'https://game-guides-nextjs.vercel.app'),
	(11, 'Portafolio v3.0 (Astro)', 'project_portfolio3_863x568.webp', 'https://github.com/TheNefelin/portfolio-astro', 'https://www.francisco-dev.cl'),
	(12, 'WebApi for Projects', 'project_webapi_863x568.webp', 'https://github.com/TheNefelin/Projects_.NETCore', 'https://dragonra.bsite.net/index.html')
SET IDENTITY_INSERT PF_Projects OFF
GO

SET IDENTITY_INSERT PF_Languages ON
GO
INSERT INTO PF_Languages
	(Id, Name, ImgUrl)
VALUES
	(1, 'CSharp', 'lang_csharp_512x512.webp'),
	(2, 'CSS 3', 'lang_css3_512x512.webp'),
	(3, 'HTML 5', 'lang_html5_512x512.webp'),
	(4, 'Java', 'lang_java_512x512.webp'),
	(5, 'JavaScript', 'lang_javascript_512x512.webp'),
	(6, 'TypeScript', 'lang_typescript_512x512.webp'),
	(7, 'Visual Basic', 'lang_vb_512x512.webp')
SET IDENTITY_INSERT PF_Languages OFF
GO

SET IDENTITY_INSERT PF_Technologies ON
GO
INSERT INTO PF_Technologies
	(Id, Name, ImgUrl)
VALUES
	(1, 'Angular', 'tech_angular_512x512.webp'),
	(2, 'Bootstrap', 'tech_bootstrap_512x512.webp'),
	(3, '.NET', 'tech_dotnet_512x512.webp'),
	(4, 'Git', 'tech_git_512x512.webp'),
	(5, 'GitHub', 'tech_github_512x512.webp'),
	(6, 'Sql Server', 'tech_mssql_512x512.webp'),
	(7, 'MySql', 'tech_mysql_512x512.webp'),
	(8, 'NextJS', 'tech_nextjs_512x512.webp'),
	(9, 'NodeJS', 'tech_nodejs_512x512.webp'),
	(10, 'PostgreSQL', 'tech_postgresql_512x512.webp'),
	(11, 'React', 'tech_react_512x512.webp'),
	(12, 'Taildwind', 'tech_tailwindcss_512x512.webp'),
	(13, 'Unity', 'tech_unity_512x512.webp'),
	(14, 'Visual Studio', 'tech_visualstudio_512x512.webp'),
	(15, 'VSCode', 'tech_vscode_512x512.webp'),
	(16, 'VueJS', 'tech_vue_512x512.webp'),
	(17, 'AstroJS', 'tech_astro_512x512.webp')	
SET IDENTITY_INSERT PF_Technologies OFF
GO

INSERT INTO PF_Pro_Lang
	(Id_Project, Id_Language)
VALUES
	(1, 7),
	(2, 7),
	(3, 1),
	(4, 2),
	(4, 3),
	(4, 5),
	(5, 2),
	(5, 3),
	(5, 6),
	(6, 2),
	(6, 3),
	(6, 5),
	(7, 1),
	(8, 1),
	(9, 1),
	(10, 2),
	(10, 3),
	(10, 6),
	(11, 2),
	(11, 3),
	(11, 6),
	(12, 1)
GO

INSERT INTO PF_Pro_Tech
	(Id_Project, Id_Technology)
VALUES
	(1, 3),
	(1, 14),
	(2, 3),
	(2, 14),
	(3, 13),
	(3, 14),
	(4, 8),
	(4, 15),
	(5, 1),
	(5, 12),
	(5, 15),
	(6, 12),
	(6, 15),
	(6, 16),
	(7, 2),
	(7, 3),
	(7, 14),
	(8, 3),
	(8, 6),
	(8, 14),
	(9, 3),
	(9, 6),
	(9, 14),
	(10, 6),
	(10, 8),
	(10, 12),
	(10, 15),
	(11, 6),
	(11, 12),
	(11, 15),
	(11, 17),
	(12, 3),
	(12, 6),
	(12, 14)
GO

SET IDENTITY_INSERT PF_UrlGrp ON
GO
INSERT INTO PF_UrlGrp
	(Id, Name, IsEnable)
VALUES
	(1,'Framework',1),
	(2,'Learning',1),
	(3,'Hosting',1),
	(4,'Images',1),
	(5,'Tools',1),
	(6,'IA',1),
	(7,'Agile',1),
	(8,'My Links',1)
GO
SET IDENTITY_INSERT PF_UrlGrp OFF
GO

SET IDENTITY_INSERT PF_Url ON
GO
INSERT INTO PF_Url
	(Id, Name, Link, IsEnable, Id_UrlGrp)
VALUES
	(1,'DaisyUI','https://daisyui.com',1,1),
	(2,'FastAPI','https://fastapi.tiangolo.com',1,1),
	(3,'Flowbite','https://flowbite.com',1,1),
	(4,'Heroicons','https://heroicons.com',1,1),
	(5,'JavaJDK','https://www.oracle.com/cl/java/technologies/downloads',1,1),
	(6,'Mui','https://mui.com',1,1),
	(7,'NextAuth','https://next-auth.js.org',1,1),
	(8,'NextJS','https://nextjs.org',1,1),
	(9,'NextUI','https://nextui.org',1,1),
	(10,'Strapi','https://strapi.io',1,1),
	(11,'Tailwindcss','https://tailwindcss.com',1,1),
	(12,'Tremor','https://www.tremor.so',1,1),
	(13,'VueJS','https://vuejs.org',1,1),
	(14,'VueUse','https://vueuse.org',1,1),
	(15,'Animate','https://animate.style',1,2),
	(16,'CodeDex','https://www.codedex.io',1,2),
	(17,'CodelyTV','https://www.youtube.com/@CodelyTV',1,2),
	(18,'DevDocs','https://devdocs.io',1,2),
	(19,'LucidChart','https://www.lucidchart.com',1,2),
	(20,'Scrimba','https://scrimba.com',1,2),
	(21,'Uiverse','https://uiverse.io/all',1,2),
	(22,'VS Dev Essentials','https://visualstudio.microsoft.com/es/dev-essentials/',1,2),
	(23,'w3schools','https://www.w3schools.com',1,2),
	(24,'WebDev','https://web.dev',1,2),
	(25,'AzureDevObs','https://dev.azure.com',1,3),
	(26,'Cloudflare','https://www.cloudflare.com/es-es',1,3),
	(27,'DigitalOcean','https://www.digitalocean.com',1,3),
	(28,'DonWeb','https://donweb.com/es-cl',1,3),
	(29,'Fly','https://fly.io',1,3),
	(30,'FreeAspHosting','https://freeasphosting.net',1,3),
	(31,'GameHost','https://www.gamehost.cl',1,3),
	(32,'InfinityFree','https://www.infinityfree.net',1,3),
	(33,'Koyeb','https://www.koyeb.com',1,3),
	(34,'Linode','https://www.linode.com/es',1,3),
	(35,'Netlify','https://www.netlify.com',1,3),
	(36,'OpenShift','https://www.redhat.com/es/technologies/cloud-computing/openshift',1,3),
	(37,'Railway','https://railway.app',1,3),
	(38,'Vercel','https://vercel.com',1,3),
	(39,'ConvertToIcon','https://convertio.co/es',1,4),
	(40,'FreePik','https://www.freepik.com',1,4),
	(41,'FreeSVG','https://freesvg.org',1,4),
	(42,'IconIfy','https://iconify.design/',1,4),
	(43,'PixaBay','https://pixabay.com',1,4),
	(44,'SocialSVG','https://www.svgrepo.com/collection/social-and-company-colored-logo-icons',1,4),
	(45,'SoftIcons','https://www.softicons.com',1,4),
	(46,'UnSplash','https://unsplash.com',1,4),
	(47,'Canva','https://www.canva.com',1,5),
	(48,'CodePen','https://codepen.io',1,5),
	(49,'DbDiagram','https://dbdiagram.io/home',1,5),
	(50,'Draw.io','https://app.diagrams.net',1,5),
	(51,'Excalidraw','https://excalidraw.com',1,5),
	(52,'Figma','https://www.figma.com',1,5),
	(53,'GitHub','https://github.com/TheNefelin',1,5),
	(54,'LabCenter','https://www.labcenter.com',1,5),
	(55,'LinkedIn','https://www.linkedin.com/in/nefelin',1,5),
	(56,'MapBox','https://www.mapbox.com/',1,5),
	(57,'Mixamo','https://www.mixamo.com/#',1,5),
	(58,'Notion','https://www.notion.so',1,5),
	(59,'Oklch','https://oklch.com',1,5),
	(60,'Owl Carousel 2','https://owlcarousel2.github.io/OwlCarousel2',1,5),
	(61,'PlayCode','https://playcode.io',1,5),
	(62,'PublicAPI','https://publicapi.dev',1,5),
	(63,'Songsterr','https://www.songsterr.com',1,5),
	(64,'AgentGPT','https://agentgpt.reworkd.ai/es',1,6),
	(65,'BlackBox','https://www.useblackbox.io',1,6),
	(66,'ChatGPT','https://chat.openai.com',1,6),
	(67,'Claude2','https://claude.ai',1,6),
	(68,'ElevenLabs','https://beta.elevenlabs.io',1,6),
	(69,'GoogleBard','https://bard.google.com',1,6),
	(70,'Leonardo','https://leonardo.ai',1,6),
	(71,'LiterallyAnything','https://www.literallyanything.io',1,6),
	(72,'Podcast','https://podcast.adobe.com/enhance',1,6),
	(73,'Asana','https://asana.com/es',1,7),
	(74,'ClickUp','https://clickup.com',1,7),
	(75,'Jira','https://www.atlassian.com/es/software/jira',1,7),
	(76,'Trello','https://trello.com/es',1,7),
	(77,'AnimeFLV','https://www3.animeflv.net',1,8),
	(78,'EldenRing Map','https://mapgenie.io/elden-ring/maps/the-lands-between',1,8),
	(79,'GetOnbrd','https://www.getonbrd.com/misempleos',1,8),
	(80,'Maps','https://geekflare.com/es/geocoding-maps-api-solution',1,8),
	(81,'Mi Insignia Java','https://www.acreditta.com/credential/67c91ad1-66c6-4fbd-9319-0bc7e3ad89f3',1,8),
	(82,'Mi Insignia JS','https://www.acreditta.com/credential/63c99def-c48d-4495-aab5-00a3158d10a0',1,8)
GO
SET IDENTITY_INSERT PF_Url OFF
GO

-- Stored Procedure ---------------------------------------------
-- --------------------------------------------------------------

-- Query --------------------------------------------------------
-- --------------------------------------------------------------

SELECT * FROM PF_UrlGrp
SELECT * FROM PF_Url

SELECT 
	Id, Name, ImgUrl, RepoUrl, AppUrl
FROM PF_Projects ORDER BY Id DESC

SELECT 
	a.Id, a.Name, a.ImgUrl, b.Id_Project
FROM PF_Languages a
	INNER JOIN PF_Pro_Lang b ON a.Id = b.Id_Language

SELECT
	a.Id, a.Name, a.ImgUrl, b.Id_Project
FROM PF_Technologies a
	INNER JOIN PF_Pro_Tech b ON a.Id = b.Id_Technology

INSERT INTO PF_Url
VALUES
	('Mi Insignia Java','https://www.acreditta.com/credential/67c91ad1-66c6-4fbd-9319-0bc7e3ad89f3',1,8),
	('AnimeFLV','https://www3.animeflv.net',1,8),
	('EldenRing Map','https://mapgenie.io/elden-ring/maps/the-lands-between',1,8)

SELECT * ,
'(' + LTRIM(STR(Id)) + ',''' + Name + ''',''' + Url + ''',' + LTRIM(STR(IsEnable)) + ',' + LTRIM(STR(Id_UrlGrp)) + '),'
FROM PF_Url
ORDER BY Id_UrlGrp, Name

-- --------------------------------------------------------------
-- --------------------------------------------------------------
